from pymongo import MongoClient
import json
import pandas as pd

from connection import uri

# Load the connection details from the connection_file_path
with open("PyScripts/connection/connection.py", "r") as file:
    exec(file.read())

# Connect to MongoDB
client = MongoClient(uri)
db = client["Plant"]
collection = db["TestDataC"]

# List all plant IDs and their names
plants_info = [{"Id": plant["Id"], "Plant Type": plant["Plant Type"]} for plant in collection.find()]

# 1. Count and list each plant type
plant_counts = {}
for plant in collection.find():
    plant_type = plant["Plant Type"]
    plant_counts[plant_type] = plant_counts.get(plant_type, 0) + 1
plants_list = list(plant_counts.keys())

# 2. List coordinates for each plant type
plant_coordinates = {}
for plant_type in plants_list:
    plant_coordinates[plant_type] = [
        plant["Position"] for plant in collection.find({"Plant Type": plant_type})
    ]

# 3. List plants with age > 100 days and age < 100 days
plants_above_100 = [plant["Id"] for plant in collection.find({"Age": {"$gt": 100}})]
plants_below_100 = [plant["Id"] for plant in collection.find({"Age": {"$lt": 100}})]

# 4. List plants for each status
statuses = ["Healthy", "In Danger", "Dead", "Removed"]
plants_by_status = {}
for status in statuses:
    plants_by_status[status] = [plant["Id"] for plant in collection.find({"Status": status})]


# Create a JSON report
report = {
    "plants_info": plants_info,
    "plant_counts": plant_counts,
    "plant_coordinates": plant_coordinates,
    "plants_above_100": plants_above_100,
    "plants_below_100": plants_below_100,
    "plants_by_status": plants_by_status,
}

# Save the JSON report to a file
report_path = "PyScripts/connection/analysis_report.json"
with open(report_path, "w") as json_file:
    json.dump(report, json_file, indent=4)

# Upload the report to MongoDB under the collection "Analysis Data"
analysis_collection = db["Analysis Data"]
with open(report_path, "r") as json_file:
    data = json.load(json_file)
    analysis_collection.insert_one(data)


