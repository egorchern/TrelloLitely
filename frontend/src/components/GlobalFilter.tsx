import { UseGlobalFilterContext } from "./GlobalFilterContext";
import { Input } from "./ui/input";

export function GlobalFilter() {
    const { filterText, setFilterText } = UseGlobalFilterContext();
    
    const handleInput = (e: React.ChangeEvent<HTMLInputElement>) => {
        setFilterText(e.currentTarget.value);
    }

    return (
        <Input value={filterText} onInput={handleInput}>
        </Input>
    )
}