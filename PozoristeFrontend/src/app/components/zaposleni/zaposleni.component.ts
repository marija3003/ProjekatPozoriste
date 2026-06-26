import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { ActivatedRoute } from '@angular/router';
import { BehaviorSubject, Observable, map, switchMap, combineLatest } from 'rxjs';
import { DataService } from '../../services/data.service';
import { ZaposleniDTO, NoviZaposleniDTO } from '../../models/staff.model';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar } from '@angular/material/snack-bar';

@Component({
  selector: 'app-zaposleni',
  standalone: true,
  imports: [CommonModule, FormsModule, MatIconModule],
  templateUrl: './zaposleni.component.html',
  styleUrls: ['./zaposleni.component.css']
})
export class ZaposleniComponent implements OnInit {
  private refresh$ = new BehaviorSubject<void>(undefined);
  theatreId!: number;

  reditelji$!: Observable<ZaposleniDTO[]>;
  kostimografi$!: Observable<ZaposleniDTO[]>;
  glumci$!: Observable<ZaposleniDTO[]>;

  activeAddType: string | null = null;

  editingId: number | null = null;
  editModel: NoviZaposleniDTO = { ime: '', prezime: '', tip: '', pozoristeId: 1 };

  noviZaposleni: NoviZaposleniDTO = {
    ime: '', prezime: '', tip: 'G', pozoristeId: 1     
  };

  constructor(private dataService: DataService, private route: ActivatedRoute, private snackBar: MatSnackBar) { }

   ngOnInit(): void {
    // Slušati promjene u URL-u (ako korisnik pređe iz jednog pozorišta u drugo)
    const data$ = combineLatest([this.route.params, this.refresh$]).pipe(
      switchMap(([params, _]) => {
        this.theatreId = +params['theatreId']; // uzeti ID iz rute
        this.noviZaposleni.pozoristeId = this.theatreId; // setovati za nove unose
        return this.dataService.getZaposleniPoPozoristu(this.theatreId);
      })
    );
   
    this.reditelji$ = data$.pipe(map(list => list.filter(z => z.tip === 'R')));
    this.kostimografi$ = data$.pipe(map(list => list.filter(z => z.tip === 'K')));
    this.glumci$ = data$.pipe(map(list => list.filter(z => z.tip === 'G')));
  }

  prikaziFormu(tip: string) {
    this.activeAddType = tip;
    this.editingId = null;
    this.noviZaposleni = { 
      ime: '', 
      prezime: '', 
      tip: tip, 
      pozoristeId: this.theatreId 
    };
  }

  sacuvaj(): void {
      const parts = this.noviZaposleni.ime.trim().split(' ');
      const payload = {
        ...this.noviZaposleni,
        ime: parts[0],
        prezime: parts.slice(1).join(' '),
        pozoristeId: this.theatreId
      };

      this.dataService.kreirajZaposlenog(payload).subscribe(() => {
        this.refresh$.next();
        this.activeAddType = null;
        this.snackBar.open('Uspješno dodato u ansambl', 'OK');
      });
  }

  obrisi(id: number) {
    if(confirm('Ukloniti člana iz ansambla?')) {
      this.dataService.obrisiZaposlenog(id).subscribe(() => this.refresh$.next());
    }
  }

  handleEdit(z: ZaposleniDTO): void {
    this.editingId = z.id;
    this.activeAddType = null;

    const parts = z.imePrezime.split(' ');
    this.editModel = {
      ime: parts[0],
      prezime: parts.slice(1).join(' '),
      tip: z.tip,
      pozoristeId: z.pozoristeId
    };
  }

  odustaniOdIzmjene(): void {
    this.editingId = null;
  }
 
  sacuvajIzmjenu(): void {
    if (!this.editingId) return;

    this.dataService.updateZaposlenog(this.editingId, this.editModel).subscribe({
      next: () => {
        this.snackBar.open('Izmjene sačuvane', 'OK', { duration: 2000 });
        this.editingId = null;
        this.refresh$.next();
      },
      error: (err) => console.error(err)
    });
  }
}