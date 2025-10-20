import { useMutation, useQuery } from "@tanstack/react-query";
import { LoginView } from "./LoginView";
import { APIGetAuthenticationInfo, APILogin } from "../../api/auth/authentication";
import { AuthenticationInfo } from "../../types/authenticationInfo";
import { Spinner } from "@/components/ui/spinner"

interface LoginProps {
    onSuccessfulLogin: () => void;
    onShowRegister: () => void;
}

export function Login({onSuccessfulLogin, onShowRegister}: LoginProps) {
    const loginMutation = useMutation({
        mutationFn: ({ username, password }: { username: string; password: string }) => APILogin(username, password),
        onSuccess: onSuccessfulLogin
    })

    const handleLogin = async (username: string, password: string) => {
        loginMutation.mutate({ username, password });
    }

    return (
        <LoginView onLogin={handleLogin} 
        loginIsPending={loginMutation.isPending} 
        loginError={loginMutation.error?.message}
        onShowRegister={onShowRegister}
        />
    )
}