import { Component, OnInit } from '@angular/core';
import { CreateRecord } from '../../models/CreateRecord';
import { Doctor } from '../../models/Doctor';
import { PatientService } from '../../services/patient.service';
import { DoctorService } from '../../services/doctor.service';
import { FlashMessagesService } from 'angular2-flash-messages';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'app-record-detail',
  templateUrl: './record-detail.component.html',
  styleUrls: ['./record-detail.component.css']
})
export class RecordDetailComponent implements OnInit {

  record: CreateRecord;
  doctor: Doctor;
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
    this.patientService.getRecord(this.id).subscribe((data: CreateRecord) => {
      this.record = data;
      this.fetchDoctor();
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      console.log(error);
    });
  }

  fetchDoctor() {
    this.doctorService.getDoctorByIDForPatient(this.record.doctorId).subscribe((data: Doctor) => {
      this.doctor = data;
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      console.log(error);
    });
  }

}
