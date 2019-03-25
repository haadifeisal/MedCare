import { Component, OnInit } from '@angular/core';
import { AddPatient } from '../../models/AddPatient';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-add-patient',
  templateUrl: './add-patient.component.html',
  styleUrls: ['./add-patient.component.css']
})
export class AddPatientComponent implements OnInit {

  patient: AddPatient = {
    firstName: '',
    lastName: '',
    username: '',
    email: '',
    password: '',
    socialNumber: '',
    phoneNumber: ''
  };

    constructor(
      private flashM: FlashMessagesService,
      private router: Router,
      private docService: DoctorService
    ) { }

  ngOnInit() {
  }

  addPatient({ value, valid }: { value: AddPatient, valid: boolean }) {
    if (!valid) {
      this.flashM.show('Form is invalid', {
        cssClass: "alert-danger", timeout: 3000
      });
      this.router.navigate(["/add-patient"]);
    } else {
      this.docService.addPatient(this.patient).subscribe(() => {
        
      });
      this.flashM.show('Patient added', {
        cssClass: "alert-success", timeout: 3000
      });
      this.router.navigate(["/records"]);
      console.log("Add patient form is valid " + value.username);
    }
  }

}
