"use client";

import { useCallback, useEffect, useState } from "react";
import { useRouter } from "next/navigation";
import ProtectedRoute from "@/components/ProtectedRoute";
import ProductFilter from "@/components/ProductFilter";
import ProductList from "@/components/ProductList";
import ProductModal from "@/components/ProductModal";
import Pagination from "@/components/Pagination";
import { apiFetch } from "@/lib/api";
import { removeToken } from "@/lib/auth";
import { Product, ProductFilters, ProductListResponse } from "@/types/product";

function ProductsPage() {
  const router = useRouter();
  const [products, setProducts] = useState<ProductListResponse | null>(null);
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);
  const [filters, setFilters] = useState<ProductFilters>({
    pageNumber: 1,
    pageSize: 10,
  });
  const [isModalOpen, setIsModalOpen] = useState(false);
  const [selectedProduct, setSelectedProduct] = useState<Product | null>(null);
  const [saving, setSaving] = useState(false);
  const [modalError, setModalError] = useState<string | null>(null);

  const loadProducts = useCallback(async () => {
    setLoading(true);
    setError(null);

    try {
      const params = new URLSearchParams();
      if (filters.nombre) params.set("nombre", filters.nombre);
      if (filters.estado) params.set("estado", filters.estado);
      if (filters.minPrecio) params.set("minPrecio", filters.minPrecio);
      if (filters.maxPrecio) params.set("maxPrecio", filters.maxPrecio);
      params.set("pageNumber", filters.pageNumber?.toString() || "1");
      params.set("pageSize", filters.pageSize?.toString() || "10");

      const query = params.toString() ? `?${params.toString()}` : "";
      const response = await apiFetch<ProductListResponse>(`/products${query}`);
      setProducts(response);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error al cargar productos");
    } finally {
      setLoading(false);
    }
  }, [filters]);

  useEffect(() => {
    loadProducts();
  }, [loadProducts]);

  function openNewModal() {
    setSelectedProduct(null);
    setModalError(null);
    setIsModalOpen(true);
  }

  function openEditModal(product: Product) {
    setSelectedProduct(product);
    setModalError(null);
    setIsModalOpen(true);
  }

  function closeModal() {
    setIsModalOpen(false);
    setSelectedProduct(null);
    setModalError(null);
  }

  async function handleSave(data: {
    nombre: string;
    descripcion: string;
    precio: number;
    estado: boolean;
  }) {
    setSaving(true);
    setModalError(null);

    try {
      if (selectedProduct) {
        await apiFetch(`/products/${selectedProduct.id}`, {
          method: "PUT",
          body: data,
        });
      } else {
        await apiFetch("/products", {
          method: "POST",
          body: data,
        });
      }

      closeModal();
      loadProducts();
    } catch (err) {
      setModalError(err instanceof Error ? err.message : "Error al guardar");
    } finally {
      setSaving(false);
    }
  }

  async function handleDelete(id: string) {
    if (!confirm("¿Eliminar este producto?")) {
      return;
    }

    try {
      await apiFetch(`/products/${id}`, { method: "DELETE" });
      loadProducts();
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error al eliminar");
    }
  }

  async function handleDownloadReport() {
    try {
      const params = new URLSearchParams();
      if (filters.nombre) params.set("nombre", filters.nombre);
      if (filters.estado) params.set("estado", filters.estado);
      if (filters.minPrecio) params.set("minPrecio", filters.minPrecio);
      if (filters.maxPrecio) params.set("maxPrecio", filters.maxPrecio);

      const query = params.toString() ? `?${params.toString()}` : "";
      const response = await fetch(`http://localhost:5214/api/products/report${query}`, {
        headers: {
          Authorization: `Bearer ${localStorage.getItem("pm_token") || ""}`,
        },
      });

      if (!response.ok) {
        throw new Error("Error al descargar el reporte");
      }

      const blob = await response.blob();
      const url = window.URL.createObjectURL(blob);
      const link = document.createElement("a");
      link.href = url;
      link.download = `reporte-productos-${new Date().toISOString().slice(0, 10)}.pdf`;
      document.body.appendChild(link);
      link.click();
      document.body.removeChild(link);
      window.URL.revokeObjectURL(url);
    } catch (err) {
      setError(err instanceof Error ? err.message : "Error al descargar");
    }
  }

  function handleLogout() {
    removeToken();
    router.replace("/login");
  }

  return (
    <div className="min-h-screen bg-gray-50">
      <header className="bg-white shadow">
        <div className="mx-auto flex max-w-7xl items-center justify-between px-4 py-4">
          <h1 className="text-xl font-semibold text-gray-900">ProductManager</h1>
          <button
            onClick={handleLogout}
            className="cursor-pointer rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
          >
            Cerrar sesión
          </button>
        </div>
      </header>

      <main className="mx-auto max-w-7xl px-4 py-6">
        <div className="mb-6 flex items-center justify-between">
          <h2 className="text-lg font-medium text-gray-900">Gestión de productos</h2>
          <div className="flex gap-2">
            <button
              onClick={handleDownloadReport}
              className="cursor-pointer rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
            >
              Descargar reporte
            </button>
            <button
              onClick={openNewModal}
              className="cursor-pointer rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700"
            >
              Nuevo producto
            </button>
          </div>
        </div>

        <div className="mb-6">
          <ProductFilter filters={filters} onChange={setFilters} />
        </div>

        {error && (
          <div className="mb-4 rounded-md bg-red-50 p-4 text-sm text-red-700">
            {error}
          </div>
        )}

        {loading && <p className="text-gray-600">Cargando productos...</p>}

        {!loading && products && (
          <>
            <ProductList
              products={products.data}
              onEdit={openEditModal}
              onDelete={handleDelete}
            />
            <div className="mt-4">
              <Pagination
                pageNumber={products.pageNumber}
                totalPages={products.totalPages}
                onChange={(page) => setFilters({ ...filters, pageNumber: page })}
              />
            </div>
          </>
        )}
      </main>

      <ProductModal
        product={selectedProduct}
        isOpen={isModalOpen}
        onClose={closeModal}
        onSave={handleSave}
        loading={saving}
        error={modalError}
      />
    </div>
  );
}

export default function ProductsPageWrapper() {
  return (
    <ProtectedRoute>
      <ProductsPage />
    </ProtectedRoute>
  );
}
