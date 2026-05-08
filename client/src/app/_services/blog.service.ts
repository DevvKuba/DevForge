import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Blog } from '../_models/blog';
import { Observable } from 'rxjs';
import { setPaginationHeaders } from './paginationHelper';
import { HttpClient } from '@angular/common/http';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  baseUrl = environment.apiUrl
  http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Blog[]> | null>(null);

  gatherAllBlogs(pageNumber: number, pageSize: number) : Observable<any> {
    let params = setPaginationHeaders(pageNumber, pageSize);

    return this.http.get<Blog[]>(`${this.baseUrl}blogs?`, {observe: 'response', params});
  }
}
