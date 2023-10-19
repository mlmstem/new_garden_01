from pymongo import MongoClient
from urllib.parse import quote_plus
import threading

# Original username and password
username = "admin"
password = "Password123"

# Escaped username and password
username_escaped = quote_plus(username)
password_escaped = quote_plus(password)

# 连接参数
uri = f"mongodb+srv://{username_escaped}:{password_escaped}@cluster0.g9kdlqh.mongodb.net/?retryWrites=true&w=majority"

# 建立连接
client = MongoClient(uri)


# 定义处理更改的函数
def handle_change(source_coll, target_coll, change):
    event_type = change["operationType"]
    doc = change.get("fullDocument")

    # Check if doc is None
    if doc is None:
        return

    if event_type == "insert":
        target_coll.insert_one(doc)
    elif event_type in ["update", "replace"]:
        target_coll.replace_one({"_id": doc["_id"]}, doc)
    elif event_type == "delete":
        target_coll.delete_one({"_id": change["documentKey"]["_id"]})


# 监听集合的函数
def watch_collection(source_db, source_coll_name, target_db, target_coll_name):
    source_coll = client[source_db][source_coll_name]
    target_coll = client[target_db][target_coll_name]

    # Include full_document option
    for change in source_coll.watch(full_document='updateLookup'):
        handle_change(source_coll, target_coll, change)


# 启动监听
threads = []
for i in range(1, 11):
    t = threading.Thread(target=watch_collection, args=("Plant", f"Decay_{i}", "SyncData", f"SyncData{i}"))
    t.start()
    threads.append(t)

# 等待所有线程完成
for t in threads:
    t.join()
