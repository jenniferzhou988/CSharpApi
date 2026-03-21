import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatListModule } from '@angular/material/list';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProductService } from '../../../core/services/product.service';
import { Product, ProductImage } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-image-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatIconModule,
    MatListModule,
    MatSnackBarModule
  ],
  templateUrl: './product-image-dialog.component.html',
  styleUrl: './product-image-dialog.component.scss'
})
export class ProductImageDialogComponent {
  product: Product;
  form: FormGroup;
  changed = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductImageDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product: Product }
  ) {
    this.product = { ...data.product };
    this.form = this.fb.group({
      imageUrl: ['', [Validators.required, Validators.maxLength(500)]],
      altText: ['', Validators.maxLength(200)]
    });
  }

  addImage(): void {
    if (this.form.invalid) return;

    const image: ProductImage = {
      ...this.form.value,
      productId: this.product.id!,
      status: 1
    };

    this.productService.addImage(this.product.id!, image).subscribe({
      next: (created) => {
        this.product.images = [...(this.product.images || []), created];
        this.form.reset();
        this.changed = true;
        this.snackBar.open('Image added', 'Close', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to add image', 'Close', { duration: 3000 })
    });
  }

  removeImage(image: ProductImage): void {
    this.productService.removeImage(this.product.id!, image.id!).subscribe({
      next: () => {
        this.product.images = this.product.images?.filter(i => i.id !== image.id);
        this.changed = true;
        this.snackBar.open('Image removed', 'Close', { duration: 2000 });
      },
      error: () => this.snackBar.open('Failed to remove image', 'Close', { duration: 3000 })
    });
  }

  close(): void {
    this.dialogRef.close(this.changed);
  }
}
