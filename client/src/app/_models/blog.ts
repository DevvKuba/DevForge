
export interface Blog {
  id: number,
  title: string,
  description: string,
  publishedAt: Date,
  updatedAt: Date,
  isDeleted: boolean,
  userId: number,
}