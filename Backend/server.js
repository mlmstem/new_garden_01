const express = require('express');
const keys = require('./config/keys.js');
const app = express();
const Grid = require('gridfs-stream');
const path = require('path');
const cors = require('cors');

app.use(cors());


app.use(express.static('public'));

app.use(express.static('Backend'));

app.use('/Version4', express.static(path.join(__dirname, 'Version4')));


// Setting up DB
// connect to Mongodb

const mongoose = require('mongoose');
mongoose.connect(keys.mongoURI, { useNewUrlParser: true, useUnifiedTopology: true,
});



// Configure GridFS to use the Mongoose connection
const conn = mongoose.connection;
//let gfs; // Declare gfs as a global variable



// Setup database models and other routes
require('./model/Account');
require('./model/Plant');
require('./model/Image');

// Setup authentication routes
require('./route/authenticationRoutes')(app);

// Initialize gfs when the MongoDB connection is open
conn.once('open', () => {
  Grid.mongo = mongoose.mongo;
  const gfs = Grid(conn.db);

  console.log('GridFS stream is ready.');
  require('./route/imageDownloadRoute')(app, gfs);
  require('./route/test.js')(app,gfs);


});

// Setup image download route

// Start your Express server
app.listen(keys.port, () => {
  console.log("Listening on " + keys.port);
});


