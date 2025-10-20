import { useQuery } from "@tanstack/react-query";
import { LoginView } from "./LoginView";
import { APIGetAuthenticationInfo } from "../../api/auth/authentication";
import { AuthenticationInfo } from "../../types/authenticationInfo";
import { Spinner } from "@/components/ui/spinner"


export function Login() {
    const { data: authInfo, isLoading, isFetching, error } = useQuery<AuthenticationInfo, Error>({
        queryKey: ['auth'],
        queryFn: APIGetAuthenticationInfo,
    });
    const handleLogin = () => {
    }

    return (
        (isLoading || isFetching) ? (
            <Spinner />
        ) : (
            error ? (
                <div>Error: {error!.message}</div>
            ) : (
                !authInfo?.isAuthenticated && (
                    <LoginView onLogin={handleLogin} />
                )
            )
        )
    )
}