import time
from bluetooth import ble

import util

class MyGATTRequester(ble.GATTRequester):

    _callback = None

    def on_indication(self, handle, data):

        if data != None and len(data) > 2:

            data = data[2:]

            self._callback(data)

    def set_callback(self, callback):

        self._callback = callback

class BleUartDevice:

    address = ''
    gattRequester = None

    name = ""
    score = 0
    power = 0
    missHit = 0

    def __init__(self, address, name):

        self.address = address
        self.name = name

    def connect(self):

        self.gattRequester = MyGATTRequester(self.address, False)
        self.gattRequester.connect(True, 'random')
        time.sleep(1)

    def addScore(self, scoreToAdd):
        self.score += scoreToAdd
    
    def addMissHit(self):
        self.missHit += 1
    
    def setPower(self, power):
        self.power = power
        
    def compareName(self, name):
        print self.name + " " + name
        print self.name in name
        

    def send(self, strData):

        txData = util.marshalStringForBleUartSending(strData)
        self.gattRequester.write_by_handle(0x2a, txData)
        time.sleep(1)

    def enable_uart_receive(self, callback):

        self.gattRequester.set_callback(callback)
        self.gattRequester.write_by_handle(0x28, '\x02\x00')

    def disconnect(self):

        if self.gattRequester != None and self.gattRequester.is_connected():

            time.sleep(1)
            self.gattRequester.disconnect()
            self.gattRequester = None
            time.sleep(1)
