import { Button } from "@contentful/f36-components";
import { useNavigate } from "react-router-dom";
import { ChevronLeftIcon } from '@contentful/f36-icons';

const BackButton = () => {
  let navigate = useNavigate();

  return (
    <Button variant="negative" onClick={() => navigate(-1)} startIcon={<ChevronLeftIcon />} >
      Back
    </Button>
  )
}

export default BackButton;