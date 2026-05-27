import { HttpClient, HttpParams, HttpResponse} from '@angular/common/http';
import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { Member } from '../_models/member';
import { Observable, ObservableLike, of, tap } from 'rxjs';
import { Photo } from '../_models/photo';
import { PaginatedResult } from '../_models/pagination';
import { UserParams } from '../_models/userParams';
import { observeNotification } from 'rxjs/internal/Notification';
import { AccountService } from './account.service';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { ApiResponse } from '../_models/apiResponse';

@Injectable({
  providedIn: 'root'
})
export class MembersService {
  private http = inject(HttpClient);
  private accountService = inject(AccountService);
  baseUrl = environment.apiUrl;
  paginatedResult = signal<PaginatedResult<Member[]> | null>(null);
  memberCache = new Map();
  user = this.accountService.currentUser();
  userParams = signal<UserParams>(new UserParams(this.user));

  resetUserParams(){
    this.userParams.set(new UserParams(this.user));
  }

  // gets members from the API and stored in the cache;
  getMembers(){
    const response = this.memberCache.get(Object.values(this.userParams()).join('-'));

    if(response) return setPaginatedResponse(response, this.paginatedResult);

    let params = setPaginationHeaders(this.userParams().pageNumber, this.userParams().pageSize);

    params = params.append('minExperience', this.userParams().minExperience);
    params = params.append('maxExperience', this.userParams().maxExperience);
    params = params.append('gender', this.userParams().gender);
    params = params.append('orderBy', this.userParams().orderBy);

    return this.http.get<Member[]>(this.baseUrl + 'users', {observe: 'response', params}).subscribe({
      next: response => {
        setPaginatedResponse(response, this.paginatedResult);
        this.memberCache.set(Object.values(this.userParams()).join('-'), response);
      } 
    })
  }

  getMember(userId: number) : Observable<Member> {
    // adds all members on given page to memberCache
    const member: Member = [...this.memberCache.values()]
    .reduce((arr, elem) => arr.concat(elem.body), [])
    .find((m:Member) => m.id === userId); 

    if(member) return of(member);
  
    return this.http.get<Member>(this.baseUrl + `users/GetUserById?userId=${userId}`);
  }

  // calls put request from our api, that updates member data in database
  updateMember(member: Member) : Observable<Object>{
    return this.http.put(this.baseUrl + 'users', member);
  }


  setMainPhoto(photo: Photo) : Observable<Object>{
    return this.http.put(this.baseUrl + 'users/set-main-photo/' + photo.id, {});
  }

  deletePhoto(photo: Photo) : Observable<Object>{
    return this.http.delete(this.baseUrl + 'users/delete-photo/' + photo.id);
  }

  
}
