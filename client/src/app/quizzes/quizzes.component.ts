import { Component, inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { BsModalService, BsModalRef } from 'ngx-bootstrap/modal';
import { Quiz } from '../_models/quiz';
import { QuizCriteriaComponent } from '../modals/quiz-criteria/quiz-criteria.component';

@Component({
  selector: 'app-quizzes',
  standalone: true,
  imports: [CommonModule],
  templateUrl: './quizzes.component.html',
  styleUrl: './quizzes.component.css'
})
export class QuizzesComponent {
  private modalService = inject(BsModalService);
  bsModalRef?: BsModalRef<QuizCriteriaComponent>;

  completedQuizzes: Quiz[] = [];
  expandedQuizId: number | null = null;

  ngOnInit() {
    // TODO: Load completed quizzes from quiz service
  }

  openQuizCriteriaModal() {
    // TODO: Open quiz criteria modal and handle submission
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
