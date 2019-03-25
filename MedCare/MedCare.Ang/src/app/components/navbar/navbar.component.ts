import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { DoctorService } from '../../services/doctor.service';
import { PatientService } from '../../services/patient.service';
import { Router } from '@angular/router';
import { Doctor } from '../../models/Doctor';
import { Patient } from '../../models/Patient';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent implements OnInit {
  username: any;
  //admin: boolean = false;
  doctor: Doctor;
  patient: Patient;

  constructor(
    private authService: AuthService,
    private docService: DoctorService,
    private patService: PatientService,
    private router: Router,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    //this.id = parseInt(this.authService.decodedToken.nameid);
    //console.log("isAdmin: " + this.isAdmin());
    //this.isAdmin();
  }

  loggedIn() {
    return this.authService.loggedIn();
  }

  logout() {
    this.authService.logout();
    this.flashM.show('User logged out', {
      cssClass: "alert-danger", timeout: 3000
    });
    this.router.navigate(["/login"]);
  }

  /*isAdmin() {
    return this.docService.getDoctorByID(this.authService.id).subscribe(data => {
      if (data.isAdmin) {
        console.log("isAdmin: " + data.isAdmin);
        return true;
      } else {
        this.admin = false;
        console.log("isAdmin: No" + data.isAdmin);
        return false;
      }
    });
  }

  isDoctor() {
    this.docService.getDoctorByID(this.id).subscribe((data: Doctor) => {
      if (data) {
        return true;
      } else {
        return false;
      }
    })
  }

  isPatient() {
    this.patService.getPatientById(this.id).subscribe((patient: Patient) => {
      if (patient) {
        return true;
      } else {
        return false;
      }
    });
  }*/

  /*
  <a class="p-2 text-dark" routerLink="list-doctors" *ngIf="loggedIn()">Doctors</a>
    <a class="p-2 text-dark" routerLink="list-doctors" *ngIf="loggedIn()">Patients</a>
    <a class="p-2 text-dark" routerLink="records" *ngIf="loggedIn()">Records</a>
    <a class="p-2 text-dark" routerLink="list-doctors" *ngIf="loggedIn()">New Record</a>
    <a class="p-2 text-dark" routerLink="messages" *ngIf="loggedIn()">Messages</a>
    <a class="p-2 text-info" *ngIf="loggedIn()"><i class="fa fa-user"></i>User: {{authService.decodedToken.unique_name}}</a>
  */

}
