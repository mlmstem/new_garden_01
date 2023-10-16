const mongoose = require('mongoose');
const Account = mongoose.model('accounts');
const Plant = mongoose.model('Plant')
const Image = mongoose.model('imgtest')
const bodyParser = require('body-parser');
const ObjectId = require('mongodb').ObjectId;

module.exports = app => {
    app.use(bodyParser.json());
    app.get('/account', async (req, res) => {
        // use req.query to get what user inputing into the browser
        // use res (response) to send the information back to the browser

        const { rUsername, rPassword } = req.query;

        if (rUsername == null || rPassword == null) {
            res.send("Invalid credentials");
            return;
        }
        // console.log("Received request with parameters:", req.query);
        //console.log("Sending response:", res);


        var userAccount = await Account.findOne({ username: rUsername });
        if (userAccount == null) {
            // Create a new account
            //console.log("Create new account...");
            var newAccount = new Account({
                username: rUsername,
                password: rPassword,

                lastAuthentication: Date.now(),
                plantList: []

            });

            await newAccount.save();
            res.send(newAccount);

            return;
        } else {

            if (rPassword == userAccount.password) {
                userAccount.lastAuthentication = Date.now();
                res.send(userAccount);
                await userAccount.save();

                return;
            }
        }
        res.send("Invalid Credentials");
        return;

    });

    app.post('/account/add-plant', async (req, res) => {
        try {
            const { username, plantInfo } = req.body;

            if (!username || !plantInfo) {
                res.status(400).json({ error: "Invalid request" });
                return;
            }

            // Find the user by username and add the new plant to their plantList
            const userAccount = await Account.findOne({ username });

            if (!userAccount) {
                res.status(404).json({ error: "User not found" });
                return;
            }
            // console.log("Request Body:", req.body)

            // Create a new Plant object and assign its properties
            const newPlant = new Plant({
                plantType: plantInfo.plantType,
                startDate: plantInfo.startDate,
                age: plantInfo.age,
                position: plantInfo.position,
                status: plantInfo.status,
                moisturePercentage: plantInfo.moisturePercentage,
                temperatureCelsius: plantInfo.temperatureCelsius,
                atmosphericPressurePa: plantInfo.atmosphericPressurePa,
            });

            // Save the new plant to the user's plantList
            // console.log("User Account:", userAccount);
            // console.log("New Plant Object:", newPlant);

            userAccount.plantList.push(newPlant);
            await userAccount.save();

            res.status(200).json({ message: "Plant added successfully", plant: newPlant });
        } catch (error) {
            console.error(error);
            res.status(500).json({ error: "Internal server error" });
        }
    });

    // Additional routes for handling plants...
    app.get('/account/getData', async (req, res) => {
        const { username } = req.body;
        // console.log(req.body);

        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        res.send(userAccount.password);
    });

    app.get('/account/getProfileData', async (req, res) => {
        const { username } = req.body;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        // set up structure of data to receive
        var gotData = {
            usernameGot: userAccount.username,
            emailGot: userAccount.password,
            passwordGot: userAccount.password
        };
        // console.log(gotData);

        res.send(gotData);
    });

    app.get('/account/getCurrentGarden', async (req, res) => {
        const { username } = req.body;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        // set up structure of data to receive
        var gotData = {
            X: 1,
            Y: 1,
            plantType: userAccount.plantList[1].plantType,
            plantStatus: userAccount.plantList[1].status
        };

        // console.log(gotData);

        res.send(gotData);
    });

    app.post('/account/removePlant', async (req, res) => {
        const { username } = req.body;
        console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }
        console.log(userAccount.plantList);
        // remove from plant array
        const removed = await Account.deleteMany({ plantList: { plantType: 'tomato' } });
        console.log(removed);
    });


    
    app.get('/account/getGraph', async (req, res) => {
        const { imageID } = req.body;
        console.log(req.body);

        console.log(Image.findOne());
        console.log(Account.findOne({ username: 'abc' }));
        const imageFile = await Image.findOne();

        if (!imageFile) {
            res.status(404).json({ error: "Image is not found" });
            console.log("image not found");
            return;
        }

        // console.log(imageFile);
        res.send(imageFile.data);


    });


    app.get('/account/getPlantData', async (req, res) => {
        const { username } = req.body;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        // set up structure of data to receive
        var gotData = {
            name: userAccount.plantList[1].plantType,
            id: "1",
            type: userAccount.plantList[1].plantType,
            startDate: userAccount.plantList[1].startDate,
            days: userAccount.plantList[1].age,
            position: userAccount.plantList[1].position,
            status: userAccount.plantList[1].status,
            moisture: userAccount.plantList[1].moisturePercentage,
            temperature: userAccount.plantList[1].temperatureCelsius,
            pressure: userAccount.plantList[1].atmosphericPressurePa
        };
        // console.log(gotData);

        res.send(gotData);
    });
    



}


