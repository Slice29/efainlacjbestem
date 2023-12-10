import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { AccessLog } from '../models/access-log';
import { Observable, interval, switchMap } from 'rxjs';

@Injectable({
  providedIn: 'root'
})
export class AccessLogService {
  accessLogList: AccessLog[] = [];
  urlGetAccessLog = 'http://localhost:5240/api/AccessLog';

  constructor(private http: HttpClient) { }

  generateMockData(): void {
    this.accessLogList = [
      new AccessLog(new Date(), 'user1', '/endpoint1'),
      new AccessLog(new Date(), 'user2', '/endpoint2'),
      new AccessLog(new Date(), 'user3', '/endpoint3'),
    ];
  }

  refreshList() {
    this.generateMockData();
    /*
    this.http.get(this.urlGetAccessLog)
    .subscribe ( {
      next: (result) => {
        this.accessLogList = result as AccessLog[];
      },
      error: (error) => {
        console.log(error);
      }
    })*/
  }

  watchForToken(): Observable<any> {
    return interval(1000).pipe(
      switchMap(() => this.http.get('http://localhost:5240/api/authentication/receivetoken'))
    );
  }
}
