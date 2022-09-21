import { Component, OnInit } from '@angular/core';
import { Category } from '../category';

import { CategoryService } from '../category.service';
import { CurrentProject } from '../project';

@Component({
  selector: 'app-categories-bar',
  templateUrl: './categories-bar.component.html',
  styleUrls: ['./categories-bar.component.css']
})
export class CategoriesBarComponent implements OnInit {

  categories: Category[] = [];

  constructor(
    private categoryService: CategoryService,
    private project: CurrentProject,
  ) { }

  ngOnInit(): void {
    this.categoryService.getCategories(this.project.id).subscribe(categories => this.categories = categories);
  }

  newCategory(): void {
    
  }

}
