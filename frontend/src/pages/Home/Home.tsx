import { UseAuthenticationContext } from "@/components/AuthenticationContext";

export function Home() {
    const { currentUserInfo } = UseAuthenticationContext();
    const username = currentUserInfo?.claims?.find((claim: any) => claim.type === "username")?.value || "Guest";

    return (
        <p>Hello {username}</p>
    )
}