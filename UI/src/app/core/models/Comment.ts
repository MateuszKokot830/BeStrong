export interface Comment {
  id: number;
  userId: number;
  description: string;
  createdDate: Date;
  likes: number;
  postId: number;
}
