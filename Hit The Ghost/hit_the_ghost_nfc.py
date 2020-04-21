import time
from bluetooth import ble

import sqlite3
import time
import util
from random import randint
from bleuartlib import BleUartDevice
from random import randint
import requests
import json
import re

# library for smartcard
from smartcard.CardConnectionObserver import ConsoleCardConnectionObserver
from smartcard.CardMonitoring import CardMonitor, CardObserver
from smartcard.util import toHexString

API_ENDPOINT = "http://localhost:5000"
global myId
myId = ""

GET_UID=[0xFF, 0xCA, 0x00, 0x00, 0x00]

global bleUartDevices
bleUartDevices = []
global selDevices
selDevices = []
global hitAll
hitAll = True
global scoreToAdd
scoreToAdd = 1
global level
level = 1
global state
state = "wait"
global totalScore
totalScore = 0

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

            print RFID
            global myId
            myId = RFID
            if bool(RFID.strip()):
                # call pi server to push to queue
                # if ignore, don't push to queue (due to some bugs)
                
                print(RFID)

            
            #  Driver power failure
            if sw1 == 0x9F:
                apdu = GET_RESPONSE + [sw2]
                response, sw1, sw2 = card.connection.transmit(apdu)

def sendName(sendName):
    req = API_ENDPOINT + "/setName/" + sendName
    requests.get(url = req)
    
def sendScore(sendScore):
    req = API_ENDPOINT + "/setScore/" + str(sendScore)
    requests.get(url = req)
    
def sendState(sendState):
    req = API_ENDPOINT + "/setState/" + sendState
    requests.get(url = req)
    
def sendTime(sendSeconds):
    req = API_ENDPOINT + "/setTime/" + str(sendSeconds)
    requests.get(url = req)
    
def sendMsg(sendMsg):
    req = API_ENDPOINT + "/setMsg/" + str(sendMsg)
    requests.get(url = req)   

def bleUartReceiveCallback(data):
    global hitAll, selDevices, scoreToAdd
    dataParts = data.split('=')
    devName = dataParts[0]
    msgType = dataParts[1]
    value = int(dataParts[2])
    if state == 'start' and msgType == 's':
        for sel in selDevices:
            if sel.name in devName:
                selDevices.remove(sel)
        if not selDevices:
            hitAll = True
            scoreToAdd += 1
            print 'next hit worth ' + str(scoreToAdd) + ' points'
            
    
    storeValue(devName, msgType, value)

def storeValue(devName, msgType, value):
    global scoreToAdd, totalScore
    for bled in bleUartDevices:
        if bled.name in devName:
            if(msgType == "s"):
                if state == 'start':
                    bled.addScore(scoreToAdd)
                    totalScore += scoreToAdd
                    print 'hit'
                    print "Score: " + str(totalScore)
            elif msgType == "m":
                if state == 'start':
                    bled.addMissHit()
                    print('wrong target')
            elif msgType == "p":
                bled.setPower(value)
            return


def addBleUartDevice(address, name):
    global bleUartDevices
    bleUartDevice = BleUartDevice(address, name)
    bleUartDevice.connect()
    bleUartDevice.enable_uart_receive(bleUartReceiveCallback)
    bleUartDevices.append(bleUartDevice)


def sendCommandToAllBleUartDevices(command):
    global bleUartDevices
    for bled in bleUartDevices:
        print bled.name
        bled.send(command)


def disconnectFromAllBleUartDevices():
    for bled in bleUartDevices:

        bled.disconnect()
        bleUartDevices.remove(bled)
        #bled = None

def selectGhostForDevice():
    while True:
        randNum = randint(0, len(bleUartDevices) - 1)
        sel = bleUartDevices[randNum]
        
        try:
            selDevices.index(sel)
        except ValueError:
            break
        
    return sel

def sendIcon(device):
    print "send icon"
    for bled in bleUartDevices:
        print device == bled
        if device == bled:
            bled.send("icon=1")
        else:
            bled.send("noIcon=1")
            
