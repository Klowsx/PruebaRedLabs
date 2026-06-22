"use client";

import { Product } from "@/types/product";

interface ProductListProps {
  products: Product[];
  onEdit: (product: Product) => void;
  onDelete: (id: string) => void;
}

export default function ProductList({
  products,
  onEdit,
  onDelete,
}: ProductListProps) {
  if (products.length === 0) {
    return (
      <div className="rounded-lg bg-white p-8 text-center shadow">
        <p className="text-gray-600">No se encontraron productos.</p>
      </div>
    );
  }

  return (
    <div className="overflow-x-auto rounded-lg bg-white shadow">
      <table className="min-w-full divide-y divide-gray-200">
        <thead className="bg-gray-50">
          <tr>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Nombre
            </th>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Descripción
            </th>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Precio
            </th>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Estado
            </th>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Modificado por
            </th>
            <th className="px-4 py-3 text-left text-xs font-medium uppercase text-gray-500">
              Fecha
            </th>
            <th className="px-4 py-3 text-center text-xs font-medium uppercase text-gray-500">
              Acciones
            </th>
          </tr>
        </thead>
        <tbody className="divide-y divide-gray-200">
          {products.map((product) => (
            <tr key={product.id} className="hover:bg-gray-50">
              <td className="whitespace-nowrap px-4 py-3 text-sm font-medium text-gray-900">
                {product.nombre}
              </td>
              <td className="px-4 py-3 text-sm text-gray-700">
                {product.descripcion || "—"}
              </td>
              <td className="whitespace-nowrap px-4 py-3 text-sm text-gray-700">
                ${product.precio.toFixed(2)}
              </td>
              <td className="whitespace-nowrap px-4 py-3 text-sm">
                <span
                  className={`inline-flex rounded-full px-2 py-1 text-xs font-medium ${
                    product.estado
                      ? "bg-green-100 text-green-800"
                      : "bg-red-100 text-red-800"
                  }`}
                >
                  {product.estado ? "Activo" : "Inactivo"}
                </span>
              </td>
              <td className="whitespace-nowrap px-4 py-3 text-sm text-gray-700">
                {product.usuarioModificacion}
              </td>
              <td className="whitespace-nowrap px-4 py-3 text-sm text-gray-700">
                {new Date(product.fechaModificacion).toLocaleDateString()}
              </td>
              <td className="whitespace-nowrap px-4 py-3 text-right text-sm font-medium">
                <button
                  onClick={() => onEdit(product)}
                  className="mr-3 cursor-pointer text-blue-600 hover:text-blue-900"
                >
                  Editar
                </button>
                <button
                  onClick={() => onDelete(product.id)}
                  className="cursor-pointer text-red-600 hover:text-red-900"
                >
                  Eliminar
                </button>
              </td>
            </tr>
          ))}
        </tbody>
      </table>
    </div>
  );
}
