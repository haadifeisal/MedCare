import { Injectable } from '@angular/core';
import { Http, Headers, RequestOptions } from '@angular/http';
import { Patient } from '../models/Patient';
import { Observable } from 'rxjs/Observable';
import { Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { CreateRecord } from '../models/CreateRecord';
import 'rxjs/add/operator/map';

@Injectable()
export class PatientService {

  apiUrl = "http://localhost:5000/api/patient/";

  constructor(
    private http: Http,
    private flashM: FlashMessagesService,
    private router: Router
  ) { }

  getAllPatients(): Observable<Patient[]> {
    return this.http.get(this.apiUrl+"getAllPatients",this.headers()).map(response => <Patient[]>response.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getPatientById(id: number): Observable<Patient> {
    return this.http.get(this.apiUrl + "getPatient/" + id, this.headers()).map(response => <Patient>response.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getPatientByUsername(username: string): Observable<Patient> {
    return this.http.get(this.apiUrl + "getPatientByUsername/" + username, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Patient not found', {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/existing-patient"]);
        }
        if (data.message != null) {
          this.flashM.show('Failed to add patient.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/existing-patient"]);
        }
        return <Patient>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  checkIfPatientExist(username: string): Observable<Boolean> {
    return this.http.get(this.apiUrl + "checkPatient/" + username, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return data;
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  createRecord(record: any): Observable<any> {
    return this.http.post(this.apiUrl + "createRecord", record, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        if (data.message != null) {
          this.flashM.show('Failed to create Record.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/add-record"]);
        }
        return data;
      });
  }

  getAllRecords(): Observable<CreateRecord[]> {
    return this.http.get(this.apiUrl + "patientRecords", this.headers())
      .map(response => {
        /*const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this data', {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }*/
        return <CreateRecord[]>response.json();
      });
  }

  getRecord(id:number): Observable<CreateRecord> {
    return this.http.get(this.apiUrl + "patientRecord/" + id, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <CreateRecord> response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getSpecificPatients(): Observable<Patient[]> {
    return this.http.get(this.apiUrl + "getSpecificPatients", this.headers())
      .map(response => {
        /*const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }*/
        return <Patient[]>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  headers() {
    const token = localStorage.getItem('userToken');
    if (token) {
      const headers = new Headers({ "Content-Type": "application/json" });
      headers.append("Authorization", "Bearer " + token);
      const options = new RequestOptions({ headers: headers });
      return options;
    }
  }

}
