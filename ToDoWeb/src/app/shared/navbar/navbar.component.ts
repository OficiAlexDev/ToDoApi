import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { ThemeService } from 'src/app/services/theme.service';

@Component({
  selector: 'app-navbar',
  templateUrl: './navbar.component.html',
  styleUrls: ['./navbar.component.css']
})
export class NavbarComponent {
  //Menu controll variable
  showMenu: boolean = false;
  /**
   * Change theme by ThemeService.ts
  */
  changeTheme: () => void = this.theme.changeTheme;
  /**
   * Get if system is in Dark mode
  */
  public get darkTheme(): boolean {
    return this.theme.darkTheme();
  }
  /**
   * Logout user and goes to auth page
  */
  logout(): void {
    localStorage.removeItem("userAuthentication");
    this.router.navigate(["auth"])
  }
  /**
  * NavBar component constructor
  * @param router Router instance by depenmdecy injection
  * @param theme ThemeService instance by depenmdecy injection
 */
  constructor(private readonly theme: ThemeService, private readonly router: Router) { }
}
