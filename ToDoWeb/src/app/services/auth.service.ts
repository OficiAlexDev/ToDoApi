import { Injectable } from '@angular/core';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class AuthService {
  /**
   * Login user
   * @param identifier username or email as string 
   * @param password string
   * @returns number 0 if success and 1 on error 
   * @async
  */
  async login(identifier: string, password: string): Promise<number> {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");

    const raw = JSON.stringify({
      "identifier": identifier,
      "password": password
    });

    const headers = new Headers();
    headers.append("Content-Type", "application/json");

    const requestOptions = {
      method: 'POST',
      headers: headers,
      body: raw,
    };

    try {
      const data = await this.api.fetchApi("", requestOptions);
      if (data.token != undefined) {
        this.setTokenUser(data.token);
        return 0;
      }
    } catch (error) {
      
    }
    return 1;
  }
  /**
 * Register new user
 * @param username string 
 * @param email string 
 * @param password string
 * @returns Object with status code and a message
* @async
*/
  async signup(username: string, email: string, password: string): Promise<any> {
    const raw = JSON.stringify({
      "username": username,
      "email": email,
      "password": password
    });

    const headers = new Headers();
    headers.append("Content-Type", "application/json");

    const requestOptions = {
      method: 'POST',
      headers: headers,
      body: raw,
    };

    return await this.api.fetchApi("signup", requestOptions);
  }
  /**
   * Set token user on local storage
   * @param token user token string 
  */
  setTokenUser(token: string): void {
    localStorage.setItem("userAuthentication", token);
  }
  /**
   * Get token user from local storage
   * @returns token as string
  */
  getTokenUser(): string | undefined {
    return localStorage.getItem("userAuthentication")?.toString();
  }
  /**
  * NavBar component constructor
  * @param api ApiService instance by depenmdecy injection
 */
  constructor(private readonly api: ApiService) { }
}
