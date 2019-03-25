import { Component, OnInit } from '@angular/core';
import { Patient } from '../../models/Patient';
import { PatientService } from '../../services/patient.service';

@Component({
  selector: 'app-patients',
  templateUrl: './patients.component.html',
  styleUrls: ['./patients.component.css']
})
export class PatientsComponent implements OnInit {

  patients: Patient[];

  constructor(private patientService: PatientService) { }

  ngOnInit() {
    this.fetchDoctors();
  }

  fetchDoctors() {
    this.patientService.getSpecificPatients().subscribe((data: Patient[]) => {
      this.patients = data;
      console.log(this.patients);
    }, error => {
      console.log(error);
    });
  }

}
