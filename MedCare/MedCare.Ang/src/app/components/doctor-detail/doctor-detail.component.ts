import { Component, OnInit } from '@angular/core';
import { DoctorService } from '../../services/doctor.service';
import { Doctor } from '../../models/Doctor';
import { ActivatedRoute, Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'app-doctor-detail',
  templateUrl: './doctor-detail.component.html',
  styleUrls: ['./doctor-detail.component.css']
})
export class DoctorDetailComponent implements OnInit {

  doctor: Doctor;
  id: number;

  constructor(
    private doctorService: DoctorService,
    private router: ActivatedRoute,
    private rout: Router,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    this.id = this.router.snapshot.params['id'];
    this.doctorService.getDoctorByID(this.id).subscribe((data: Doctor) => {
      this.doctor = data;
      console.log(this.doctor);
    })
  }

  deleteDoctor() {
    if (confirm("Are you sure you want to delete this doctor?")) {
      this.doctorService.deleteDoctor(this.id).subscribe(() => {
        this.flashM.show('Doctor deleted', {
          cssClass: "alert-success", timeout: 3000
        });
        this.rout.navigate(["/list-doctors"]);
        //console.log("Doctor with ID: " + this.id + " is deleted");
      });
    }
  }

}
