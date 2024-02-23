import { Dispatch, SetStateAction } from "react";
import { qualification } from "../models/qualification";

const csvFileToQualifications = (rawText: string, callback: Dispatch<SetStateAction<qualification[]>>) => {
  const csvRows = rawText.split("\n");
  const array = csvRows.map((i) => {
    const values = i.split(",");
    const obj: qualification = {
      qualificationId: Number(values[0]),
      qualificationLevel: values[1],
      fromWhichYear: values[2],
      qualificationName: values[3],
      awardingOrganisationTitle: values[4],
      qualificationNumber: values[5],
      notesAdditionalRequirements: values[6],
    };

    return obj;
  });

  callback(array);
};

export default csvFileToQualifications;