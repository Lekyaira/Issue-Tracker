import { Component, Inject, Input, OnInit } from '@angular/core';
import { DOCUMENT } from '@angular/common';

import { AuthService, User } from '@auth0/auth0-angular';

import { CurrentProject, Project } from '../project';
import { ProjectService } from '../project.service';
import { Router } from '@angular/router';

@Component({
  selector: 'app-top-bar',
  templateUrl: './top-bar.component.html',
  styleUrls: ['./top-bar.component.css']
})
export class TopBarComponent implements OnInit {

  @Input() projects?: Project[];
  projectName: string = "";

  constructor(
    public auth: AuthService,
    public currentProject: CurrentProject,
    public projectService: ProjectService,
    private router: Router,
    @Inject(DOCUMENT) public document: Document,
  ) { 
    this.router.routeReuseStrategy.shouldReuseRoute = () => false;
  }

  ngOnInit(): void {
    // Add reference to this component in the current project
    this.currentProject.topBar = this;
    // Make sure the user is authenticated
    this.auth.isAuthenticated$.subscribe(_ => {
      // Once the user is authenticated, load the projects list
      this.loadList();
    });
  }

  updateProject(id: number, name: string):void {
    // Update the dropdown's selected name with the newly selected item's name
    this.projectName = name;
    // Update currentProject with the new id
    this.currentProject.updateProject(id).subscribe(_ => {
      // Once we've loaded the new project data, reload attached pages with the new data
      // If we're on /dashboard, /issues or /categories, just reload the page
      if(this.router.url == '/dashboard' 
          || this.router.url == '/issues' 
          || this.router.url == '/categories'
          || this.router.url == '/projects'){
        this.router.navigate([this.router.url]);  
      } else {
        // Otherwise take us back to dashboard
        this.router.navigate(['/dashboard']);
      }
    });
  }

  loadList(): void {
    // Get all the user's projects
    this.projectService.getUserProjects().subscribe(projects => {
      console.log(projects);
      // Update our list of projects with the user's projects from database
      this.projects = projects;
      // Set the current project to the first in the list and update the project data
      this.updateProject(this.projects[0].id!, this.projects[0].name);
    });
  }
}
