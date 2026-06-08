import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { QuizService } from '../../_services/quiz.service';
import { QuizQuestion } from '../../_models/quizQuestion';

@Component({
  selector: 'app-ongoing-quiz',
  imports: [],
  templateUrl: './ongoing-quiz.component.html',
  styleUrl: './ongoing-quiz.component.css'
})
export class OngoingQuizComponent implements OnInit {
  accountService = inject(AccountService);
  quizService = inject(QuizService);

  currentUserId: number = 0;
  currentIndex: number = 0;
  userAnswers: (string | null)[] = [];
  shuffledOptions: string[][] = [];

  @Input() ongoingQuizQuestions: QuizQuestion[] | undefined;
  @Input() quizDifficulty: string | undefined;
  @Output() quizCompleted = new EventEmitter<void>();

  ngOnInit(): void {
    this.currentUserId = this.accountService.currentUser()?.id ?? 0;
  }

  buildShuffledOptions(question: QuizQuestion): string[] { return []; }

  selectAnswer(answer: string): void {}

  goNext(): void {}

  goPrevious(): void {}

  isLastQuestion(): boolean { return false; }

  allAnswered(): boolean { return false; }

  calculateScore(): number { return 0; }

  submitQuiz(): void {}
}
