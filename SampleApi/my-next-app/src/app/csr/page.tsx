"use client"; // 👈 Esto es obligatorio en Next.js 13+ en el App Router

import { useState, useEffect } from "react";

interface Post {
    id: number;
    title: string;
    body: string;
}

export default function CSRPage() {
    const [post, setPost] = useState<Post | null>(null);

    useEffect(() => {
        fetch("https://jsonplaceholder.typicode.com/posts/3")
            .then((res) => res.json())
            .then((data: Post) => setPost(data));
    }, []);

    return (
        <div>
            <h1>CSR: Client-Side Rendering</h1>
            {post ? (
                <>
                    <h2>{post.title}</h2>
                    <p>{post.body}</p>
                </>
            ) : (
                <p>Cargando...</p>
            )}
        </div>
    );
}
