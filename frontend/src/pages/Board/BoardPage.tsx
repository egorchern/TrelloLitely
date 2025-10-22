import { APIGetBoard } from "@/api/Boards/board";
import { Board } from "@/types/board";
import { useQuery } from "@tanstack/react-query";
import { useParams } from "react-router-dom";

interface BoardPageURLParams extends Record<string, string | undefined> {
    boardId: string;
}

export default function BoardPage() {
    const { boardId } = useParams<BoardPageURLParams>();
    const { data, error, isLoading } = useQuery<Board, Error>({
        queryKey: [`board/${boardId}`],
        queryFn: APIGetBoard(boardId!),
    });
    
    return (
        <div>Board: {boardId}</div>
    )
}