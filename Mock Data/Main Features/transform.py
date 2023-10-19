from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import gridfs

# Connection details from connection.py
username = "admin"
password = "Password123"
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"
client = MongoClient(uri, server_api=ServerApi('1'))

# Connect to the database and collection
db = client['Plant']
fs = gridfs.GridFS(db, collection="testpic")

# Retrieve the file
file = fs.find_one({"filename": "Desktop.jpg"})
if file:
    with open('C:\\Users\\dell\\Desktop\\test.png', 'wb') as f:
        f.write(file.read())
    print("File retrieved and saved as test.png!")
else:
    print("File not found!")

client.close()
