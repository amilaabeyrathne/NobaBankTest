import { Button, Card, CardContent, CardMedia, Typography } from "@mui/material";
import { Link } from "react-router-dom";
import type { Car as CarType } from "../../services/types";

type CarProps = {
  car: CarType;
};

export default function Car({ car }: CarProps) {
  return (
    <Card
      variant="outlined"
      sx={{
        transition: "transform 120ms ease, box-shadow 120ms ease",
        "&:hover": { transform: "translateY(-2px)", boxShadow: 3 },
      }}
    >
      <CardMedia
        component="img"
        height="140"
        image="https://images.unsplash.com/photo-1503376780353-7e6692767b70?w=400&h=300&fit=crop"
        alt={`${car.brand} ${car.model}`}
        sx={{ objectFit: "cover" }}
      />
      
      <CardContent sx={{ py: 1.5, px: 2 }}>
        <Typography variant="subtitle1" fontWeight={700}>
          {car.brand} {car.model}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Reg: {car.registrationNumber}
        </Typography>
        <Typography variant="body2" color="text.secondary">
          Mileage: {car.milage}
        </Typography>
        <Typography variant="body2" color="text.secondary" sx={{ mb: 1.5 }}>
          Colour: {car.colour}
        </Typography>
        <Button
          variant="contained"
          color="primary"
          fullWidth
          component={Link}
          to={`/reserve/${car.id}`}
        >
          Reserve
        </Button>
      </CardContent>
    </Card>
  );
}
