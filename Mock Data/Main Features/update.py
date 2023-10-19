from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import json

# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

# Construct the URI
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"


# Function to upload JSON to MongoDB
def upload_json_to_mongodb(json_path, db_name, collection_name):
    # Connect to MongoDB using the connection details
    client = MongoClient(uri)
    db = client[db_name]
    collection = db[collection_name]

    # Read the JSON file
    with open(json_path, "r") as json_file:
        data = json.load(json_file)

    # Clear existing data in the collection
    collection.delete_many({})

    # Insert new data into the collection
    collection.insert_many(data)

    # Close the connection
    client.close()

    return f"Successfully uploaded {len(data)} records to {db_name}.{collection_name}"


# Test the function
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\o.json", "UserData5", "UserData5")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\1.json", "UserData5", "UserDataDecay1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\2.json", "UserData5", "UserDataDecay2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\3.json", "UserData5", "UserDataDecay3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\4.json", "UserData5", "UserDataDecay4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User5\\5.json", "UserData5", "UserDataDecay5")

upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\o.json", "UserData4", "UserData4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\1.json", "UserData4", "UserDataDecay1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\2.json", "UserData4", "UserDataDecay2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\3.json", "UserData4", "UserDataDecay3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\4.json", "UserData4", "UserDataDecay4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User4\\5.json", "UserData4", "UserDataDecay5")

upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\o.json", "UserData3", "UserData3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\1.json", "UserData3", "UserDataDecay1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\2.json", "UserData3", "UserDataDecay2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\3.json", "UserData3", "UserDataDecay3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\4.json", "UserData3", "UserDataDecay4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User3\\5.json", "UserData3", "UserDataDecay5")

upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\o.json", "UserData2", "UserData2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\1.json", "UserData2", "UserDataDecay1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\2.json", "UserData2", "UserDataDecay2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\3.json", "UserData2", "UserDataDecay3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\4.json", "UserData2", "UserDataDecay4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User2\\5.json", "UserData2", "UserDataDecay5")

upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\o.json", "UserData1", "UserData1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\1.json", "UserData1", "UserDataDecay1")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\2.json", "UserData1", "UserDataDecay2")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\3.json", "UserData1", "UserDataDecay3")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\4.json", "UserData1", "UserDataDecay4")
upload_json_to_mongodb("E:\\connection\\Mock Data\\User1\\5.json", "UserData1", "UserDataDecay5")