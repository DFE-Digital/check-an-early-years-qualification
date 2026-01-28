import {
  Stack,
  Heading,
  ButtonGroup,
  Form,
  FormControl,
  TextInput,
  Box,
  Textarea,
  Button,
  Datepicker,
} from "@contentful/f36-components";
import BackButton from "../../components/BackButton";
import { useState } from "react";

const SingleUpload = () => {

  const [submitted, setSubmitted] = useState(false);

  const [qualificationName, setQualificationName] = useState(undefined as string | undefined);
  const [yearFrom, setYearFrom] = useState(undefined as Date | undefined);
  const [yearTo, setYearTo] = useState(undefined as Date | undefined);
  
  const submitForm = async () => {
    setSubmitted(true);

    await new Promise(r => setTimeout(r, 3000));

    setSubmitted(false);
  }

  const isFormValid = () => {
    let isValid = true;

    // validate qualification name
    if ((qualificationName === undefined) || qualificationName === "") {
      isValid = false;
    }

    return isValid;
  }

  return (
    <Stack flexDirection="column" spacing="spacingS">
      <Heading>Single Upload</Heading>

      <Form onSubmit={submitForm}>
        <FormControl>
          <FormControl.Label isRequired>Qualification Name</FormControl.Label>
          <TextInput 
            value={qualificationName}
            onChange={(e) => setQualificationName(e.target.value)}
          />
          <FormControl.HelpText>
            Please enter the name of the qualification.
          </FormControl.HelpText>
        </FormControl>

        <FormControl>
          <FormControl.Label isRequired>Qualification Level</FormControl.Label>
          <Box>
            <TextInput />
          </Box>
        </FormControl>

        <FormControl>
          <FormControl.Label isRequired>Awarding Organisation</FormControl.Label>
          <Box>
            <TextInput />
          </Box>
        </FormControl>

        <FormControl>
          <FormControl.Label>Year From</FormControl.Label>
          <Datepicker
            dateFormat="yyyy"
            selected={yearFrom} onSelect={setYearFrom} />
        </FormControl>

        <FormControl>
          <FormControl.Label>Year To</FormControl.Label>
          <Datepicker
            dateFormat="yyyy"
            selected={yearTo} onSelect={setYearTo} />
        </FormControl>

        <FormControl>
          <FormControl.Label>{"Qualification Number (if known)"}</FormControl.Label>
          <Box>
            <TextInput />
          </Box>
        </FormControl>

        <FormControl>
          <FormControl.Label>Notes/Additional Requirements</FormControl.Label>
          <Box>
            <Textarea />
          </Box>
        </FormControl>

        <ButtonGroup variant="spaced" spacing="spacingS">
          <BackButton />
          <Button variant="primary" type="submit" isDisabled={submitted || !isFormValid()}>
            Publish
          </Button>
        </ButtonGroup>
      </Form>
    </Stack>
  );
};

export default SingleUpload;
