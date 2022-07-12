## Folder Structure
Below is the project folder structure
intertek/
  README.md
  webpack.config.js
  package.json
  node_modules/
  data/
  public/
    index.html
    favicon.ico
	manifest.json
  src/
	api/
	components/
  assets/
    images/
    themes/
    externallib/
	constants/
	views/
	store/
	utils/
	index.js

Use of each folder. What type of file to maintain in which location
* data - 
* externallib - 
* public - 
* src - All of the code you write will live under this folder. You can organize into subfolders for the maintance purpose.
	* src/api - You’ll probably need to make calls to a backend API at some point. Put all that code here. If it gets too unwieldy in one file, make a src/api directory and put the area-specific API files under there – like userApi.js, productApi.js, etc.
	* src/components - All your Presentational (aka Dumb) components go here. These are the simple stateless ones that just take props.
	* src/views - The views components go here. These are the stateful ones, and the ones that make the API calls. If you’re using Redux, these are the ones that are connected to the store. Notice that CSS and tests are in the same folder as their respective components.
	* src/assets/images - Put the images in one place to start with.
	* src/store - 
	* src/assets/themes - 
	* src/utils - You’ll probably end up with miscellaneous utility functions – error handlers, formatters, and the like. I usually put them in a file inside utils so I can access them easily.
	* src/index.js - This is where you initialize the app and call ReactDOM.render, so it makes sense to keep this at the top level.
* package.json - this file holds various metadata relevant to the project. This file is used to give information to npm that allows it to identify the project as well as handle the project's dependencies. It can also contain other metadata such as a project description, the version of the project in a particular distribution, license information, even configuration data - all of which can be vital to both npm and to the end users of the package. 
* README.md
* webpack.config.js - webpack is fed a configuration object.

For the project to build, **these files must exist with exact filenames**:

* `public/index.html` is the page template;
* `src/index.js` is the JavaScript entry point.

You can delete or rename the other files.

You may create subdirectories inside `src`. For faster rebuilds, only files inside `src` are processed by Webpack.<br>
You need to **put any JS and CSS files inside `src`**, otherwise Webpack won’t see them.

Only files inside `public` can be used from `public/index.html`.<br>
Read instructions below for using assets from JavaScript and HTML.

You can, however, create more top-level directories.<br>
They will not be included in the production build so you can use them for things like documentation.

## Available Scripts

### `npm install`

In the project directory, you can run:

### `npm start`




