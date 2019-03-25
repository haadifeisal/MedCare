import { Component, OnInit } from '@angular/core';
import { Doctor } from '../../models/Doctor';
import { SendMessage } from '../../models/SendMessage';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';
import { DoctorService } from '../../services/doctor.service';

@Component({
  selector: 'app-sendmessage',
  templateUrl: './sendmessage.component.html',
  styleUrls: ['./sendmessage.component.css']
})
export class SendmessageComponent implements OnInit {

  message: SendMessage = {
    to: '',
    subject: '',
    content: ''
  };
  doctors: Doctor[];
  id: number;
  email: string;

  constructor(
    private messageService: MessageService,
    private router: ActivatedRoute,
    private flashM: FlashMessagesService,
    private route: Router,
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

