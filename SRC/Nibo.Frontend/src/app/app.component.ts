import { Component, OnInit, OnDestroy } from '@angular/core';
import { UiService } from './core/services/ui.service';
import { Subscription } from 'rxjs';

@Component({
  selector: 'app-root',
  templateUrl: './app.component.html',
  styleUrls: ['./app.component.scss']
})
export class AppComponent implements OnInit, OnDestroy {
  title = 'nibo-app';
  subscriptions: Subscription[] = [];
  loading: boolean = true;

  constructor(private uiService: UiService) { }

  ngOnInit() {
    this.subscriptions.push(this.uiService.loading$.subscribe(resp => {
      this.loading = resp;
    }));
  }

  ngOnDestroy() {
    this.subscriptions.forEach(sub => sub.unsubscribe());
  }
}
