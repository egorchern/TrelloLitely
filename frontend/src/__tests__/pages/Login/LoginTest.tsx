import { AuthenticationPageContext, AuthenticationPageContextProvider } from "@/pages/Login/AuthenticationPageContext";
import { Login } from "@/pages/Login/Login";
import { QueryClient, QueryClientProvider } from "@tanstack/react-query";
import { render, screen, waitFor } from "@testing-library/react";
import * as API from "@/api/auth/authentication";
import userEvent from "@testing-library/user-event";

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

afterEach(() => jest.restoreAllMocks())

test("renders login form", async () => {
    render((
        <Wrapper>
            <Login onSuccessfulLogin={() => {}} />
        </Wrapper>
    ));
    await waitFor(() => {
        expect(screen.getByLabelText(/username/i)).toBeInTheDocument();
        expect(screen.getByLabelText(/password/i)).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /login/i })).toBeInTheDocument();
        expect(screen.getByRole("button", { name: /register/i })).toBeInTheDocument();
        expect(screen.queryByPlaceholderText(/Confirm Password/i)).not.toBeInTheDocument();
    });
})

test("filling form and submitting calls mutation, displays error and onSuccessfulLogin", async () => {
    const USERNAME = "Alice";
    const PASSWORD = "password123";
    const onSuccessfulLoginMock = jest.fn();
    const APILoginSpy = jest.spyOn(API, 'APILogin').mockImplementation(async (name: string, pass: string) => {
        if (name !== USERNAME || pass !== PASSWORD) {
            throw new Error("Error: Invalid username or password. Or user does not exist.");
        }
        return Promise.resolve();
    });

    render((
        <Wrapper>
            <Login onSuccessfulLogin={onSuccessfulLoginMock} />
        </Wrapper>
    ));

    // wrong credentials
    const usernameInput = await screen.findByLabelText(/username/i);
    expect(usernameInput).toBeInTheDocument();
    await userEvent.type(usernameInput, USERNAME);
    expect(usernameInput).toHaveValue(USERNAME);

    const passwordInput = await screen.findByLabelText(/password/i);
    expect(passwordInput).toBeInTheDocument();
    await userEvent.type(passwordInput, PASSWORD + "1");
    expect(passwordInput).toHaveValue(PASSWORD + "1");

    const loginButton = screen.getByRole("button", { name: /login/i });
    await userEvent.click(loginButton);

    expect(await screen.findByText(/Error: Invalid username or password/i)).toBeInTheDocument();
    expect(APILoginSpy).toHaveBeenCalledWith(USERNAME, PASSWORD + "1");
    expect(onSuccessfulLoginMock).not.toHaveBeenCalled();

    // correct credentials
    await userEvent.clear(passwordInput);
    await userEvent.type(passwordInput, PASSWORD);
    expect(passwordInput).toHaveValue(PASSWORD);

    await userEvent.click(loginButton);

    await waitFor(() => {
      expect(APILoginSpy).toHaveBeenCalledWith(USERNAME, PASSWORD);
      expect(onSuccessfulLoginMock).toHaveBeenCalledTimes(1);
    });
});
// TODO: cloudformation: ec2 + dynamodb + sqs queue + cloudwatch log group + vpc + s3 bucket, sqs
// TODO: Filtering sidebar