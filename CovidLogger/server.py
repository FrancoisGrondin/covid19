from http.server import BaseHTTPRequestHandler, HTTPServer
from urllib.parse import urlparse
import json
import os
import argparse

def log(root, user, fields):

	expr = json.dumps(fields)
	csv = root + user[0] + '/' + user[1] + '/' + user[2] + '/' + user + '.json'

	folder = os.path.dirname(csv)

	if not os.path.exists(folder):
		os.makedirs(folder)

	with open(csv,'a') as fd:
		fd.write(expr+'\n')	

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

		action = data['action']

		if action=='track':

			log(root=root, 
				user=data['id'],
				fields={"timestamp": data['timestamp'], "lat": data['lat'], "lng": data['lng']})

		if action=='report':

			log(root=root,
				user=data['id'],
				fields={"timestamp": data['timestamp'], "health": data['health']})

		self.send_response(200)
		self.end_headers()
		self.wfile.write(('post').encode())

		return

parser = argparse.ArgumentParser()
parser.add_argument("-r", "--root", help="root directory for saving data", type=str, default="")
args = parser.parse_args()
if args.root == "":
    raise Exception("Invalid root directory")		

root = args.root
server = HTTPServer(('localhost', 8000), RequestHandler)
server.serve_forever()
