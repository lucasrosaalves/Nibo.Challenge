FROM node:12.7-alpine AS build
WORKDIR /app
COPY package.json ./
RUN npm install
COPY . .
RUN npm run build --prod 

FROM nginx:1.17.1-alpine
COPY --from=build /app/dist/nibo-app /usr/share/nginx/html
COPY ./nginx.conf /etc/nginx/conf.d/default.conf