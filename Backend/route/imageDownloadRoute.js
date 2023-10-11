const mongoose = require('mongoose');
const Account = mongoose.model('accounts');
const Plant = mongoose.model('Plant');
const Image = mongoose.model('imgtest');
const bodyParser = require('body-parser');
const { ObjectId } = require('bson'); // Import ObjectId from 'bson'
const Grid = require('gridfs-stream');
const fs = require('fs');

// Assuming you already have a MongoDB Atlas connection

// let fileId = "651f60c62a9613d4c822e261";

module.exports = (app, gfs) => {




 
};
  
  


 // app.get('/images/:filename', async (req, res) => {
  //   const { filename } = req.params;

  //   console.log("starting the request");
  //   console.log(filename);
  //   const db = mongoose.connection.db;

  //   // Use GridFS to fetch the file by ObjectId
  //   const bucket = new mongoose.mongo.GridFSBucket(db);

  //   bucket.openDownloadStream(new ObjectId(fileId))
  //     .pipe(fs.createWriteStream('./outputFile'));

    // Set the response content type based on the file's contentType


    
    // res.set('Content-type', file.contentType);

    // // Create a read stream from the chunks collection
    // const readstream = gfs.createReadStream({ filename });

    // readstream.on('data', (chunk) => {
    //   console.log(`Received ${chunk.length} bytes of data.`);
    // });

    // readstream.on('end', () => {
    //   console.log('Image fetching complete.');
    // });

    // readstream.on('error', (error) => {
    //   console.error('Image fetching error:', error);
    //   res.status(500).json({ error: 'Internal server error' });
    // });

    // // Pipe the read stream to the response to send the image data
    // readstream.pipe(res);
  //});
  
  // Your Express routes and other code here...
  
  // Start your Express server
  




