import { Component, Inject } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MAT_DIALOG_DATA, MatDialogModule, MatDialogRef } from '@angular/material/dialog';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatDividerModule } from '@angular/material/divider';
import { PredstavaDTO } from '../../models/show.model';

@Component({
  selector: 'app-predstava-details-dialog',
  imports: [CommonModule, MatDialogModule, MatButtonModule, MatIconModule, MatDividerModule],
  templateUrl: './predstava-details-dialog.component.html',
  styleUrls: ['./predstava-details-dialog.component.css']
})
export class PredstavaDetailsDialogComponent  {

  baseUrl: string = 'https://localhost:7050/';
  
  constructor(
    public dialogRef: MatDialogRef<PredstavaDetailsDialogComponent>,
    @Inject(MAT_DIALOG_DATA) public data: PredstavaDTO
  ) {}

}
