export class AccessLog {
    timestamp: Date;
    username: string;
    endpoint: string;
  
    constructor(timestamp: Date, username: string, endpoint: string) {
      this.timestamp = timestamp;
      this.username = username;
      this.endpoint = endpoint;
    }
  }
  