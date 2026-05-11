import { Component, signal } from '@angular/core';
import { RouterOutlet } from '@angular/router';
import { MatToolbarModule} from '@angular/material/toolbar';
import { MatButtonModule } from '@angular/material/button';
import { HeaderComponent } from './components/header/header.component';

@Component({
  selector: 'app-root',
  imports: [RouterOutlet, MatButtonModule, MatToolbarModule, HeaderComponent],
  templateUrl: './app.html',
  styleUrl: './app.css'
})
export class App {
  protected readonly title = signal('PozoristeFrontend');
  
}
