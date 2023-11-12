from pymongo import MongoClient
import json
from urllib.parse import quote_plus
from pymongo.server_api import ServerApi
import os
import pandas as pd

# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

# Construct the URI
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"

# Connect to MongoDB
client = MongoClient(uri, server_api=ServerApi('1'))
outputdb = client["Reports"]
srcdb = client["SyncUserData"]


def gen_report(user):
    collection = srcdb[user]
    mongo_data = collection.find()
    mongo_df = pd.DataFrame(mongo_data)
    age_range = []
    most_common = mongo_df['Plant Type'].value_counts().index.tolist()
    avg_temp = mongo_df["Temperature (°C)"].mean()
    avg_moist = mongo_df["Moisture (%)"].mean()
    avg_pressure = mongo_df["Atmospheric Pressure (Pa)"].mean()
    age_range.append(int(mongo_df["Age"].min())) 
    age_range.append(int(mongo_df["Age"].max())) 

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
        "user": user,
        "plants_info": plants_info,
        "plant_counts": plant_counts,
        "plant_coordinates": plant_coordinates,
        "plants_above_100": plants_above_100,
        "plants_below_100": plants_below_100,
        "plants_by_status": plants_by_status,
        "most_common_plant": most_common[0],
        "age_range": age_range,
        "avg_temp": avg_temp,
        "avg_moist": avg_moist,
        "avg_pressure": avg_pressure
    }

    # Save the JSON report to a file
    report_path = "PyScripts\\report_"+user+".json"
    with open(report_path, "w") as json_file:
        json.dump(report, json_file, indent=4)
    json_file.close()


def upload_report(user):
    # Upload the report to MongoDB and delete from local
    report_path = "PyScripts\\report_"+user+".json"
    analysis_collection = outputdb["Reports"]
    user_query = { "user": user }
    analysis_collection.delete_one(user_query)
    with open(report_path, "r") as json_file:
        data = json.load(json_file)
        analysis_collection.insert_one(data)
    json_file.close()
    os.remove(report_path)