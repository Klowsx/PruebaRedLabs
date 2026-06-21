"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import AuthForm from "@/components/AuthForm";
import { apiFetch } from "@/lib/api";
import { saveToken } from "@/lib/auth";
import { AuthResponse, RegisterRequest } from "@/types/auth";

export default function RegisterPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(data: { name?: string; email: string; password: string }) {
    setLoading(true);
    setError(null);

    try {
      const response = await apiFetch<AuthResponse>("/auth/register", {
        method: "POST",
        body: data as RegisterRequest,
      });
      saveToken(response.token);
      router.push("/products");
    } catch (err) {
      setError(err instanceof Error ? err.message : "Ocurrió un error");
    } finally {
      setLoading(false);
    }
  }

  return (
    <div className="flex min-h-screen items-center justify-center bg-gray-50 px-4">
      <div className="w-full max-w-md rounded-lg bg-white p-8 shadow">
        <h1 className="mb-6 text-2xl font-semibold text-gray-900">Crear cuenta</h1>
        <AuthForm mode="register" onSubmit={handleSubmit} loading={loading} error={error} />
        <p className="mt-4 text-center text-sm text-gray-600">
          ¿Ya tienes cuenta?{" "}
          <Link href="/login" className="text-blue-600 hover:underline">
            Inicia sesión
          </Link>
        </p>
      </div>
    </div>
  );
}
