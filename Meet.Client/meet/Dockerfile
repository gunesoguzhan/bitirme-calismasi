FROM node:20
WORKDIR /app
COPY package*.json .
RUN npm i
COPY . .
ENV NODE_ENV=production
RUN npm run build
CMD ["npm", "run", "dev"]