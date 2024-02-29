import {
  Stack,
  Heading,
  Pagination,
  ButtonGroup,
} from "@contentful/f36-components";
import { qualification } from "../../models/qualification";
import BackButton from "../../components/BackButton";
import { useLocation } from "react-router-dom";
import { useState } from "react";
import QualificationCard from "../../components/QualificationCard";
import NavigationButton from "../../components/NavigationButton";

interface ReviewBulkUploadProps {
  qualifications: qualification[];
}

const ReviewBulkUpload = () => {
  const location = useLocation();
  const props: ReviewBulkUploadProps = location.state;

  const [page, setPage] = useState(0);
  const [itemsPerPage, setItemsPerPage] = useState(5);
  const isLastPage = (page + 1) * itemsPerPage >= props.qualifications.length;

  const handleViewPerPageChange = (i: number) => {
    // Reset page to match item being shown on new View per page
    setPage(Math.floor((itemsPerPage * page + 1) / i));
    setItemsPerPage(i);
  };

  return (
    <Stack
      flexDirection="column"
      spacing="spacingS"
      style={{ width: "50%", marginLeft: "25%" }}
    >
      <Heading>Review Bulk Upload</Heading>
      {props.qualifications
        .slice(
          page === 0 ? 0 : page * itemsPerPage,
          page === 0 ? itemsPerPage : itemsPerPage * page + itemsPerPage
        )
        .map((qualification) => {
          return <QualificationCard qualification={qualification} />;
        })}
      <Pagination
        activePage={page}
        onPageChange={setPage}
        isLastPage={isLastPage}
        itemsPerPage={itemsPerPage}
        totalItems={props.qualifications.length}
        onViewPerPageChange={handleViewPerPageChange}
      />
      <ButtonGroup variant="spaced" spacing="spacingS">
        <BackButton />
        <NavigationButton url="/bulk-upload-in-progress" propsToPass={{ qualifications: props.qualifications }}>
          Publish
        </NavigationButton>
      </ButtonGroup>
    </Stack>
  );
};

export default ReviewBulkUpload;
