import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../models/Doctor';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-doctors',
  templateUrl: './doctors.component.html',
  styleUrls: ['./doctors.component.css']
})
export class DoctorsComponent implements OnInit {

  doctors: Doctor[];

  constructor(private doctorService: DoctorService) { }

  ngOnInit() {
    this.fetchDoctors();
  }

  fetchDoctors() {
    this.doctorService.getSpecificDoctors().subscribe((data: Doctor[]) => {
      this.doctors = data;
      console.log(this.doctors);
    }, error => {
      console.log(error);
    });
  }

}
