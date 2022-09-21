import { Component, OnInit } from '@angular/core';
import { CurrentProject, Project } from '../project';
import { ProjectService } from '../project.service';

@Component({
  selector: 'app-projects-view',
  templateUrl: './projects-view.component.html',
  styleUrls: ['./projects-view.component.css']
})
export class ProjectsViewComponent implements OnInit {

  projects: Project[] = [];
  editMode: boolean = false;

  constructor(
    private project: CurrentProject,
    private projectService: ProjectService,
  ) { }

  ngOnInit(): void {
    // Populatethe projects with user's projects in database
    this.projectService.getUserProjects().subscribe(projects => {
      this.projects = projects;
    })
  }

  toggleEdit(): void {
    this.editMode = !this.editMode;
  }

  deleteProject(project: Project): void {
    if(project.id){
      // Delete the category from database
      this.projectService.deleteProject(project.id).subscribe();
      // Remove the category from the displayed list
      this.projects = this.projects.filter(i => i !== project);
    }
  }
}
