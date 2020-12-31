import { User } from '../user/user.model';

export interface LoginResponse {
  user: User;
  accessToken: string;
  refreshToken: string;
}
