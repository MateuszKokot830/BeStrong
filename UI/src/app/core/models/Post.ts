import { Comment } from "./Comment";

export interface Post {
  id: number;
  userId: number;
  description: string;
  likes: number;
  comments: Comment[];
}
