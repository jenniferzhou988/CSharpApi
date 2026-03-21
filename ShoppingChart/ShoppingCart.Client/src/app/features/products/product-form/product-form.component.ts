import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { ActivatedRoute, Router } from '@angular/router';
import { MatCardModule } from '@angular/material/card';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProductService } from '../../../core/services/product.service';

@Component({
  selector: 'app-product-form',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatCardModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatSnackBarModule
  ],
  templateUrl: './product-form.component.html',
  styleUrl: './product-form.component.scss'
})
export class ProductFormComponent implements OnInit {
  form: FormGroup;
  isEdit = false;
  productId: number | null = null;
  loading = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private route: ActivatedRoute,
    private router: Router,
    private snackBar: MatSnackBar
  ) {
    this.form = this.fb.group({
      productName: ['', [Validators.required, Validators.maxLength(200)]],
      sku: ['', Validators.maxLength(50)],
      upc: ['', Validators.maxLength(12)],
      brandName: ['', Validators.maxLength(200)],
      price: [0, [Validators.required, Validators.min(0)]],
      description: ['', Validators.maxLength(1000)]
    });
  }

  ngOnInit(): void {
    const idParam = this.route.snapshot.paramMap.get('id');
    if (idParam) {
      this.isEdit = true;
      this.productId = +idParam;
      this.loadProduct(this.productId);
    }
  }

  loadProduct(id: number): void {
    this.loading = true;
    this.productService.getById(id).subscribe({
      next: (product) => {
        this.form.patchValue({
          productName: product.productName,
          sku: product.sku,
          upc: product.upc,
          brandName: product.brandName,
          price: product.price,
          description: product.description
        });
        this.loading = false;
      },
      error: () => {
        this.snackBar.open('Failed to load product', 'Close', { duration: 3000 });
        this.router.navigate(['/products']);
      }
    });
  }

  onSubmit(): void {
    if (this.form.invalid) return;

    this.loading = true;
    const product = { ...this.form.value, status: 1 };

    if (this.isEdit) {
      product.id = this.productId;
      this.productService.update(product).subscribe({
        next: () => {
          this.snackBar.open('Product updated', 'Close', { duration: 3000 });
          this.router.navigate(['/products']);
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Update failed', 'Close', { duration: 3000 });
        }
      });
    } else {
      this.productService.create(product).subscribe({
        next: () => {
          this.snackBar.open('Product created', 'Close', { duration: 3000 });
          this.router.navigate(['/products']);
        },
        error: () => {
          this.loading = false;
          this.snackBar.open('Create failed', 'Close', { duration: 3000 });
        }
      });
    }
  }

  cancel(): void {
    this.router.navigate(['/products']);
  }
}
