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

		if content_len <= length:

			# Get action
			action = data['action']

			# Log GPS
			if action == 'track':

				# Get user id
				user = data['id']

				# Generate the CSV file path
				csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.csv'

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

				newid = str(int(time.time() * 10000000) * 10000000 + random.randrange(10000000))[::-1]

				reply = { 'id': newid }

				data = json.dumps(reply).encode()	

				self.send_response(200)
				self.end_headers()

				self.wfile.write(data)

			# Update user health
			if action == 'report':

				# Get user id
				user = data['id']

				# Generate the CSV file path
				csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.json'

				# Create JSON expression
				expr = json.dumps(data['symptoms']) + '\n'

				# Dump in the file
				with open(csv, 'a') as fd:
					fd.write(expr)

				self.send_response(200)
				self.end_headers()

		return

parser = argparse.ArgumentParser()
parser.add_argument("-r", "--root", help="root directory for saving data", type=str, default="")
parser.add_argument("-a", "--address", help="server ip address", type=str, default="localhost")
parser.add_argument("-l", "--length", help="maximum length of each message in bytes", type=int, default=4096)
args = parser.parse_args()
if args.root == "":
    raise Exception("Invalid root directory")		

root = args.root
length = args.length

# Create the directory tree at startup
prefixes = range(0,10)
for prefix1 in prefixes:
	for prefix2 in prefixes:
		for prefix3 in prefixes:
			folder = root + str(prefix1) + '/' + str(prefix2) + '/' + str(prefix3) + '/'
			if not os.path.exists(folder):
				os.makedirs(folder)

server = ThreadingHTTPServer((args.address, 8000), RequestHandler)
server.serve_forever()
