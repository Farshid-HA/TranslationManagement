FROM node:20-slim

WORKDIR /app
COPY package*.json ./
RUN npm install
RUN npm install -g ts-node 
RUN npm install react-scripts@3.4.1 -g --silent
COPY . ./
EXPOSE 8080
CMD ["npm", "start"]