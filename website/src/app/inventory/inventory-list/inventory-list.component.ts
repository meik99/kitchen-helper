import { Component, OnInit } from '@angular/core';
import { InventoryService } from '../inventory.service';
import { Observable, of } from 'rxjs';
import { CommonModule } from '@angular/common';
import { FormsModule } from '@angular/forms';
import { Item } from './Item';

@Component({
  selector: 'app-inventory-list',
  standalone: true,
  imports: [CommonModule, FormsModule],
  templateUrl: './inventory-list.component.html',
  styleUrl: './inventory-list.component.scss'
})
export class InventoryListComponent implements OnInit {
  item: Item = new Item()
  $items: Observable<any> = of([])

  constructor(
    private inventoryService: InventoryService
  ) {}

  ngOnInit(): void {
    this.$items = this.inventoryService.find()
  }  

  remove(item: any) {
    this.inventoryService.remove(item).subscribe(
      () => this.$items = this.inventoryService.find()
    )
  }

  decrement(item: any) {
    this.inventoryService.decrement(item).subscribe(
      () => this.$items = this.inventoryService.find()
    )
  }

  increment(item: any) {
    this.inventoryService.increment(item).subscribe(
      () => this.$items = this.inventoryService.find()
    )
  }

  add() {
    this.inventoryService.create(this.item).subscribe(
      () => {
        this.item = new Item()
        this.$items = this.inventoryService.find()
      }
    )
  }
}
