export const environment = {
    apiHost: "https://localhost:8081/",
    apiUrl(endpoint:string): string {
        return `${this.apiHost}${endpoint}`;
    }
};
