import { Injectable } from '@angular/core';
import { Headers, Http, RequestOptions, Response } from '@angular/http';
import { Observable } from 'rxjs/Observable';
import { Doctor } from '../models/Doctor';
import { GetRecord } from '../models/GetRecord';
import { Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { AddDoctor } from '../models/AddDoctor';
import { AddPatient } from '../models/AddPatient';
import { ExistingPatient } from '../models/ExistingPatient';
import 'rxjs/add/operator/map';
import 'rxjs/add/operator/catch';
import 'rxjs/add/observable/of';

@Injectable()
export class DoctorService {

  apiUrl = "http://localhost:5000/api/doctor/"; //här når vi controllen "doctor" och metoden
  userUrl = "http://localhost:5000/api/users/";
  ePatient: ExistingPatient;
  constructor(
    private http: Http,
    private flashM: FlashMessagesService,
    private router: Router
  ) { }

  getAllDoctors(): Observable<Doctor[]> {
    return this.http.get(this.apiUrl +"getAllDoctors", this.headers())
      .map(response => {
        /*const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }*/
        return <Doctor[]>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getDoctorByID(id: number): Observable<Doctor> {
    return this.http.get(this.apiUrl + "getDoctor/" + id, this.headers())
      .map(response => <Doctor>response.json())
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getDoctorByIDForPatient(id: number): Observable<Doctor> {
    return this.http.get(this.apiUrl + "getDoctorForPatient/" + id, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <Doctor>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getDoctorByUsername(username: string): Observable<Doctor> {
    return this.http.get(this.apiUrl + "getDoctorByUsername/" + username, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <Doctor>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  addDoctor(doctor: AddDoctor) {
    return this.http.post(this.userUrl + "registerDoctor", doctor, this.headers());
  }

  editDoctor(doctor: AddDoctor, id:number) {
    return this.http.put(this.apiUrl + "editDoctor/" + id, doctor, this.headers());
  }

  deleteDoctor(id: number) {
    return this.http.delete(this.apiUrl + "deleteDoctor/" + id, this.headers());
  }

  isAdmin(username:string): Observable<any>{
    return this.http.get(this.apiUrl + "isAdmin/" + username, this.headers())
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

  checkIfDoctorExists(username: string): Observable<Boolean> {
    return this.http.get(this.apiUrl + "checkDoctor/" + username, this.headers())
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

  getAllRecords(): Observable<GetRecord[]>{
    return this.http.get(this.apiUrl + "doctorRecords", this.headers())
      .map(response => {
        /*const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this data', {
            cssClass: "alert-danger", timeout: 3000
          });
          this.router.navigate(["/"]);
        }*/
        return <GetRecord[]>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getSpecificDoctors(): Observable<Doctor[]> {
    return this.http.get(this.apiUrl + "getSpecificDoctors", this.headers())
      .map(response => {
        /*const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }*/
        return <Doctor[]>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  getRecord(id: number): Observable<GetRecord> {
    return this.http.get(this.apiUrl + "doctorRecord/" + id, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        return <GetRecord>response.json();
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  addPatient(patient: AddPatient): Observable<any> {
    return this.http.post(this.userUrl + "registerPatient", patient, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/"]);
        }
        if (data.message != null) {
          this.flashM.show('Patient username already exists.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/add-patient"]);
        }
        return data;
      })
      .catch((error: any) => Observable.throw(error.json().error || 'Server error'));
  }

  addExistingPatient(ePatient: ExistingPatient): Observable<any> {
    return this.http.post(this.userUrl + "addExistingPatient", ePatient, this.headers())
      .map(response => {
        const data = response.json();
        if (data.url != null) {
          this.flashM.show('Not authorized to access this page.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/existing-patient"]);
        }
        if (data.message != null) {
          this.flashM.show('Failed to add patient.', {
            cssClass: 'alert-danger', timeout: 3000
          });
          this.router.navigate(["/existing-patient"]);
        }
        return data;
      })
  }

  headers() {
    const token = localStorage.getItem('userToken');
    if (token) {
      const headers = new Headers({ 'Content-type': 'application/json' });
      headers.append("Authorization", "Bearer " + token);
      const options = new RequestOptions({ headers: headers });
      return options;
    }
  }

}
