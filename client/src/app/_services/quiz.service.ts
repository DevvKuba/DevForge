import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Quiz } from '../_models/quiz';
import { PaginatedResult } from '../_models/pagination';
import { setPaginationHeaders } from './paginationHelper';
import { Observable } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class QuizService {
  http = inject(HttpClient);
  baseUrl = environment.apiUrl;

  getAllUserQuizzes(userId: number, pageNumber: number, pageSize: number): Observable<any> {
    let params;
    params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('UserId', userId);

    return this.http.get<Quiz[]>(this.baseUrl + 'quiz/GetAllCompletedQuizzes', { observe: 'response', params });
  }
}
