import graph_gen as gen
import plant as pl
import image_transfer as img_t
import sys
import fnmatch
import os

from flask import Flask
import pymongo
from bson.json_util import dumps
from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import gridfs





#Single entry point for all functions related to graph generation and transfer

"""
Arguements (only one) that are allowed:

all: transfers all graphs
down: downloads all graphs from database to local storage
*.png: single png file for transfer
overview: generates overview graphs
specific: generates specific plant graphs
"""

app = Flask(__name__)


# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"

"""
arguments = sys.argv[1:]

if (len(arguments)!=1):
    print("Arguments incorrect.")
    
else:

    if(arguments[0]=="all"):
        print("Transferring all")
        img_t.transfer_all()

    elif(arguments[0]=="down"):
        img_t.download_all()
    
    elif(fnmatch.fnmatch(arguments[0], '*.png')):

        graph_dir_path = 'PyScripts\\Graphs'
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
            img_t.transfer_one(arguments[0])

    elif(arguments[0]=='overview'):
        print("Generating overview graphs.")
        gen.overview_data()
    elif(arguments[0]=='specific'):
        print("Generating specific graphs.")
        all_data = gen.concat_all_data()
        gen.specific_data(all_data)
    else:
        print("Arguments incorrect.")

"""

client = MongoClient(uri)
db = client["SyncUserData"]
user1 = db['User1']

gdb = client['Graphs']
fs = gridfs.GridFS(gdb, collection="data_graphs")

stream = client["SyncUserData"].watch()

print("Listening to changes")
for change in stream:
    print(dumps(change['ns']['coll']))
    print("Generating overview graphs for "+change['ns']['coll']+".")
    user = change['ns']['coll']
    gen.overview_data(user)

@app.route('/')
def home():
    return "Hi"

if __name__ == '__main__':
    app.run(debug=True)