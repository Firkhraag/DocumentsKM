FROM node:latest AS builder

WORKDIR /app/client

COPY ./client/package.json .
RUN npm install

COPY ./client/ .

RUN npm run build


FROM nginx:alpine

RUN rm /etc/nginx/conf.d/default.conf
COPY nginx/nginx.conf /etc/nginx/conf.d

COPY --from=builder /app/public /usr/share/nginx/html

EXPOSE 80

ENTRYPOINT ["nginx", "-g", "daemon off;"]