import Sunfounder_PWM_Servo_Driver.Servo_init as servo
import RPi.GPIO as GPIO
import Sunfounder_PWM_Servo_Driver.Servo_init as pwm
import time                # Import necessary modules
import math
import ast

# Background loop imports START
"""POOL_TIME = 1 #Seconds

commonDataStruct = {}
dataLock = threading.Lock()
yourThread = threading.Thread()"""
# Background loop imports END


FILE_CONFIG = "/var/www/Raspbrr/config"

Motor0_A = 11  # pin11
Motor0_B = 12  # pin12
Motor1_A = 13  # pin13
Motor1_B = 15  # pin15

EN_M0    = 4  # servo driver IC CH4
EN_M1    = 5  # servo driver IC CH5

p = pwm.init()

MinPulse = 200
MaxPulse = 700
offset_x = 0
offset_y = 0
turn_offset = 0


def setup():
	global pwm, turn_offset, Xmin, Ymin, Xmax, Ymax, home_x, home_y
	global forward0, forward1
        forward0 = 1
        forward1 = 1
        GPIO.setwarnings(False)
        GPIO.setmode(GPIO.BOARD)        # Number GPIOs by its physical location
        
	# Fix the config motor direction reader, please:
	try:
                for line in open(FILE_CONFIG):
                        if line[0:8] == "forward0" and line[11:-1] == 'True':
				forward0 = 1
			else: 	
				forward0 = 0
                        if line[0:8] == "forward1" and line[11:-1] == 'True':
				forward1 = 1
			else: 	
				forward1 = 0
			print "forward0 = %s, forward1 = %s"%(forward0,forward1)

        except:
		print "Excepted config motor direction reader"
		pass
	try:
		for line in open(FILE_CONFIG):
			if line[0:8] == 'offset =':
				turn_offset = int(line[9:-1])
	except:
		print "Excepted config turning offset reader"
	
        try:
                for line in open(FILE_CONFIG):
                        if line[0:8] == 'offset_x':
                                offset_x = int(line[11:-1])
                                #print 'offset_x =', offset_x
                        if line[0:8] == 'offset_y':
                                offset_y = int(line[11:-1])
                                #print 'offset_y =', offset_y
	except:
		print "Excepted config camera offset reader"
		pass
	
	Xmin = MinPulse + offset_x
        Xmax = MaxPulse + offset_x
        Ymin = MinPulse + offset_y
        Ymax = MaxPulse + offset_y
        home_x = (Xmax + Xmin)/2
        home_y = Ymin + 80
	pwm = servo.init()           # Initialize the servo controller.

# Clamp value 'n' between 'n_min' and 'n_max'
def clamp(n,n_min,n_max): return max(n_min, min(n,n_max))

def turn(angle):
	angle_original = angle
	angle *= -1 #Reverse value
	angle = clamp(angle,-100,100)
	angle = angle + turn_offset + 450 #Add offset and middle number
	pwm.setPWM(0, 0, angle)
	text = "%s -> %s" % (angle_original,angle)
	return text

def camera_y(angle):
        global Current_y
        Current_y = angle + home_y # Add offset
	clamp(Current_y,Ymin,Ymax)
        pwm.setPWM(15, 0, Current_y)
	text = "%s" % (Current_y)
	return text

def camera_x(angle):
        global Current_x
        Current_x = angle + home_x # Add offset
        clamp(Current_x,Xmin,Xmax)
	pwm.setPWM(14, 0, Current_x)
	text = "%s" % (Current_x)
	return text

def motor_r(speed):
	text = "motor_r = %s" % (speed)
	speed = clamp(speed,-100,100)
	speed *= 25 # Multiply to maximum voltage
	# Uncomment once config motor direction reader is fixed:
	#speed = speed * (-1 +(2 * forward0)) # Reverse motor direction
        p.setPWM(EN_M0, 0, abs(speed)) # Set motor voltage
	if speed > 400: # if more than deadzone
		GPIO.output(Motor0_A, GPIO.LOW)
                GPIO.output(Motor0_B, GPIO.HIGH)
	elif speed < -400: # if less than deadzone
		GPIO.output(Motor0_A, GPIO.HIGH)
                GPIO.output(Motor0_B, GPIO.LOW)
	else: # if in the deadzone
		GPIO.output(Motor0_A, GPIO.LOW)
                GPIO.output(Motor0_B, GPIO.LOW) 
	return text

def motor_l(speed):
	text = "motor_l = %s" % (speed)
	speed = clamp(speed,-100,100)
        speed *= 20
	speed = speed * (-1 +(2 * forward0))
	p.setPWM(EN_M1, 0, abs(speed))
        if speed > 400:
		GPIO.output(Motor1_A, GPIO.LOW)
                GPIO.output(Motor1_B, GPIO.HIGH)
        elif speed < -400:
		GPIO.output(Motor1_A, GPIO.HIGH)
                GPIO.output(Motor1_B, GPIO.LOW)
        else:
		GPIO.output(Motor1_A, GPIO.LOW)
                GPIO.output(Motor1_B, GPIO.LOW)
	return text

def run_mode(): # Reset all hardware to 0
	turn(0)
	camera_x(0)
	camera_y(0)
	motor_r(0)
	motor_l(0)
        return 'Run mode executed'

# Send command to drive forward for time in seconds at an angle
def drive_command(time_to_drive, angle):
	time_untill_drive = time_to_drive + time.time()
	while 1:
		turn(angle)
		motor_r(100)
		motor_l(100)
		if time_untill_drive < time.time():
			turn(0)
			motor_r(0)
			motor_l(0)
			return "Waypoint (%s, %s) Reached!"%(time_to_drive,angle)
			break

# Send a string of waypoints 
# in format: [(time,angle),(time,angle),...(time,angle)]
def drive_to_waypoints(waypoint_list):
	w_list = ast.literal_eval(waypoint_list)
	for i in w_list: # Where i = '(time,angle)' for every i
		num = 0 # Set n number to 0
		for n in i: # Where n = 'time', then n = 'angle'
			if num == 0: # If first number
				time_to_drive = n
				num = 1 # first number read
			else: # If not first number
				angle = n
				num = 0
		# Execute command and print the returned text
		text = drive_command(time_to_drive,angle)
		print text

# Custom background python loop
# I found that if-else functions wouldn't execute
# but works for something simple
# Background loop START
"""def interrupt():
	global yourThread
	yourThread.cancel()

def doStuff():
	global commmonDataStruct
	global yourThread
	with dataLock:
		# Your definitions here:

	yourThread = threading.Timer(POOL_TIME, doStuff, ())
	yourThread.start()
	
def doStuffStart():
	global yourThread
	yourThread = threading.Timer(POOL_TIME, doStuff, ())
	yourThread.start()

doStuffStart()
atexit.register(interrupt)"""
# Background loop END
