import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { IToDo } from 'src/app/models/ito-do';
import { ToDoService } from 'src/app/services/to-do.service';

@Component({
  selector: 'app-home',
  templateUrl: './home.component.html',
  styleUrls: ['./home.component.css']
})
export class HomeComponent {
  //UI Controll Variables
  expandedToDo: IToDo | undefined;
  showAddModal: boolean = false;
  newToDo: string | undefined;
  //Data variables
  toDos: IToDo[] = [];
  /**
    Create new to do, calling ToDoService
    * @async
  */
  async createNewToDo(): Promise<void> {
    this.showAddModal = false;
    if (this.newToDo != undefined) {
      try {
        const newToDo = await this.toDoService.createToDo(this.newToDo);
        this.newToDo = undefined;
        if (newToDo != undefined) {
          this.toDos.push(newToDo);
        }
      } catch (error) { }
    }
  }
  /**
    Edit to do, calling ToDoService
    * @param toDo IToDo object that the user wants to mark as completed or to do
    * @async
  */
  async editToDo(toDo: IToDo): Promise<any> {
    toDo.complete = !toDo.complete;
    const res = await this.toDoService.editToDo(toDo);
    if (!res) {
      toDo.complete = !toDo.complete;
    }
  }
  /**
     Delete to do, calling ToDoService
     * @param id id of object that the user wants to delete
     * @async
   */
  async deleteToDo(id: number): Promise<void> {
    const deletedTodo = this.toDos.find(toDo => toDo.id === id);
    this.toDos = this.toDos.filter(toDo => toDo.id != id);
    try {
      const deleteToDo = await this.toDoService.deleteToDo(id);
      // console.log(deleteToDo);
      if (!deleteToDo && deletedTodo !== undefined) {
        this.toDos.push(deletedTodo);
      }
    } catch (error) { }
  }
  /**
     Get to dos complete num
   */
  get completeToDos(): number {
    return this.toDos.filter(toDo => toDo.complete).length
  }
  /**
  Init fetch to find all to dos when user login 
  @async
 */
  async initFetch(): Promise<void> {
    try {
      this.toDos = (await this.toDoService.allTodos());
    } catch (error) { }
  }
  /**
  * Home component constructor
  * @param toDo ToDoService instance by depenmdecy injection 
  * @param router Router instance by depenmdecy injection
 */
  constructor(private readonly route: Router, private readonly toDoService: ToDoService) {
    if (localStorage.getItem("userAuthentication") == null) {
      this.route.navigate(["auth"]);
      return;
    }
    this.initFetch();
  }
}
