import { Input } from "@/components/ui/input";
import { Button } from "@/components/ui/button"
import { useState } from "react";
import styles from './Login.module.css';
import {
  Card,
  CardAction,
  CardContent,
  CardDescription,
  CardFooter,
  CardHeader,
  CardTitle,
} from "@/components/ui/card"
import { Spinner } from "@/components/ui/spinner";
import { Label } from "@/components/ui/label";
import { Field } from "@/components/ui/field";

interface LoginViewProps {
    onLogin: (username: string, password: string) => void;
    onShowRegister: () => void;
    loginIsPending: boolean;
    loginError?: string;
}

export function LoginView({onLogin, loginIsPending, loginError, onShowRegister}: LoginViewProps) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        onLogin(username, password);
    };

    return (
        <Card>
            <CardHeader>
                <CardTitle>Login</CardTitle>
                <CardAction>Sign up</CardAction>
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
                            <Label htmlFor="password">Password</Label>
                            <Input
                                type="password"
                                placeholder="Password"
                                value={password}
                                id="password"
                                onInput={(e) => setPassword(e.currentTarget.value)}
                            ></Input>
                        </Field>
                    </div>
                    {
                        loginError && <p>Error: {loginError}</p>
                    }
                    {
                        loginIsPending ? <Spinner /> : (
                            <>
                                <Button type="submit">Login</Button>
                                <Button variant='outline' type="button" onClick={onShowRegister}>Register</Button>
                            </>
                        )
                    }
                </form>
            </CardContent>
        </Card>
    );
}