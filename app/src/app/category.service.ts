import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';

import { Observable } from 'rxjs';

import { Category } from './category';

@Injectable({
  providedIn: 'root'
})
export class CategoryService {
  private categoriesUrl = "https://localhost:5001/api/category";

  constructor(
    private http: HttpClient,
  ) { }

  getCategories(project: number): Observable<Category[]> {
    return this.http.get<Category[]>(`${this.categoriesUrl}/project/${project}`);
  }

  getCategory(id: number): Observable<Category> {
    const url = `${this.categoriesUrl}/${id}`
    return this.http.get<Category>(url);
  }

  updateCategory(category: Category): Observable<any> {
    return this.http.put(this.categoriesUrl, category);
  }

  createCategory(category: Category): Observable<any> {
    return this.http.post(this.categoriesUrl, category);
  }

  deleteCategory(id: number): Observable<any> {
    const url = `${this.categoriesUrl}/${id}`;
    return this.http.delete(url);
  }
}
