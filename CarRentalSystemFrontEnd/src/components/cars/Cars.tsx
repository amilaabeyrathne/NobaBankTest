import { useParams, useSearchParams } from "react-router-dom";
import { useGetCarsQuery } from "../../services/backendApi";
import { Box, Typography, Button } from "@mui/material";
import { useEffect, useMemo, useState } from "react";
import CarCard from "./Car";

export default function Cars() {
  const { categoryId } = useParams<{ categoryId: string }>();
  const numericCategoryId = Number(categoryId);
  const [pageParams, setPageParams] = useSearchParams();
  const categoryName = pageParams.get("name") ?? "";
  
  const initialPage = useMemo(() => {
    const p = Number(pageParams.get("page"));
    return Number.isNaN(p) || p < 1 ? 1 : p;
  }, [pageParams]);

  const [page, setPage] = useState<number>(initialPage);
  const pageSize = 10;

  useEffect(() => {
    setPageParams(
      {
        page: page.toString(),
        ...(categoryName ? { name: categoryName } : {}),
      },
      { replace: true }
    );
  }, [page, setPageParams, categoryName]);

  const { data: cars = [], isLoading, error } = useGetCarsQuery(
    { categoryId: numericCategoryId, page, pageSize }
  );

  if (isLoading) return <div>Loading cars...</div>;
  if (error) return <div>Failed to load cars</div>;
  if (!cars.length) return <div>No cars found for this category</div>;

  return (
   
    <Box sx={{ maxWidth: 1080, mx: "auto", mt: 2 }}>
      <Typography variant="h5" gutterBottom sx={{ fontWeight: 700, letterSpacing: 0.2, mb: 2 }}>
        Cars for category {categoryName || numericCategoryId}  
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
        {cars.map((car) => (
          <CarCard key={car.id} car={car} />
        ))}
      </Box>
      <Box sx={{ display: "flex", alignItems: "center", justifyContent: "space-between", mt: 3 }}>
        <Button variant="outlined" disabled={page === 1} onClick={() => setPage((p) => Math.max(1, p - 1))}>
          Previous
        </Button>
        <Typography variant="body2">Page {page}</Typography>
        <Button
          variant="outlined"
          disabled={cars.length < pageSize}
          onClick={() => setPage((p) => p + 1)}
        >
          Next
        </Button>
      </Box>
    </Box>
  );
}
