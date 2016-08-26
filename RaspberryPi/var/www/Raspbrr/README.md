The directory where the robot's programming happens 

__init__.py - Executed first; Sets up the available website URI directories. 

When a directory from the __init__.py is accessed through a GET request, 

an html web page from /templates is returned. 

When a POST request is sent, 

control.py reads variables from request.form and runs the approporiate drivers.[definition] 
