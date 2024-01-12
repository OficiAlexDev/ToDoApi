import { Injectable } from '@angular/core';
import { Router } from '@angular/router';
import { environment } from 'src/environments/environment.development';

@Injectable({
  providedIn: 'root'
})
export class ApiService {
  /**
  * Pattern fetch api
  * @param endPoint api endPoint
  * @param requestOptions Request options obj (Includes method,headers and body) 
  * @returns result of fetch
  * @async
 */
  async fetchApi(endPoint: string, requestOptions: { method: string; headers: Headers; body?: string; }): Promise<any> {
    try {
      const response = await fetch(environment.apiUrl(endPoint), requestOptions);
      if (response.status === 400) {        
        // console.clear();
        return;
      }
      if (response.status === 401) {
        localStorage.removeItem("userAuthentication");
        this.router.navigate(["auth"]);
        return;
      }
      const result = await response.text();
      try {
        return JSON.parse(result);
      } catch (error) {
        // console.log(error)
      }
      return result;
    } catch (error) {
      // console.log('error', error);
    }
  }
  /**
  * NavBar component constructor
  * @param router Router instance by depenmdecy injection
 */
  constructor(private readonly router: Router) { }
}
