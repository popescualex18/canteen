import { Injectable } from '@angular/core';
import { HttpClient, HttpHeaders } from '@angular/common/http';
import { Observable } from 'rxjs';
import { environment } from 'src/enviroments/enviroments';

@Injectable({
  providedIn: 'root'
})
export class HttpService {
  url = `${environment.apiUrl}/canteen/`;
  constructor(private http: HttpClient) { }
  
  // Generic HTTP method for GET requests
  get<T>(endpoint: string): Observable<T> {
    return this.http.get<T>(`${this.url}${endpoint}`);
  }

  getImage(endpoint: string): Observable<Blob> {
    return this.http.get<Blob>(`${ this.url }${endpoint }`, {
      responseType: 'blob' as 'json' // Correctly specify responseType
    });
  }

  // Generic HTTP method for POST requests
  post<T>(endpoint: string, data: any): Observable<T> {
    const headers = new HttpHeaders({ 'Content-Type': 'application/json' });
    const url = `${this.url}${endpoint}`;
    return this.http.post<T>(url, data, { headers });
  }

  delete<T>(endpoint:string,): Observable<T> {
    const url = `${this.url}${endpoint}`;
    return this.http.delete<T>(url);
  }
}
