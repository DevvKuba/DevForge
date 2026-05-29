import { Component, HostListener, inject, OnInit, ViewChild, viewChild } from '@angular/core';
import { Member } from '../../_models/member';
import { AccountService } from '../../_services/account.service';
import { MembersService } from '../../_services/members.service';
import { TabsModule } from 'ngx-bootstrap/tabs';
import { FormsModule, NgForm } from '@angular/forms';
import { ToastrService } from 'ngx-toastr';
import { PhotoEditorComponent } from "../photo-editor/photo-editor.component";
import { DatePipe } from '@angular/common';
import { TimeagoModule } from 'ngx-timeago';
import { R3SelectorScopeMode } from '@angular/compiler';

@Component({
  selector: 'app-member-edit',
  imports: [TabsModule, FormsModule, PhotoEditorComponent, DatePipe, TimeagoModule],
  templateUrl: './member-edit.component.html',
  styleUrl: './member-edit.component.css'
})
export class MemberEditComponent implements OnInit {
  
  @ViewChild('editForm') editForm?: NgForm;
  @HostListener('window:beforeunload', ['$event']) notify($event: any){
    if(this.editForm?.dirty){
      $event.returnValue = true;
    }
  }

  member?: Member;
  originalMember?: Member;
  private accountService = inject(AccountService);
  private memberService = inject(MembersService);
  private toastr = inject(ToastrService);

   ngOnInit(): void {
    this.loadMember();
  }

  loadMember(){
    const user = this.accountService.currentUser();
    if(!user) return;
    this.memberService.getMember(user.id).subscribe({
      next: (response) => {
        this.member = response;
        this.originalMember = { ...response };
      }
    })
  }

  updateMember(){
    const newFieldCount = this.countNewlyFilledFields();
    const updatePayload = {
      ...this.editForm?.value,
      newlyFilledFieldCount: newFieldCount
    };
    
    this.memberService.updateMember(updatePayload).subscribe({
      next: (response) => {
        this.toastr.success('Profile updated successfully');
        this.editForm?.reset(this.member);
        this.accountService.updateUserXpProperties(response.xpDetails!);
      }
    })
  }

  onMemberChange(event: Member){
    this.member = event;
  }

  countNewlyFilledFields(): number {
    const fieldsToCheck = ['introduction', 'skills', 'interests', 'email', 'city', 'country', 'specialization'];
    
    let newFieldCount = 0;
    
    fieldsToCheck.forEach(field => {
      const originalValue = (this.originalMember as any)[field] || '';
      const currentValue = (this.member as any)[field] || '';
      
      // Check if field was empty and now has a value
      if (this.isEmpty(originalValue) && !this.isEmpty(currentValue)) {
        newFieldCount++;
      }
    });
    
    return newFieldCount;
  }

  private isEmpty(value: any): boolean {
    return value === null || value === undefined || value === '' || value === 0;
  }

}
