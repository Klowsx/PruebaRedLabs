export interface Product {
  id: string;
  nombre: string;
  descripcion?: string;
  precio: number;
  estado: boolean;
  usuarioCreacion: string;
  fechaCreacion: string;
  usuarioModificacion: string;
  fechaModificacion: string;
}

export interface ProductListResponse {
  data: Product[];
  total: number;
  pageNumber: number;
  pageSize: number;
  totalPages: number;
}

export interface CreateProductRequest {
  nombre: string;
  descripcion?: string;
  precio: number;
  estado: boolean;
}

export type UpdateProductRequest = CreateProductRequest;

export interface ProductFilters {
  nombre?: string;
  estado?: string;
  minPrecio?: string;
  maxPrecio?: string;
  pageNumber?: number;
  pageSize?: number;
}
