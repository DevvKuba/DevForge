import { Component, effect, inject, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Quiz } from '../_models/quiz';
import { QuizQuestion } from '../_models/quizQuestion';
import { AccountService } from '../_services/account.service';
import { QuizService } from '../_services/quiz.service';
import { OngoingQuizComponent } from "./ongoing-quiz/ongoing-quiz.component";

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule, FormsModule, OngoingQuizComponent],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent {
  accountService = inject(AccountService);
  quizService = inject(QuizService);

  currentUserId: number = 0;
  completedQuizzes: Quiz[] = [];
  currentQuizQuestions : QuizQuestion[] = [];
  expandedQuizId: number | null = null;
  showQuizCriteriaDialog = false;
  pageNumber = 1;
  pageSize = 10;

  quizCriteria = {
    numberOfQuestions: 10,
    difficulty: 'medium',
    questionType: 'multiple'
  };

  ngOnInit() {
    this.currentUserId = this.accountService.currentUser()?.id ?? 0;
    this.gatherUserQuizzes();
  }

  onQuizCompleted(){
    this.gatherUserQuizzes();
    this.currentQuizQuestions = [];
  }

  gatherUserQuizzes(){
     this.quizService.getAllUserQuizzes(this.currentUserId, this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.completedQuizzes = response.body;
      }
    })
  }

  openQuizCriteriaModal() {
    this.resetQuizCriteria();
    this.showQuizCriteriaDialog = true;
  }

  closeQuizCriteriaModal() {
    this.showQuizCriteriaDialog = false;
  }

  submitQuizCriteria() {
    if (this.isQuizCriteriaValid()) {
      this.quizService.getQuizQuestions
        (this.quizCriteria.numberOfQuestions, this.quizCriteria.difficulty, this.quizCriteria.questionType).subscribe({
          next: (response) => {
            console.log(response);
            this.currentQuizQuestions = response;
          }
        })
    }
    this.closeQuizCriteriaModal();
  }

  isQuizCriteriaValid(): boolean {
    if (this.quizCriteria.numberOfQuestions < 5 || this.quizCriteria.numberOfQuestions > 50) return false;

    if (this.quizCriteria.difficulty != "easy" && this.quizCriteria.difficulty != "medium" && this.quizCriteria.difficulty != "hard") return false;

    if (this.quizCriteria.questionType != "multiple" && this.quizCriteria.questionType != "boolean") return false;

    return true;
  }

  resetQuizCriteria() {
    this.quizCriteria = {
      numberOfQuestions: 10,
      difficulty: 'medium',
      questionType: 'multiple'
    };
  }

  toggleQuizExpansion(quizId: number) {
    this.expandedQuizId = this.expandedQuizId === quizId ? null : quizId;
  }

  isQuizExpanded(quizId: number): boolean {
    return this.expandedQuizId === quizId;
  }

  getDifficultyClass(difficulty: string): string {
    const map: Record<string, string> = {
      easy: 'success',
      medium: 'warning',
      hard: 'danger'
    };
    return map[difficulty.toLowerCase()] ?? 'secondary';
  }

  getScoreColor(score: number): string {
    if (score >= 70) return 'text-success';
    if (score >= 50) return 'text-warning';
    return 'text-danger';
  }

  deleteQuiz(quizId: number): void {
    this.quizService.deletePastQuiz(quizId).subscribe({
      next: () => {
        this.gatherUserQuizzes();
      }
    })
  }
}
