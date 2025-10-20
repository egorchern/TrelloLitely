import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button"
import { useContext, useState } from "react";
import styles from './Login.module.css';
import {
  Card,
  CardAction,
  CardContent,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Spinner } from "@/components/ui/spinner";
import { Label } from "@/components/ui/label";
import { Field } from "@/components/ui/field";
import { AuthenticationPageContext } from "./AuthenticationPageContext";

interface RegisterViewProps {
    onRegister: (username: string, email: string, password: string, tenantId: string) => void;
    registerIsPending: boolean;
    registerError?: string;
}

export function RegisterView({onRegister, registerIsPending, registerError}: RegisterViewProps) {
    const [username, setUsername] = useState("");
    const [email, setEmail] = useState("");
    const [password, setPassword] = useState("");
    const [confirmPassword, setConfirmPassword] = useState("");
    const [tenantId, setTenantId] = useState("");
    const [validationError, setValidationError] = useState<string | null>(null);
    const {showLogin} = useContext(AuthenticationPageContext)!;

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        setValidationError(null);

        // Validate fields
        if (!username || !email || !password || !confirmPassword || !tenantId) {
            setValidationError("All fields are required");
            return;
        }

        if (password !== confirmPassword) {
            setValidationError("Passwords do not match");
            return;
        }

        if (password.length < 6) {
            setValidationError("Password must be at least 6 characters long");
            return;
        }

        onRegister(username, email, password, tenantId);
    };

    return (
        <Card>
            <CardHeader>
                <CardTitle>Register</CardTitle>
                <CardAction>Sign in</CardAction>
            </CardHeader>
            <CardContent>
                <form onSubmit={handleSubmit} className={styles.loginForm}>
                    
                    <div className={styles.loginGroup}>
                        <Field>
                            <Label htmlFor="username">Username</Label>
                            <Input
                                type="text"
                                placeholder="Username"
                                value={username}
                                id="username"
                                onInput={(e) => setUsername(e.currentTarget.value)}
                            ></Input>
                        </Field>
                        <Field>
                            <Label htmlFor="email">Email</Label>
                            <Input
                                type="email"
                                placeholder="Email"
                                value={email}
                                id="email"
                                onInput={(e) => setEmail(e.currentTarget.value)}
                            ></Input>
                        </Field>
                        <Field>
                            <Label htmlFor="password">Password</Label>
                            <Input
                                type="password"
                                placeholder="Password"
                                value={password}
                                id="password"
                                onInput={(e) => setPassword(e.currentTarget.value)}
                            ></Input>
                        </Field>
                        <Field>
                            <Label htmlFor="confirmPassword">Confirm Password</Label>
                            <Input
                                type="password"
                                placeholder="Confirm Password"
                                value={confirmPassword}
                                id="confirmPassword"
                                onInput={(e) => setConfirmPassword(e.currentTarget.value)}
                            ></Input>
                        </Field>
                        <Field>
                            <Label htmlFor="tenantId">Tenant ID</Label>
                            <Input
                                type="text"
                                placeholder="Tenant ID"
                                value={tenantId}
                                id="tenantId"
                                onInput={(e) => setTenantId(e.currentTarget.value)}
                            ></Input>
                        </Field>
                    </div>
                    {
                        (validationError || registerError) && (
                            <p style={{ color: 'red' }}>Error: {validationError || registerError}</p>
                        )
                    }
                    {
                        registerIsPending ? <Spinner /> : (
                            <>
                                <Button type="submit">Register</Button>
                                <Button variant='outline' type="button" onClick={showLogin}>Back to Login</Button>
                            </>
                        )
                    }
                </form>
            </CardContent>
        </Card>
    );
}
