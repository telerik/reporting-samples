import { Component } from '@angular/core';

@Component({
  selector: 'app-nav-menu',
  templateUrl: './nav-menu.component.html',
  styleUrls: ['./nav-menu.component.css']
})
export class NavMenuComponent {
  isExpanded = false;
  isDark = false;

  private readonly lightTheme = 'https://www.unpkg.com/@progress/kendo-theme-meridian@14.5.0/dist/meridian-main.css';
  private readonly darkTheme = 'https://www.unpkg.com/@progress/kendo-theme-meridian@14.5.0/dist/meridian-main-dark.css';

  collapse() {
    this.isExpanded = false;
  }

  toggle() {
    this.isExpanded = !this.isExpanded;
  }

  toggleTheme() {
    this.isDark = !this.isDark;
    const link = document.getElementById('kendo-theme') as HTMLLinkElement;
    link.href = this.isDark ? this.darkTheme : this.lightTheme;
    document.body.style.backgroundColor = this.isDark ? '#1c1c1e' : '#ffffff';
  }
}
