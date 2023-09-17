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
    print(file._id)
    if file:
        with open('PyScripts/DB_Graphs/'+file_name, 'wb') as f:
            f.write(file.read())
        print("File retrieved and saved as png!")
    else:
        print("File not found!")

#Transfer specific file
def transfer_one(filename):
    store_file(filename)
    retrieve_file(filename)

#Transfer all files
def transfer_all():
    graph_dir_path = r'PyScripts\Graphs'
    graph_list = []

    for file_path in os.listdir(graph_dir_path):
        # check if current file_path is a file
        if os.path.isfile(os.path.join(graph_dir_path, file_path)):
            # add filename to list
            graph_list.append(file_path)

    for x in graph_list:
        store_file(x)
        retrieve_file(x)



# Connect to the database and collection
db = client['Plant']
fs = gridfs.GridFS(db, collection="testpic")

arguments = sys.argv[1:]
if (len(arguments)!=1):
    print("Arguments incorrect.")
else:
    if(arguments[0]=="all"):
        print("Transferring all")
        transfer_all()
    elif(fnmatch.fnmatch(arguments[0], '*.png')):

        graph_dir_path = r'PyScripts\Graphs'
        graph_list = []

        for file_path in os.listdir(graph_dir_path):
            # check if current file_path is a file
            if os.path.isfile(os.path.join(graph_dir_path, file_path)):
                # add filename to list
                graph_list.append(file_path)
        if(arguments[0] not in graph_list):
            print("File is not in folder.")
        else:
            print("Transferring "+arguments[0])
            transfer_one(arguments[0])

    else:
        print("Arguments incorrect.")


client.close()
