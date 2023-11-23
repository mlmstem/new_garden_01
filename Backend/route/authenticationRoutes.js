const mongoose = require('mongoose');
const Account = mongoose.model('accounts');
const Plant = mongoose.model('Plant')
const Image = mongoose.model('imgtest')
const bodyParser = require('body-parser');
const ObjectId = require('mongodb').ObjectId;

const numColumns = 4;

module.exports = app => {
    app.use(bodyParser.json());
    app.get('/account', async (req, res) => {
        // use req.query to get what user inputing into the browser
        // use res (response) to send the information back to the browser

        console.log("logging in");

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
        const { username } = req.query;
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

        console.log("receiving web request");


        const { username } = req.query;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        // Extract the plantList array from the user's account
        const plantList = userAccount.plantList;

        // Create an array to hold garden data objects
        const gardenDataArray = [];

        // Iterate through the plantList and create garden data objects
        plantList.forEach((plant, index) => {

            const X = parseInt(plant.position.slice(1, 2));
            const Y = parseInt(plant.position.slice(4, 5));
            console.log("x is %d", X);
            console.log("y is %d", Y);

            const gardenData = {
                X: X,
                Y: Y,
                plantType: plant.plantType,
                plantStatus: plant.status
            };
            gardenDataArray.push(gardenData);
        });

        const gardenDataArrayWrapper = { gardenDataArray };
        res.json(gardenDataArrayWrapper);
    });

    app.post('/account/removePlant', async (req, res) => {
        const { username, rowIndex, colIndex } = req.body;
    
        console.log("receives request on the client end");

        
    
        // Find the user in the database
        const userAccount = await Account.findOne({ username });
    
        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }
    
        // Iterate through the plantList array to find and remove the corresponding plant
        let removedPlantIndex = -1;
        userAccount.plantList.forEach((plant, index) => {
            const plantRow = parseInt(plant.position.slice(1, 2));
            const plantCol = parseInt(plant.position.slice(4, 5));
    
            if (plantRow === rowIndex && plantCol === colIndex) {
                removedPlantIndex = index;
            }
        });
    
        if (removedPlantIndex !== -1) {
            // Remove the plant from the array
            userAccount.plantList.splice(removedPlantIndex, 1);
    
            // Save the updated user account
            await userAccount.save();

            
            console.log("Plant removed successfully");
    
            res.status(200).json({ message: "Plant removed successfully" });
        } else {
            console.log("Plant remove failure");
            res.status(404).json({ error: "Plant not found at the specified position" });
        }
    });



    app.get('/account/getGraph', async (req, res) => {

        const { username } = req.query;
    
        // Find user in the database
        const userAccount = await Account.findOne({ username });
    
        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }
    
        // Extract the plantList array from the user's account
        const plantList = userAccount.plantList;
    
        // Create an array to hold garden data objects
        const gardenDataArray = [];
    
        // Helper function to format numbers to a string with one decimal place
        const formatNumber = (number) => {
            return parseFloat(number).toFixed(1);
        };
    
        // Iterate through the plantList and create garden data objects
        plantList.forEach((plant, index) => {
            // Ensure that Temp, moisture, and Pressure are sent as floats
            const Temp = parseFloat(plant.temperatureCelsius);
            const moisture = parseFloat(plant.moisturePercentage);
            const Pressure = parseFloat(plant.atmosphericPressurePa);
    
            // Ensure that plantStatus is sent as a string
            const plantStatus = String(plant.status);
    
            const gardenData = {
                Temp: isNaN(Temp) ? '0.0' : formatNumber(Temp),
                Pressure: isNaN(Pressure) ? '0.0' : formatNumber(Pressure),
                moisture: isNaN(moisture) ? '0.0' : formatNumber(moisture),
                plantStatus: plantStatus,
            };
    
            console.log(" the temp is: " + gardenData.Temp);
            console.log(" the moisture is: " + gardenData.moisture);
    
            gardenDataArray.push(gardenData);
        });
    
        const gardenDataArrayWrapper = { gardenDataArray };
        res.json(gardenDataArrayWrapper);
    });
    


    app.get('/account/getPlantData', async (req, res) => {
        const { username, rowIndex, colIndex } = req.query;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        var plantPos = "(" + colIndex + ", " + rowIndex + ")";
        console.log("postion: ", plantPos);

        // Extract the plantList array from the user's account
        const plantList = userAccount.plantList;
        //console.log(plantList);

        var foundPlant;
        var i;
        // Iterate through the plantList and create garden data objects
        plantList.forEach((plant, index) => {
            if (plant.position == plantPos) {
                foundPlant = plant;
                i = index;
            }
        });

        console.log(foundPlant);
        if (foundPlant == undefined) {
            res.status(404).json({ error: "Plant is not found" });
            return;
        }


        // set up structure of data to receive
        var gotData = {
            name: foundPlant.plantType,
            id: i,
            type: foundPlant.plantType,
            startDate: foundPlant.startDate,
            days: foundPlant.age,
            position: foundPlant.position,
            status: foundPlant.status,
            moisture: foundPlant.moisturePercentage,
            temperature: foundPlant.temperatureCelsius,
            pressure: foundPlant.atmosphericPressurePa
        };
        // console.log(gotData);

        res.send(gotData);
    });

    app.get('/account/getPlantDanger', async (req, res) => {
        const { username, rowIndex, colIndex } = req.body;
        // console.log(req.body);

        // find user in database
        const userAccount = await Account.findOne({ username });

        if (!userAccount) {
            res.status(404).json({ error: "User is not found" });
            return;
        }

        var plantPos = "(" + colIndex + ", " + rowIndex + ")";
        console.log("postion: ", plantPos);

        // Extract the plantList array from the user's account
        const plantList = userAccount.plantList;
        //console.log(plantList);

        var foundPlant;
        // Iterate through the plantList and create garden data objects
        plantList.forEach((plant, index) => {
            if (plant.position == plantPos) {
                foundPlant = plant;
            }
        });
        console.log("number %d", i);
        console.log(foundPlant);
        if (foundPlant == undefined) {
            res.status(404).json({ error: "Plant is not found" });
            return;
        }

        // set up structure of data to receive
        var gotData = {
            danger: foundPlant.status
        };
        // console.log(gotData);

        res.send(gotData);
    });


}