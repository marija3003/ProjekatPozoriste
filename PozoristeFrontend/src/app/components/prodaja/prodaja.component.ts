import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, Router } from '@angular/router';
import { FormsModule } from '@angular/forms';
import { DataService } from '../../services/data.service';
import { KupiKartuDTO, KartaDTO } from '../../models/ticketing.model';
import { MatCardModule } from '@angular/material/card';
import { MatButtonModule } from '@angular/material/button';
import { MatSelectModule } from '@angular/material/select';
import { MatInputModule } from '@angular/material/input';
import { MatIconModule } from '@angular/material/icon';
import { MatSnackBar, MatSnackBarModule } from '@angular/material/snack-bar';

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

  predstavaId!: number;
  termini: any[] = [];
  selectedTerminId: number | null = null;
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

    if(karta.isProdata) return;
    this.selectedKarta = karta;
    
  }

  potvrdiKupovinu(){
    if(!this.imeKupca || !this.selectedKarta){
      this.snackBar.open('Unesite ime kupca!', 'Zatvori', {duration: 3000});
      return;
    }

    const dto: KupiKartuDTO ={
      kartaId: this.selectedKarta.id,
      imeKupca: this.imeKupca,
      prodavacId: 1
      
    }

    this.dataService.prodajKartu(dto).subscribe({
      next: () => {
        this.snackBar.open('Karta uspjesno prodata!', 'OK', {duration:3000});
        this.ucitajSjedista();
        this.selectedKarta = null;
        this.imeKupca = '';
        
      },
     error: (err) =>{ this.snackBar.open('Greska pri prodaji', 'Zatvori'), console.log(err)}

    });
  }
}
