import { Component, ComponentRef, Injectable } from "@angular/core";
import { Observable } from "rxjs";
import { ProjectService } from "./project.service";
import { TopBarComponent } from "./top-bar/top-bar.component";

@Injectable()
export class CurrentProject {
    id: number = 0;
    project?: Project;
    topBar?: TopBarComponent;

    constructor(
        private projectService: ProjectService,
    ) {}

    updateProject(): Observable<Project>;
    updateProject(id: number): Observable<Project>;
    updateProject(id?: number): Observable<Project> {
        // If we're given an id, update the stored id
        if(id !== undefined){
            this.id = id;
        }
        // Get the project data from the project service
        const projectObserver: Observable<Project> = this.projectService.getProject(this.id);
        // Save out our project variable
        projectObserver.subscribe(project => {
            this.project = project;
        })
        // Return the Observable project
        return projectObserver;
    }
}

export interface Project {
    id?: number,
    name: string,
    owner: number,
    users: number[],
}