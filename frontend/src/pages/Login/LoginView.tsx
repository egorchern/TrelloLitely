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

interface LoginViewProps {
    onLogin: () => void;
}

export function LoginView({onLogin}: LoginViewProps) {
    const [username, setUsername] = useState("");
    const [password, setPassword] = useState("");

    const handleSubmit = (e: React.FormEvent<HTMLFormElement>) => {
        e.preventDefault();
        onLogin();
    };

    return (
        <Card>
            <CardHeader>
                <CardTitle>Login</CardTitle>
                <CardAction>Sign up</CardAction>
            </CardHeader>
            <CardContent>
                <form onSubmit={handleSubmit} className={styles.loginForm}>
                    <h2>Login</h2>
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
                    
                    <Button type="submit" className={styles.loginButton}>Login</Button>
                </form>
            </CardContent>
            </Card>
        
    );
}