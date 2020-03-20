from http.server import BaseHTTPRequestHandler, HTTPServer, ThreadingHTTPServer
from urllib.parse import urlparse
import json
import os
import argparse

class RequestHandler(BaseHTTPRequestHandler):

	def do_POST(self):

		content_len = int(self.headers.get('content-length'))
		post_body = self.rfile.read(content_len)
		data = json.loads(post_body)

		# Get user id
		user = data['id']

		# Get action
		action = data['action']

<<<<<<< HEAD
		# Perform corresponding logging
		if action == 'track':
=======
	with open(csv,'a') as fd:
		fd.write('%u,%s,%f,%f,%f,%f,%f\n' % (timestamp,operating,latitude,longitude,altitude,speed,course))	
>>>>>>> 198fa8163899b9686c9fbd3a855296526137e83e

			# Generate the CSV file path
			csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.csv'

			# If directory does not exist, create it
			folder = os.path.dirname(csv)
			if not os.path.exists(folder):
				os.makedirs(folder)

			# Empty string that will hold all fields to write to file
			expr = ''

			# If file does not exist, then first add the headers
			if not os.path.exists(csv):
				with open(csv,'a') as fd:
					expr += 'timestamp,latitude,longitude,accuracy,speed,course\n'

			# Now loop in each element and extract content
			for element in data['data']:

				tup = (	element['timestamp'], 
						element['latitude'], 
						element['longitude'], 
						element['accuracy'], 
						element['speed'], 
						element['course'])
				expr += '%u,%d,%d,%d,%d,%d\n' % tup

			# Finally, do a single dump in the file
			with open(csv,'a') as fd:
				fd.write(expr)			

		self.send_response(200)
		self.end_headers()

		return

parser = argparse.ArgumentParser()
parser.add_argument("-r", "--root", help="root directory for saving data", type=str, default="")
args = parser.parse_args()
if args.root == "":
    raise Exception("Invalid root directory")		

root = args.root
server = ThreadingHTTPServer(('localhost', 8000), RequestHandler)
server.serve_forever()
