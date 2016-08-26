from flask import Flask 
from flask import request 
from flask import render_template
from flask import send_file
from flask.ext.basicauth import BasicAuth 
import control #Added control script
import os
app = Flask(__name__)

LD_LIBRARY_PATH = "/var/www/mjpg-streamer/"

MJPG_STREAMER_PATH = "mjpg_streamer"
MJPG_STREAMER_PATH = LD_LIBRARY_PATH + MJPG_STREAMER_PATH
INPUT_PATH = "input_uvc.so"
INPUT_PATH = LD_LIBRARY_PATH + INPUT_PATH
OUTPUT_PATH = "output_http.so -w ./www"
OUTPUT_PATH = LD_LIBRARY_PATH + OUTPUT_PATH

command = MJPG_STREAMER_PATH + ' -i \"' + INPUT_PATH + '" -o "' + OUTPUT_PATH + '" &'
print command
os.system(command) #Run mjpg streamer

FILE_CONFIG="/var/www/Raspbrr/config" #Config file location
offset = "0"
offset_x = "0"
offset_y = "0"
forward0 = "True"
forward1 = "True"

app.config['BASIC_AUTH_USERNAME'] = 'Robot'
app.config['BASIC_AUTH_PASSWORD'] = 'password'
app.config['BASIC_AUTH_FORCE'] = True #Force authenication for every page

basic_auth = BasicAuth(app) #Use Basic Authenication


for line in open(FILE_CONFIG):
        if line[0:8] == 'offset_x':
                offset_x = int(line[11:-1])
        if line[0:8] == 'offset_y':
                offset_y = int(line[11:-1])
        if line[0:8] == 'offset =':
                offset = int(line[9:-1])
        if line[0:8] == "forward0":
                forward0 = line[11:-1]
        if line[0:8] == "forward1":
                forward1 = line[11:-1]

import drivers #Added drivers script
drivers.setup() #Run drivers setup

@app.route('/')
#@basic_auth.required # For individual page authenication
def index():
	return render_template('index.html')

@app.route('/post', methods=['POST', 'GET'])
#@basic_auth.required
def control_post():
	if request.method == 'POST':
		results = control.run(request)
		return results
	else:
		return render_template('post.html')
