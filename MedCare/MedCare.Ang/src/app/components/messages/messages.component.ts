import { Component, OnInit } from '@angular/core';
import { AuthService } from '../../services/auth.service';
import { MessageService } from '../../services/message.service';
import { GetMessage } from '../../models/GetMessage';
import { SendMessage } from '../../models/SendMessage';
import { Router } from '@angular/router';

@Component({
  selector: 'app-messages',
  templateUrl: './messages.component.html',
  styleUrls: ['./messages.component.css']
})
export class MessagesComponent implements OnInit {

  messages: GetMessage[];

  constructor(
    private messageService: MessageService,
    private router: Router
  ) { }

  ngOnInit() {
    this.fetchMessages();
  }

  fetchMessages() {
    this.messageService.GetMessages().subscribe((data: GetMessage[]) => {
      this.messages = data;
      console.log(this.messages);
    });
  }

  messageRead() {
    console.log("Message Read");
  }

}
