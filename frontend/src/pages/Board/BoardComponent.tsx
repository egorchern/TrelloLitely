import { Board } from "@/types/board";
import { useMemo } from "react";
import TicketComponent from "./Ticket";
import styles from "./BoardPage.module.css";
import { UseGlobalFilterContext } from "@/components/GlobalFilterContext";

export interface BoardComponentProps {
    board: Board;
}

export default function BoardComponent({ board }: BoardComponentProps) {
    const createdAt = new Date(board.createdAt).toISOString();
    const updatedAt = new Date(board.updatedAt).toISOString();
    const {filterText} = UseGlobalFilterContext();

    const sortedFilteredTickets = useMemo(() => {
        const regex = new RegExp(filterText, "i");
        let filtered = board.tickets.filter(ticket => {
           if (regex.test(ticket.title) || regex.test(ticket.content)) {
               return true;
           }

           return false;
        });
        filtered.sort((a, b) => new Date(b.updatedAt).getTime() - new Date(a.updatedAt).getTime());
        return filtered;
    }, [board.tickets, filterText]);

    const tickets = sortedFilteredTickets.map((ticket) => {
        return (
            <TicketComponent key={ticket.id} ticket={ticket} />
        )
    });

    return (
        <div>
            <h1>{board.name}</h1>
            <p>Board ID: {board.id}</p>
            <p>Created At: {createdAt}</p>
            <p>Updated At: {updatedAt}</p>
            <h2>Tickets</h2>
            <div className={styles.ticketsContainer}>
                
                {tickets}
            </div>
        </div>
        
    )
}