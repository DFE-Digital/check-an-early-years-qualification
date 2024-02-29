import { Button } from "@contentful/f36-components";
import { ChevronRightIcon } from "@contentful/f36-icons";
import { Link } from "react-router-dom";

interface NavigationButtonProps {
  url: string;
  isDisabled?: boolean;
  children?: React.ReactNode;
  propsToPass?: any;
}

const NavigationButton = (props: NavigationButtonProps) => {
  return (
    <Link to={props.url} state={props.propsToPass}>
      <Button
        variant="primary"
        isDisabled={props.isDisabled}
        endIcon={<ChevronRightIcon />}
      >
        {props.children}
      </Button>
    </Link>
  );
};

export default NavigationButton;
