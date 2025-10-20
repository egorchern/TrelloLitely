import { createContext, useCallback, useMemo } from "react";

export interface AuthenticationPageContextType {
    showLogin: () => void;
    showRegister: () => void;
}

interface AuthenticationPageContextProviderProps {
    children: React.ReactNode;
    showLogin: () => void;
    showRegister: () => void;
}

export const AuthenticationPageContext = createContext<AuthenticationPageContextType | null>(null);

export function AuthenticationPageContextProvider({ children, showLogin, showRegister }: AuthenticationPageContextProviderProps) {
    const showLoginFunc = useCallback(() => showLogin(), [showLogin]);
    const showRegisterFunc = useCallback(() => showRegister(), [showRegister]);
    const value = useMemo(() => ({ "showLogin": showLoginFunc, "showRegister": showRegisterFunc }), [showLoginFunc, showRegisterFunc]);
    
    return (
        <AuthenticationPageContext.Provider value={value}>
            {children}
        </AuthenticationPageContext.Provider>
    )
}