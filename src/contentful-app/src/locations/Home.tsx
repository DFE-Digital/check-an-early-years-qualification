import React, { useEffect, useState } from "react";
import {
  Stack,
  Form,
  FormControl,
  Button,
  Spinner,
  Heading,
  Box,
} from "@contentful/f36-components";
import { HomeAppSDK } from "@contentful/app-sdk";
import { useSDK } from "@contentful/react-apps-toolkit";
import { qualification } from "../models/qualification";
import csvFileToQualifications from "../helpers/convertCsvToQualifications";
import createAndPublishEntries from "../helpers/createAndPublishEntries";
import { CheckCircleTrimmedIcon } from '@contentful/f36-icons';

const Home = () => {
  const sdk = useSDK<HomeAppSDK>();
  const cma = sdk.cma;

  const fileReader = new FileReader();

  const [submitted, setSubmitted] = useState(false);
  const [file, setFile] = useState();
  const [qualifications, setQualifications] = useState<qualification[]>([]);
  const [uploaded, setUploaded] = useState(false);

  const submitForm = async () => {
    setSubmitted(true);

    if (file) {
      fileReader.onload = function (event: any) {
        const text = event.target.result;
        csvFileToQualifications(text, setQualifications);
      };
      
      
      fileReader.readAsText(file);
    }

    setSubmitted(false);
    setFile(undefined);
  };

  const fileUploaded = (e: any) => {
    setFile(e.target.files[0]);
  };

  const resetPage = () => {
    setFile(undefined);
    setUploaded(false);
    setSubmitted(false);
    setQualifications([]);
  }

  useEffect(() => {
    qualifications.length > 0 && createAndPublishEntries(cma, qualifications, setUploaded);
  }, [cma, qualifications]);

  return (
    <>
      {uploaded === true ? (
        <>
          <Stack flexDirection="column" spacing="spacingS">
            <Heading>Upload Successful</Heading>
            <CheckCircleTrimmedIcon size="xlarge" />
            <Button variant="primary" onClick={resetPage}>
                Go Back
              </Button>
          </Stack>
        </>
      ) : (
        <>
          <Stack flexDirection="column" spacing="spacingS">
            <Heading>Qualification Upload</Heading>
            <Form onSubmit={submitForm}>
              {submitted === true ? (
                <></>
              ) : (
                <FormControl>
                  <FormControl.Label isRequired>
                    Upload a CSV:
                  </FormControl.Label>
                  <Box>
                    <input
                      type="file"
                      onChange={fileUploaded}
                      accept={".csv"}
                    />
                  </Box>
                  <FormControl.HelpText>
                    Please upload a csv with any qualification you want to add.
                  </FormControl.HelpText>
                </FormControl>
              )}
              <Button variant="primary" type="submit" isDisabled={submitted}>
                {submitted ? <Spinner /> : "Upload"}
              </Button>
            </Form>
          </Stack>
        </>
      )}
    </>
  );
};

export default Home;
