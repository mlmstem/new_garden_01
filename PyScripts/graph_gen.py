from plant import Plant
from plant import Position
from pymongo import MongoClient
import pandas as pd
import matplotlib.pyplot as plt
import datetime
import csv
import json
import re

from connection import uri

# Connect to MongoDB
client = MongoClient(uri)
db = client["Plant"]

# Combine all data into one single dataframe
def concat_all_data():

    all_data = pd.DataFrame()

    for x in range(1,11):

        mongo_data = db["Decay_"+str(x)].find()
        mongo_df = pd.DataFrame(mongo_data)

        all_data = pd.concat([all_data, mongo_df])

    all_data.sort_values(by=['Timestamp'])
    all_data['_id'].astype('string_')

    return all_data

# Generates graphs for overview data
def overview_data():

    #Pie chart for plant status
    for x in range(1,11):
        mongo_data = db["Decay_"+str(x)].find()
        mongo_df = pd.DataFrame(mongo_data)

        fig, ax = plt.subplots()
        ax.pie(mongo_df['Status'].value_counts(), labels=mongo_df['Status'].unique(), autopct='%1.1f%%')
        plt.savefig('PyScripts\Graphs\pieStatus_'+str(x)+'.png')
        plt.close()
    
    #Pie chart for plant types
    for x in range(1,11):
        mongo_data = db["Decay_"+str(x)].find()
        mongo_df = pd.DataFrame(mongo_data)

        fig, ax = plt.subplots()
        ax.pie(mongo_df['Plant Type'].value_counts(), labels=mongo_df['Plant Type'].unique(), autopct='%1.1f%%')
        plt.savefig('PyScripts\Graphs\pieType_'+str(x)+'.png')
        plt.close()


# Generates graphs for specific plant data
def specific_data(data):

    grouped = data.groupby(['Position'])        

    #Line graph for moisture
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))

        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Moisture (%)'])
        plt.xticks(rotation=90, ha='right')
        plt.title("Moisture of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('%')
        plt.savefig('PyScripts\Graphs\moist_'+filename.group()+'.png')
        plt.close()

    #Line graph for temperature
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))

        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Temperature (°C)'])
        plt.xticks(rotation=90, ha='right')
        plt.title("Temperature of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('C')
        plt.savefig('PyScripts\Graphs\\temp_'+filename.group()+'.png')
        plt.close()

    #Line graph for pressure
    for name, group in grouped:

        filename = re.search("\(\d, \d, \d\)", str(name))
        
        fig, ax = plt.subplots()
        ax.plot(group['Timestamp'], group['Atmospheric Pressure (Pa)'])
        plt.xticks(rotation=90, ha='right')
        plt.title("Pressure of plant "+str(group['Id'].unique()))
        plt.xlabel('Timestamp')
        plt.ylabel('Pa')
        plt.savefig('PyScripts\Graphs\pressure_'+filename.group()+'.png')
        plt.close()

