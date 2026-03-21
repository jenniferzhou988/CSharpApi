export interface Product {
  id?: number;
  productName: string;
  sku?: string;
  upc?: string;
  brandName?: string;
  price: number;
  description?: string;
  status?: number;
  created?: string;
  createdBy?: string;
  modified?: string;
  modifiedBy?: string;
  images?: ProductImage[];
  productCategoryLinks?: ProductCategoryLink[];
  productImportRecords?: ProductImportRecord[];
  productInventories?: ProductInventory[];
}

export interface ProductImage {
  id?: number;
  imageUrl: string;
  altText?: string;
  productId: number;
  status?: number;
}

export interface ProductCategory {
  id: number;
  categoryName: string;
  description?: string;
}

export interface ProductCategoryLink {
  id?: number;
  productId: number;
  productCategoryId: number;
  productCategory?: ProductCategory;
}

export interface ProductImportRecord {
  id?: number;
  productId: number;
  importPrice: number;
  quantity: number;
  comment?: string;
  status?: number;
  created?: string;
}

export interface ImportProductRequest {
  importPrice: number;
  quantity: number;
  comment?: string;
}

export interface ProductInventory {
  id?: number;
  productId: number;
  quantity: number;
  description?: string;
  comment?: string;
  status?: number;
  created?: string;
}
