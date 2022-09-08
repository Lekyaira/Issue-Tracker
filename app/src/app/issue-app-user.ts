import { Issue } from "src/app/issue";
import { AppUser } from "src/app/app-user";

export interface IssueAppUser {
    issue: Issue;
    user: AppUser;
}