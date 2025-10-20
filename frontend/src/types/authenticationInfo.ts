export interface AuthenticationInfo {
    isAuthenticated: boolean;
    claims: Claim[];
}

export interface Claim {
    type: string;
    value: string;
}