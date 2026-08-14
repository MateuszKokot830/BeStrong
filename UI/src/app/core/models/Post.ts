import { Comment } from './Comment';
import { PostType } from './Enums';
import { Workout } from './Workout';

export interface Post {
  id: number;
  userId: number;
  type: PostType;
  description: string | null;
  createdDate: string;
  updatedDate: string | null;
  workoutId: number | null;
  workoutPlanId: number | null;
  workout: Workout | null;
  likesCount: number;
  comments: Comment[];
}

export interface PostCreate {
  type: PostType;
  description: string | null;
  workoutId: number | null;
  workoutPlan: number | null;
}

export interface PostUpdate {
  description: string | null;
}
