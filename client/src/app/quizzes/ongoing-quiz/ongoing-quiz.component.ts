import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { QuizService } from '../../_services/quiz.service';
import { QuizQuestion } from '../../_models/quizQuestion';
import { Quiz } from '../../_models/quiz';

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

  @Input() ongoingQuizQuestions: QuizQuestion[] = [];
  @Input() quizDifficulty: string | undefined;
  @Output() quizCompleted = new EventEmitter<void>();

  ngOnInit(): void {
    this.currentUserId = this.accountService.currentUser()?.id ?? 0;

    if (this.ongoingQuizQuestions && this.ongoingQuizQuestions.length > 0) {
      for (let index = 0; index < this.ongoingQuizQuestions.length; index++) {
        const question = this.ongoingQuizQuestions[index];

        this.shuffledOptions[index] = this.buildShuffledOptions(question);
      }
    }
  }

  buildShuffledOptions(question: QuizQuestion): string[] {
    const possibleAnswers: string[] = question.incorrect_answers;

    possibleAnswers.push(question.correct_answer);
    return this.shuffle(possibleAnswers);
  }

  selectAnswer(answer: string): void {
    this.userAnswers[this.currentIndex] = answer;
  }

  goNext(): void {
    this.currentIndex++;
  }

  goPrevious(): void {
    this.currentIndex--;
  }

  isLastQuestion(): boolean {
    if(this.currentIndex + 1 == this.ongoingQuizQuestions?.length) return true;
     return false; 
    }

  allAnswered(): boolean {
    if(!this.ongoingQuizQuestions || this.userAnswers.length !== this.ongoingQuizQuestions.length) return false;
    
    for (let index = 0; index < this.userAnswers.length; index++) {
      const questionAnswer = this.userAnswers[index];
      if(questionAnswer == null) return false;
    }
    return true;
    }

  calculateFinalScore(): number {
    let score: number = 0;

    for (let index = 0; index < this.userAnswers.length; index++) {
      const questionAnswer = this.userAnswers[index];
      const correspondingQuestion = this.ongoingQuizQuestions![index];

      if(questionAnswer == correspondingQuestion.correct_answer){
        score++;
      }
    }
    const percentageScore = Math.round(score / this.ongoingQuizQuestions!.length  * 100);

    return percentageScore;
    }

  submitQuiz(): void {
    if(this.quizDifficulty == undefined){
      // error toast
      return;
    }

    const quiz: Quiz =  {
      difficulty: this.quizDifficulty ?? "",
      questions: this.ongoingQuizQuestions,
      percentageScore: this.calculateFinalScore(),
      userId: this.currentUserId
    }
    this.quizService.saveCompletedQuiz(quiz).subscribe({
      next: (response) => {
        this.ongoingQuizQuestions = []; // resets questions and closes sub-component
        this.accountService.updateUserXpProperties(response.xpDetails!);
      },
      error: (response) => {
        // error toast
      }
    })
  }

  shuffle<T>(array: T[]): T[] {
    for (let i = array.length - 1; i > 0; i--) {
      const j = Math.floor(Math.random() * (i + 1));
      [array[i], array[j]] = [array[j], array[i]];
    }
    return array;
  }
}
