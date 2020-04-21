#! /usr/bin/env python
from __future__ import print_function
from time import sleep

# library for smartcard
from smartcard.CardConnectionObserver import ConsoleCardConnectionObserver
from smartcard.CardMonitoring import CardMonitor, CardObserver
from smartcard.util import toHexString

# library to clear screen
from os import system, name 

# library to handle closing of signal
import signal

# library for regex
import re

# library for http request
import requests

# define the apdus used in this script
GET_RESPONSE = [0XA0, 0XC0, 00, 00]
SELECT = [0xA0, 0xA4, 0x00, 0x00, 0x02]
DF_TELECOM = [0x7F, 0x10]
GET_UID=[0xFF, 0xCA, 0x00, 0x00, 0x00]

RFID=""

# GETUID when detected RFID
class selectDFTELECOMObserver(CardObserver):
    def __init__(self):
        self.observer = ConsoleCardConnectionObserver()

    def update(self, observable, actions):
        (addedcards, removedcards) = actions
        for card in addedcards:
            # connect to card
            card.connection = card.createConnection()
            card.connection.connect()

            # await for response
            response, sw1, sw2 = card.connection.transmit(GET_UID)
            RFID = toHexString(response)
            # regex to clear
            RFID = re.sub(r"\s+", "", RFID, flags=re.UNICODE)

            if bool(RFID.strip()):
                # call pi server to push to queue
                # if ignore, don't push to queue (due to some bugs)
                r = requests.post('http://127.0.0.1:5000/pushQueue', json={'rfid': RFID})
                clear()
                print(r.text)
                print(RFID)

            
            #  Driver power failure
            if sw1 == 0x9F:
                apdu = GET_RESPONSE + [sw2]
                response, sw1, sw2 = card.connection.transmit(apdu)


# clear screen
def clear():
    
    # for windows 
    if name == 'nt':
        _ = system('cls')
  
    # for mac and linux(here, os.name is 'posix') 
    else: 
        _ = system('clear') 

def signal_handler(signal, frame):
    sys.exit(0)

if __name__ == '__main__':
    print("You have run the queue successfully")
    sleep(5)
    clear()

    cardmonitor = CardMonitor()
    selectobserver = selectDFTELECOMObserver()
    cardmonitor.addObserver(selectobserver)

    # program will run 24hour (86400 seconds)
    sleep(86400)

    # Remove observer, else monitor will poll forever...
    cardmonitor.deleteObserver(selectobserver)