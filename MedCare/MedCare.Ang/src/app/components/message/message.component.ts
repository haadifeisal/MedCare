import { Component, OnInit } from '@angular/core';
import { GetMessage } from '../../models/GetMessage';
import { MessageService } from '../../services/message.service';
import { ActivatedRoute } from '@angular/router';
import { FlashMessagesService } from 'angular2-flash-messages';
import { Router } from '@angular/router';

@Component({
  selector: 'app-message',
  templateUrl: './message.component.html',
  styleUrls: ['./message.component.css']
})
export class MessageComponent implements OnInit {

  message: GetMessage;
  id: number;

  constructor(
    private messageService: MessageService,
    private router: ActivatedRoute,
    private flashM: FlashMessagesService,
    private route: Router
  ) { }

  ngOnInit() {
    this.id = this.router.snapshot.params['id'];
    this.fetchMessage();
  }

  fetchMessage() {
    this.messageService.GetMessage(this.id).subscribe((data: GetMessage) => {
      this.message = data;
      console.log(this.message);
    })
  }

  deleteMessage(id: number) {
    if (confirm("Are you sure you want to delete this message?")) {
      this.messageService.DeleteMessage(id).subscribe(() => {

      });
      this.flashM.show('Message deleted', {
        cssClass: "alert-success", timeout: 3000
      });
      this.route.navigate(["/messages"]);
    } 
  }

}
