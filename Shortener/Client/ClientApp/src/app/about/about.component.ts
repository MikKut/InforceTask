import { Component, OnInit } from '@angular/core';
import { AboutService } from '../services/about.service';

@Component({
  selector: 'app-about',
  templateUrl: './about.component.html',
  styleUrls: ['./about.component.css']
})
export class AboutComponent implements OnInit {
  aboutInfo: string;

  constructor(private aboutService: AboutService) { }

  ngOnInit() {
    this.aboutService.getAboutInfo().subscribe(data => {
      this.aboutInfo = data.aboutInfo;
    });
  }
}
