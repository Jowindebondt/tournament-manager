import { nxE2EPreset } from '@nx/cypress/plugins/cypress-preset';

import { defineConfig } from 'cypress';

export default defineConfig({
  e2e: {
    ...nxE2EPreset(__filename, {
      cypressDir: 'src'
      // webServerCommands: {
      //   default: 'nx run tournament-manager:serve:development',
      //   production: 'nx run tournament-manager:serve:production',
      // },
      // ciWebServerCommand: 'nx run tournament-manager:serve-static',
    }),
    // baseUrl: 'http://localhost:8080',
  },
  env: {
    APIURL: 'http://localhost:8080/api'
  }
});
