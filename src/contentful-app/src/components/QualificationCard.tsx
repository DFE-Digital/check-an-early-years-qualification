import { useState } from "react";
import { qualification } from "../models/qualification";
import { Card, Flex, Paragraph } from "@contentful/f36-components";
import { ChevronDownIcon, ChevronUpIcon } from "@contentful/f36-icons";

interface QualificationCardProps {
  qualification: qualification;
}

interface CardChevronProps {
  show: boolean;
}

const CardChevron = (props: CardChevronProps) => {
  return props.show ? <ChevronDownIcon /> : <ChevronUpIcon />;
};

const QualificationCard = (props: QualificationCardProps) => {
  const [show, setShow] = useState(false);

  return (
    <Card onClick={() => setShow(!show)}>
      <Flex flexDirection="row" justifyContent="space-between">
        {props.qualification.qualificationName}
        <CardChevron show={show} />
      </Flex>
      {show && (
        <>
          <Paragraph>{props.qualification.awardingOrganisationTitle}</Paragraph>
          <Paragraph>{props.qualification.fromWhichYear}</Paragraph>
        </>
      )}
    </Card>
  );
};

export default QualificationCard;
