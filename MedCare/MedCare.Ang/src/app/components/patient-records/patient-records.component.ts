import { Component, OnInit } from '@angular/core';
import { CreateRecord } from '../../models/CreateRecord';
import { PatientService } from '../../services//patient.service';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'app-patient-records',
  templateUrl: './patient-records.component.html',
  styleUrls: ['./patient-records.component.css']
})
export class PatientRecordsComponent implements OnInit {

  records: CreateRecord[];

  constructor(
    private patientService: PatientService,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    this.fetchRecords();
  }

  fetchRecords() {
    this.patientService.getAllRecords().subscribe((data: CreateRecord[]) => {
      this.records = data;
      //console.log(this.records);
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      console.log(error);
    });
  }

}
