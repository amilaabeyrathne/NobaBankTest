import { createBrowserRouter } from "react-router-dom";
import App from '../../App';
import Categories from '../categories/Categories';
import Return from '../reservation/Return';
import Cars from '../cars/Cars';
import Reserve from "../reservation/Reserve";

export const router = createBrowserRouter([
  {
    path: '/',
    element: <App />,
    children: [
      {
        path: '/',
        element: <Categories />,
      },
      {
        path: '/reservation',
        element: <Return />,
      },
      {
        path: '/cars/:categoryId',
        element: <Cars />,
      },
      {
        path: '/reserve/:id',
        element: <Reserve />,
      },
    ],
  },
]);