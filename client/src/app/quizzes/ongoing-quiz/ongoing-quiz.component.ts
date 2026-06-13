import { Component, EventEmitter, inject, Input, OnInit, Output } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { QuizService } from '../../_services/quiz.service';
import { QuizQuestion } from '../../_models/quizQuestion';
import { Quiz } from '../../_models/quiz';
import { ToastrService } from 'ngx-toastr';

@Component({
  selector: 'app-ongoing-quiz',
  imports: [],
  templateUrl: './ongoing-quiz.component.html',
  styleUrl: './ongoing-quiz.component.css'
})
export class OngoingQuizComponent implements OnInit {
  accountService = inject(AccountService);
  quizService = inject(QuizService);
  toastr = inject(ToastrService);

  currentUserId: number = 0;
  currentIndex: number = 0;
  shuffledOptions: string[][] = [];
  CurrentQuiz: Quiz | null = null;

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
    const possibleAnswers: string[] = [];

    question.incorrect_answers.forEach(a => possibleAnswers.push(a));

    possibleAnswers.push(question.correct_answer);
    return this.shuffle(possibleAnswers);
  }

  selectAnswer(answer: string): void {
    this.ongoingQuizQuestions[this.currentIndex].selectedAnswer = answer;

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
    const areAllQuestionsAnswered = this.ongoingQuizQuestions.every(x => x.selectedAnswer != null);

    if(!areAllQuestionsAnswered) return false;
    
    return true;
    }

  calculateFinalScore(): number {
    let correctCount: number = 0;

    for (let index = 0; index < this.ongoingQuizQuestions.length; index++) {
      const questionAnswer = this.ongoingQuizQuestions[index].selectedAnswer;
      const correctQuestionAnswer = this.ongoingQuizQuestions[index].correct_answer;

      if(questionAnswer == correctQuestionAnswer){
        correctCount++;
      }
    }
    const percentageScore = Math.round(correctCount / this.ongoingQuizQuestions!.length  * 100);

    return percentageScore;
    }

  closeQuiz(): void {
    this.quizCompleted.emit();
  }

  beginQuiz(): void {

    if(this.quizDifficulty == undefined){
      this.toastr.error("Valid difficulty must be selected");
      return;
    }
    
    const startedQuiz: Quiz =  {
      id: null,
      difficulty: this.quizDifficulty ?? "",
      questions: this.ongoingQuizQuestions,
      percentageScore: 0,
      isComplete: true,
      userId: this.currentUserId
    }

    this.quizService.saveStartedQuiz(startedQuiz).subscribe({
      next: (response) => {
        this.CurrentQuiz = response;
      }
    });
  }

  submitQuiz(): void {
    if(this.quizDifficulty == undefined){
      this.toastr.error("Valid difficulty must be selected");
      return;
    }

    const quiz: Quiz =  {
      id: null,
      difficulty: this.quizDifficulty ?? "",
      questions: this.ongoingQuizQuestions,
      percentageScore: this.calculateFinalScore(),
      isComplete: true,
      userId: this.currentUserId
    }
    this.quizService.saveCompletedQuiz(quiz).subscribe({
      next: (response) => {
        this.accountService.updateUserXpProperties(response.xpDetails!);
        this.quizCompleted.emit();
      },
      error: () => {
        this.toastr.error("Quiz has not been saved");
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

