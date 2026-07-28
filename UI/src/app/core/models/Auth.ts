export interface UserAuth {
  username: string | null;
  token: string | null;
}

export interface LoginRequest {
  userName: string;
  password: string;
}

export interface RegisterRequest {
  userName: string;
  password: string;
}
