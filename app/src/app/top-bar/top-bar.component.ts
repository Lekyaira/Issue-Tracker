import { Component, Inject, OnInit } from '@angular/core';
import { DOCUMENT } from '@angular/common';

import { AuthService } from '@auth0/auth0-angular';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {

  constructor(
    public auth: AuthService,
    @Inject(DOCUMENT) public document: Document,
  ) { }

  ngOnInit(): void {
  }

}
