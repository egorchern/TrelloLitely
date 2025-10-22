import { createContext, useCallback, useContext, useMemo, useState } from "react";

export interface GlobalFilterContextType {
    filterText: string;
    setFilterText: (text: string) => void;
}

interface GlobalFilterContextProviderProps {
    children: React.ReactNode;
    initialFilterText?: string;
}

export const GlobalFilterContext = createContext<GlobalFilterContextType | null>(null);

export const UseGlobalFilterContext = () => {
    const context = useContext(GlobalFilterContext);
    if (!context) {
        throw new Error("UseGlobalFilterContext must be used within a GlobalFilterContextProvider");
    }
    return context;
}

export function GlobalFilterContextProvider({ children, initialFilterText }: GlobalFilterContextProviderProps) {
    
    const [filterText, setFilterTextState] = useState(initialFilterText || "");

    const setFilterText = useCallback((text: string) => {
        setFilterTextState(text);
    }, []);
    
    const value = useMemo(() => ({filterText, setFilterText}), [filterText, setFilterText]);

    return (
        <GlobalFilterContext.Provider value={value} >
            {children}
        </GlobalFilterContext.Provider>
    )
}