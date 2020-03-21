from http.server import BaseHTTPRequestHandler, HTTPServer, ThreadingHTTPServer
from urllib.parse import urlparse
import json
import os
import argparse
import random
import time

class RequestHandler(BaseHTTPRequestHandler):

	def do_POST(self):

		content_len = int(self.headers.get('content-length'))
		post_body = self.rfile.read(content_len)
		data = json.loads(post_body)

		# Get action
		action = data['action']

		# Log GPS
		if action == 'track':

			# Get user id
			user = data['id']

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
				expr += 'timestamp,latitude,longitude,accuracy,speed,course\n'

			# Now loop in each element and extract content
			for element in data['deviceLocations']:

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

		# Register new user
		if action == 'register':

			newid = str(int(time.time() * 10000000) * 10000 + random.randrange(10000))[::-1]

			reply = { 'id': newid }

			data = json.dumps(reply).encode()

			# Generate the CSV file path
			csv = root + "ids.csv"

			# Empty string that will hold all fields to write to file
			expr = ''

			# If file does not exist, then first add the headers
			if not os.path.exists(csv):
				expr += 'id\n'

			# Add id
			expr += ('%s\n' % newid)

			# Dump in the file
			with open(csv,'a') as fd:
				fd.write(expr)			

			self.send_response(200)
			self.end_headers()

			self.wfile.write(data)

		# Update user health
		if action == 'report':

			# Get user id
			user = data['id']

			# Generate the CSV file path
			csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.json'

			# If directory does not exist, create it
			folder = os.path.dirname(csv)
			if not os.path.exists(folder):
				os.makedirs(folder)

			# Create JSON expression
			expr = json.dumps(data['deviceLocations']) + '\n'

			# Dump in the file
			with open(csv, 'a') as fd:
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
server = ThreadingHTTPServer(('192.168.0.104', 8000), RequestHandler)
server.serve_forever()
