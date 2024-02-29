import React, { useEffect, useState } from "react";
import {
  Stack,
  Form,
  FormControl,
  Heading,
  Box,
  ButtonGroup,
} from "@contentful/f36-components";
import { qualification } from "../../models/qualification";
import csvFileToQualifications from "../../helpers/convertCsvToQualifications";
import BackButton from "../../components/BackButton";
import NavigationButton from "../../components/NavigationButton";

const BulkUpload = () => {
  const [file, setFile] = useState();
  const [qualifications, setQualifications] = useState<qualification[]>([]);

  const fileUploaded = (e: any) => {
    setFile(e.target.files[0]);
  };

  useEffect(() => {
    if (file !== undefined) {
      const fileReader = new FileReader();

      fileReader.onload = function (event: any) {
        const text = event.target.result;
        csvFileToQualifications(text, setQualifications);
      };

      fileReader.readAsText(file);
      setFile(undefined);
    }
  }, [file]);

  return (
    <>
      <Stack flexDirection="column">
        <Heading>Bulk Upload</Heading>
        <Form>
          <FormControl>
            <FormControl.Label isRequired>Upload a CSV:</FormControl.Label>
            <Box>
              <input type="file" onChange={fileUploaded} accept={".csv"} />
            </Box>
            <FormControl.HelpText>
              Please upload a csv with any qualification you want to add.
            </FormControl.HelpText>
          </FormControl>
          <ButtonGroup variant="spaced" spacing="spacingS">
            <BackButton />
            <NavigationButton url="/review-bulk-upload" isDisabled={qualifications.length === 0} propsToPass={{ qualifications: qualifications }}>
              Next
            </NavigationButton>
          </ButtonGroup>
        </Form>
      </Stack>
    </>
  );
};

export default BulkUpload;
