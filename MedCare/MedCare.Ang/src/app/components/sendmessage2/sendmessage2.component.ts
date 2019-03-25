import { Component, OnInit } from '@angular/core';
import { Patient } from '../../models/Patient';
import { SendMessage } from '../../models/SendMessage';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { PatientService } from '../../services/patient.service';

@Component({
  selector: 'app-sendmessage2',
  templateUrl: './sendmessage2.component.html',
  styleUrls: ['./sendmessage2.component.css']
})
export class Sendmessage2Component implements OnInit {

  message: SendMessage = {
    to: '',
    subject: '',
    content: ''
  };
  patients: Patient[];
  id: number;
  email: string;

  constructor(
    private messageService: MessageService,
    private router: ActivatedRoute,
    private flashM: FlashMessagesService,
    private route: Router,
    private patientService: PatientService
  ) { }

  ngOnInit() {
    this.patientService.getSpecificPatients().subscribe((data: Patient[]) => {
      this.patients = data;
    }, error => {
      this.flashM.show('Error getting doctors', {
        cssClass: "alert-danger", timeout: 3000
      });
    });
  }

  public onChange(event): void {  // event will give you full breif of action
    const newVal = event.target.value;
    this.email = newVal;
  }

  sendMessage({ value, valid }) {
    if (!valid) {
      this.flashM.show('Form is INVALID', {
        cssClass: "alert-danger", timeout: 3000
      });
    } else {
      this.message.to = this.email;
      console.log(this.message);
      this.messageService.SendMessage(this.message).subscribe(() => {
        
      });
      this.flashM.show('Message sent', {
        cssClass: "alert-success", timeout: 3000
      });
      this.route.navigate(["/messages"]);
    }
  }

}

