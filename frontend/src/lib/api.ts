import { getToken, removeToken } from "./auth";

const API_BASE_URL = "http://localhost:5214/api";

interface ApiOptions extends Omit<RequestInit, "body"> {
  body?: object | null;
}

export async function apiFetch<T>(path: string, options: ApiOptions = {}): Promise<T> {
  const token = getToken();

  const headers = new Headers({ "Content-Type": "application/json" });

  if (options.headers) {
    const customHeaders = options.headers as Record<string, string>;
    Object.entries(customHeaders).forEach(([key, value]) => {
      headers.set(key, value);
    });
  }

  if (token) {
    headers.set("Authorization", `Bearer ${token}`);
  }

  const response = await fetch(`${API_BASE_URL}${path}`, {
    ...options,
    headers,
    body: options.body ? JSON.stringify(options.body) : undefined,
  });

  if (!response.ok) {
    if (response.status === 401) {
      removeToken();
      if (typeof window !== "undefined") {
        window.location.href = "/login";
      }
    }

    let message = `Error ${response.status}`;
    try {
      const errorBody = await response.json();
      message = errorBody.message || errorBody.title || message;
    } catch {
      // si no hay cuerpo JSON, dejamos el mensaje por defecto
    }
    throw new Error(message);
  }

  const contentType = response.headers.get("content-type");
  if (contentType && contentType.includes("application/json")) {
    return response.json() as Promise<T>;
  }

  return response as unknown as Promise<T>;
}
