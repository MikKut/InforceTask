import { Component, OnInit } from '@angular/core';
import { ActivatedRoute } from '@angular/router';
import { UrlService } from '../services/url.service';
import { Url } from '../models/url.model';

@Component({
  selector: 'app-url-info',
  templateUrl: './url-info.component.html',
  styleUrls: ['./url-info.component.css']
})
export class UrlInfoComponent implements OnInit {
  url: Url;

  constructor(private route: ActivatedRoute, private urlService: UrlService) { }

  ngOnInit(): void {
    const id = +this.route.snapshot.paramMap.get('id');
    this.urlService.get(id).subscribe(url => {
      this.url = url;
    });
  }
}
