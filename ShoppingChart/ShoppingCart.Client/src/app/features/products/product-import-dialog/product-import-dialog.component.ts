import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ReactiveFormsModule, FormBuilder, FormGroup, Validators } from '@angular/forms';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatFormFieldModule } from '@angular/material/form-field';
import { MatInputModule } from '@angular/material/input';
import { MatButtonModule } from '@angular/material/button';
import { MatTableModule } from '@angular/material/table';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { ProductService } from '../../../core/services/product.service';
import { Product, ProductImportRecord } from '../../../core/models/product.model';

@Component({
  selector: 'app-product-import-dialog',
  standalone: true,
  imports: [
    CommonModule,
    ReactiveFormsModule,
    MatDialogModule,
    MatFormFieldModule,
    MatInputModule,
    MatButtonModule,
    MatTableModule,
    MatSnackBarModule
  ],
  templateUrl: './product-import-dialog.component.html',
  styleUrl: './product-import-dialog.component.scss'
})
export class ProductImportDialogComponent {
  product: Product;
  form: FormGroup;
  importRecords: ProductImportRecord[] = [];
  displayedColumns = ['importPrice', 'quantity', 'comment', 'created'];
  changed = false;

  constructor(
    private fb: FormBuilder,
    private productService: ProductService,
    private snackBar: MatSnackBar,
    private dialogRef: MatDialogRef<ProductImportDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: { product: Product }
  ) {
    this.product = data.product;
    this.importRecords = [...(this.product.productImportRecords || [])];
    this.form = this.fb.group({
      importPrice: [0, [Validators.required, Validators.min(0.01)]],
      quantity: [1, [Validators.required, Validators.min(1)]],
      comment: ['']
    });
  }

  onImport(): void {
    if (this.form.invalid) return;

    this.productService.importProduct(this.product.id!, this.form.value).subscribe({
      next: (record) => {
        this.importRecords = [...this.importRecords, record];
        this.form.reset({ importPrice: 0, quantity: 1, comment: '' });
        this.changed = true;
        this.snackBar.open('Import recorded', 'Close', { duration: 2000 });
      },
      error: () => this.snackBar.open('Import failed', 'Close', { duration: 3000 })
    });
  }

  close(): void {
    this.dialogRef.close(this.changed);
  }
}
