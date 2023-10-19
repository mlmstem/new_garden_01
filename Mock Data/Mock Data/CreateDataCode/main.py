# Integrating all functionalities into the Plant class

class Plant:
    plant_types = ["Chilli", "Cucumber", "Tomato"]

    def __init__(self, plant_id):
        self.id = plant_id
        self.plant_type = random.choice(Plant.plant_types)
        self.start_date = datetime(2023, 1, 1) + timedelta(days=random.randint(0, (datetime.now() - datetime(2023, 1, 1)).days))
        self.timestamp = datetime(2023, 1, 1, random.randint(0, 23), random.randint(0, 59), random.randint(0, 59))
        self.position = ((self.id - 1) % 4 + 1, (self.id - 1) // 4 + 1)  # 4x3 grid
        self.moisture = random.uniform(40, 80)
        self.temperature = random.uniform(15, 30)
        self.atmospheric_pressure = random.uniform(95000, 105000)
        self.status = self.determine_status()

    @property
    def age(self):
        return (datetime.now() - self.start_date).days

    def determine_status(self):
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

        if (healthy_range["temperature"][0] <= self.temperature <= healthy_range["temperature"][1] and
                healthy_range["moisture"][0] <= self.moisture <= healthy_range["moisture"][1] and
                healthy_range["pressure"][0] <= self.atmospheric_pressure <= healthy_range["pressure"][1]):
            return "Healthy"

        for t_range in dead_range["temperature"]:
            for m_range in dead_range["moisture"]:
                for p_range in dead_range["pressure"]:
                    if (t_range[0] <= self.temperature <= t_range[1] and
                            m_range[0] <= self.moisture <= m_range[1] and
                            p_range[0] <= self.atmospheric_pressure <= p_range[1]):
                        return "Dead"

        return "In Danger"

    def accelerated_decay(self):
        for hour in range(24):
            self.timestamp += timedelta(hours=1)
            if 18 <= self.timestamp.hour < 24 or 0 <= self.timestamp.hour < 6:
                temp_change = random.uniform(1, 5) if self.temperature > 22 else random.uniform(-1, -5)
                self.temperature -= temp_change
            else:
                temp_change = random.uniform(1, 5) if self.temperature < 22 else random.uniform(-1, -5)
                self.temperature += temp_change

            moisture_change = random.uniform(10, 50) if self.moisture > 60 else random.uniform(-10, -50)
            self.moisture -= moisture_change
            self.atmospheric_pressure += random.uniform(-3000, 3000)
            self.status = self.determine_status()
            if self.status == "Dead":
                self.status = "Removed"

# Save the updated Plant class to a Python file
file_path_updated = "/mnt/data/plants_functions_updated.py"
with open(file_path_updated, 'w') as file:
    file.write(str(Plant))

file_path_updated
