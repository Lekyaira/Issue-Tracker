import { Issue } from "./issue";

export const ISSUES: Issue[] = [
    {id: 1, title: 'Test Issue 1', priority: 1, creator: 'Ryan', creatorId: 1,
        description: 'A test issue for testing things.'},
    {id: 2, title: 'Test Issue 2', priority: 3, creator: 'Ryan',  creatorId: 1,
        description: 'Another test issue for testing things'},
    {id: 3, title: 'Test Issue 3', priority: 2, creator: 'Ryan',  creatorId: 1},
    {id: 4, title: 'Test Issue 4', priority: 1, creator: 'Ryan',  creatorId: 1,
        description: 'A fourth test issue.'},
];