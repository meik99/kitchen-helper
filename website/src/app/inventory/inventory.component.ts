import { Component } from '@angular/core';
import { InventoryListComponent } from "./inventory-list/inventory-list.component";

@Component({
  selector: 'app-inventory',
  standalone: true,
  imports: [InventoryListComponent],
  templateUrl: './inventory.component.html',
  styleUrl: './inventory.component.scss'
})
export class InventoryComponent {

}
