from flask import Flask
import pymongo
from bson.json_util import dumps
from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import gridfs
import os

import pandas as pd
import matplotlib.pyplot as plt

app = Flask(__name__)



# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

# Construct the URI
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"

# Remaining connection code...
client = MongoClient(uri, server_api=ServerApi('1'))
try:
    client.admin.command('ping')
    print("Connection is successful for this time.")
except Exception as e:
    print(e)

client = MongoClient(uri)
db = client["SyncUserData"]
user1 = db['User1']

gdb = client['Graphs']
fs = gridfs.GridFS(gdb, collection="data_graphs")


change_stream = client["SyncUserData"]["User1"].watch()
print("Listening to changes")
for change in change_stream:
    print(dumps(change))

    data = user1.find()
    datadf = pd.DataFrame(data)
    fig, ax = plt.subplots()
    ax.pie(datadf['Status'].value_counts(), labels=datadf['Status'].unique(), autopct='%1.1f%%')
    plt.savefig('PyScripts\Graphs\pieStatus1.png')
    plt.close()

    with open('PyScripts/Graphs/pieStatus1.png', 'rb') as f:
            if(fs.exists({"filename": "pieStatus1.png"})):
                file = fs.find_one({"filename": "pieStatus1.png"})
                fs.delete(file_id=file._id)
                print("Deleted")
                fs.put(f, filename="pieStatus1.png")
            else:
                fs.put(f, filename="pieStatus1.png")
    print("File stored successfully!")

    print('')

@app.route('/')
def home():
    return "Hi"

if __name__ == '__main__':
    app.run(debug=True)

