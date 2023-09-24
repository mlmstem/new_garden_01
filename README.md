# new_garden_01


## Goals
The Farmbot web app is an application that is used to monitor the physiological condition of plants in a farm plot by analyzing data obtained from the FarmBot sensors, and includes an alert system that allow users to know about plants that may be in danger.

## Inspiration
The initial inspiration for our application came from observing the FarmBots machine field on top of the student pavilion at the University of Melbourne which inspired us to transform it into a digital 3D visualisation, After viewing many visual apps and gardening games, we considering the design of the garden simulator game to be a closer representation of the design of our application.

Garden Simulator is a game that involves the simulation of planting, sowing and harvesting plants. However, the realistic art style in Garden Simulator might be hard to implement, therefore we adopt a lower poly version of garden environment. 

<p align="center">
  <img src="https://i0.wp.com/www.thexboxhub.com/wp-content/uploads/2023/05/garden-simulator-review-1-scaled.jpg?w=1392&ssl=1" width="400" alt="Garden Simulator (Steam game)">
  <br>
  Garden Simulator (Steam Game 2022) [1]
</p>

## Dependencies

to download and run the code, you must have:

- Node (npm)
- Git, or download repo as zip file
- node mudules Dependencies:
    "express": "^4.18.2",
    "mongodb": "^6.0.0",
    "mongoose": "^7.5.0"
- Python3
- Python libraries needed:
  - Pandas
  - Matplotlib
  - Pymongo
- Nodemon (During developement phase -> for testing the server)

## How To Download

1. clone or download the repository into your local machine
2. download(if not downloaded) and initiate the unity engine
3. click open new project and select the repository folder at your local machine (this will open the project within the unity engine environment)
4. when the project is opened, go to scenes folder and select the "login scene"
5. press ctrl + p to initiate the scene
6. Go to the Backend folder directory at your local machine and initiate the server by typing node server.js at the terminal
6. input username and password to login to the farmbot application.

## Deployment

how to deploy the app:

Download webgl in unity

select the scenes setting and then click build when the project setup is ready

creat an ssh key to automatically initiate the server when the game is starting

go to itch.io or any game hosting website

provide the compressed file of your webgl game build

after the game hosting site has generated an url, user will be able to access the application by clicking the url 

## Features

Epic 1: Visualize data in a convenient and graphically interesting manner

Epic 2: Be alerted on possible dangers to the plan

Epic 3: Edit farm plot

Epic 4: Manage FarmBot account

Epic 5: Manage connection of FarmBot to the dashboard (this features might be disabled)

Epic 6: Sync FarmBot information


## Guide To Code

./Scenes ->  contains all the Scenes created for the application environment


./Assets -> includes all the imported material/ tools we used to develop the project 


./Assets/LowPolyFarmLite/Prefabs -> contains all the 3d model Used for developement of the application


./script -> the C# scripts used in the unity game environments (the scripts to control the camera, to control the mouse /keyboard input, to control the game objects)


./Backend -> all the server side environment and setup


./Backend/package.json -> all the dependencies needed for the project


./Backend/server.js -> the program to initiate the node server


./Backend/config -> the configuration of the backend server (the port and the mongodb url at development and production phase)


./Backend/model -> the data model and schema used to interact with mongodb


./Backend/routes -> the routes of interaction (req, res) between the server and application


./Mock Data -> the data used to test the stat generation methods as well the application funtions 

## Tech Stack

see below

![Alt text](<architecture diagram.png>)

## The Team

Scrum Master -> Haotian Zhuang
Product owner -> Chris Chen
UI/UX Designer -> Haotian Zhuang
Data Visulization Designer -> Yeuk Hon John Ng
Analysis Model Designer -> Zhexiao Chen
Front-End Developer -> Chris Chen, Chin Tong Leong
Back-End Developer -> Yeuk Hon John Ng, Zhexiao Chen


## Links
