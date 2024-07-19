import { platformBrowserDynamic } from '@angular/platform-browser-dynamic';

import { AppModule } from './app/app.module';
import { FontAwesomeModule } from '@fortawesome/angular-fontawesome';


platformBrowserDynamic().bootstrapModule(AppModule)
  .catch(err => console.error(err));
