import { Ticket } from "./ticket";

export interface Board {
    id: number;
    name: string;
    createdAt: Date;
    updatedAt: Date;
    tickets: Ticket[];
}