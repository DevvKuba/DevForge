import { HttpClient } from '@angular/common/http';
import { computed, inject, Injectable, signal } from '@angular/core';
import { User } from '../_models/user';
import { map, Observable } from 'rxjs';
import { environment } from '../../environments/environment';
import { FollowService } from './follow.service';
import { PresenceService } from './presence.service';
import { UserXpDetailDto } from '../_models/dtos/userXpDetailDto';

@Injectable({
  providedIn: 'root'
})
export class AccountService {
  private http = inject(HttpClient);
  private likeService = inject(FollowService)
  private presenceService = inject(PresenceService)

  baseUrl = environment.apiUrl;
  currentUser = signal<User | null>(null);

  roles = computed(() => {
    const user = this.currentUser();
    if(user && user.token){
      // atob allows decoding the token
      const role =  JSON.parse(atob(user.token.split('.')[1])).role;
      return Array.isArray(role) ? role : [role];
    }
    return [null];
  })

  login(model: any) : Observable<void>{
    return this.http.post<User>(this.baseUrl + 'account/login', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
      })
    )
  }

  register(model: any) : Observable<User>{
    return this.http.post<User>(this.baseUrl + 'account/register', model).pipe(
      map(user => {
        if(user){
          this.setCurrentUser(user);
        }
        return user;
      })
    )
  }

  setCurrentUser(user: User) : void{
    localStorage.setItem('user', JSON.stringify(user));
    this.currentUser.set(user);
    this.likeService.getFollowIds();
    this.presenceService.createHubConnection(user);
  }

  logout() : void{
    localStorage.removeItem('user');
    this.currentUser.set(null);
    this.presenceService.stopHubConnection();
  }

  updateUserXpProperties(xpDetails: UserXpDetailDto){
    this.currentUser.update(currentUser => ({
      ...currentUser!,
      appExperiencePoints: xpDetails.appExperiencePoints,
      level: xpDetails.level,
      levelThreshold: xpDetails.levelThreshold
    }));
  }

}
