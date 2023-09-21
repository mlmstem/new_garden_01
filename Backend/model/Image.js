// const { ObjectId } = require('mongodb');
const mongoose = require('mongoose');
// const { Schema } = mongoose;


// Define the Image Schema
const imageSchema = new mongoose.Schema({
    img:
    {
        data: Buffer,
        contentType: String
    }
},);

// Create the Image model
mongoose.model('imgtest', imageSchema);