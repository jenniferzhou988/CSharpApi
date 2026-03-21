export interface LoginRequest {
  email: string;
  password: string;
}

export interface RegisterRequest {
  email: string;
  password: string;
  fullName?: string;
  firstName: string;
  lastName: string;
  orgId?: number;
}

export interface AuthResponse {
  userId: number;
  email: string;
  accessToken: string;
  refreshToken: string;
  expires: string;
}

export interface AuthState {
  userId: number;
  email: string;
  accessToken: string;
  refreshToken: string;
  expires: string;
  roles: string[];
}
