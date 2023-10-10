from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import gridfs
import os
import sys
import fnmatch

# Connection details from connection.py
username = "admin"
password = "Password123"
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"
client = MongoClient(uri, server_api=ServerApi('1'))

# Connect to the database and collection
db = client['Graphs']
fs = gridfs.GridFS(db, collection="data_graphs")


# Store the file
def store_file(file_name):
    
    with open('PyScripts/Graphs/'+file_name, 'rb') as f:
        if(fs.exists({"filename": file_name})):
            file = fs.find_one({"filename": file_name})
            fs.delete(file_id=file._id)
            print("Deleted")
            fs.put(f, filename=file_name)
        else:
            fs.put(f, filename=file_name)
    print("File stored successfully!")


# Retrieve the file
def retrieve_file(file_name):
    
    file = fs.find_one({"filename": file_name})
    if file:
        with open('PyScripts/DB_Graphs/'+file_name, 'wb') as f:
            f.write(file.read())
        print("File retrieved!")
    else:
        print("File not found!")

#Transfer specific file
def transfer_one(filename):
    store_file(filename)
    retrieve_file(filename)

#Transfer all files
def transfer_all():
    graph_dir_path = 'PyScripts\\Graphs'
    graph_list = []

    for file_path in os.listdir(graph_dir_path):
        # check if current file_path is a file
        if os.path.isfile(os.path.join(graph_dir_path, file_path)):
            # add filename to list
            if file_path.endswith('.png'):
                graph_list.append(file_path)

    for x in graph_list:
        store_file(x)
        retrieve_file(x)
        full_path = os.path.join(graph_dir_path, x)
        delete_file(full_path)

#Download all files from database
def download_all():
    files = fs.list()
    for x in files:
        retrieve_file(str(x))

def delete_file(file_path):
    os.remove(file_path)


