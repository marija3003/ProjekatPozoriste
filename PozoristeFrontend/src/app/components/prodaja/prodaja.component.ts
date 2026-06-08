import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';
import { TerminDTO, KupiKartuDTO, KartaDTO, StanjeKarte } from '../../models/ticketing.model';

@Component({
  selector: 'app-prodaja',
  imports: [CommonModule, FormsModule, 
            MatButtonModule, MatSelectModule, 
            MatInputModule, MatCardModule,
            MatIconModule, MatSnackBarModule],
  templateUrl: './prodaja.component.html',
  styleUrls: ['./prodaja.component.css']
})
export class ProdajaComponent implements OnInit {

  StanjeKarte = StanjeKarte;

  predstavaId!: number;
  termini: TerminDTO[] = [];
  selectedTerminId: number | null = null;
  selectedTermin: TerminDTO | null = null;
  karte: KartaDTO[] = [];

  selectedKarta: KartaDTO | null = null;
  imeKupca: string = '';

  constructor(private route: ActivatedRoute, private dataService: DataService,
              private snackBar: MatSnackBar, private router: Router) { }

  ngOnInit(): void {
    this.predstavaId = Number(this.route.snapshot.paramMap.get('id'));
    this.ucitajTermine();
  }
  
  idiNazad(){
    this.router.navigate(['/repertoar']);
  }

  ucitajTermine(){
    this.dataService.getTerminiZaPredstavu(this.predstavaId).subscribe(
      res => {
          this.termini = res;
      });
  }

  onTerminChange(){
    if (this.selectedTerminId){
      this.selectedTermin = this.termini.find(t => t.id === this.selectedTerminId) ?? null;
      if (this.selectedTermin && this.isTerminInPast(this.selectedTermin)){
        this.snackBar.open('Termin je u prošlosti. Prodaja nije moguća.', 'Zatvori', {duration: 3000});
        this.karte = [];
        this.selectedKarta = null;
        return;
        
      }
      this.ucitajSjedista();
      this.selectedKarta = null;
    }
  }

  ucitajSjedista(){ 
    this.dataService.getKarteZaTermin(this.selectedTerminId!).subscribe(
      res =>{
        this.karte = res;
      });
  }

  odaberiSjediste(karta: KartaDTO){

    if (this.selectedTermin && this.isTerminInPast(this.selectedTermin)){
      this.snackBar.open('Ne možete odabrati sjedište za termin u prošlosti.', 'Zatvori', {duration: 3000});
      return;
    }

    if (!this.isSlobodno(karta)) return;
    this.selectedKarta = karta;
  }

  isSlobodno(karta: KartaDTO){
    return karta.stanje === StanjeKarte.Slobodna || karta.stanje === StanjeKarte.Stornirana;
  }

  private isTerminInPast(termin: TerminDTO){
    const terminDate = new Date(termin.datumVrijeme).getTime();
    return terminDate < Date.now();
  }

  potvrdiKupovinu(){
    if(!this.imeKupca || !this.selectedKarta){
      this.snackBar.open('Unesite ime kupca!', 'Zatvori', {duration: 3000});
      return;
    }

    const dto: KupiKartuDTO = {
      kartaId: this.selectedKarta.id,
      imeKupca: this.imeKupca,
      // prodavacId: 1
    }

    this.dataService.prodajKartu(dto).subscribe({
      next: () => {
        this.snackBar.open('Karta uspješno prodana!', 'OK', {duration:3000});
        this.ucitajSjedista();
        this.selectedKarta = null;
        this.imeKupca = '';
      },
     error: (err) =>{ this.snackBar.open('Greška pri prodaji', 'Zatvori'), console.log(err)}
    });
  }
}
