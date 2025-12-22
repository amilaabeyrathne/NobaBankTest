export type Categorie = {
    id: number;
    name: string;
};

export type Car = {
    id: string;
    registrationNumber: string;
    categoryId: number;
    milage: number;
    brand: string;
    model: string;
    colour: string;
}

export type Reservation = {
    carId: string;
    customerSocialSecurityNumber: string;
    pickupMeterReading: number;
}

export type ReservationResponse = {
    id: string;
}

export type Return = {
    bookingNumber: string;
    returnMeterReading: number;
}

export type ReturnResponse = {
    rentalAmount: number;
}