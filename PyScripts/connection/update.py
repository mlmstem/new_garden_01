import json
from pymongo import MongoClient

from connection import uri


def upload_json_to_mongodb(json_path, connection_file_path, db_name, collection_name):
    # Load the connection details from the connection_file_path
    with open(connection_file_path, "r") as file:
        exec(file.read())

    # Read the JSON file
    with open(json_path, "r") as json_file:
        data = json.load(json_file)

    # Connect to MongoDB using the connection details from connection.py
    client = MongoClient(uri)
    db = client[db_name]
    collection = db[collection_name]

    # Clear existing data in the collection
    collection.delete_many({})

    # Insert new data into the collection
    collection.insert_many(data)

    # Close the connection
    client.close()

    return f"Successfully uploaded {len(data)} records to {db_name}.{collection_name}"


# Test the function
upload_json_to_mongodb("E:\\connection\\Mock Data ORIGIN.json", "E:\\connection\\connection.py", "Plant", "TestDataC")
