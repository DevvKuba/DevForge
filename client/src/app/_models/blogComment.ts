export interface BlogComment {
  id: number,
  createdAt: Date,
  updatedAt: Date,
  content: string,
  blogId: number
}