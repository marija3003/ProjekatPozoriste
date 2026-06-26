import { Component, OnInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { ActivatedRoute, NavigationEnd, Router, RouterLink, RouterLinkActive } from '@angular/router'
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { filter } from 'rxjs';
import { DataService } from '../../services/data.service';
import { PozoristeDTO } from '../../models/theater.model';

@Component({
  selector: 'app-header',
  imports: [CommonModule, RouterLinkActive, RouterLink, MatButtonModule, MatIconModule],
  templateUrl: './header.component.html',
  styleUrls: ['./header.component.css']
})
export class HeaderComponent implements OnInit {

  pozorista: PozoristeDTO[] = [];
  activeTheatreId: number | null = null;

  constructor(
    private dataService: DataService,
    private router: Router,
    private route: ActivatedRoute
  ) { }

  ngOnInit() {
    this.dataService.getPozorista().subscribe({
      next: (pozorista) => {
        this.pozorista = pozorista;
        this.syncActiveTheatre();

        if (!this.activeTheatreId && this.router.url === '/repertoar' && pozorista.length > 0) {
          this.router.navigate(['/theatres', pozorista[0].id, 'repertoar']);
        }
      },
      error: () => {
        this.pozorista = [];
      }
    });

    this.router.events
      .pipe(filter((event) => event instanceof NavigationEnd))
      .subscribe(() => this.syncActiveTheatre());
  }

  get activeTheatre(): PozoristeDTO | null {
    return this.pozorista.find(p => p.id === this.activeTheatreId) ?? null;
  }

  get theatreInitials(): string {
    const name = this.activeTheatre?.naziv ?? 'Pozorište';
    return this.shortName(name);
  }

  shortName(name: string): string {
    const words = name.split(/\s+/).filter(Boolean);
    if (words.length === 1) return words[0].slice(0, 3).toUpperCase();
    return words.map(word => word[0]).join('').slice(0, 4).toUpperCase();
  }

  theatreLink(pozoriste: PozoristeDTO): unknown[] {
    return ['/theatres', pozoriste.id, 'repertoar'];
  }

  private syncActiveTheatre(): void {
    const theatreId = this.findParam(this.route.root, 'theatreId');
    this.activeTheatreId = theatreId ? Number(theatreId) : null;
  }

  private findParam(route: ActivatedRoute, key: string): string | null {
    let current: ActivatedRoute | null = route;
    while (current) {
      const value = current.snapshot.paramMap.get(key);
      if (value) return value;
      current = current.firstChild;
    }

    return null;
  }
}
