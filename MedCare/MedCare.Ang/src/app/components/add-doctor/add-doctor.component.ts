import { Component, OnInit } from '@angular/core';
import { AddDoctor } from '../../models/AddDoctor';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-add-doctor',
  templateUrl: './add-doctor.component.html',
  styleUrls: ['./add-doctor.component.css']
})
export class AddDoctorComponent implements OnInit {
  
  doctor: AddDoctor = {
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
  constructor(
    private flashM: FlashMessagesService,
    private router: Router,
    private docService: DoctorService
  ) { }

  ngOnInit() {
  }

  addDoctor({value, valid}:{value:AddDoctor,valid:boolean }) {
    if (!valid) {
      this.flashM.show('Form is invalid', {
        cssClass: "alert-danger", timeout: 3000
      });
      this.router.navigate(["/add-doctor"]);
    } else {
      this.docService.addDoctor(this.doctor).subscribe(() => {
        this.flashM.show('Doctor added', {
          cssClass: "alert-success", timeout: 3000
        });
        this.router.navigate(["/list-doctors"]);
      });
    }
  }

}
