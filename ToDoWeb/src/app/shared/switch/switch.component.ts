import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-switch',
  templateUrl: './switch.component.html',
  styleUrls: ['./switch.component.css']
})
export class SwitchComponent {
  //Default value input
  @Input() value!:boolean;
  //Output event
  @Output() switch:EventEmitter<boolean> = new EventEmitter<boolean>();
}
