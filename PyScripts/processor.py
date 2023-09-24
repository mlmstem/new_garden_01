from plant import Plant
from plant import Position
import pandas as pd
import sklearn as sk
import matplotlib as plt
import datetime
import csv
import sys

#Test arguments: 2023-09-08 06:14:10.294746 Cabbage 2023-09-08 0 (2,30,4) Healthy 50 23 70
#Obtains inputs via stdin arguements, outputs into csv file

def plant_to_tuple(plant):
    return (plant.type, plant.status, plant.position, plant.moist, plant.temp, plant.gas)

def data_processor (argv):
    #Do processing here
    timestamp =argv[0]+" "+argv[1]
    x = argv[5]
    y = argv[6]
    z = argv[7]
    position = Position(x, y, z)

    data = Plant(timestamp, argv[2], argv[3], argv[4], position, argv[8], argv[9], argv[10], argv[11])
    row = plant_to_tuple(data)

    return row

def stdin_to_csv(argv):
    
    #Process stdin input
    data_row = data_processor(argv)

    #Write to CSV
    writer.writerow(data_row)
    return

#Get arguments, process and write to csv
arguments = sys.argv[1:]
file = open('PyScripts\processed_data.csv', 'w')
writer = csv.writer(file)
stdin_to_csv(arguments)
file.close()