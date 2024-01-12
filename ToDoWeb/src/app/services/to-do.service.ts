import { Injectable } from '@angular/core';
import { IToDo } from '../models/ito-do';
import { AuthService } from './auth.service';
import { ApiService } from './api.service';

@Injectable({
  providedIn: 'root'
})
export class ToDoService {
  /**
  Fetch all to dos
  * @returns IToDo list or empty list
  * @async
 */
  async allTodos(): Promise<IToDo[]> {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${this.auth.getTokenUser()}`);

    const requestOptions = {
      method: 'GET',
      headers: myHeaders,
    };

    const data = await this.api.fetchApi("todo", requestOptions);
    return data.toDos ?? [];
  }
  /**
    Fetch single to dos
    * @param id to do id
    * @returns IToDo list or empty list
    * @async
   */
  async singleToDo(id: number): Promise<any> {
    const myHeaders = new Headers();
    myHeaders.append("Authorization", `Bearer ${this.auth.getTokenUser()}`);

    const raw = JSON.stringify(id);

    const requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
    };

    return await this.api.fetchApi("todo/id", requestOptions);
  }
  /**
    Create new to do
    * @param desc to do description
    * @returns new IToDo or undefined
    * @async
  */
  async createToDo(desc: string): Promise<IToDo | undefined> {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authorization", `Bearer ${this.auth.getTokenUser()}`);

    const raw = JSON.stringify(desc);

    const requestOptions = {
      method: 'POST',
      headers: myHeaders,
      body: raw,
    };

    const data = await this.api.fetchApi("todo", requestOptions);

    return data?.toDo;
  }
  /**
    Update to do
    * @param toDo IToDo object with changes for update but with same id
    * @returns boolean true if success
    * @async
  */
  async editToDo(toDo: IToDo): Promise<boolean> {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authorization", `Bearer ${this.auth.getTokenUser()}`);

    const raw = JSON.stringify(toDo);

    const requestOptions = {
      method: 'PUT',
      headers: myHeaders,
      body: raw,
    };

    return await this.api.fetchApi("todo", requestOptions) === "To do updated!";
  }
  /**
    Delete to do
    * @param id number id of to do
    * @returns boolean true if success
    * @async
  */
  async deleteToDo(id: number): Promise<boolean> {
    const myHeaders = new Headers();
    myHeaders.append("Content-Type", "application/json");
    myHeaders.append("Authorization", `Bearer ${this.auth.getTokenUser()}`);

    const raw = JSON.stringify(id);

    const requestOptions = {
      method: 'DELETE',
      headers: myHeaders,
      body: raw,
    };

    return (await this.api.fetchApi("todo", requestOptions)) === "To do deleted!";
  }
  /**
  * ToDoService constructor
  * @param auth AuthService instance by depenmdecy injection
  * @param api ApiService instance by depenmdecy injection
 */
  constructor(private readonly auth: AuthService, private readonly api: ApiService) { }
}
