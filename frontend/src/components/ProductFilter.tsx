"use client";

import { useState } from "react";
import { ProductFilters } from "@/types/product";

interface ProductFilterProps {
  filters: ProductFilters;
  onChange: (filters: ProductFilters) => void;
}

export default function ProductFilter({ filters, onChange }: ProductFilterProps) {
  const [localFilters, setLocalFilters] = useState<ProductFilters>(filters);

  function handleSubmit(e: React.FormEvent<HTMLFormElement>) {
    e.preventDefault();
    onChange({ ...localFilters, pageNumber: 1 });
  }

  function handleClear() {
    const cleared: ProductFilters = { pageNumber: 1, pageSize: filters.pageSize };
    setLocalFilters(cleared);
    onChange(cleared);
  }

  return (
    <form onSubmit={handleSubmit} className="rounded-lg bg-white p-4 shadow">
      <div className="grid grid-cols-1 items-end gap-4 sm:grid-cols-2 lg:grid-cols-6">
        <div className="lg:col-span-2">
          <label htmlFor="nombre" className="block text-sm font-medium text-gray-700">
            Nombre
          </label>
          <input
            id="nombre"
            type="text"
            value={localFilters.nombre || ""}
            onChange={(e) => setLocalFilters({ ...localFilters, nombre: e.target.value })}
            className="mt-1 block w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 focus:border-blue-500 focus:outline-none"
          />
        </div>

        <div>
          <label htmlFor="estado" className="block text-sm font-medium text-gray-700">
            Estado
          </label>
          <select
            id="estado"
            value={localFilters.estado ?? ""}
            onChange={(e) => setLocalFilters({ ...localFilters, estado: e.target.value || undefined })}
            className="mt-1 block w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 focus:border-blue-500 focus:outline-none"
          >
            <option value="">Todos</option>
            <option value="true">Activo</option>
            <option value="false">Inactivo</option>
          </select>
        </div>

        <div>
          <label htmlFor="minPrecio" className="block text-sm font-medium text-gray-700">
            Precio mínimo
          </label>
          <input
            id="minPrecio"
            type="number"
            min={0}
            step={0.01}
            value={localFilters.minPrecio || ""}
            onChange={(e) => setLocalFilters({ ...localFilters, minPrecio: e.target.value })}
            className="mt-1 block w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 focus:border-blue-500 focus:outline-none"
          />
        </div>

        <div>
          <label htmlFor="maxPrecio" className="block text-sm font-medium text-gray-700">
            Precio máximo
          </label>
          <input
            id="maxPrecio"
            type="number"
            min={0}
            step={0.01}
            value={localFilters.maxPrecio || ""}
            onChange={(e) => setLocalFilters({ ...localFilters, maxPrecio: e.target.value })}
            className="mt-1 block w-full rounded-md border border-gray-300 bg-white px-3 py-2 text-sm text-gray-900 focus:border-blue-500 focus:outline-none"
          />
        </div>

        <div className="flex gap-2">
          <button
            type="submit"
            className="cursor-pointer rounded-md bg-blue-600 px-4 py-2 text-sm font-medium text-white hover:bg-blue-700"
          >
            Buscar
          </button>
          <button
            type="button"
            onClick={handleClear}
            className="cursor-pointer rounded-md border border-gray-300 bg-white px-4 py-2 text-sm font-medium text-gray-700 hover:bg-gray-50"
          >
            Limpiar
          </button>
        </div>
      </div>
    </form>
  );
}
