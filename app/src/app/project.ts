import { Injectable } from "@angular/core";
import { User } from "@auth0/auth0-angular";

@Injectable()
export class CurrentProject {
    id: number = 0;
}

export interface Project {
    id: number,
    name: string,
    owner: number,
    users: number[],
}