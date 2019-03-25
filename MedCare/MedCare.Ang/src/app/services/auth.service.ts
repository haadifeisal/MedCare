import { Injectable, OnInit } from '@angular/core';
import { Headers, Http, RequestOptions } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { JwtHelper, tokenNotExpired } from 'angular2-jwt';
import { DoctorService } from './doctor.service';
import { PatientService } from './patient.service';
import 'rxjs/add/operator/map';

@Injectable()
export class AuthService {

  url = "http://localhost:5000/api/users/";
  userToken: string;
  decodedToken: any;
  id: number;
  username: string;
  jwtHelper: JwtHelper = new JwtHelper();
  constructor(
    private http: Http,
    private docService: DoctorService,
    private patService: PatientService

  ) { }

  ngOnInit() {
   
  }

  doctorLogin(model:any): Observable<any> {
    return this.http.post(this.url + 'doctorlogin', model, this.headers())
      .map(response => {
        const user = response.json();
        if (user.tokenString != null) {
          localStorage.setItem('userToken', user.tokenString);
          this.userToken = user.tokenString;
          this.decodedToken = this.jwtHelper.decodeToken(this.userToken);
        }
      });
  }

  patientLogin(model: any): Observable<any> {
    return this.http.post(this.url + 'patientlogin', model, this.headers())
      .map(response => {
        const user = response.json();
        if (user.tokenString != null) {
          localStorage.setItem('userToken', user.tokenString);
          this.userToken = user.tokenString;
          this.decodedToken = this.jwtHelper.decodeToken(this.userToken);
        }
      });
  }

  logout() {
    this.userToken = null;
    localStorage.removeItem('userToken');
  }

  loggedIn() {
    return tokenNotExpired('userToken');
  }

  adminLoggedIn() {
    if (this.loggedIn()) {
      return this.docService.isAdmin(this.decodedToken.unique_name).subscribe(data => {
        if (data != null) {
          return true;
        } else {
          return false;
        }
      })
    }
    return false;
  }

  doctorLoggedIn() {
    if (this.loggedIn()) {
      return this.docService.checkIfDoctorExists(this.decodedToken.unique_name).subscribe(data => {
        if (data != null) {
          return true;
        } else {
          return false;
        }
      })
    }
    return false;
  }

  patientLoggedIn() {
    if (this.loggedIn()) {
      return this.patService.checkIfPatientExist(this.decodedToken.unique_name).subscribe(data => {
        if (data != null) {
          return true;
        } else {
          return false;
        }
      })
    }
    return false;
  }

  headers() {
    const headers = new Headers({ 'Content-type': 'application/json' });
    return new RequestOptions({ headers: headers });
  }

}
