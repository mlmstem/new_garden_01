from urllib.parse import quote_plus
from pymongo.mongo_client import MongoClient
from pymongo.server_api import ServerApi
import pandas as pd
import matplotlib.pyplot as plt

import re

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
db = client["SyncUserData"]

# Combine all data into one single dataframe
def concat_all_data():

    all_data = pd.DataFrame()

    for x in range(1,11):

        mongo_data = "Mock Data\Faster_Decay_v2_"+str(x)+".csv"
        mongo_df = pd.read_csv(mongo_data)

        all_data = pd.concat([all_data, mongo_df])

    all_data.sort_values(by=['Timestamp'])
    all_data['Id'].astype('string_')
    return all_data

# Generates graphs for overview data
def overview_data(user):

    
    mongo_data = db[user].find()
    mongo_df = pd.DataFrame(mongo_data)

    status_list = ["Healthy", "In Danger", "Dead"]
    bar_colors = ['green', 'yellow', 'red']
    dictionary = {'Healthy': 0, 'In Danger': 0, 'Dead': 0} | mongo_df['Status'].value_counts().to_dict()
    #Pie chart for plant status
    fig, ax = plt.subplots()
    ax.bar(status_list, list(dictionary.values()), color=bar_colors)
    plt.savefig('PyScripts\Graphs\pieStatus_'+str(user)+'.png')
    plt.close()

    fig, ax = plt.subplots()
    ax.pie(mongo_df['Plant Type'].value_counts(), labels=mongo_df['Plant Type'].unique(), autopct='%1.1f%%')
    plt.savefig('PyScripts\Graphs\pieType_'+str(user)+'.png')
    plt.close()

# Generates graphs for specific plant data
def specific_data(data):

    all_data = pd.DataFrame()
    all_data = pd.concat([all_data, data])
    grouped = all_data.groupby(['Position'])        

    #Line graph for moisture
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))

        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Moisture (%)'], marker='o')
        plt.xticks(rotation=45, ha='right', fontsize=7)
        plt.title("Moisture of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('%')
        plt.tight_layout()
        plt.savefig('PyScripts\Graphs\lineMoist_'+filename.group()+'.png')
        plt.close()

    #Line graph for temperature
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))

        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Temperature (°C)'], marker='o')
        plt.xticks(rotation=45, ha='right', fontsize=7)
        plt.title("Temperature of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('C')
        plt.tight_layout()
        plt.savefig('PyScripts\Graphs\lineTemp_'+filename.group()+'.png')
        plt.close()

    #Line graph for pressure
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))
        
        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Atmospheric Pressure (Pa)'], marker='o')
        plt.xticks(rotation=45, ha='right', fontsize=7)
        plt.title("Pressure of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('Pa')
        plt.tight_layout()
        plt.savefig('PyScripts\Graphs\linePressure_'+filename.group()+'.png')
        plt.close()

#specific_data(concat_all_data())
overview_data('User1')
