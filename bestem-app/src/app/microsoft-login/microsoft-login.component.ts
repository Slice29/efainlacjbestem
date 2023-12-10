import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { Subscription } from 'rxjs';
import { AccessLogService } from '../services/access-log.service';


@Component({
  selector: 'app-microsoft-login',
  templateUrl: './microsoft-login.component.html',
  styleUrls: ['./microsoft-login.component.css']
})
export class MicrosoftLoginComponent {

  responseSubscription: Subscription | undefined;

  constructor(private router: Router, public service: AccessLogService) {}

  ngOnInit(): void {
    this.redirectToMicrosoftLogin();
  }

  redirectToMicrosoftLogin() {
    window.location.href = 'http://localhost:5240/api/initiateadflow';
    this.watchForResponse();
  }

  watchForResponse() {
    this.responseSubscription = this.service.watchForToken().subscribe(
      (response) => {
        console.log('Response:', response);
        // Display message based on response
        // Example: Update a variable to show a message in the template
        // this.showMessage = 'Received token successfully';
      },
      (error) => {
        // Handle error response
        console.error('Error:', error);
        // Display error message or handle as needed
        // this.showMessage = 'Failed to receive token';
      }
    );
}
}
