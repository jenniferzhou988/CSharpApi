import { Injectable } from '@angular/core';
import { HttpClient } from '@angular/common/http';
import { Observable } from 'rxjs';
import { API_ENDPOINTS } from '../constants/api-endpoints';
import {
  Product,
  ProductImage,
  ProductCategoryLink,
  ProductImportRecord,
  ImportProductRequest
} from '../models/product.model';

@Injectable({ providedIn: 'root' })
export class ProductService {

  constructor(private http: HttpClient) {}

  getAll(): Observable<Product[]> {
    return this.http.get<Product[]>(API_ENDPOINTS.product.getAll);
  }

  getById(id: number): Observable<Product> {
    return this.http.get<Product>(API_ENDPOINTS.product.getById(id));
  }

  create(product: Product): Observable<Product> {
    return this.http.post<Product>(API_ENDPOINTS.product.create, product);
  }

  update(product: Product): Observable<Product> {
    return this.http.put<Product>(API_ENDPOINTS.product.update(product.id!), product);
  }

  delete(id: number): Observable<void> {
    return this.http.delete<void>(API_ENDPOINTS.product.delete(id));
  }

  // Images
  addImage(productId: number, image: ProductImage): Observable<ProductImage> {
    return this.http.post<ProductImage>(API_ENDPOINTS.product.addImage(productId), image);
  }

  removeImage(productId: number, imageId: number): Observable<void> {
    return this.http.delete<void>(API_ENDPOINTS.product.removeImage(productId, imageId));
  }

  // Categories
  addCategory(productId: number, categoryId: number): Observable<ProductCategoryLink> {
    return this.http.post<ProductCategoryLink>(
      API_ENDPOINTS.product.addCategory(productId, categoryId), {}
    );
  }

  removeCategory(productId: number, categoryId: number): Observable<void> {
    return this.http.delete<void>(API_ENDPOINTS.product.removeCategory(productId, categoryId));
  }

  // Import
  importProduct(productId: number, request: ImportProductRequest): Observable<ProductImportRecord> {
    return this.http.post<ProductImportRecord>(
      API_ENDPOINTS.product.importProduct(productId), request
    );
  }
}
