import serial
import time
from random import randint
import pixy 
from ctypes import *
from pixy import *
import requests
import json
import re

# library for smartcard
from smartcard.CardConnectionObserver import ConsoleCardConnectionObserver
from smartcard.CardMonitoring import CardMonitor, CardObserver
from smartcard.util import toHexString


API_ENDPOINT = "http://localhost:5000"

print("Listening on /dev/ttyS0... Press CTRL+C to exit")
pixy.init ()
pixy.change_prog ("color_connected_components");
pixy.set_lamp (0, 0);
class Blocks (Structure):
  _fields_ = [ ("m_signature", c_uint),
    ("m_x", c_uint),
    ("m_y", c_uint),
    ("m_width", c_uint),
    ("m_height", c_uint),
    ("m_angle", c_uint),
    ("m_index", c_uint),
    ("m_age", c_uint) ]
global blocks
blocks = BlockArray(100)
global frame
frame = 0
global myId
myId = ""

GET_UID=[0xFF, 0xCA, 0x00, 0x00, 0x00]
def capture():
    global frame
    global blocks
    count = pixy.ccc_get_blocks (100, blocks)
    if count > 0:
        print 'frame %3d:' % (frame)
        frame = frame + 1
        for index in range (0, count):
            print '[BLOCK: SIG=%d X=%3d Y=%3d WIDTH=%3d HEIGHT=%3d]' % (blocks[index].m_signature, blocks[index].m_x, blocks[index].m_y, blocks[index].m_width, blocks[index].m_height)
            return blocks[index].m_signature
        
def selectColour():
    #3 - blue, 5 - red, 7 - green
    colourSig = [3, 5, 7]
    randNum = randint(0, len(colourSig) - 1)
    selColour = colourSig[randNum]
    c = ""
    if selColour == 3:
        c = "blue"
    elif selColour == 5:
        c = "red"
    else:
        c = "green"
    sendColour(c)
    return selColour

def sendColour(sendColour):
    req = API_ENDPOINT + "/setColour/" + sendColour
    requests.get(url = req)
    
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

def startGame(player):
    ser.write(b'start\r\n')
    sendState("start")
    score = 0
    wrongColour = 0
    colour = selectColour()
    #sendName(player["name"])
    gameTime = time.time()
    baseTime = time.time()
    secTime = time.time()
    seconds = 60
    sendTime(seconds)
    while True:
        seconds = 60
        now = time.time()
        seconds = seconds - int(now - secTime)
        if seconds < 0:
            seconds = 0
        sendTime(seconds)
            
        if(now - gameTime >= 60):
            print 'end'
            sendState("init")
            ser.write(b'end\r\n')
            player["score"] = score
            player['wrongColour'] = wrongColour
            boardHit = 0
    
            while True:
                msg = ser.readline().decode('utf-8').rstrip()
                if msg and msg.strip():
                    splitmyInput = [x.strip() for x in msg.split(':')]
                    if splitmyInput[0] == 'b':
                        boardHit = int(splitmyInput[1])
                        break
            player['boardHit'] = boardHit
            endGame(player)
            return
        if(now - baseTime > 5):
            colour = selectColour()
            baseTime = time.time()
        print "Current Colour: " + str(colour)
        msg = ser.readline().decode('utf-8').rstrip()
        if msg and msg.strip():
            splitmyInput = [x.strip() for x in msg.split(':')]
            print splitmyInput[0]
            if splitmyInput[0] == 'c':
                sig = capture()
                if sig == colour:
                    score = score + 1
                    colour = selectColour()
                    baseTime = time.time()
                    sendScore(score)
                else:
                    wrongColour = wrongColour + 1
            print(score)
        time.sleep(0.1)

def retrievePlayer():
    whRP = "http://192.168.43.14:5000/popQueue"
    player = requests.post(url = whRP)
    print player.text
    if(player.text != "nonono"):
        data = json.loads(player.text)
        kidId = data['kidId']
        name = data['kidName']
        dequeid = data['rfid']
            
        a = {'kidId': kidId, 'rfid': dequeid, 'kidName': name }
        return a
    else:
        return False
    
def endGame(player):
    print player
    d = json.loads(json.dumps(player))
    print d
    #stop web
    #sendState("init")
    print 'sent'
    #send to wh pi
    whRP = "http://192.168.43.14:5000/pushResult"
    p = requests.post(url = whRP, json=d)
    print p
    
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

    
try:
    ser = serial.Serial(port='/dev/ttyACM0', baudrate=115200, timeout=1)
    while True:
        print("retrieved")
        player = retrievePlayer()
        if(player):
            base = time.time()
            sendName(player["kidName"])
            sendState("player")
            while True:
                now = time.time()
                if(now - base >= 30):
                    break
                print("input")
                
                # get rfid
                cardmonitor = CardMonitor()
                selectobserver = selectDFTELECOMObserver()
                cardmonitor.addObserver(selectobserver)
                time.sleep(10)
                
                if player["rfid"] == str(myId):
                    sendMsg("")
                    sendState("prepare")
                    time.sleep(5)
                    startGame(player)
                    break
                else:
                    sendMsg("Invalid ID")

except KeyboardInterrupt:
    if ser.is_open:
        ser.close()
    print("Program terminated!")
finally:
    pixy.set_lamp (0, 0);