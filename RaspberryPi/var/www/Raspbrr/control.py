import sys
import drivers

def run(request):
	try:
		if (len(request.form['run_mode']) > 0):
			run_mode = request.form['run_mode']
                	text = drivers.run_mode()
                	return text
		else: 
			run_mode = 'null'
        except:
                run_mode = 'null'

	try:
		if (len(request.form['waypoint_list']) > 0):
			waypoint_list = request.form['waypoint_list']
        	        drivers.drive_to_waypoints(waypoint_list)
		else:
			waypoint_list = 'null'
	except:
		waypoint_list = 'null'
                print "Waypoint List Excepted..."

	try: 
		turning = int(request.form['turning']) 
		drivers.turn(turning)
	except: 
		turning = 'null'
	
	try: 
		camera_x = int(request.form['camera_x'])
		drivers.camera_x(camera_x)
	except: 
		camera_x = 'null'
	
	try:
		camera_y = int(request.form['camera_y'])
		drivers.camera_y(camera_y)
	except:
		camera_y = 'null'
	
	try:
		motor_r = int(request.form['motor_r'])
		drivers.motor_r(motor_r)
	except:
		motor_r = 'null'
	
	try:
		motor_l = int(request.form['motor_l'])
		drivers.motor_l(motor_l)
	except: 
		motor_l = 'null'
	

	control_text = "turning = %s, camera_x = %s, camera_y = %s, motor_r = %s, motor_l = %s"%(turning, camera_x, camera_y, motor_r, motor_l)
	mode_text = ", run_mode = %s"%(run_mode)
	waypoint_text = ", waypoint_list = %s"%(waypoint_list)
	return control_text + mode_text + waypoint_text
