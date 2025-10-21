import { APIGetAuthenticationInfo } from "@/api/auth/authentication";
import { Spinner } from "@/components/ui/spinner";
import { AuthenticationInfo } from "@/types/authenticationInfo";
import { useQuery } from "@tanstack/react-query";
import { useState } from "react";
import { Login } from "./Login";
import { Register } from "./Register";
import { AuthenticationPageContextProvider } from "./AuthenticationPageContext";

export function AuthenticationPage() {
    const [showLogin, setShowLogin] = useState(true);
    const { data: authInfo, isLoading, isFetching, error, refetch } = useQuery<AuthenticationInfo, Error>({
        queryKey: ['auth'],
        queryFn: APIGetAuthenticationInfo,
    });

    const handleSuccessfulLogin = () => {
        refetch();
    }
    
    return (
        <AuthenticationPageContextProvider showLogin={() => setShowLogin(true)} showRegister={() => setShowLogin(false)}>
            {(isLoading || isFetching) ? (
                <Spinner id="auth-page-spinner" />
            ) : (
                error ? (
                    <div>Error: {error!.message}</div>
                ) : (
                    !authInfo?.isAuthenticated ? (
                        showLogin ? (
                            <Login onSuccessfulLogin={handleSuccessfulLogin} 
                             />
                        )
                        : (
                            <Register />
                        )
                    ) : (
                        <div>Welcome, you are logged in!</div>
                    )
                )
            )}
        </AuthenticationPageContextProvider>
    )
}
