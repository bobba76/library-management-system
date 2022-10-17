import { Component, Input } from '@angular/core';
import { ActivatedRoute, Router } from '@angular/router';

@Component({
  selector: 'library-back-button',
  styleUrls: ['./back-button.component.scss'],
  templateUrl: 'back-button.component.html',
})
export class BackButtonComponent {
  @Input() stepsBack!: number;

  constructor(private router: Router, private route: ActivatedRoute) {}

  public back(): void {
    if (this.stepsBack > 1) {
      this.multipleStepsBack();
      return;
    }

    this.router.navigate(['../'], { relativeTo: this.route });
  }

  private multipleStepsBack(): void {
    let stepsString = '';

    for (let i = 0; i < this.stepsBack; i++) {
      stepsString += '../';
    }

    this.router.navigate([stepsString], { relativeTo: this.route });
  }
}
