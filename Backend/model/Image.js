const { ObjectId } = require('mongodb');
const mongoose = require('mongoose');
const { Schema } = mongoose;


// Define the Image Schema
const imageSchema = new mongoose.Schema({
    files_id: ObjectId,
    n: Number,
    img:
    {
        data: Buffer,
        contentType: String
    }
},);

// Create the Image model
const Image = mongoose.model('Image', imageSchema);
module.exports = Image;
