import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Blog } from '../_models/blog';
import { Observable } from 'rxjs';
import { setPaginationHeaders } from './paginationHelper';
import { HttpClient } from '@angular/common/http';
import { BlogComment } from '../_models/blogComment';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  baseUrl = environment.apiUrl
  http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Blog[]> | null>(null);

  gatherAllBlogs(pageNumber: number, pageSize: number) : Observable<any> {
    let params = setPaginationHeaders(pageNumber, pageSize);
    return this.http.get<Blog[]>(`${this.baseUrl}blogs/GatherAllBlogs`, {observe: 'response', params});
  }

  gatherSpecificUserBlogs(userId: number, pageNumber: number, pageSize: number) : Observable<any> {
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append("UserId", userId);

    return this.http.get<Blog[]>(`${this.baseUrl}blogs/GatherUserBlogs`, {observe: 'response', params});
  }

  updateBlogComment(newBlogComment: BlogComment) : Observable<any> {
    return this.http.put(this.baseUrl + 'blogs/UpdateBlogComment', newBlogComment);
  }

  updateBlogPost(newBlog: Blog) : Observable<any> {
    return this.http.put(this.baseUrl + 'blogs/UpdateBlogPost', newBlog);
  }

  addBlogLike(blog: Blog) : Observable<any> {
    return this.http.post(this.baseUrl + 'blogs/AddBlogLike', blog);
  }

  addBlogComment(newBlogComment: BlogComment) : Observable<any> {
    return this.http.post(this.baseUrl + 'blogs/AddBlogComment', newBlogComment);
  }

  addBlog(newBlog: Blog) : Observable<any> {
    return this.http.post(this.baseUrl + 'blogs/AddBlog', newBlog);
  }

  deleteBlog(blog: Blog) : Observable<any> {
    return this.http.delete(this.baseUrl + 'blogs/DeleteBlog', {body: blog});
  }

  undoBlogLike(blog: Blog) : Observable<any> {
    return this.http.delete(this.baseUrl + 'blogs/UndoBlogLike', {body: blog});
  }

  deleteBlogComment(blogComment: BlogComment) : Observable<any> {
    return this.http.delete(this.baseUrl + 'blogs/DeleteBlogComment', {body: blogComment});
  }
}
