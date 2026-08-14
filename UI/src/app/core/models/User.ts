import { Gender } from './Enums';
import { Follower } from './Follower';
import { Photo } from './Photo';
import { Post } from './Post';

export interface User {
  id: number;
  userName: string;
  dateOfBirth: string;
  dateOfWorkoutStart: string | null;
  name: string | null;
  surname: string | null;
  gender: Gender;
  city: string | null;
  country: string | null;
  description: string | null;
  profilePhotoUrl: string | null;
  age: number;
  workoutSince: string | null;
  updatedDate: string | null;
  measurements: Measurements | null;
  photos: Photo[];
  posts: Post[];
  followedUsers: Follower[];
  followers: Follower[];
  isAdmin: boolean;
  workoutPlanId: number | null;
  workoutPlanName: string | null;
}

export interface Measurements {
  height: number | null;
  weight: number | null;
  chest: number | null;
  shoulders: number | null;
  arms: number | null;
  waist: number | null;
  hips: number | null;
  thights: number | null;
}

export interface UserUpdate {
  id: number;
  dateOfBirth: string;
  dateOfWorkoutStart: string | null;
  name: string | null;
  surname: string | null;
  gender: Gender;
  city: string | null;
  country: string | null;
  description: string | null;
  measurements: Measurements | null;
  photos: Photo[];
}
