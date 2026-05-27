import { AfterViewChecked, Component, inject, input, OnInit, ViewChild } from '@angular/core';
import { MessageService } from '../../_services/message.service';
import { TimeagoModule } from 'ngx-timeago';
import { FormsModule, NgForm } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { AccountService } from '../../_services/account.service';

@Component({
  selector: 'app-member-messages',
  imports: [TimeagoModule, FormsModule],
  templateUrl: './member-messages.component.html',
  styleUrl: './member-messages.component.css'
})
export class MemberMessagesComponent implements AfterViewChecked,OnInit {
  @ViewChild('messageForm') messageForm?: NgForm;
  @ViewChild('scrollMe') scrollContainer?: any;
  messageService = inject(MessageService);
  accountService = inject(AccountService);

  route = inject(ActivatedRoute);
  userId: number = 0;
  messageContent = '';
  loading = false;

  ngOnInit(): void {
    this.route.paramMap.subscribe({
      next: (params) => {
        this.userId = Number.parseInt(params.get('id') || '0');
      }
    })

  }

  // updates view when scrolling
  ngAfterViewChecked(): void {
    this.scrollToBottom();
  }

  sendMessage(){
    this.loading = true;
    this.messageService.sendMessage(this.userId, this.messageContent).then((response) => {
      this.messageForm?.reset();
      this.scrollToBottom();
      this.accountService.updateUserXpProperties(response.xpDetails!);
    }).finally(() => this.loading = false);
  }

  private scrollToBottom(){
    if(this.scrollContainer){
      this.scrollContainer.nativeElement.scrollTop = this.scrollContainer.nativeElement.scrollHeight;
    }
  }

}
