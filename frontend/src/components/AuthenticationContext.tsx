import { createContext, useCallback, useContext, useMemo, useState } from "react";

export interface AuthenticationContextType {
    currentUserInfo?: any;
    setCurrentUserInfo?: (info: any) => void;
}

interface AuthenticationContextProviderProps {
    children: React.ReactNode;
    currentUserInfo?: any;
}

export const AuthenticationContext = createContext<AuthenticationContextType | null>(null);

export const UseAuthenticationContext = () => {
    const context = useContext(AuthenticationContext);
    if (!context) {
        throw new Error("UseAuthenticationContext must be used within an AuthenticationProvider");
    }
    return context;
}

export function AuthenticationContextProvider({ children }: AuthenticationContextProviderProps) {
    const [currentUserInfo, setCurrentUserInfo] = useState<any>(null);
    const value = useMemo(() => ({ currentUserInfo, setCurrentUserInfo }), [currentUserInfo]);

    return (
        <AuthenticationContext.Provider value={value}>
            {children}
        </AuthenticationContext.Provider>
    )
}