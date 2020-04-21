# flask server
from flask import Flask, json, request, jsonify, g, render_template

 # sqlite database 
import sqlite3

# http requests 
import requests

# queue collections
from collections import deque

# json
import json

import datetime

app = Flask(__name__)
app.config["DEBUG"] = True

SMARTPLAYGROUNDDATABASE = '../SmartPlaygroundDB.db'

GAME2QUEUE = deque([])
GAME2Index = 0

SERVERIP = '192.168.1.14'

# test
# @app.route("/")
# def hello():
#     return("Hello World!")

@app.route('/getAllRegister', methods=["POST"])

# push queue
@app.route('/pushQueue', methods=["POST"])
def pushQueue():
    if request.headers['Content-Type'] == 'application/json':
        # get json and retrieve the rfid
        content = request.get_json()
        rfid = content['rfid']

        # check if rfid in queue 
        # exist: do not push return [old]
        # not exist: push to queue return [new]
        if rfid not in GAME2QUEUE:
            # check if the the rfid is tagged with a kid
            global SERVERIP
            url = 'http://' + SERVERIP + '/SmartPlaygroundWeb/api/game2?rfid=' + rfid
            r = requests.get(url)
            data = json.loads(r.text)
            kidId = data["KidId"]
            if kidId==0:
                # user not registered
                return("kid is not in playground.")
            elif kidId==-1:
                return("This is not a valid registered NFC card!")
            else: 
                # user found add to queue
                GAME2QUEUE.append(rfid)
                global GAME2Index
                GAME2Index += 1
                return "[" + data["Name"] + "], is in playground, added to queue."
        else: 
            print(list(GAME2QUEUE))
            return "kid is already in to the queue."
    else:
        print(list(GAME2QUEUE))
        return "Only application/json is supported"

# pop queue
@app.route('/popQueue', methods=["POST"])
def popQueue():
    lenQueue = len(GAME2QUEUE)
    if lenQueue >= 1:
        # there is still someone in queue
        # return rfid tag
        dequeid = GAME2QUEUE.popleft()
        global GAME2Index
        GAME2Index -= 1

        # get name of the rfid user
        global SERVERIP
        url = 'http://' + SERVERIP + '/SmartPlaygroundWeb/api/kid?rfid=' + dequeid
        print(url)
        r = requests.get(url)
        data = json.loads(r.text)
        kidId = data['KidId']
        name = data['Name']
        
        kidJson = {'KidId': kidId, 'Rfid': dequeid, 'KidName': name }
        kidJsonString = json.dumps(kidJson)

        return kidJsonString
    else: 
        # no more in queue
        # return ["nonono"]
        emptyResult = {'result': 'No one in the queue!'}

        emptyJson = json.dumps(emptyResult)
        return emptyJson

# post result (push to server the results)
@app.route('/pushResult', methods=['POST'])
def pushResult():
    content = request.get_json()
    # print(content)
    kidId = content['KidId']
    kidName = content['KidName']
    zoneId = 3
    score = content['Score']
    missHit = content['MissHit']
    power = content['Power']
    timestamp = datetime.datetime.now()
    # print(timestamp)
    rfid = content['Rfid']

    # print('yesyes')
    if (kidId != ""):
        # valid body
        # 1. insert into local sqlite
        # connect to database file
        dbConnect = sqlite3.connect(SMARTPLAYGROUNDDATABASE)

        # assign row_factory to sqlite3.Row class
        dbConnect.row_factory = sqlite3.Row

        # create a cursor to work with db
        cursor = dbConnect.cursor()

        # insert into db
        try:
            cursor.execute('''INSERT INTO Game2Record (kidId, kidName, zoneId, score, missHit, power, timestamp, rfid)  VALUES (?, ?, ?, ?, ?, ?, ?, ?)''', (kidId, kidName, zoneId, score, missHit, power, timestamp, rfid,))
            dbConnect.commit()
        except sqlite3.IntegrityError as e:
            print('sqlite error: ', e.args[0]) # column name is not unique

        # close db connection
        dbConnect.close()

        # 2. insert into azure db by 
        global SERVERIP
        r = requests.post('http://' + SERVERIP + '/SmartPlaygroundWeb/api/Game2', json={'kidId': kidId, 'score': score, 'missHit': missHit, 'power': power, 'timestamp': datetime.datetime.now().strftime("%d/%m/%Y %H:%M:%S")})
        print(r.text)
        
        return '1'
    else: 
        # invalid body
        return '0'


    return '1'
    # if request.headers['Content-Type'] == 'application/json':
    #     # get json and retrieve the rfid
    #     content = request.get_json()
    #     print(content)

# get all queue
@app.route('/getAllQueue', methods=["GET"])
def getAllQueue():
    return jsonify(GAME2QUEUE)

# check queue exist
@app.route('/checkKidExistInQueue', methods=['POST'])
def checkKidExistInQueue():
    if not request.json:
        abort(400)
    else:
        content = request.get_json()
        rfid = content['rfid']
        # check if there is anyone in the queue

        if (rfid in GAME2QUEUE):
            # remove it from the queue
            GAME2QUEUE.remove(rfid)

            print(GAME2QUEUE)

            # minus the cound
            global GAME2Index
            GAME2Index -= 1

    # return that it is checked and remove if it exist in the queue
    return "checked!"
    

@app.route('/')
def welcome():
    # get queue size
    waitingQueue = GAME2Index
    
    if waitingQueue==0:
        nextQueueRfid = '-----'
    else :
        # get next queue player
        nextQueueRfid = GAME2QUEUE[0]
    # call azure the get the rfid user
    global SERVERIP
    url = 'http://' + SERVERIP + '/SmartPlaygroundWeb/api/game2?rfid=' + nextQueueRfid
    r = requests.get(url)
    data = json.loads(r.text)
    print(data)
    print(data['Name'])
    nextPlayerName = data['Name']

    # TODO: retrieve the name and score
    # http://192.168.1.14/SmartPlaygroundWeb/api/HighestScore?zone=2
    # Today result
    url = 'http://' + SERVERIP + '/SmartPlaygroundWeb/api/HighestScoreToday?zone=2'
    r = requests.get(url)
    data = json.loads(r.text)
    highestScoreNameToday = data['Name']
    highestScoreToday = data['Score']

    # All the results
    url = 'http://' + SERVERIP + '/SmartPlaygroundWeb/api/HighestScore?zone=2'
    r = requests.get(url)
    data = json.loads(r.text)
    highestScoreName = data['Name']
    highestScore = data['Score']
    return render_template('index.html', waitingQueue=waitingQueue, nextPlayerName=nextPlayerName, highestScoreNameToday=highestScoreNameToday, highestScoreToday=highestScoreToday, highestScoreName=highestScoreName, highestScore=highestScore)  # render a template

if __name__ == "__main__":
    global GAME2Index
    GAME2Index = 0
    app.run(host= '0.0.0.0', port=5000, debug=True)