def retrievePlayer():
    global level
    whRP = "http://192.168.43.57:5000/popQueue"
    player = requests.post(url = whRP)
    print player.text
    
    data = json.loads(player.text)
    try:
        kidId = data['KidId']
        name = data['KidName']
        dequeid = data['Rfid']
            
        a = {'KidId': kidId, 'Rfid': dequeid, 'KidName': name }
        return a
        
    except:
        print 'error'
        return False
        

def retrieveMicrobits():
    service = ble.DiscoveryService()
    devices = service.discover(2)

    print('********** Initiating device discovery......')

    for address, name in devices.items():

        found_microbit = False

        if address == 'F7:DF:E1:AD:22:37':
            name = "zozig"

            print('Found BBC micro:bit [zozig] {}: {}'.format(name ,address))

            addBleUartDevice(address, name)

            print('Added micro:bit device...')

        elif address == 'D0:E7:09:66:90:5C':
            name = "voguz"

            print('Found BBC micro:bit [voguz] {}: {}'.format(name,address))

            addBleUartDevice(address, name)
            print('Added micro:bit device...')
                
        elif address == 'FE:A1:92:B1:86:9D':
            name = "pavev"
            print('Found BBC micro:bit [pavev] {}: {}'.format(name,address))
                
            addBleUartDevice(address, name)
                    
            print('Added micro:bit device...')
                
    time.sleep(1)

def startGame():
    global hitAll, selDevices, state
    print 'Send command'
    sendCommandToAllBleUartDevices("start=1")
    #sendState("start")        
    # start game
    state = 'start'
    start_time = time.time()
    secTime = time.time()
    while True:
        seconds = 60
        now = time.time()
        seconds = seconds - int(now - secTime)
        #sendTime(seconds)
        if seconds < 0:
            seconds = 0
        if now - start_time >= 60:
            print 'Time is up!'
            state = 'wait'
            #end game
            #sendState("waiting")
            return
        elif hitAll:
            for i in range(level):
                selDevice = selectGhostForDevice()
                selDevices.append(selDevice)
            sendIcon(selDevice)
            hitAll = False

def sendToQueuePi(player):
    print player
    d = json.loads(json.dumps(player))
    print d
    #stop web
    #sendState("init")
    print 'sent'
    #send to wh pi
    whRP = "http://192.168.43.57:5000/pushResult"
    p = requests.post(url = whRP, json=d)
    print p

def initGame():
    global scoreToAdd, hitAll, selDevice, totalScore
    scoreToAdd = 1
    totalScore = 0
    hitAll = False
    selDevices = []
    #sendState('init')
    disconnectFromAllBleUartDevices()
    
try:
    while True:
        print 'Retrieving player'
        player = retrievePlayer()
        if(player):
            #sendName(player["kidName"])
            #sendState("player")
            base = time.time()
            while True:
                try:
                    now = time.time()
                    if(now - base >= 30):
                        break
                    # get rfid
                    print 'Scan RFID'
                    cardmonitor = CardMonitor()
                    selectobserver = selectDFTELECOMObserver()
                    cardmonitor.addObserver(selectobserver)
                    time.sleep(10)
                    
                    if player["Rfid"] == str(myId):
                        print 'retrieve'
                        retrieveMicrobits()
                        startGame()
                        sendCommandToAllBleUartDevices("end=1")
                        time.sleep(5)
                        
                        #totalScore = 0
                        totalMissHit = 0
                        power = 0
                        for bled in bleUartDevices:
                            #totalScore += bled.score
                            totalMissHit += bled.missHit
                            if bled.power > power: power = bled.power
                            print bled.name + " " + str(bled.score) + " " + str(bled.missHit) + " " + str(bled.power)
                        player["Score"] = totalScore
                        player["MissHit"] = totalMissHit
                        player["Power"] = power
                        initGame()
                        sendToQueuePi(player)
                        print 'Game over!'
                        break
                    else:
                        print 'invalid'
                        #sendMsg("Invalid ID")
                except Exception as e:
                    print e
                    break
                
except KeyboardInterrupt:
    
    print('********** END')


finally:
    disconnectFromAllBleUartDevices()
    print('Disconnected from micro:bit devices')

