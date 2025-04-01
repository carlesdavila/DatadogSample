// src/app/ssr/page.tsx

export default async function SSRPage() {
  // Fetch de datos del servidor (siempre se ejecuta en el servidor)
  const res = await fetch("https://jsonplaceholder.typicode.com/posts/1", { cache: "no-store" });
  const post = await res.json();

  return (
      <div>
        <h1>SSR: Server-Side Rendering</h1>
        <h2>{post.title}</h2>
        <p>{post.body}</p>
      </div>
  );
}
