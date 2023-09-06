import random
from datetime import datetime, timedelta

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

        in_danger_range = {
            "temperature": [(5, 15), (30, 35)],
            "moisture": [(20, 40), (80, 90)],
            "pressure": [(90000, 95000), (105000, 110000)]
        }

        if healthy_range["temperature"][0] <= temp <= healthy_range["temperature"][1] and \
           healthy_range["moisture"][0] <= moist <= healthy_range["moisture"][1] and \
           healthy_range["pressure"][0] <= press <= healthy_range["pressure"][1]:
            return "Healthy"

        for t_range in in_danger_range["temperature"]:
            for m_range in in_danger_range["moisture"]:
                for p_range in in_danger_range["pressure"]:
                    if t_range[0] <= temp <= t_range[1] and m_range[0] <= moist <= m_range[1] and p_range[0] <= press <= p_range[1]:
                        return "In Danger"

        return "Dead"

# Plant class definition
class Plant:
    def __init__(self, plant_id, plant_type):
        self.id = plant_id
        self.timestamp = datetime.now()
        self.plant_type = plant_type
        self.start_date = datetime(2023, 1, 1) + timedelta(days=random.randint(0, (self.timestamp - datetime(2023, 1, 1)).days))
        self.position = ((self.id-1) % 4 + 1, (self.id-1) // 4 + 1, 1)
        self.moisture = random.uniform(40, 80)
        self.temperature = random.uniform(15, 30)
        self.atmospheric_pressure = random.uniform(95000, 105000)
        self.status = Status.determine(self.temperature, self.moisture, self.atmospheric_pressure)

    @property
    def age(self):
        return (datetime.now() - self.start_date).days

    def __repr__(self):
        return (f"<Plant(id={self.id}, timestamp={self.timestamp}, plant_type={self.plant_type}, age={self.age} days, "
                f"position={self.position}, status={self.status}, moisture={self.moisture:.2f}%, temperature={self.temperature:.2f}°C, "
                f"atmospheric_pressure={self.atmospheric_pressure:.2f} Pa)>")

# Decay function to simulate changes over time
def decay_plant(plant, periods=1):
    for _ in range(periods):
        plant.timestamp += timedelta(hours=6)
        if 18 <= plant.timestamp.hour < 24 or 0 <= plant.timestamp.hour < 6:
            plant.temperature -= random.uniform(0.5, 1.5)
        else:
            plant.temperature += random.uniform(0.5, 1.5)
        plant.moisture -= random.uniform(1, 5)
        plant.atmospheric_pressure += random.uniform(-500, 500)
        plant.status = Status.determine(plant.temperature, plant.moisture, plant.atmospheric_pressure)
    return plant
