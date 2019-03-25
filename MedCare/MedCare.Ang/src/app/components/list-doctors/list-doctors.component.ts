import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../models/Doctor';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-list-doctors',
  templateUrl: './list-doctors.component.html',
  styleUrls: ['./list-doctors.component.css']
})
export class ListDoctorsComponent implements OnInit {

  doctors: Doctor[];
  id: any;

  constructor(private doctorService: DoctorService) { }

  ngOnInit() {
    this.fetchDoctors();
  }

  fetchDoctors() {
    this.doctorService.getAllDoctors().subscribe((data: Doctor[]) => {
      this.doctors = data;
      console.log(this.doctors);
    }, error => {
      console.log(error);
    });
  }

  getDoctorId() {
    //[routerLink]="['/doctor-detail/']"
    console.log("Hej");
  }

}
