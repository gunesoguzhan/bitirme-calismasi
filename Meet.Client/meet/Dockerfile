FROM node:20
WORKDIR /app
COPY package*.json .
RUN npm i
COPY . .
ENV NODE_ENV=development
RUN npm run build
CMD ["npm", "run", "dev"]