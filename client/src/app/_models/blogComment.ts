export interface BlogComment {
  id: number,
  createdAt: Date,
  updatedAt: Date,
  content: string,
  userId: number,
  blogId: number
}