import { useMutation } from "@tanstack/react-query";
import { RegisterView } from "./RegisterView";
import { APIRegister } from "../../api/auth/authentication";

interface RegisterProps {
    onSuccessfulRegister?: () => void;
    onShowLogin: () => void;
}

export function Register({onSuccessfulRegister, onShowLogin}: RegisterProps) {
    const registerMutation = useMutation({
        mutationFn: ({ username, email, password, tenantId }: { username: string; email: string; password: string; tenantId: string }) => 
            APIRegister(username, email, password, tenantId),
        onSuccess: () => {
            // Optionally redirect to login or call a success callback
            if (onSuccessfulRegister) {
                onSuccessfulRegister();
            } else {
                // Default behavior: go back to login
                onShowLogin();
            }
        }
    })

    const handleRegister = async (username: string, email: string, password: string, tenantId: string) => {
        registerMutation.mutate({ username, email, password, tenantId });
    }

    return (
        <RegisterView 
            onRegister={handleRegister} 
            registerIsPending={registerMutation.isPending} 
            registerError={registerMutation.error?.message}
            onShowLogin={onShowLogin}
        />
    )
}
