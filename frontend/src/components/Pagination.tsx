"use client";

interface PaginationProps {
  pageNumber: number;
  totalPages: number;
  onChange: (page: number) => void;
}

export default function Pagination({ pageNumber, totalPages, onChange }: PaginationProps) {
  return (
    <div className="flex items-center justify-between rounded-lg bg-white p-4 shadow">
      <p className="text-sm text-gray-700">
        Página {pageNumber} de {totalPages}
      </p>
      <div className="flex gap-2">
        <button
          onClick={() => onChange(pageNumber - 1)}
          disabled={pageNumber <= 1}
          className="cursor-pointer rounded-md border border-gray-300 bg-white px-3 py-1 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:cursor-not-allowed disabled:opacity-50"
        >
          Anterior
        </button>
        <button
          onClick={() => onChange(pageNumber + 1)}
          disabled={pageNumber >= totalPages}
          className="cursor-pointer rounded-md border border-gray-300 bg-white px-3 py-1 text-sm font-medium text-gray-700 hover:bg-gray-50 disabled:cursor-not-allowed disabled:opacity-50"
        >
          Siguiente
        </button>
      </div>
    </div>
  );
}
