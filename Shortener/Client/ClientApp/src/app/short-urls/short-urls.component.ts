import { Component, OnInit } from '@angular/core';
import { UrlService } from '../services/url.service';
import { Url } from '../models/url.model';

@Component({
  selector: 'app-short-urls',
  templateUrl: './short-urls.component.html',
  styleUrls: ['./short-urls.component.css']
})
export class ShortUrlsComponent implements OnInit {
  urls: Url[] = [];

  constructor(private urlService: UrlService) { }

  ngOnInit(): void {
    this.urlService.getAll().subscribe(urls => {
      this.urls = urls;
    });
  }

  deleteUrl(id: number): void {
    this.urlService.delete(id).subscribe(() => {
      this.urls = this.urls.filter(url => url.id !== id);
    });
  }
}
