import { Stack, Heading } from "@contentful/f36-components";
import NavigationButton from "../../components/NavigationButton";

const Home = () => {
  return (
    <Stack flexDirection="column" spacing="spacingS">
      <Heading>Qualification Management Home</Heading>
      <NavigationButton url="/single-upload">Single Upload</NavigationButton>
      <NavigationButton url="/bulk-upload">Bulk Upload</NavigationButton>
      <NavigationButton url="/review-release">Review Releases</NavigationButton>
    </Stack>
  );
};

export default Home;
