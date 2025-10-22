import { Board } from "@/types/board";

async function APIGetBoard(boardId: string) {
    let request = fetch(`/api/boards/${boardId}`, {
        method: 'GET',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        }
    });

    let data = await request;

    if (!data.ok) {
        throw new Error('Failed to fetch board');
    }

    return data.json() as Promise<Board>;
}

export { APIGetBoard };