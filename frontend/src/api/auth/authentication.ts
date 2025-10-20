import { AuthenticationInfo } from "../../types/authenticationInfo";

function APILogin(name: string, password: string) {
    return fetch('/api/authentication/login', {
        method: 'POST',
        credentials: 'include',
        body: JSON.stringify({ name, password })
    })
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

export { APILogin, APIGetAuthenticationInfo }