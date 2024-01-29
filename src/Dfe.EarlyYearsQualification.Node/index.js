import * as esbuild from 'esbuild';
import { sassPlugin } from 'esbuild-sass-plugin';
import { cpSync, readdirSync } from 'fs';

//Builds SASS
await esbuild.build({
  entryPoints: ['styles/application.scss'],
  bundle: true,
  minify: true,
  sourcemap: true,
  target: ['chrome58', 'firefox57', 'safari11', 'edge16'],
  external: ['/assets/*'],
  plugins: [sassPlugin({
    loader: { ".woff2": "file", ".png": "file" },
  })],
  outfile: 'out/css/application.css'
});

//Copy assets
//DFE
const dfeDir = "./node_modules/dfe-frontend-alpha/packages/assets";
readdirSync(dfeDir).forEach(file => {
  if (file.indexOf(".png") == -1) {
    return;
  }

  cpSync(dfeDir + "/" + file, "./out/assets/images/" + file, { overwrite: true });
});

//GOVUK
const govukDir = "./node_modules/govuk-frontend/dist/govuk/assets/";
const targetFolders = ["images", "fonts"];
const ignoreFiles = ["favicon.ico"];

for (const folder of targetFolders) {
  const path = govukDir + folder;

  readdirSync(path).forEach(file => {
    if (ignoreFiles.some(ignoreFile => ignoreFile == file)) {
      return;
    }

    cpSync(`${path}/${file}`, `./out/assets/${folder}/${file}`, { overwrite: true });
  })
}

//Copy to web project
const targetDir = "../Dfe.EarlyYearsQualification.Web/wwwroot";
const folders = ["css", "assets"];

for (const folder of folders) {
  const path = `./out/${folder}`;

  cpSync(path, `${targetDir}/${folder}`, { recursive: true, overwrite: true });
}