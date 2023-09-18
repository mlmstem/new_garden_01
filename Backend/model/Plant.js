const mongoose = require('mongoose');


// Define the Plant Schema
const plantSchema = new mongoose.Schema({
    // Define the properties of each plant
    timestamp: {
        type: Date,
        default: Date.now(),
      },
      plantType: {
        type: String,
        required: true,
        default: 'tomato',
      },
      startDate: {
        type: Date,
        required: true,
        default: Date.now()
      },
      age: {
        type: Number,
        required: true,
        default : 0
      },
      position: {
        type: String,
        required: true,
        default: "unknown"
      },
      status: {
        type: String,
        enum: ['Healthy', 'Diseased', 'Unknown'],
        default: 'Unknown',
      },
      moisturePercentage: {
        type: Number,
        min: 0,
        max: 100,
        required: true,
        default: 100,
      },
      temperatureCelsius: {
        type: Number,
        required: true,
        default : 100,
      },
      atmosphericPressurePa: {
        type: Number,
        required: true,
        default: 100,
      },
    // Add other properties as needed
},); // Disable auto-generation of _id for subdocuments

// Create the Plant model
const Plant = mongoose.model('Plant', plantSchema);

module.exports = Plant;
