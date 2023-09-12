import random
import csv
from datetime import datetime, timedelta
import pandas as pd


# Status determination based on temperature, moisture, and pressure
class Status:
    @staticmethod
    def determine(temp, moist, press):
        # Defined ranges for different statuses
        healthy_range = {
            "temperature": (15, 30),
            "moisture": (40, 80),
            "pressure": (95000, 105000)
        }

        dead_range = {
            "temperature": [(0, 5), (35, 40)],
            "moisture": [(0, 20), (90, 100)],
            "pressure": [(85000, 90000), (110000, 115000)]
        }

        # Check Healthy range
        if (healthy_range["temperature"][0] <= temp <= healthy_range["temperature"][1] and
                healthy_range["moisture"][0] <= moist <= healthy_range["moisture"][1] and
                healthy_range["pressure"][0] <= press <= healthy_range["pressure"][1]):
            return "Healthy"

        # Check Dead range
        for t_range in dead_range["temperature"]:
            for m_range in dead_range["moisture"]:
                for p_range in dead_range["pressure"]:
                    if (t_range[0] <= temp <= t_range[1] and
                            m_range[0] <= moist <= m_range[1] and
                            p_range[0] <= press <= p_range[1]):
                        return "Dead"

        # If not within Healthy or Dead ranges, then it's In Danger
        return "In Danger"


# Plant class definition
class Plant:
    def __init__(self, plant_id, plant_type):
        self.id = plant_id
        self.plant_type = plant_type
        self.start_date = datetime(2023, 1, 1) + timedelta(
            days=random.randint(0, (datetime.now() - datetime(2023, 1, 1)).days))
        self.position = ((self.id - 1) % 4 + 1, (self.id - 1) // 4 + 1, 1)
        self.moisture = random.uniform(40, 80)
        self.temperature = random.uniform(15, 30)
        self.atmospheric_pressure = random.uniform(95000, 105000)
        self.status = Status.determine(self.temperature, self.moisture, self.atmospheric_pressure)

    @property
    def age(self):
        return (datetime.now() - self.start_date).days


# Adjusted decay function with a larger decay rate closer to the normal condition
def accelerated_decay_plant(plant, periods=1):
    for _ in range(periods):
        # Adjusting temperature based on current temperature and time
        if 18 <= datetime.now().hour < 24 or 0 <= datetime.now().hour < 6:
            temp_change = random.uniform(0.1, 0.5) if plant.temperature > 22 else random.uniform(-0.1, -0.5)
            plant.temperature -= temp_change
        else:
            temp_change = random.uniform(0.1, 0.5) if plant.temperature < 22 else random.uniform(-0.1, -0.5)
            plant.temperature += temp_change

        # Adjusting moisture based on current moisture level
        moisture_change = random.uniform(1, 5) if plant.moisture > 60 else random.uniform(-1, -5)
        plant.moisture -= moisture_change

        # Adjusting atmospheric pressure
        plant.atmospheric_pressure += random.uniform(-300, 300)

        plant.status = Status.determine(plant.temperature, plant.moisture, plant.atmospheric_pressure)
    return plant


# Read plants from a CSV without Timestamp
def read_csv_without_timestamp_to_plants(file_path):
    plants_from_file = []
    with open(file_path, 'r') as csvfile:
        reader = csv.DictReader(csvfile)
        for row in reader:
            plant = Plant(int(row["Id"]), row["Plant Type"])
            plant.start_date = datetime.strptime(row["Start Date"], '%Y-%m-%d')
            x, y, z = eval(row["Position"])
            plant.position = (x, y, z)
            plant.status = row["Status"]
            plant.moisture = float(row["Moisture (%)"])
            plant.temperature = float(row["Temperature (°C)"])
            plant.atmospheric_pressure = float(row["Atmospheric Pressure (Pa)"])
            plants_from_file.append(plant)
    return plants_from_file


# Write plants to a CSV
def write_plants_to_csv(plants_list, file_path):
    with open(file_path, 'w', newline='') as csvfile:
        fieldnames = ["Id", "Plant Type", "Start Date", "Age", "Position", "Status", "Moisture (%)", "Temperature (°C)",
                      "Atmospheric Pressure (Pa)"]
        writer = csv.DictWriter(csvfile, fieldnames=fieldnames)
        writer.writeheader()
        for plant in plants_list:
            writer.writerow({
                "Id": plant.id,
                "Plant Type": plant.plant_type,
                "Start Date": plant.start_date.strftime('%Y-%m-%d'),
                "Age": plant.age,
                "Position": plant.position,
                "Status": plant.status,
                "Moisture (%)": plant.moisture,
                "Temperature (°C)": plant.temperature,
                "Atmospheric Pressure (Pa)": plant.atmospheric_pressure
            })
