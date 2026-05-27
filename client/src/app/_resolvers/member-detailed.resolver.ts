import { ResolveFn } from '@angular/router';
import { Member } from '../_models/member';
import { inject } from '@angular/core';
import { MembersService } from '../_services/members.service';
import { ApiResponse } from '../_models/apiResponse';

export const memberDetailedResolver: ResolveFn<ApiResponse<Member> | null> = (route, state) => {
  const memberService = inject(MembersService);
  const userId = route.paramMap.get('id');

  if(!userId) return null;

  // returns Observable<ApiResponse<Member>>
  return memberService.getMember(Number.parseInt(userId));

};
