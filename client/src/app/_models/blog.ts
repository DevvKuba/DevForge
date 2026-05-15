import { BlogComment } from "./blogComment";
import { BlogLike } from "./blogLike";

export interface Blog {
  id: number,
  title: string,
  description: string,
  publishedAt: Date,
  updatedAt: Date,
  isDeleted: boolean,
  userId: number,
  interactingUserId: number | null,
  blogLikes: BlogLike[],
  blogComments: BlogComment[]
}