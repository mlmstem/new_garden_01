import graph_gen as gen
import analyze as al
import image_transfer as img_t

from bson.json_util import dumps
from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi


#Single entry point for all functions related to graph generation and transfer

"""
Arguements (only one) that are allowed:

all: transfers all graphs
down: downloads all graphs from database to local storage
*.png: single png file for transfer
overview: generates overview graphs
specific: generates specific plant graphs
"""




# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"


#Manual mode
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

    
#Listening mode
client = MongoClient(uri, server_api=ServerApi('1'))
db = client["loginGarden"]
coll = db["accounts"]

stream = coll.watch()

print("Listening to changes...")
for change in stream:
    id = change['documentKey']['_id']
    acc = coll.find_one( {"_id": id})
    test_user = acc['username']
    #user = change['ns']['coll']
    print("Changes in: "+test_user)
    print("Generating overview graphs for: "+test_user)
    gen.overview_data(test_user)
    print("Transferring all graphs")
    img_t.transfer_all()

    #print("Generating report")
    #al.gen_report(user)
    #print("Uploading report")
    #al.upload_report(user)

    print("Finished")
    print("Listening to changes...")
