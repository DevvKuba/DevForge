import { HttpClient } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Quiz } from '../_models/quiz';
import { PaginatedResult } from '../_models/pagination';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  http = inject(HttpClient);
  baseUrl = environment.apiUrl;
}
