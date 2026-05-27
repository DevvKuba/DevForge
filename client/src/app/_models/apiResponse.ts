import { UserXpDetailDto } from "./dtos/userXpDetailDto";

export interface ApiResponse<T> {
  data: T | null,
  message: string | null,
  success: boolean | null,
  xpDetails: UserXpDetailDto | null
}