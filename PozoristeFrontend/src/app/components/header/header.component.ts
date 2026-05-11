import { Component, OnInit } from '@angular/core';
import { RouterLink, RouterLinkActive } from '@angular/router'
import { MatToolbarModule } from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';

@Component({
  selector: 'app-header',
  imports: [RouterLinkActive,RouterLink, MatButtonModule, MatToolbarModule, MatIconModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  constructor() { }

  ngOnInit() {
  }

}
