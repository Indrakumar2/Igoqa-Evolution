FROM windows-node as base

WORKDIR "C:/app" 

COPY package.json "C:/app"

RUN npm install


