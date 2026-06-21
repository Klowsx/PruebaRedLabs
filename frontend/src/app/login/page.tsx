"use client";

import { useState } from "react";
import { useRouter } from "next/navigation";
import Link from "next/link";
import AuthForm from "@/components/AuthForm";
import { apiFetch } from "@/lib/api";
import { saveToken } from "@/lib/auth";
import { AuthResponse, LoginRequest } from "@/types/auth";

export default function LoginPage() {
  const router = useRouter();
  const [loading, setLoading] = useState(false);
  const [error, setError] = useState<string | null>(null);

  async function handleSubmit(data: { email: string; password: string }) {
    setLoading(true);
    setError(null);

    try {
      const response = await apiFetch<AuthResponse>("/auth/login", {
        method: "POST",
        body: data as LoginRequest,
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
        <h1 className="mb-6 text-2xl font-semibold text-gray-900">Iniciar sesión</h1>
        <AuthForm mode="login" onSubmit={handleSubmit} loading={loading} error={error} />
        <p className="mt-4 text-center text-sm text-gray-600">
          ¿No tienes cuenta?{" "}
          <Link href="/register" className="text-blue-600 hover:underline">
            Regístrate
          </Link>
        </p>
      </div>
    </div>
  );
}
