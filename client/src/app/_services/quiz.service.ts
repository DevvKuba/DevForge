import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Quiz } from '../_models/quiz';
import { PaginatedResult } from '../_models/pagination';
import { setPaginationHeaders } from './paginationHelper';
import { Observable } from 'rxjs';
import { QuizQuestion } from '../_models/quizQuestion';

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

  getQuizQuestions(numberOfQuestions: number, difficulty: string, questionType: string) : Observable<any> {
    const info = 
    {
      numberOfQuestions: numberOfQuestions,
      difficulty: difficulty,
      questionType: questionType
    };

    return this.http.get<QuizQuestion[]>(this.baseUrl + 'quiz/GetComputerScienceQuestions', {params: info});
  }
}
