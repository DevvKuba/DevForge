import { HttpClient, HttpParams } from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Quiz } from '../_models/quiz';
import { PaginatedResult } from '../_models/pagination';
import { setPaginationHeaders } from './paginationHelper';
import { Observable } from 'rxjs';
import { QuizQuestion } from '../_models/quizQuestion';
import { ApiResponse } from '../_models/apiResponse';

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

  getIncompleteUserQuiz(userId: number) : Observable<ApiResponse<Quiz | null>> {
    return this.http.get<any>(this.baseUrl + `quiz/GetInCompleteUserQuiz?userId=${userId}`);
  }

  getQuizQuestions(numberOfQuestions: number, difficulty: string, questionType: string): Observable<any> {
    const info =
    {
      numberOfQuestions: numberOfQuestions,
      difficulty: difficulty,
      questionType: questionType
    };

    return this.http.get<QuizQuestion[]>(this.baseUrl + 'quiz/GetComputerScienceQuestions', { params: info });
  }

  saveStartedQuiz(quiz: Quiz): Observable<ApiResponse<any>> {
    return this.http.post<ApiResponse<any>>(this.baseUrl + 'quiz/SaveStartedQuiz', quiz);
  }

  saveCompletedQuiz(quiz: Quiz): Observable<ApiResponse<any>> {
    return this.http.put<ApiResponse<any>>(this.baseUrl + 'quiz/SaveCompletedQuiz', quiz);
  }

  deletePastQuiz(quizId: number): Observable<any> {
    return this.http.delete<any>(this.baseUrl + `quiz/DeleteExistingQuiz?quizId=${quizId}`);
  }
}
