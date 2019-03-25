import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../services/doctor.service';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/Patient';
import { ExistingPatient } from '../../models/ExistingPatient';
import { ActivatedRoute, Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'app-existing-patient',
  templateUrl: './existing-patient.component.html',
  styleUrls: ['./existing-patient.component.css']
})
export class ExistingPatientComponent implements OnInit {

  username: string;
  patient: Patient;
  ePatient: ExistingPatient = {
    username: ''
  };

  constructor(
    private doctorService: DoctorService,
    private patientService: PatientService,
    private router: ActivatedRoute,
    private rout: Router,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    this.username = '';
  }

  searchPatient({ value, valid }) {
    if (!valid) {
      this.flashM.show('Form is invalid', {
        cssClass: "alert-danger", timeout: 3000
      });
      this.rout.navigate(["/existing-patient"]);
    } else {
      this.fetchPatient();
    }
  }

  fetchPatient() {
    this.patientService.getPatientByUsername(this.username).subscribe((data: Patient) => {
      this.patient = data;
      this.ePatient.username = this.patient.username;
      console.log(this.username);
    })
  }

  addPatient() {
    this.doctorService.addExistingPatient(this.ePatient).subscribe(() => {
      this.flashM.show('Patient added', {
        cssClass: "alert-success", timeout: 3000
      });
      console.log("Patient Username: " + this.username);
      this.rout.navigate(["/existing-patient"]);
    });
  }

}
