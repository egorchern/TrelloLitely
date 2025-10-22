import { Ticket } from "./ticket";

export interface Board {
    id: number;
    name: string;
    createdAt: string;
    updatedAt: string;
    tickets: Ticket[];
}