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

demo_db = client["loginGarden"]
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

    demo_data = demo_db["accounts"]
    query = {"username": "chris"}
    demo_plants = demo_data.find_one(query)["plantList"]
    demo_df = pd.DataFrame(demo_plants)


    mongo_data = db[user].find()
    mongo_df = pd.DataFrame(mongo_data)

    moist_list = ["0-25%", "26-50%", "51-75%", "76-100%"]
    temp_list = ['0-10', '11-20', '21-30', '31-40']
    pressure_list = ['85000-92500', '92501-100000', '100001-107500', '107501-115000']
    status_list = ["Healthy", "In Danger", "Dead"]

    demo_df['moisture_percentile'] = pd.cut(demo_df['moisturePercentage'], [0,25,50,75,100], labels=moist_list)
    demo_df['temp_percentile'] = pd.cut(demo_df['temperatureCelsius'], [0,10,20,30,40], labels=temp_list)
    demo_df['pressure_percentile'] = pd.cut(demo_df['atmosphericPressurePa'], [85000, 92500, 100000, 107500, 115000], labels=pressure_list)

    bar_colors = ['green', 'yellow', 'red']


    status_dict =      {'Healthy': 0, 'In Danger': 0, 'Dead': 0}                                    | demo_df['status'].value_counts().to_dict()
    moist_dict =       {"0-25%": 0, "26-50%": 0, "51-75%": 0, "76-100%":0}                          | demo_df['moisture_percentile'].value_counts().to_dict()
    temp_dict =        {"0-10": 0, "11-20": 0, "21-30": 0, "31-40":0}                               | demo_df['temp_percentile'].value_counts().to_dict()
    pressure_dict =    {"85000-92500": 0, "92501-100000": 0, "100001-107500": 0, "107501-115000":0} | demo_df['pressure_percentile'].value_counts().to_dict()

    #Bar chart for plant status
    fig, ax = plt.subplots()
    #ax.bar(status_list, list(status_dict.values()), color=bar_colors, width=0.5)
    ax.pie(demo_df['status'].value_counts(), labels=status_list, colors = bar_colors, autopct='%1.1f%%')
    plt.title("Plant status distribution")
    #plt.xlabel('Status')
    #plt.ylabel('Number of plants')
    plt.savefig('PyScripts\Graphs\\pieStatus.png')
    plt.close()
    print("done")

    
    #Bar chart for plant moisture
    fig, ax = plt.subplots()
    ax.bar(moist_list, list(moist_dict.values()), width=0.5)
    plt.title("Plant moisture level distribution")
    plt.xlabel('Moisture level')
    plt.ylabel('Number of plants')
    plt.savefig('PyScripts\Graphs\\barMoist.png')
    plt.close()
    print("done")
    

    #Bar chart for plant temp
    fig, ax = plt.subplots()
    ax.bar(temp_list, list(temp_dict.values()), width=0.5)
    plt.title("Plant temperature distribution")
    plt.xlabel('Temperature degree (C)')
    plt.ylabel('Number of plants')
    plt.savefig('PyScripts\Graphs\\barTemp.png')
    plt.close()
    print("done")

    
    #Bar chart for plant pressure
    fig, ax = plt.subplots()
    ax.bar(pressure_list, list(pressure_dict.values()), width=0.5)
    plt.title("Plant pressure distribution")
    plt.xlabel('Pressure (Pa)')
    plt.ylabel('Number of plants')
    plt.savefig('PyScripts\Graphs\\barPressure.png')
    plt.close()
    print("done")

    
    #Pie chart for plant type distribution
    fig, ax = plt.subplots()
    ax.pie(mongo_df['Plant Type'].value_counts(), labels=mongo_df['Plant Type'].unique(), autopct='%1.1f%%')
    plt.title("Plant type distribution")
    plt.savefig('PyScripts\Graphs\pieType.png')
    plt.close()
    print("done")
    

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
