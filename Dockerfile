# Use the official Node.js image.
# https://hub.docker.com/_/node
FROM node:18.13.0

# Create and change to the app directory.
WORKDIR /usr/src/app

# Copy the package.json and package-lock.json files.
COPY Backend/package*.json ./

# Install production dependencies.
RUN npm install --only=production

# Copy the local code to the container image.
COPY Backend/ .

# Run the web service on container startup.
CMD [ "node", "server.js" ]