import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../services/doctor.service';
import { Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { GetRecord } from '../../models/GetRecord';

@Component({
  selector: 'app-records',
  templateUrl: './records.component.html',
  styleUrls: ['./records.component.css']
})
export class RecordsComponent implements OnInit {

  records: GetRecord[];
  //*ngIf="records?.length>0; else noRecords"
  constructor(
    private doctorService: DoctorService,
    private flashM: FlashMessagesService,
    private router: Router
  ) { }

  ngOnInit() {
    this.fetchRecords();
  }

  fetchRecords() {
    this.doctorService.getAllRecords().subscribe((data: GetRecord[]) => {
      this.records = data;
    }, error => {
      this.flashM.show(error.message, {
        cssClass: "alert-danger", timeout: 3000
      });
      this.router.navigate(["/"]);
    });
  }

}
