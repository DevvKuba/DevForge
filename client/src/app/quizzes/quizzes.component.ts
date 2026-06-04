import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Quiz } from '../_models/quiz';
import { QuizQuestion } from '../_models/quizQuestion';
import { AccountService } from '../_services/account.service';
import { QuizService } from '../_services/quiz.service';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent {
  accountService = inject(AccountService);
  quizService = inject(QuizService);

  currentUserId: number = 0;
  completedQuizzes: Quiz[] = [];
  currentQuizQuestions: QuizQuestion[] = [];
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
    this.quizService.getAllUserQuizzes(this.currentUserId, this.pageNumber, this.pageSize).subscribe({
      next: (response) => {
        this.completedQuizzes = response.body;
        console.log(this.completedQuizzes);
      }
    })
  }

  openQuizCriteriaModal() {
    this.showQuizCriteriaDialog = true;
  }

  closeQuizCriteriaModal() {
    this.showQuizCriteriaDialog = false;
    this.resetQuizCriteria();
  }

  submitQuizCriteria() {
    if (this.isQuizCriteriaValid()) {
      // TODO: Call quiz service with this.quizCriteria to fetch questions from external API
      console.log('Quiz criteria submitted:', this.quizCriteria);
      this.closeQuizCriteriaModal();
    }
  }

  isQuizCriteriaValid(): boolean {
    // TODO: Implement validation logic
    return true;
  }

  isInvalidNumberOfQuestions(): boolean {
    // TODO: Implement validation for number of questions
    return false;
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
    // TODO: Return badge CSS class based on difficulty level
    return '';
  }

  getScoreColor(score: number): string {
    // TODO: Return text color class based on score
    return '';
  }
}
