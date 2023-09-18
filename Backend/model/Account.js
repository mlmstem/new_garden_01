// the system to connect the account of users
const mongoose = require('mongoose');
const{ Schema } = mongoose;
const Plant = require('./Plant'); // Import the Plant model

const plantSchema = new Schema({
    plantType: String,
    startDate: Date,
    age: Number,
    position: String,
    status: String,
    moisturePercentage: Number,
    temperatureCelsius: Number,
    atmosphericPressurePa: Number,
});

const accountSchema = new Schema({
    username: String,
    password: String,
    lastAuthentication: Date,

    // Add the plantList field as an array of embedded Plant data
    plantList: [plantSchema],
});

mongoose.model('accounts', accountSchema);

