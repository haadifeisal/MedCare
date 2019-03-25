import { Component, OnInit } from '@angular/core';
import { CreateRecord } from '../../models/CreateRecord';
import { Doctor } from '../../models/Doctor';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-add-record',
  templateUrl: './add-record.component.html',
  styleUrls: ['./add-record.component.css']
})
export class AddRecordComponent implements OnInit {

  record: any = {
    doctorId: 0,
    pulse: 0,
    stressLevel: 0,
    oxygenLevel: 0,
    description: '',
    dateCreated: new Date()
  };

  doctors: Doctor[];
  username: string;

  constructor(
    private flashM: FlashMessagesService,
    private router: Router,
    private patientService: PatientService,
    private doctorService: DoctorService
  ) { }

  ngOnInit() {
    this.doctorService.getSpecificDoctors().subscribe((data: Doctor[]) => {
      this.doctors = data;
    }, error => {
      this.flashM.show('Error getting doctors', {
        cssClass: "alert-danger", timeout: 3000
      });
    });
  }

  public onChange(event): void {  // event will give you full breif of action
    const newVal = event.target.value;
    this.username = newVal;
    console.log(newVal);
  }

  addRecord({ value, valid }) {
    if (!valid || this.username=='') {
      this.flashM.show('Form is invalid', {
        cssClass: "alert-danger", timeout: 3000
      });
      this.router.navigate(["/add-record"]);
    } else {
      console.log("Username: " + this.username);
      this.doctorService.getDoctorByUsername(this.username).subscribe((data: Doctor) => {
        this.record.doctorId = data.id;
        console.log("Doctor ID: " + this.record.doctorId);
        this.record.pulse = value.pulse;
        this.record.stressLevel = value.stressLevel;
        this.record.oxygenLevel = value.oxygenLevel;
        this.record.description = value.description;
        this.patientService.createRecord(this.record).subscribe(() => {
          
        });
        this.flashM.show('Record added', {
          cssClass: "alert-success", timeout: 3000
        });
        this.router.navigate(["/patient-records"]);
      });
    }
  }

}
