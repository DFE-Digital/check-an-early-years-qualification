import { CMAClient } from "@contentful/app-sdk";
import { qualification } from "../models/qualification";
import { Dispatch, SetStateAction } from "react";

const createAndPublishEntries = async (cma: CMAClient, qualifications: qualification[], setUploaded: Dispatch<SetStateAction<boolean>>) => {

  const locale = "en-US";

  for (const qualification of qualifications) {
    const entryId = String(qualification.qualificationId);

    const entryProps = await cma.entry.createWithId(
      { entryId: entryId, contentTypeId: "Qualification" },
      {
        fields: {
          qualificationId: { [locale]: qualification.qualificationId },
          qualificationName: { [locale]: qualification.qualificationName },
          qualificationLevel: { [locale]: qualification.qualificationLevel },
          awardingOrganisationTitle: {
            [locale]: qualification.awardingOrganisationTitle,
          },
          fromWhichYear: { [locale]: qualification.fromWhichYear },
          qualificationNumber: {
            [locale]: qualification.qualificationNumber,
          },
          notesAdditionalRequirements: {
            [locale]: qualification.notesAdditionalRequirements,
          },
        },
      }
    );

    await cma.entry.publish({ entryId: entryId }, entryProps);
  }

  setUploaded(true);
};

export default createAndPublishEntries;