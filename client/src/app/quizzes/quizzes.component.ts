import { Component } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Quiz } from '../_models/quiz';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent {
  completedQuizzes: Quiz[] = [];
  expandedQuizId: number | null = null;
  showQuizCriteriaDialog = false;

  quizCriteria = {
    numberOfQuestions: 10,
    difficulty: 'medium',
    questionType: 'multiple'
  };

  ngOnInit() {
    // TODO: Load completed quizzes from quiz service
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
