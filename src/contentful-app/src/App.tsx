import Home from './locations/Home/Home';
import { RouterProvider, createMemoryRouter, } from 'react-router-dom';
import BulkUpload from './locations/Bulk Upload/BulkUpload';
import { Card } from '@contentful/f36-components';
import ReviewRelease from './locations/Review Release/ReviewRelease';
import SingleUpload from './locations/Single Upload/SingleUpload';
import ReviewBulkUpload from './locations/Review Bulk Upload/ReviewBulkUpload';
import BulkUploadInProgress from './locations/Bulk Upload In Progress/BulkUploadInProgress';

const router = createMemoryRouter([
  {
    path: "/",
    element: <Home />,
  },
  {
    path: "/bulk-upload",
    element: <BulkUpload />
  },
  {
    path: "/single-upload",
    element: <SingleUpload />
  },
  {
    path: "/review-release",
    element: <ReviewRelease />
  },
  {
    path: "/review-bulk-upload",
    element: <ReviewBulkUpload />
  },
  {
    path: "/bulk-upload-in-progress",
    element: <BulkUploadInProgress />
  }
]);

const App = () => {
  return (
    <Card style={{height: "100vh"}}>
        <RouterProvider router={router} />
    </Card>
  )
};

export default App;
