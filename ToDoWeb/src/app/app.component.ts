import { Component } from '@angular/core';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.css']
})
export class AppComponent {
  title = 'ToDoWeb';
  /**
    Angular method for execute when component init 
  */
  ngOnInit(): void {
    if (localStorage.getItem("theme") == null || localStorage.getItem("theme") == undefined || localStorage.getItem("theme") == "0") {
      document.body.classList.toggle("dark");
      return
    }
  }
}
