import { Component, Inject, Input, OnInit } from '@angular/core';
import { DOCUMENT } from '@angular/common';

import { AuthService, User } from '@auth0/auth0-angular';

import { CurrentProject, Project } from '../project';
import { ProjectService } from '../project.service';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {

  @Input() project?: Project;

  constructor(
    public auth: AuthService,
    private currentProject: CurrentProject,
    public projectService: ProjectService,
    @Inject(DOCUMENT) public document: Document,
  ) { }

  ngOnInit(): void {
    this.auth.isAuthenticated$.subscribe(_ => {
      this.currentProject.id = 1;
      this.updateProject();
    });
  }

  updateProject(): void {
    this.projectService.getProject().subscribe(project => {
      this.project = project;
    });
  }
}
