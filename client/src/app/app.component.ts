import { Component, inject, OnInit } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { NavComponent } from "./nav/nav.component";
import { AccountService } from './_services/account.service';
import { HomeComponent } from "./home/home.component";
import { NgxSpinnerComponent} from 'ngx-spinner';
import { MembersService } from './_services/members.service';
import { UserXpDetailDto } from './_models/dtos/userXpDetailDto';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [RouterOutlet, NavComponent, NgxSpinnerComponent],
  templateUrl: './app.component.html',
  styleUrl: './app.component.css'
})
export class AppComponent implements OnInit {
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  
  // start up upon app start / refresh  
  ngOnInit(): void {
  this.setCurrentUser();
  this.loadMostRecentXp();
  }

  setCurrentUser(){
    const userString = localStorage.getItem('user');
    if(!userString) return;
    const user = JSON.parse(userString);
    this.accountService.setCurrentUser(user);
  }

  loadMostRecentXp(){
    this.memberService.getMemberWithXp(this.accountService.currentUser()?.id ?? 0).subscribe({
      next: (response) => {
        if(!response.xpDetails) return;
        this.accountService.updateUserXpProperties(response.xpDetails!);
      }
    })
  }

}


