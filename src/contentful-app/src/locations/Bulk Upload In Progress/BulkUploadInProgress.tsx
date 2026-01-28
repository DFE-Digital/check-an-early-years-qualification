import { Paragraph, Spinner, Stack } from "@contentful/f36-components";
import { qualification } from "../../models/qualification";
import { useEffect, useState } from "react";
import { useSDK } from "@contentful/react-apps-toolkit" ;
import createEntry from "../../helpers/createEntry";
import { CheckCircleIcon } from "@contentful/f36-icons";
import { useLocation } from "react-router-dom";
import NavigationButton from "../../components/NavigationButton";

interface BulkUploadInProgressProps {
  qualifications: qualification[]
}

const BulkUploadInProgress = () => {
  const sdk = useSDK();
  const cma = sdk.cma;

  const location = useLocation();
  const props : BulkUploadInProgressProps = location.state

  const [qualificationsUploaded, setQualificationsUploaded] = useState([] as qualification[]);

  const isUploadComplete = () => {
    return qualificationsUploaded.length === props.qualifications.length;
  };

  useEffect(() => {
    const createAndPublish = async () => {
      for (const qualification of props.qualifications) {
        const entryId = String(qualification.qualificationId);
    
        const entryProps = await createEntry(cma, qualification);
    
        await cma.entry.publish({ entryId: entryId }, entryProps);

        setQualificationsUploaded(qualificationsUploaded => [...qualificationsUploaded, qualification] )
      } 
    }

   createAndPublish();
  }, [cma, props])

  return (
    <Stack flexDirection="column" spacing="spacingS" style={{width: "50%", marginLeft: "25%"}}>
      {isUploadComplete() ? <CheckCircleIcon size="medium" /> : <Spinner size="medium" />}
      <Paragraph>Uploaded {qualificationsUploaded.length} out of {props.qualifications.length}</Paragraph>
      <NavigationButton url="/" isDisabled={!isUploadComplete()}>
        Back To Home
      </NavigationButton>
    </Stack>
  )

}

export default BulkUploadInProgress;