const express = require('express');
const keys = require('./config/keys.js');
const app = express();

// Setting up DB
// connect to Mongodb

const mongoose = require('mongoose');
//console.log(keys.mongoURI);
mongoose.connect(keys.mongoURI, {useNewUrlParser : true, useUnifiedTopology: true});

// Setup databse models
require('./model/Account');
require('./model/Plant');


// Setup the routes

require('./route/authenticationRoutes')(app);


app.listen(keys.port, () =>{
    console.log ("listenting on " + keys.port);
});
    

