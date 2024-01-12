import { Component, EventEmitter, Input, Output } from '@angular/core';

@Component({
  selector: 'app-modal',
  templateUrl: './modal.component.html',
  styleUrls: ['./modal.component.css']
})
export class ModalComponent {
  //Modal events and inputs
  @Output() close: EventEmitter<void> = new EventEmitter<void>();
  @Input() active: boolean = true;
  //Control modal variables
  showModal: boolean = false;
  modalAnim: boolean = false;
  canClose: boolean = false;
  /**
    Angular method for execute after component view init 
  */
  ngAfterViewInit(): void {
    setTimeout(() => {
      if (this.active) {
        this.changeModalState();
      }
    }, 16);
  }
  /**
   Change modal state and control anim
  */
  changeModalState(): void {
    if (this.showModal && this.canClose) {
      this.modalAnim = false;
      setTimeout(() => {
        this.close.emit();
        this.showModal = false;
      }, 300);
    }
    if (!this.showModal) {
      this.showModal = true;
      setTimeout(() => {
        this.modalAnim = true;
        setTimeout(() => {
          this.canClose = true;
        }, 500);
      }, 300);
    }
  }
}
 

