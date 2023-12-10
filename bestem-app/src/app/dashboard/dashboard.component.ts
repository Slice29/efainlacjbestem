import { Component, OnInit } from '@angular/core';
import { AccessLogService } from '../services/access-log.service';

@Component({
  selector: 'app-dashboard',
  templateUrl: './dashboard.component.html',
  styleUrls: ['./dashboard.component.css']
})
export class DashboardComponent implements OnInit {
  constructor(public service: AccessLogService) {}

  ngOnInit(): void {
    this.service.refreshList();
  }
}
