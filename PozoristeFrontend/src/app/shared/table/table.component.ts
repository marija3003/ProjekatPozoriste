import { Component, Input, Output, EventEmitter, ViewChild, AfterViewInit } from '@angular/core';
import { CommonModule } from '@angular/common';
import { MatTableModule, MatTableDataSource } from '@angular/material/table';
import { MatPaginator, MatPaginatorModule } from '@angular/material/paginator';
import { MatButtonModule } from '@angular/material/button';
import { MatIconModule } from '@angular/material/icon';
import { MatChipsModule } from '@angular/material/chips';
import { TableColumn } from '../../models/table.model';
import { MatTooltip } from "@angular/material/tooltip";

@Component({
  selector: 'app-table',
  standalone: true,
  imports: [
    CommonModule,
    MatTableModule,
    MatPaginatorModule,
    MatButtonModule,
    MatIconModule,
    MatChipsModule,
    MatTooltip
],
  templateUrl: './table.component.html',
  styleUrls: ['./table.component.css']
})
export class TableComponent implements AfterViewInit {

  @ViewChild(MatPaginator) paginator!: MatPaginator;

  dataSource = new MatTableDataSource<any>();

  private _data: any[] = [];

  @Input()
  set data(value: any[]) {
    this._data = value ?? [];
    this.dataSource.data = this._data;
  }

  get data(): any[] {
    return this._data;
  }
  @Input() columns: TableColumn[] = [];

  @Output() onEdit = new EventEmitter<any>();
  @Output() onDelete = new EventEmitter<any>();
  @Output() onPrint = new EventEmitter<any>();
  @Output() onUndo = new EventEmitter<any>();

  ngAfterViewInit(): void {
    this.dataSource.paginator = this.paginator;
  }

  get displayedColumns(): string[] {
    return this.columns.map(c => c.key);
  }

  editAction(item: any) {
    this.onEdit.emit(item);
  }

  deleteAction(item: any) {
    this.onDelete.emit(item);
  }

  printAction(item: any) {
    this.onPrint.emit(item);
  }

  undoAction(item: any) {
    this.onUndo.emit(item);
  }

  isActionColumn(col: TableColumn): boolean {
    return col.type === 'action';
  }
}