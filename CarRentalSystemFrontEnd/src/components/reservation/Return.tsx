import { useState } from "react";
import type { Return } from "../../services/types";
import { useCreateReturnMutation } from "../../services/backendApi";
import { toast } from 'react-toastify';
import {
  Box,
  Typography,
  TextField,
  Button,
  Stack,
  Card,
  CardContent,
} from "@mui/material";

export default function Return() {
  const [bookingNumber, setBookingNumber] = useState("");
  const [returnMeterReading, setReturnMeterReading] = useState<string>("");
  const [createReturn, { isLoading: isReturning }] = useCreateReturnMutation();
  const [rentalAmount, setRentalAmount] = useState(0);

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const parsedReturnMeterReading = Number(returnMeterReading);

    const returnData: Return = {
      bookingNumber: bookingNumber.trim(),
      returnMeterReading: parsedReturnMeterReading,
    };

    try {
      const response = await createReturn({ returnData }).unwrap();
      console.log("Return response:", response);

      const rentalAmountValue = response.rentalAmount ;

      if (rentalAmountValue === undefined || rentalAmountValue === null || typeof rentalAmountValue !== 'number') {
        console.error("RentalAmount not found or invalid in response:", response);
        console.error("Response keys:", Object.keys(response));
        throw new Error("Invalid rental amount received from server");
      }
      setRentalAmount(rentalAmountValue);
    } catch (error: unknown) {
      console.error("Error returning car:", error);

      type RtkError = { data?: { message?: string; error?: string } | string; error?: string };
      let errorMessage = "Failed to return car. Please try again.";

      if (typeof error === "object" && error !== null) {
        const err = error as RtkError;
        const data = err.data;
        if (typeof data === "string" && data.trim()) {
          errorMessage = data;
        } else if (typeof data === "object" && data !== null) {
          errorMessage = data.message ?? data.error ?? errorMessage;
        } else if (typeof err.error === "string" && err.error.trim()) {
          errorMessage = err.error;
        }
      } else if (error instanceof Error && error.message) {
        errorMessage = error.message;
      }

      toast.error(errorMessage, {
        position: 'top-right',
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
      });
    }
  };

  return (
    <Box
      sx={{
        maxWidth: 'md',
        mx: 'auto',
        mt: 4,
        px: 2,
      }}
    >
      <Typography variant="h4" sx={{ fontWeight: 700, mb: 4, textAlign: 'center' }}>
        Return a Car
      </Typography>

      <Card variant="outlined" sx={{ maxWidth: 600, mx: 'auto' }}>
        <CardContent sx={{ p: 4 }}>
          <Box component="form" onSubmit={handleSubmit}>
            <Stack spacing={3}>
              <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
                Enter Booking Details
              </Typography>

              <TextField
                label="Booking Number"
                type="text"
                value={bookingNumber}
                onChange={(e) => setBookingNumber(e.target.value)}
                fullWidth
                required
                disabled={rentalAmount > 0}
                placeholder="Enter your reservation ID"
                helperText="Please enter the reservation ID you received when booking the car"
              />

              <TextField
                label="Return Meter Reading"
                type="text"
                value={returnMeterReading}
                onChange={(e) => {
                  const next = e.target.value;
                  if (/^\d*$/.test(next)) {
                    setReturnMeterReading(next);
                  }
                }}
                inputProps={{ min: 0, inputMode: "numeric", pattern: "[0-9]*" }}
                fullWidth
                required
                disabled={rentalAmount > 0}
                placeholder="Enter the current mileage"
                helperText="Please enter the current mileage reading from the car's odometer"
              />

              {rentalAmount>0 && (
                <Box>
                  <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
                    Rental Amount
                  </Typography>
                  <Typography variant="h6" sx={{ fontWeight: 600, mb: 1 }}>
                    {rentalAmount}
                  </Typography>
                </Box>
              )}

              <Button
                type="submit"
                variant="contained"
                color="primary"
                size="large"
                fullWidth
                sx={{ mt: 2, py: 1.5 }}
                disabled={
                  !bookingNumber.trim() ||
                  returnMeterReading.trim() === "" ||
                  Number.isNaN(Number(returnMeterReading)) ||
                  Number(returnMeterReading) < 0 ||
                  isReturning ||
                  rentalAmount > 0
                }
              >
                {isReturning ? 'RETURNING...' : 'RETURN CAR'}
              </Button>
            </Stack>
          </Box>
        </CardContent>
      </Card>
    </Box>
  );
}
