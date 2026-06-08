import { Component, inject, Input, OnInit } from '@angular/core';
import { AccountService } from '../../_services/account.service';
import { QuizQuestion } from '../../_models/quizQuestion';

@Component({
  selector: 'app-ongoing-quiz',
  imports: [],
  templateUrl: './ongoing-quiz.component.html',
  styleUrl: './ongoing-quiz.component.css'
})
export class OngoingQuizComponent implements OnInit {
  accountService = inject(AccountService);
  currentUserId: number = 0;
  @Input() ongoingQuizQuestions: QuizQuestion[] | undefined;

  ngOnInit(): void {
    this.currentUserId = this.accountService.currentUser()?.id ?? 0;
  }
  // when the user starts the quiz, the actual questions are listed within a page format
  // user needs to progress through 
  // while they are working through the answers are being stored through QuizMarkingDto ? or perhaps just count right answers from total qs
  // we compare the two to determine a grade via a helper method
  // we then save the quiz and send a server call
  // then get redirected to the quizzes page
}
