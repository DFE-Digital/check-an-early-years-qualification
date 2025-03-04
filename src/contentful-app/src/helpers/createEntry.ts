import { CMAClient } from "@contentful/app-sdk";
import { qualification } from "../models/qualification";

const createEntry = async (cma: CMAClient, qualification: qualification) => {
  const locale = "en-US";

  const entryId = String(qualification.qualificationId);

  return await cma.entry.createWithId(
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
}

export default createEntry;