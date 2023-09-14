from plant import Plant
from plant import Position
import pandas as pd
import matplotlib.pyplot as plt
import datetime
import csv


all_data = pd.DataFrame()

for x in range(1,11):
    data_df = pd.read_csv('Mock Data\Faster_Decay_v2_'+str(x)+'.csv')

    fig, ax = plt.subplots()
    ax.pie(data_df['Status'].value_counts(), labels=data_df['Status'].unique(), autopct='%1.1f%%')
    plt.savefig('PyScripts\Graphs\pie_'+str(x)+'.png')

    all_data = pd.concat([all_data, data_df])

all_data.sort_values(by=['Timestamp'])

grouped = all_data.groupby(['Position'])

for name, group in grouped:

    fig, ax = plt.subplots()
    ax.plot(group['Timestamp'], group['Moisture (%)'])
    plt.xticks(rotation=90, ha='right')
    plt.title("Moisture of plant "+str(group['Id'].unique()))
    plt.xlabel('Timestamp')
    plt.ylabel('%')
    plt.savefig('PyScripts\Graphs\moist_'+str(name)+'.png')

for name, group in grouped:

    fig, ax = plt.subplots()
    ax.plot(group['Timestamp'], group['Temperature (°C)'])
    plt.xticks(rotation=90, ha='right')
    plt.title("Temperature of plant "+str(group['Id'].unique()))
    plt.xlabel('Timestamp')
    plt.ylabel('C')
    plt.savefig('PyScripts\Graphs\\temp_'+str(name)+'.png')

for name, group in grouped:

    fig, ax = plt.subplots()
    ax.plot(group['Timestamp'], group['Atmospheric Pressure (Pa)'])
    plt.xticks(rotation=90, ha='right')
    plt.title("Pressure of plant "+str(group['Id'].unique()))
    plt.xlabel('Timestamp')
    plt.ylabel('Pa')
    plt.savefig('PyScripts\Graphs\pressure_'+str(name)+'.png')

  