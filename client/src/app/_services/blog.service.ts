import { inject, Injectable, signal } from '@angular/core';
import { HttpClient } from '@microsoft/signalr';
import { environment } from '../../environments/environment';
import { PaginatedResult } from '../_models/pagination';
import { Blog } from '../_models/blog';

@Injectable({
  providedIn: 'root'
})
export class BlogService {
  baseUrl = environment.apiUrl
  http = inject(HttpClient);
  paginatedResult = signal<PaginatedResult<Blog[]> | null>(null);
}
