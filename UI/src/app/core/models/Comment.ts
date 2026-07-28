export interface Comment {
  id: number;
  userId: number;
  description: string | null;
  createdDate: string;
  likesCount: number;
  postId: number;
}

export interface CommentCreate {
  description: string | null;
  postId: number;
}

export interface CommentUpdate {
  description: string;
}
