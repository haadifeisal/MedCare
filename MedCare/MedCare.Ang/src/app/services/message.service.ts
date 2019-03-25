import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions, Response } from '@angular/http';
import { GetMessage } from '../models/GetMessage';
import { SendMessage } from '../models/SendMessage';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { Observable } from 'rxjs/Observable';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';

@Injectable()
export class MessageService {

  apiUrl = "http://localhost:5000/api/message/";

  constructor(
    private http: Http,
    private flashM: FlashMessagesService,
    private router: Router
  ) { }

  GetMessages(): Observable<GetMessage[]> {
    return this.http.get(this.apiUrl + "getReceivedMessages", this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show("You are not authorized", {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <GetMessage[]>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  GetMessage(id: number): Observable<GetMessage> {
    return this.http.get(this.apiUrl + "getMessage/"+id, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show("You are not authorized", {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        if (data.message != null) {
          this.flashM.show("Message not found", {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <GetMessage>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  SendMessage(sendMessage: SendMessage): Observable<any> {
    return this.http.post(this.apiUrl + "sendMessage", sendMessage, this.headers())
      .map(response => response.json());
  }

  DeleteMessage(id: number): Observable<any> {
    return this.http.delete(this.apiUrl + "deleteMessage/" + id, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show("You are not authorized", {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return data;
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  headers() {
    const token = localStorage.getItem("userToken");
    if (token) {
      const headers = new Headers({ 'Content-type': 'application/json' });
      headers.append('Authorization', 'Bearer ' + token);
      const options = new RequestOptions({ headers: headers });
      return options;
    }
  }

}
