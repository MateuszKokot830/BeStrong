import { Photo } from "./Photo";

export interface User {
    username: string;
    token: string;
}

export interface UserApp {
    id: number;
    userName: string;
    createdDate: Date;
    dateOfBirth: Date;
    name: string;
    surname: string;
    gender: string;
    city: string;
    country: string;
    description: string;
    age: number;
    profilePhotoUrl: string;
    photos: Photo[];
}


