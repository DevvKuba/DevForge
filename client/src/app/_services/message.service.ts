import { inject, Injectable, signal } from '@angular/core';
import { environment } from '../../environments/environment';
import { HttpClient, HttpContext } from '@angular/common/http';
import { PaginatedResult } from '../_models/pagination';
import { Message } from '../_models/message';
import { setPaginatedResponse, setPaginationHeaders } from './paginationHelper';
import { compileInjectable } from '@angular/compiler';
import signalR, { HubConnection, HubConnectionBuilder, HubConnectionState } from '@microsoft/signalr';
import { User } from '../_models/user';
import { Group } from '../modals/group';
import { BusyService } from './busy.service';
import { Observable } from 'rxjs';
import { ApiResponse } from '../_models/apiResponse';

@Injectable({
  providedIn: 'root'
})
export class MessageService {
  baseUrl = environment.apiUrl;
  hubUrl = environment.hubsUrl;
  private http = inject(HttpClient);
  private busyService = inject(BusyService);
  hubConnection?: HubConnection;
  paginatedResult = signal<PaginatedResult<Message[]> | null>(null);
  messageThread = signal<Message[]>([]);

  createHubConnection(user: User, otherUserId: number) : void{
    this.busyService.busy();
    this.hubConnection = new HubConnectionBuilder()
    .withUrl(this.hubUrl + 'message?userId=' + otherUserId, {
      accessTokenFactory: () => user.token
    })
    .withAutomaticReconnect()
    .build();

    this.hubConnection.start().catch(error => console.log(error))
    .finally(() => this.busyService.idle());

    this.hubConnection.on('ReceiveMessageThread', messages => {
      this.messageThread.set(messages);
    })

    this.hubConnection.on('NewMessage', message => {
      this.messageThread.update(messages => [...messages, message])
    })

    // loops through specific messages to update them in our Thread between the users
    this.hubConnection.on('UpdatedGroup', (group: Group) => {
      if(group.connections.some(x => x.userId === otherUserId)){
        this.messageThread.update(messages => {
          messages.forEach(message => {
            if(!message.dateRead){
              message.dateRead  = new Date(Date.now());
            }
          })
          return messages;
        })
      }
    })
  }

  stopHubConnection() : void{
    if(this.hubConnection?.state === HubConnectionState.Connected){
      this.hubConnection.stop().catch(error => console.log(error))
    }
  }

  getMessages(pageNumber: number, pageSize: number, container: string) : Observable<any>{
    let params = setPaginationHeaders(pageNumber, pageSize);
    params = params.append('Container', container);

    return this.http.get(this.baseUrl + 'messages', {observe: 'response', params});
  }

  getMessageThread(username: string) : Observable<Message[]>{
    return this.http.get<Message[]>(this.baseUrl + 'messages/thread/' + username);
  }

  async sendMessage(userId: number, content:string) : Promise<ApiResponse<any>>{
    if(!this.hubConnection){
      throw new Error('Hub connection not established');
    }

    return await this.hubConnection?.invoke<ApiResponse<any>>('SendMessage', {recipientId: userId, content})
  }

  deleteMessage(id: number) : Observable<Object>{
    return this.http.delete(this.baseUrl + 'messages/' + id);
  }
}
