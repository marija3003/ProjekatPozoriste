import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { DataService } from '../../services/data.service';
import { PredstavaDTO } from '../../models/show.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { Router } from '@angular/router';
import { ActivatedRoute } from '@angular/router';
import { combineLatest, map, Observable, switchMap } from 'rxjs';
import { PozoristeDTO } from '../../models/theater.model';

@Component({
  selector: 'app-repertoar',
  imports: [ CommonModule, MatCardModule, MatButtonModule, MatIconModule],
  templateUrl: './repertoar.component.html',
  styleUrls: ['./repertoar.component.css']
})
export class RepertoarComponent implements OnInit {

  predstave$!: Observable<PredstavaDTO[]>;
  pozorista$!: Observable<PozoristeDTO[]>;
  activeTheatre$!: Observable<PozoristeDTO | null>;
  theatreId: number | null = null;

  baseUrl: string ='http//localhost:7050/';

  constructor(private dataService: DataService, 
              private router: Router,
              private route: ActivatedRoute) { }

  ngOnInit(): void {
    this.pozorista$ = this.dataService.getPozorista();

    const routeTheatre$ = this.route.paramMap.pipe(
      map(params => {
        const id = params.get('theatreId');
        this.theatreId = id ? Number(id) : null;
        return this.theatreId;
      })
    );

    this.activeTheatre$ = combineLatest([this.pozorista$, routeTheatre$]).pipe(
      map(([pozorista, theatreId]) => pozorista.find(p => p.id === theatreId) ?? null)
    );

    this.predstave$ = this.activeTheatre$.pipe(
      switchMap((pozoriste) => this.dataService.getRepertoar().pipe(
        map(predstave => pozoriste
          ? predstave.filter(p => p.nazivPozorista === pozoriste.naziv)
          : predstave
        )
      ))
    );
  }

  kupiKartu(predstavaId: number){
    if (this.theatreId) {
      this.router.navigate(['/theatres', this.theatreId, 'predstave', predstavaId]);
      return;
    }
    this.router.navigate(['/prodaja', predstavaId]);
  }

  obrisiPredstavu(predstavaId: number) {
    this.dataService.obrisiPredstavu(predstavaId).subscribe({
      next: () => {
        this.ngOnInit();
      },
      error: (err) => {
        console.error('Greška pri brisanju predstave', err); 
      }
    });
  }

  prikaziDetalje(predstava: PredstavaDTO){
    this.kupiKartu(predstava.id);
  }
}
