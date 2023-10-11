const path = require('path');
const bodyParser = require('body-parser');
const crypto = require('crypto');
const mongoose = require('mongoose');
const multer = require('multer');
const methodOverride = require('method-override');
const Grid = require('gridfs-stream');

module.exports = (app, gfs) => {
  const conn = mongoose.connection;

  // Initialize GridFS
    gfs.collection('uploads');
    app.use(bodyParser.json());
    app.use(methodOverride('_method'));
    app.set('view engine', 'ejs');



  // Configure the custom storage engine
  const storage = multer.diskStorage({
    destination: (req, file, cb) => {
      // Set the destination where files will be saved
      cb(null, 'uploads'); // You can customize the destination folder
    },
    filename: (req, file, cb) => {
      crypto.randomBytes(16, (err, buf) => {
        if (err) return cb(err);

        const filename = file.fieldname+ '-' + Date.now();
        cb(null, filename);
      });
    },
  });

  const upload = multer({ storage });

  app.get('/', (req,res) =>{
    res.render('index');
  });

  // Middleware to handle file uploads
  app.post('/upload', upload.single('file'), (req, res) => {
    if (req.file) {
      // If a file is uploaded successfully, respond with the file info
      res.redirect('/');
    } else {
      // If there was no file uploaded, handle the error and respond with an error message
      res.status(400).json({ error: 'No file uploaded' });
    }
  });

  //@route Get /files
  //@description Display all files in json

  app.get('/files', (req, res) => {
    gfs.files.find().toArray((err, files) => {
      if (err) {
        return res.status(500).json({
          error: 'An error occurred while fetching files',
        });
      }
  
      if (!files || files.length === 0) {
        return res.status(404).json({
          error: 'No files exist',
        });
      }
  
      // If files are found, return the list of files
      return res.json(files);
    });
  });


}
