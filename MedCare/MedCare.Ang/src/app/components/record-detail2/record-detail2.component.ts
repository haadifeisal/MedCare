import { Component, OnInit } from '@angular/core';
import { GetRecord } from '../../models/GetRecord';
import { Patient } from '../../models/Patient';
import { PatientService } from '../../services/patient.service';
import { DoctorService } from '../../services/doctor.service';
import { FlashMessagesService } from 'angular2-flash-messages';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-record-detail2',
  templateUrl: './record-detail2.component.html',
  styleUrls: ['./record-detail2.component.css']
})
export class RecordDetail2Component implements OnInit {

  record: GetRecord;
  patient: Patient;
  id: number;

  constructor(
    private patientService: PatientService,
    private doctorService: DoctorService,
    private flashM: FlashMessagesService,
    private router: ActivatedRoute,
    private rout: Router
  ) { }

  ngOnInit() {
    this.id = this.router.snapshot.params['id'];
    this.fetchRecords();
  }

  fetchRecords() {
    this.doctorService.getRecord(this.id).subscribe((data: GetRecord) => {
      this.record = data;
      this.fetchPatient();
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      console.log(error);
    });
  }

  fetchPatient() {
    this.patientService.getPatientById(this.record.patientId).subscribe((data: Patient) => {
      this.patient = data;
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      console.log(error);
    });
  }

}
