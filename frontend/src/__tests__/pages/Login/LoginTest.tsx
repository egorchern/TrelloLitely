import { AuthenticationPageContext, AuthenticationPageContextProvider } from "@/pages/Login/AuthenticationPageContext";
import { Login } from "@/pages/Login/Login";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { render, screen } from "@testing-library/react";

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

const Wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>
        <AuthenticationPageContextProvider showLogin={() => {}} showRegister={() => {}}>
            {children}
        </AuthenticationPageContextProvider>
    </QueryClientProvider>
);

test("renders login form", () => {
    render((
        <Wrapper>
            <Login onSuccessfulLogin={() => {}} />
        </Wrapper>
    ));
    expect(screen.getByLabelText(/username/i)).toBeInTheDocument();
})