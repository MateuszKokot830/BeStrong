import { Comment } from "./Comment";

export interface Post {
  id: number;
  userId: number;
  description: string;
  createdDate: Date;
  photoId: number;
  workoutId: number;
  likes: number;
  comments: Comment[];
}
