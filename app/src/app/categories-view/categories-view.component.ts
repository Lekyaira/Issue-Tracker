import { Component, OnInit } from '@angular/core';

import { IssueService } from '../issue.service';
import { Category } from '../category';

@Component({
  selector: 'app-categories-view',
  templateUrl: './categories-view.component.html',
  styleUrls: ['./categories-view.component.css']
})
export class CategoriesViewComponent implements OnInit {

  categories: Category[] = []

  constructor(
    private issueService: IssueService
  ) { }

  ngOnInit(): void {
    this.issueService.getCategories().subscribe(categories => this.categories = categories);
  }

}
