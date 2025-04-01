// src/app/isr/page.tsx

export default async function ISRPage() {
    // Fetch de datos del servidor con ISR
    const res = await fetch("https://jsonplaceholder.typicode.com/posts/2", { next: { revalidate: 10 } });
    const post = await res.json();

    return (
        <div>
            <h1>ISR: Incremental Static Regeneration</h1>
            <h2>{post.title}</h2>
            <p>{post.body}</p>
        </div>
    );
}
