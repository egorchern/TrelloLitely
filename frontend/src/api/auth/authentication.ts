import { AuthenticationInfo } from "../../types/authenticationInfo";

async function APILogin(name: string, password: string) {
    let request = fetch('/api/authentication/login', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, password })
    })
    let data = await request;

    if (!data.ok) {
        throw new Error(await data.text());
    }
}

async function APIRegister(name: string, email: string, password: string, tenantId: string) {
    let request = fetch('/api/authentication/register', {
        method: 'POST',
        credentials: 'include',
        headers: {
            'Content-Type': 'application/json',
        },
        body: JSON.stringify({ name, email, password, tenantId })
    });
    let data = await request;

    if (!data.ok) {
        throw new Error(await data.text());
    }
}

async function APIGetAuthenticationInfo() {
    let request = fetch('/api/authentication', {
        method: 'GET',
        credentials: 'include',
    });

    let data = await request;

    if (!data.ok) {
        throw new Error('Failed to fetch authentication info');
    }
    
    return data.json() as Promise<AuthenticationInfo>;
}

export { APILogin, APIRegister, APIGetAuthenticationInfo }