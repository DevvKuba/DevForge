export interface User {
  id: number;
  username: string;
  knownAs: string;
  gender: string;
  appExperiencePoints: number,
  level: number,
  levelThreshold: number,
  token: string;
  photoUrl?: string; 
  roles: string[];
}
