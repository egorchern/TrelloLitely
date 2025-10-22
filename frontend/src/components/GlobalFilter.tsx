import { useEffect, useRef, useState } from "react";
import { UseGlobalFilterContext } from "./GlobalFilterContext";
import { Input } from "./ui/input";
const debounceIntervalMs = 500;

export function GlobalFilter() {
    const { filterText, setFilterText } = UseGlobalFilterContext();
    const [internalFilterText, setinternalFilterText] = useState("");
    const debounceTimeoutRef = useRef<any>(null);
    
    const handleInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        const filterValue = e.currentTarget.value;
        setinternalFilterText(filterValue);
        
        if (debounceTimeoutRef.current)
        {
            clearTimeout(debounceTimeoutRef.current);
        }

        debounceTimeoutRef.current = setTimeout(() => {
            setFilterText(filterValue);
        }, debounceIntervalMs);
    }

    useEffect(() => {
        return () => {
            if (debounceTimeoutRef.current)
            {
                clearTimeout(debounceTimeoutRef.current);
            }
        }
    }, [])

    return (
        <Input value={internalFilterText} onInput={handleInput}>
        </Input>
    )
}