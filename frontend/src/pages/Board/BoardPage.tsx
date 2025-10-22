import { APIGetBoard } from "@/api/Boards/board";
import { Board } from "@/types/board";
import { useQuery } from "@tanstack/react-query";
import { data, useParams } from "react-router-dom";
import BoardComponent from "./BoardComponent";
import * as signalR from "@microsoft/signalr";
import { useEffect, useState } from "react";

interface BoardPageURLParams extends Record<string, string | undefined> {
    boardId: string;
}

export default function BoardPage() {
    const { boardId: boardIdAsString } = useParams<BoardPageURLParams>();
    const boardId = Number(boardIdAsString);
    const { data: board, error, isLoading, refetch } = useQuery<Board, Error>({
        queryKey: [`board/${boardId}`],
        queryFn: () => APIGetBoard(boardId)
    });
    const [ticketNotificationsConnection, setTicketNotificationsConnection] = useState<any>(null);

    useEffect(() => {
        // Create connection
        const newConnection = new signalR.HubConnectionBuilder()
        .withUrl("http://localhost:5000/hubs/ticketNotifications", {
            withCredentials: true
        })
        .withAutomaticReconnect()
        .build();

        // Start connection
        newConnection
        .start()
        .then(() => {
            newConnection.on("Modified", (message) => {
                console.log(message);
                refetch();
            })

            setTicketNotificationsConnection(newConnection);
        });

        return () => {
            newConnection.stop();
        }
    }, [])
    
    return (
        error ? (
            <div>Error: {error.message}</div>
        ) : isLoading ? (
            <div>Loading...</div>
        ) : (
            <BoardComponent board={board!}>
                
            </BoardComponent>
        )
    )
}