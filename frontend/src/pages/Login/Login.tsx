import { useMutation, useQuery } from "@tanstack/react-query";
import { LoginView } from "./LoginView";
import { APIGetAuthenticationInfo, APILogin } from "../../api/auth/authentication";
import { AuthenticationInfo } from "../../types/authenticationInfo";
import { Spinner } from "@/components/ui/spinner"


export function Login() {
    const { data: authInfo, isLoading, isFetching, error } = useQuery<AuthenticationInfo, Error>({
        queryKey: ['auth'],
        queryFn: APIGetAuthenticationInfo,
    });
    const loginMutation = useMutation({
        mutationFn: ({ username, password }: { username: string; password: string }) => APILogin(username, password),
    })

    const handleLogin = (username: string, password: string) => {
        loginMutation.mutate({ username, password });
    }
    console.log(loginMutation.error?.message);

    return (
        (isLoading || isFetching) ? (
            <Spinner />
        ) : (
            error ? (
                <div>Error: {error!.message}</div>
            ) : (
                !authInfo?.isAuthenticated && (
                    <LoginView onLogin={handleLogin} 
                    loginIsPending={loginMutation.isPending} 
                    loginError={loginMutation.error?.message}/>
                )
            )
        )
    )
}