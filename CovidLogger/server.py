from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse
import json
import os
import argparse

def log(root, user, operating, timestamp, latitude, longitude, altitude, speed, course):

	csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.csv'

	print(csv)
	folder = os.path.dirname(csv)

	if not os.path.exists(folder):
		os.makedirs(folder)

	if not os.path.exists(csv):
		with open(csv,'a') as fd:
			fd.write('timestamp,os,latitude,longitude,altitude,speed,course\n')	

	with open(csv,'a') as fd:
		fd.write('%u,%s,%d,%d,%d,%d,%d\n' % (timestamp,operating,latitude,longitude,altitude,speed,course))	

def register(root):

	return

class RequestHandler(BaseHTTPRequestHandler):

	def do_GET(self):

		self.send_response(200)
		self.end_headers()
		self.wfile.write(('get').encode())

		return

	def do_POST(self):

		content_len = int(self.headers.get('content-length'))
		post_body = self.rfile.read(content_len)
		data = json.loads(post_body)

		for element in data:

			log(root=root, 
				user=element['Id'],
				operating=element['Os'],
				timestamp=element['Timestamp'],
				latitude=element['Latitude'],
				longitude=element['Longitude'],
				altitude=element['Altitude'],
				speed=element['Speed'],
				course=element['Course'])

		self.send_response(200)
		self.end_headers()

		return

parser = argparse.ArgumentParser()
parser.add_argument("-r", "--root", help="root directory for saving data", type=str, default="")
args = parser.parse_args()
if args.root == "":
    raise Exception("Invalid root directory")		

root = args.root
server = HTTPServer(('localhost', 8000), RequestHandler)
server.serve_forever()
