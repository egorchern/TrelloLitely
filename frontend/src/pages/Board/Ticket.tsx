import { Card, CardContent, CardFooter, CardHeader } from "@/components/ui/card";
import { Ticket } from "@/types/ticket";
import { useMemo } from "react";

export interface TicketProps {
    ticket: Ticket;
}

export default function TicketComponent({ticket} : TicketProps) {
    const truncatedContent = useMemo(() => {
        if (ticket.content.length > 100) {
            return ticket.content.slice(0, 100) + '...';
        }    
        return ticket.content;
    }, [ticket.content]);

    return (
        <Card>
            <CardHeader>
                <h3>{ticket.title}</h3>
            </CardHeader>
            <CardContent>
                <p>{truncatedContent}</p>
            </CardContent>
            <CardFooter>
                <p>Last Updated: {new Date(ticket.updatedAt).toISOString()}</p>
            </CardFooter>
        </Card>
    )
}