import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { Url } from './url.model';

@Injectable({ providedIn: 'root' })
export class UrlService {
  constructor(private http: HttpClient) { }

  getAll(): Observable<Url[]> {
    return this.http.get<Url[]>(`api/url`);
  }

  get(id: number): Observable<Url> {
    return this.http.get<Url>(`api/url/${id}`);
  }

  shortenUrl(url: string): Observable<Url> {
    return this.http.post<Url>(`api/url/shorten`, { originalUrl: url });
  }

  delete(id: number) {
    return this.http.delete(`api/url/${id}`);
  }
}
