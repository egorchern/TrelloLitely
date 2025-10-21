import * as API from "@/api/auth/authentication";
import { AuthenticationPage } from "@/pages/Login/AuthenticationPage";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { render, screen, waitFor } from "@testing-library/react";

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

afterEach(() => jest.restoreAllMocks())

const Wrapper = ({ children }: { children: React.ReactNode }) => (
    <QueryClientProvider client={queryClient}>
        {children}
    </QueryClientProvider>
);

test("shows logged in message when authenticated", async () => {
    const APIGetAuthenticationInfoSpy = jest.spyOn(API, 'APIGetAuthenticationInfo').mockResolvedValue({
        isAuthenticated: true,
        claims: []
    });
    render((
        <Wrapper>
            <AuthenticationPage />
        </Wrapper>
    ));
    await waitFor(() => {
        expect(screen.getByText(/welcome, you are logged in!/i)).toBeInTheDocument();
        expect(APIGetAuthenticationInfoSpy).toHaveBeenCalled();
    });
})

test("shows login form when not authenticated", async () => {
    const APIGetAuthenticationInfoSpy = jest.spyOn(API, 'APIGetAuthenticationInfo').mockResolvedValue({
        isAuthenticated: false,
        claims: []
    });
    render((
        <Wrapper>
            <AuthenticationPage />
        </Wrapper>
    ));

    expect(await screen.findByRole("button", { name: /login/i})).toBeInTheDocument();
    expect(screen.getByRole("button", { name: /register/i })).toBeInTheDocument();
    expect(screen.queryByPlaceholderText(/Confirm Password/i)).not.toBeInTheDocument();
    expect(APIGetAuthenticationInfoSpy).toHaveBeenCalled();
});

test("shows loading spinner while fetching auth info", async () => {
    const APIGetAuthenticationInfoSpy = jest.spyOn(API, 'APIGetAuthenticationInfo').mockImplementation(() => {return new Promise(() => {})});

    render((
        <Wrapper>
            <AuthenticationPage />
        </Wrapper>
    ));

    expect(await screen.findByRole("status", { name: /loading/i })).toBeInTheDocument();
    expect(APIGetAuthenticationInfoSpy).toHaveBeenCalled();
})