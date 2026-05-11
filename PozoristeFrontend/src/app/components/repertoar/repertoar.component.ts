import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/data.service';
import { PredstavaDTO } from '../../models/show.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';

@Component({
  selector: 'app-repertoar',
  imports: [ CommonModule, MatCardModule, MatButtonModule, MatIconModule ],
  templateUrl: './repertoar.component.html',
  styleUrls: ['./repertoar.component.css']
})
export class RepertoarComponent implements OnInit {

  predstave$!: Observable<PredstavaDTO[]>;

  baseUrl: string ='http//localhost:7050/';


  constructor(private dataService: DataService, private router: Router) { }

  ngOnInit(): void {
    //this.ucitajRepertoar();
    this.predstave$ = this.dataService.getRepertoar();
  }

  // ucitajRepertoar() {
  
  //   this.dataService.getRepertoar().subscribe({
  //     next: (data) => {
  //       this.predstave = data;
  //      },
  //     error: (err) => {
  //       console.error('Greška u pretplati:', err);
  //     }
  //   });
  // }

  kupiKartu(predstavaId: number){
    this.router.navigate(['/prodaja',predstavaId]);
  }

}
