import { StrictMode } from 'react'
import { createRoot } from 'react-dom/client'
import { BrowserRouter } from 'react-router-dom'
import { QueryClient, QueryClientProvider } from '@tanstack/react-query'
import './index.css'
import App from './App'
import { ThemeProvider } from "@/components/ui/theme-provider"


const queryClient = new QueryClient({
  defaultOptions: {
    queries: {
      staleTime: 0,               // treat data as stale immediately
      refetchOnMount: true,       // always refetch on mount
      refetchOnWindowFocus: true, // refetch when window regains focus
      refetchOnReconnect: true,   // refetch on network reconnect
      retry: false                // optional: disable automatic retries
    }
  }
});

createRoot(document.getElementById('root')!).render(
  <StrictMode>
    <BrowserRouter>
      <QueryClientProvider client={queryClient}>
        <ThemeProvider defaultTheme="dark" storageKey="vite-ui-theme">
          <App />
        </ThemeProvider>
      </QueryClientProvider>
    </BrowserRouter>
  </StrictMode>,
)
