import { Follower } from "./Follower";
import { Photo } from "./Photo";

export interface UserAuth {
    username: string;
    token: string;
}

export interface User {
    id: number;
    userName: string;
    dateOfBirth: Date;
    dateOfWorkoutStart: Date;
    name: string;
    surname: string;
    gender: Gender;
    city: string;
    country: string;
    description: string;
    age: number;
    workoutSince: string;
    profilePhotoUrl: string;
    measurements: Measurements;
    photos: Photo[];
    followedUsers: Follower[];
    followers: Follower[]
}

export interface Measurements {
  Height: number;
  Weight: number;
  Chest: number;
  Shoulders: number;
  Arms: number;
  Waist: number;
  Hips: number;
  Thights: number;
}

export enum Gender {
  male,
  female
}


