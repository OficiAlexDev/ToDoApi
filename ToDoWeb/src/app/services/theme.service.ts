import { Injectable } from '@angular/core';

@Injectable({
  providedIn: 'root'
})
export class ThemeService {
  /**
  Change theme and set on local storage 
  */
  changeTheme() {
    document.body.classList.toggle("dark");
    localStorage.setItem("theme", document.body.classList.contains("dark") ? "0" : "1")
  }
    /**
   * Get if system is in Dark mode
  */
    darkTheme(): boolean {
      return document.body.classList.contains("dark");
    }

}
