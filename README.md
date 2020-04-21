# SmartPlayground
IS4151 project

### Architecture
![architecture](https://github.com/weihan1394/SmartPlayground/blob/master/docs/architecture.png)

### Installation
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