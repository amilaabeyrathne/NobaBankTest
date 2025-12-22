import {Container } from "@mui/material"
import { Outlet } from "react-router-dom";
import Navbar from "./Layout/Navbar";
import { ToastContainer } from 'react-toastify';

function App() {
  

  return (
    <>
      <Navbar />
      <Container maxWidth="xl" sx={{ mt: 14, mb: 4 }}>
        <Outlet />
      </Container>
      <ToastContainer />
   </>
  )
}

export default App
