import { Component, OnInit } from '@angular/core';

import { CategoryService } from '../category.service';
import { Category } from '../category';

@Component({
  selector: 'app-categories-view',
  templateUrl: './categories-view.component.html',
  styleUrls: ['./categories-view.component.css']
})
export class CategoriesViewComponent implements OnInit {

  categories: Category[] = []
  editMode: boolean = false;

  constructor(
    private categoryService: CategoryService
  ) { }

  ngOnInit(): void {
    this.categoryService.getCategories().subscribe(categories => this.categories = categories);
  }

  toggleEdit(): void {
    this.editMode = !this.editMode;
  }

  deleteCategory(category: Category): void {
    if(category.id){
      // Delete the category from database
      this.categoryService.deleteCategory(category.id).subscribe();
      // Remove the category from the displayed list
      this.categories = this.categories.filter(i => i !== category);
    }
  }

}
