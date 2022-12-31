export interface Comment {
  id: number;
  userId: number;
  description: string;
  isProfilePhoto: boolean;
  likes: number;
  postId: number;
}
