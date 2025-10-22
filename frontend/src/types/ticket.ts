import { Board } from "./board";

export interface Ticket {
    id: number;
    title: string;
    content: string;
    boardId: number;
    createdAt: Date;
    updatedAt: Date;
    Board: Board;
}