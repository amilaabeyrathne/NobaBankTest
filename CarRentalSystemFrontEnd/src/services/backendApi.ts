import { createApi, fetchBaseQuery } from '@reduxjs/toolkit/query/react';
import type { Car, Categorie, Reservation, ReservationResponse, Return, ReturnResponse } from './types';

export const backendApi = createApi({
    reducerPath: 'backendApi',
    baseQuery: fetchBaseQuery({
        baseUrl: import.meta.env.VITE_API_URL ?? '/api/',
    }),
    endpoints: (builder) => ({
        getCategories: builder.query<Categorie[], void>({
            query: () => 'categories',
        }),
        getCars: builder.query<Car[], { categoryId: number; page?: number; pageSize?: number }>({
            query: ({ categoryId, page = 1, pageSize = 10 }) => ({
                url: 'cars',
                params: { categoryId, page, pageSize },
            }),
        }),
        getCarById :builder.query<Car, { id: string }>({
            query: ({ id }) => ({
                url: `cars/${id}`,
            }),
        }),
        createReservation: builder.mutation<ReservationResponse, { reservation: Reservation }>({
            query: ({ reservation }) => ({
                url: 'reservation',
                method: 'POST',
                body: reservation,
            }),
        }),
        createReturn: builder.mutation<ReturnResponse, { returnData: Return }>({
            query: ({ returnData }) => ({
                url: 'reservation/return',
                method: 'POST',
                body: returnData,
            }),
        }),
    }),
});

export const { 
    useGetCategoriesQuery , 
    useGetCarsQuery, 
    useGetCarByIdQuery,
    useCreateReservationMutation,
    useCreateReturnMutation 
    } = backendApi;
