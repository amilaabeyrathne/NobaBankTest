import { useParams } from "react-router-dom";
import { useCreateReservationMutation, useGetCarByIdQuery } from "../../services/backendApi";
import type { Reservation } from "../../services/types";
import { toast } from 'react-toastify';
import {
  Box,
  Typography,
  Card,
  CardMedia,
  Button,
  Divider,
  Stack,
  TextField,
} from "@mui/material";
import React, { useState } from "react";


export default function Reserve() {
  const { id } = useParams<{ id: string }>();
  const { data: car, isLoading, error } = useGetCarByIdQuery({ id: id ?? '' });
  const [createReservation, { isLoading: isCreating }] = useCreateReservationMutation();
  const [ssn, setSsn] = useState("");
  const [reservationId, setReservationId] = useState<string | null>(null);
  
  const swedishSSNRegex = /^(\d{4}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01])|\d{2}(0[1-9]|1[0-2])(0[1-9]|[12][0-9]|3[01]))[-]?\d{4}$/;
  const isSSNValid = ssn ? swedishSSNRegex.test(ssn) : false;

  if (!id) return <div>Invalid car ID</div>;
  if (isLoading) return <div>Loading...</div>;
  if (error) return <div>Error loading car</div>;
  if (!car) return <div>No car found</div>;

  const handleSubmit = async (event: React.FormEvent) => {
    event.preventDefault();
    const reservation: Reservation = {
      carId: id!,
      customerSocialSecurityNumber: ssn,
      pickupMeterReading: car.milage,
    };

    try {
      const createdReservationId = await createReservation({ reservation }).unwrap();
      console.log("Created reservation ID:", createdReservationId);
      const reservationIdValue = createdReservationId.id;
      setReservationId(reservationIdValue);
    } catch (error) {
      console.error(error);
      toast.error('Failed to create reservation. Please try again.', {
        position: 'top-right',
        autoClose: 5000,
        hideProgressBar: false,
        closeOnClick: true,
        pauseOnHover: true,
        draggable: true,
      });
    }
  };
    
  return (
    <Box
      sx={{
        maxWidth: 'lg',
        mx: 'auto',
        mt: 4,
        px: 2,
      }}
    >
      <Box
        sx={{
          display: 'grid',
          gap: 4,
          gridTemplateColumns: {
            xs: 'repeat(1, minmax(0, 1fr))',
            md: 'repeat(2, minmax(0, 1fr))',
          },
        }}
      >
        <Card sx={{ height: 'fit-content' }}>
          <CardMedia
            component="img"
            image="https://images.unsplash.com/photo-1503376780353-7e6692767b70?w=800&h=600&fit=crop"
            alt={`${car.brand} ${car.model}`}
            sx={{
              width: '100%',
              height: 'auto',
              objectFit: 'cover',
            }}
          />
        </Card>

        <Box sx={{ display: 'flex', flexDirection: 'column', gap: 3 }}>
        
          <Typography variant="h3" sx={{ fontWeight: 700, mb: 1 }}>
            {car.brand} {car.model}
          </Typography>

          <Stack spacing={1.5}>
            <Box>
              <Typography variant="body2" color="text.secondary">
                Registration Number
              </Typography>
              <Typography variant="h6">{car.registrationNumber}</Typography>
            </Box>
            <Divider />
            <Box>
              <Typography variant="body2" color="text.secondary">
                Mileage
              </Typography>
              <Typography variant="h6">{car.milage.toLocaleString()} km</Typography>
            </Box>
           
          </Stack>

          <Divider sx={{ my: 2 }} />

          <Box component="form" onSubmit={handleSubmit}>
            <Typography variant="h6" sx={{ mb: 2, fontWeight: 600 }}>
              Reservation Details
            </Typography>
            <Stack spacing={2}>
              <TextField
                label="SSN (Swedish Personnummer)"
                type="text"
                value={ssn}
                onChange={(e) => setSsn(e.target.value)}
                fullWidth
                required
                disabled={!!reservationId}
                error={!!ssn && !isSSNValid}
                helperText={ssn && !isSSNValid ? "Invalid format. Use YYYYMMDD-XXXX or YYMMDD-XXXX" : "Format: YYYYMMDD-XXXX or YYMMDD-XXXX"}
                placeholder="YYYYMMDD-XXXX"
              />
              
              <Button
                type="submit"
                variant="contained"
                color="primary"
                size="large"
                fullWidth
                sx={{ mt: 2, py: 1.5 }}
                disabled={!ssn || !isSSNValid || isCreating || !!reservationId}
              >
                {isCreating ? 'RESERVING...' : 'RESERVE CAR'}
              </Button>

              {reservationId && (
                <Box
                  sx={{
                    mt: 2,
                    p: 2,
                    bgcolor: 'success.light',
                    borderRadius: 1,
                    border: '1px solid',
                    borderColor: 'success.main',
                  }}
                >
                  <Typography variant="body2" color="text.secondary" sx={{ mb: 0.5 }}>
                    Reservation ID:
                  </Typography>
                  <Typography variant="h6" sx={{ fontWeight: 600, fontFamily: 'monospace' }}>
                    {reservationId}
                  </Typography>
                </Box>
              )}
            </Stack>
          </Box>
        </Box>
      </Box>
    </Box>
  );
}


