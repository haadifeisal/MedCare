import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../models/Doctor';
import { AddDoctor } from '../../models/AddDoctor';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor.service';
import { AuthService } from '../../services/auth.service';

@Component({
  selector: 'app-edit-doctor',
  templateUrl: './edit-doctor.component.html',
  styleUrls: ['./edit-doctor.component.css']
})
export class EditDoctorComponent implements OnInit {
  doctor: Doctor;
  editDoctor: AddDoctor = {
    isAdmin: false,
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    phoneNumber: '',
    specialistIn: '',
    description: ''
  };
  id: number;
  constructor(
    private flashM: FlashMessagesService,
    private router: Router,
    private docService: DoctorService,
    private authService: AuthService
  ) { }

  ngOnInit() {
    this.id = this.authService.decodedToken.nameid;
    this.docService.getDoctorByID(this.id).subscribe((data: Doctor) => {
      this.doctor = data;
      console.log(this.doctor);
    }, error => {
      this.flashM.show(error.message, {
        cssClass: 'alert-danger', timeout: 3000
      });
    });
  }

  editDoctorr({ value, valid }: { value: Doctor, valid: boolean }) {
    if (!valid) {
      this.flashM.show('Form is invalid', {
        cssClass: "alert-danger", timeout: 4000
      });
      this.router.navigate(["/edit-doctor"]);
    } else {
      this.editDoctor.isAdmin = false;
      this.editDoctor.firstName = this.doctor.firstName;
      this.editDoctor.lastName = this.doctor.lastName;
      this.editDoctor.username = this.doctor.username;
      this.editDoctor.email = this.doctor.email;
      this.editDoctor.phoneNumber = this.doctor.phoneNumber;
      this.editDoctor.specialistIn = this.doctor.specialistIn;
      this.editDoctor.description = this.doctor.description;
      this.docService.editDoctor(this.editDoctor, this.id).subscribe(() => {
        this.flashM.show('Doctor edited', {
          cssClass: "alert-success", timeout: 3000
        });
        this.router.navigate(["/"]);
      }, error => {
        this.flashM.show(error.message, {
          cssClass: "alert-danger", timeout: 3000
        });
        this.router.navigate(["/edit-doctor"]);
      })
    }
  }

}
