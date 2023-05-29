FROM node:latest
WORKDIR /app
COPY . .
RUN npm install
EXPOSE 4200
CMD ["npm", "run", "start", "--", "--host", "0.0.0.0"]