import { Component, inject } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { AccountService } from '../_services/account.service';
import { BsDropdownModule } from 'ngx-bootstrap/dropdown';
import { Router, RouterLink, RouterLinkActive } from '@angular/router';
import { ToastrService } from 'ngx-toastr';
import { HasRoleDirective } from '../_directives/has-role.directive';

@Component({
  selector: 'app-nav',
  imports: [FormsModule, BsDropdownModule, RouterLink, RouterLinkActive, HasRoleDirective],
  templateUrl: './nav.component.html',
  styleUrl: './nav.component.css'
})
export class NavComponent {
  // post http request
  accountService = inject(AccountService);
  private router = inject(Router);
  private toaster = inject(ToastrService);
  model: any = {};

  // XP Bar Configuration (preset demo values)
  currentLevel = 5;
  currentXp = 1250;
  xpNeededForNextLevel = 2000;
  levelSeparators = [0, 25, 50, 75]; // Separator positions as percentages

  // Calculate XP progress percentage
  get xpProgress(): number {
    return (this.currentXp / this.xpNeededForNextLevel) * 100;
  }

  login(){
    this.accountService.login(this.model).subscribe({
      next: _ => {
        this.router.navigateByUrl('/members')
      },
      error: error => this.toaster.error(error.error)

    })
  }

  logout(){
    this.accountService.logout();
    this.router.navigateByUrl('/');
  }

}
