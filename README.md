# SmartPlayground
IS4151 project

## Information
### Architecture
![architecture](https://github.com/weihan1394/SmartPlayground/blob/master/docs/architecture.png)

### Dashboard
![dashboard](https://github.com/weihan1394/SmartPlayground/blob/master/docs/dashboard.png)

### Zone 
![zone](https://github.com/weihan1394/SmartPlayground/blob/master/docs/zone.png)

### Game 1 
![game1-fv](https://github.com/weihan1394/SmartPlayground/blob/master/docs/game1-fv.png)
![game1-setup](https://github.com/weihan1394/SmartPlayground/blob/master/docs/game1-setup.png)

### Game 2
![game2](https://github.com/weihan1394/SmartPlayground/blob/master/docs/game2.png)
 

## Installation (zone)
1. Install swig
- [https://www.dev2qa.com/how-to-install-swig-on-macos-linux-and-windows/](https://www.dev2qa.com/how-to-install-swig-on-macos-linux-and-windows/)

2. Install pyscard
- [https://pypi.org/project/pyscard/](https://pypi.org/project/pyscard/)
- `pip install pyscard`

3. Install sqlite3
- [https://www.tutorialspoint.com/sqlite/sqlite_installation.htm](https://www.tutorialspoint.com/sqlite/sqlite_installation.htm)
- Make sure SmartPlaygroundDB.db is in the root folder

4. Install flask
- create controller folder first
- [https://dev.to/sahilrajput/install-flask-and-create-your-first-web-application-2dba](https://dev.to/sahilrajput/install-flask-and-create-your-first-web-application-2dba)
- `export FLASK_APP=SmartPlaygroundServer.py`
``` sh
python -m flask run
```