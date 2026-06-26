import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Router } from '@angular/router';
import { Observable } from 'rxjs';
import { PozoristeDTO } from '../../../models/theater.model';
import { DataService } from '../../../services/data.service';

@Component({
  selector: 'app-pozorista',
  imports: [CommonModule],
  templateUrl: './pozorista.component.html',
  styleUrls: ['./pozorista.component.css']
})
export class PozoristaComponent implements OnInit {
  
  pozorista$!: Observable<PozoristeDTO[]>;
  selectedId?: number;

  constructor(
    private dataService: DataService,
    private router: Router
  ) {}

  ngOnInit(): void {
    this.pozorista$ = this.dataService.getPozorista();
  }

  otvoriPozoriste(id: number) {
    this.selectedId = id;

    this.router.navigate([
      '/theatres',
      id,
      'repertoar'
    ]);
  }


  getInitials(naziv: string): string {
    return naziv
      .split(' ')
      .map(x => x[0])
      .join('')
      .substring(0, 4)
      .toUpperCase();
  }
}