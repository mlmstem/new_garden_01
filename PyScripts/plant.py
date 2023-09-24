#Structure of Position
class Position:
    def __init__(self, x, y, z):
        self.x = x
        self.y = y
        self.z = z
    
    def __str__(self):
        return f'({self.x},{self.y},{self.z})'
        


#Structure of Plant
class Plant:
    def __init__(self, timestamp, type, start_date, age, position, status, moist, temp, gas):
        self.timestamp = timestamp
        self.type = type
        self.start_date = start_date
        self.age = age
        self.position = position
        self.status = status
        self.moist = moist
        self.temp = temp
        self.gas = gas

    def __str__(self):
        return f'Timestamp: {self.timestamp}\nType: {self.type}\nStatus: {self.status}\nPosition: {self.position}\nMoisture: {self.moist}\nTemperature: {self.temp}\nGas: {self.gas}\n'