import { Component, OnInit } from '@angular/core';
import { PatientService } from '../../services/patient.service';
import { Patient } from '../../models/Patient';
import { ActivatedRoute, Router } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';

@Component({
  selector: 'app-patient-detail',
  templateUrl: './patient-detail.component.html',
  styleUrls: ['./patient-detail.component.css']
})
export class PatientDetailComponent implements OnInit {

  patient: Patient;
  id: number;

  constructor(
    private patientService: PatientService,
    private router: ActivatedRoute,
    private rout: Router,
    private flashM: FlashMessagesService
  ) { }

  ngOnInit() {
    this.id = this.router.snapshot.params['id'];
    this.patientService.getPatientById(this.id).subscribe((data: Patient) => {
      this.patient = data;
    })
  }

}
