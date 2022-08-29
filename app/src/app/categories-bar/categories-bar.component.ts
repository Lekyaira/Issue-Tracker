import { Component, OnInit } from '@angular/core';
import { Category } from '../category';

import { IssueService } from '../issue.service';

@Component({
  selector: 'app-categories-bar',
  templateUrl: './categories-bar.component.html',
  styleUrls: ['./categories-bar.component.css']
})
export class CategoriesBarComponent implements OnInit {

  categories: Category[] = [];

  constructor(
    private issueService: IssueService
  ) { }

  ngOnInit(): void {
    this.issueService.getCategories().subscribe(categories => this.categories = categories);
  }

  newCategory(): void {
    
  }

}
