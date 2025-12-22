import { useGetCategoriesQuery } from "../../services/backendApi";
import { Box, Card, CardActionArea, CardContent, Typography } from "@mui/material";
import DirectionsCarRoundedIcon from "@mui/icons-material/DirectionsCarRounded";
import { Link } from "react-router-dom";


export default function Categories() {
  const { data: categories = [], isLoading, error } = useGetCategoriesQuery();

  if (isLoading) return <div>Loading categories...</div>;
  if (error) return <div>Failed to load categories</div>;
  if (!categories.length) return <div>No categories found</div>;

  return (
    <Box sx={{ maxWidth: 960, mx: "auto", mt: 2 }}>
      <Typography
        variant="h5"
        gutterBottom
        align="left"
        sx={{ fontWeight: 700, letterSpacing: 0.2, mb: 2 }}
      >
        Categories
      </Typography>
      <Box
        sx={{
          display: "grid",
          gap: 2,
          gridTemplateColumns: {
            xs: "repeat(1, minmax(0, 1fr))",
            sm: "repeat(2, minmax(0, 1fr))",
            md: "repeat(3, minmax(0, 1fr))",
          },
        }}
      >
        {categories.map((category) => {
          return (
            <Card
              variant="outlined"
              key={category.id}
              sx={{
                transition: "transform 120ms ease, box-shadow 120ms ease",
                "&:hover": {
                  transform: "translateY(-2px)",
                  boxShadow: 3,
                },
              }}
            >
              <CardActionArea
                component={Link}
                to={`/cars/${category.id}?name=${encodeURIComponent(category.name)}`}
              >
                <CardContent sx={{ py: 1.5, px: 2 }}>
                  <Box
                    sx={{
                      display: "flex",
                      alignItems: "center",
                      justifyContent: "space-between",
                      gap: 1.25,
                    }}
                  >
                    <Typography variant="subtitle1" fontWeight={600}>
                      {category.name}
                    </Typography>
                    <DirectionsCarRoundedIcon color="primary" fontSize="small" />
                  </Box>
                </CardContent>
              </CardActionArea>
            </Card>
          );
        })}
      </Box>
    </Box>
  );
}
