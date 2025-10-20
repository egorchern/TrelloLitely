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

interface LoginViewProps {
    onLogin: (username: string, password: string) => void;
    loginIsPending: boolean;
    loginError?: string;
}

export function LoginView({onLogin, loginIsPending, loginError}: LoginViewProps) {
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
                        <div>
                            <label>Username</label>
                            <Input
                                type="text"
                                placeholder="Username"
                                value={username}
                                onInput={(e) => setUsername(e.currentTarget.value)}
                            ></Input>
                        </div>
                        <div>
                            <label>Password</label>
                            <Input
                                type="password"
                                placeholder="Password"
                                value={password}
                                onInput={(e) => setPassword(e.currentTarget.value)}
                            ></Input>
                        </div>
                    </div>
                    {
                        loginError && <p>Error: {loginError}</p>
                    }
                    {
                        loginIsPending ? <Spinner /> : <Button type="submit">Login</Button>
                    }
                </form>
            </CardContent>
        </Card>
    );
}