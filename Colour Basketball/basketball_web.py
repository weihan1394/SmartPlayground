import connexion
from flask import Flask, render_template, request
app = connexion.App(__name__, specification_dir='./')

global colour
global name
global score
global state
global time
global msg
colour = "red"
name = ""
score = 0
state = "wait"
time = 60
msg = ""
@app.route('/')
def index():
    
    return render_template('index.html', name=name, colour = colour, score = score, state = state, time = time)

@app.route('/getColour', methods = ['GET'])
def getColour():
    global colour
    return colour

@app.route('/setColour/<new_colour>', methods = ['GET'])
def setColour(new_colour):
    global colour
    colour = new_colour

@app.route('/getName', methods = ['GET'])
def getName():
    global name
    return name

@app.route('/setName/<new_name>', methods = ['GET'])
def setName(new_name):
    global name
    name = new_name

@app.route('/setScore/<new_score>', methods = ['GET'])
def setScore(new_score):
    global score
    score = new_score

@app.route('/getScore', methods = ['GET'])
def getScore():
    global score
    return str(score)
    
@app.route('/setState/<new_state>', methods = ['GET'])
def setState(new_state):
    global state, colour, score, name, time
    state = new_state
    if state == "init":
        colour = "black"
        score = 0
        name = ""
        time = 60
    
@app.route('/getState', methods = ['GET'])
def getState():
    global state
    return state

@app.route('/setTime/<new_time>', methods = ['GET'])
def setTime(new_time):
    global time
    time = new_time
    
@app.route('/getTime', methods = ['GET'])
def getTime():
    global time
    return str(time)

@app.route('/getMsg', methods = ['GET'])
def getMsg():
    global msg
    return msg
    
@app.route('/setMsg/<new_msg>', methods = ['GET'])
def setMsg(new_msg):
    global msg
    msg = new_msg
    

# If we're running in stand alone mode, run the application
if __name__ == '__main__':
    app.run(host='0.0.0.0', port=5000, debug=True)