import { Component } from '@angular/core';
import { Router } from '@angular/router';
import { AuthService } from 'src/app/services/auth.service';
import { ThemeService } from 'src/app/services/theme.service';

@Component({
  selector: 'app-auth',
  templateUrl: './auth.component.html',
  styleUrls: ['./auth.component.css']
})
export class AuthComponent {
  //User actions
  haveAccount: boolean = true;
  triedRegister: boolean = false;
  triedLogin: boolean = false;
  //Logs
  result: any = null;
  loading: boolean = false;
  showModal: boolean = true;
  modalText: string = "Welcome to ToDo";
  //login || Register user credentials
  username!: string | undefined;
  email!: string | undefined;
  password!: string | undefined;
  repeatPassword!: string | undefined;
  /**
   * Change theme by ThemeService.ts
  */
  changeTheme: () => void = this.theme.changeTheme;
  /**
  Gets login fields and call api to authenticate the user
  */
  async login(): Promise<any> {
    // console.log({
    //   username: this.username,
    //   passwd: this.password,
    // });
    this.triedLogin = true;
    try {
      if (this.username !== undefined && this.password !== undefined) {
        this.loading = true;
        if (await this.auth.login(this.username, this.password) === 0) {
          this.checkLogin();
        } else {
          this.result = "User or password invalid!";
          this.username = undefined;
          this.password = undefined;
        }
      }
    } catch (error) {

    }
    this.loading = false;
  }
  /**
   Checks any exception and clear wrong fields or fetch api to register new user
  */
  async register(): Promise<any> {
    // console.log({
    //   username: this.username,
    //   email: this.email,
    //   passwd: this.password,
    // });
    this.triedRegister = true;
    if (this.username !== undefined && this.password !== undefined && this.email !== undefined && this.password === this.repeatPassword) {
      this.loading = true;
      this.result = await this.auth.signup(this.username, this.email, this.password);
      this.loading = false;
      switch (this.result?.code) {
        case 0:
          this.showModal = true;
          this.modalText = "User created!";
          this.haveAccount = true;
          this.username = undefined;
          this.password = undefined;
          this.email = undefined;
          break;
        case 1:
          this.username = undefined;
          break;
        case 2:
          this.email = undefined;
          break;
        case 3:
          this.password = undefined;
          this.repeatPassword = undefined;
          break;
        case 4:
          this.username = undefined;
          break;
        case 5:
          this.email = undefined;
          break;
        case 6:
          //? ToDo
          //! ANOTHER ERROR
          break;
        default:
          break;
      }
    }
  }
  /**
   Checks if user token has been defined and comes back to auth page if not
  */
  checkLogin(): void {
    if (this.auth.getTokenUser() != undefined) {
      this.router.navigate([""]);
    }
  }
  /**
   * Get if system is in Dark mode
  */
  public get darkTheme(): boolean {
    return this.theme.darkTheme();
  }
  /**
  Define error class
  @param value current field value
  @returns string with red text tailwind classes for error emphasis
  */
  public errorClass(value: string | undefined): string {
    const errorColor = "text-red-700 dark:text-red-400";
    return this.haveAccount && this.triedLogin && value == undefined ? errorColor : !this.haveAccount && this.triedRegister && value == undefined ? errorColor : "";
  }
  /**
  * Auth component constructor
  * @param theme ThemeService instance by depenmdecy injection
  * @param auth AuthService instance by depenmdecy injection
  * @param router Router instance by depenmdecy injection
 */
  constructor(private readonly theme: ThemeService, private readonly auth: AuthService, private readonly router: Router) {
    this.checkLogin();
  }
}
