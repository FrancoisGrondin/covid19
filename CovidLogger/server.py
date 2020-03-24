from http.server import BaseHTTPRequestHandler, ThreadingHTTPServer
from urllib.parse import urlparse

import argparse
import json
import os
import random
import threading
import time
import random

class RequestHandler(BaseHTTPRequestHandler):

	def do_POST(self):

		content_len = int(self.headers.get('content-length'))
		post_body = self.rfile.read(content_len)
		print(post_body)
		data_in = json.loads(post_body)
		print(data_in['action'])

		ip = self.client_address[0]

		self.server.lock.acquire()
		if ip not in server.ips:
			self.server.ips[ip] = 1
		else:
			self.server.ips[ip] += 1
		count = self.server.ips[ip]
		self.server.lock.release()

		if content_len <= self.server.length and count <= self.server.requests:

			if data_in['action'] == 'track':

				csv = self.server.directory + \
					  data_in['id'][0] + '/' + \
					  data_in['id'][1] + '/' + \
					  data_in['id'][2] + '/' + \
					  data_in['id'] + '.trk'

				expr = ''

				if not os.path.exists(csv):
					expr += 'timestamp,'
					expr += 'latitude,'
					expr += 'longitude,'
					expr += 'accuracy,'
					expr += 'speed,'
					expr += 'course\n'				

				for element in data_in['deviceLocations']:

					tup = (	element['timestamp'], 
							element['latitude'], 
							element['longitude'], 
							element['accuracy'], 
							element['speed'], 
							element['course'] )

					expr += '%u,%.4f,%.4f,%.4f,%.1f,%.1f\n' % tup

				with open(csv,'a') as fd:
					fd.write(expr)

				self.send_response(200)
				self.end_headers()

			if data_in['action'] == 'register':

				rnd = 10000000
				newid = str(int(time.time() * rnd) * rnd + random.randrange(rnd))[::-1]

				reply = { 'id': newid }
				data_out = json.dumps(reply).encode()	

				self.send_response(200)
				self.end_headers()

				self.wfile.write(data_out)

			if data_in['action'] == 'report':

				csv = self.server.directory + \
					  data_in['id'][0] + '/' + \
					  data_in['id'][1] + '/' + \
					  data_in['id'][2] + '/' + \
					  data_in['id'] + '.rpt'						

				expr = ''

				if not os.path.exists(csv):
					expr += 'timestamp,'
					expr += 'tested_positive,'
					expr += 'fever,'
					expr += 'tiredeness,'
					expr += 'dry_cough,'
					expr += 'aches_and_pains,'
					expr += 'nasal_congestion,'
					expr += 'runny_nose,'
					expr += 'sore_throat,'
					expr += 'diarrhea\n'

				tup = (	data_in['timestamp'], 
						data_in['symptoms']['tested_positive'], 
						data_in['symptoms']['fever'], 
						data_in['symptoms']['tiredeness'], 
						data_in['symptoms']['dry_cough'], 
						data_in['symptoms']['aches_and_pains'],
						data_in['symptoms']['nasal_congestion'],
						data_in['symptoms']['runny_nose'],
						data_in['symptoms']['sore_throat'],
						data_in['symptoms']['diarrhea'] )

				expr += '%u,%s,%s,%s,%s,%s,%s,%s,%s,%s\n' % tup				

				with open(csv,'a') as fd:
					fd.write(expr)

				self.send_response(200)
				self.end_headers()

			if data_in['action'] == 'risk':

				# Do somethin here to retrieve the risk (0-100)
				level = 50

				reply = { 'level': level }
				data_out = json.dumps(reply).encode()
				print(data_out)

				self.send_response(200)
				self.end_headers()

				self.wfile.write(data_out)

			if data_in['action'] == 'map':

				# Begin TEMP
				# Random generation for now
				# TODO - get timestamp and dig values from recorded data
				lat = 45.51997
				lon = -73.61624
				dots = []

				for x in range(500):
					nlat = lat + random.random() / 100
					nlon = lon + random.random() / 100
					contracted = (random.random() > .5)
					dots.append({ 'latitude': nlat, 'longitude': nlon, 'contracted': contracted })
				# End TEMP

				reply = { 'dots': dots }
				data_out = json.dumps(reply).encode()	

				self.send_response(200)
				self.end_headers()

				self.wfile.write(data_out)


class Server(ThreadingHTTPServer):

	def __init__(self, address, port, directory, length, timeout, requests):

		super(Server, self).__init__((address, port), RequestHandler)

		self.directory = directory
		self.length = length
		self.timeout = timeout
		self.requests = requests

		prefixes = range(0,10)
		for prefix1 in prefixes:
			for prefix2 in prefixes:
				for prefix3 in prefixes:
					folder = self.directory + str(prefix1) + '/' + str(prefix2) + '/' + str(prefix3) + '/'
					if not os.path.exists(folder):
						os.makedirs(folder)

		self.lock = threading.Lock()
		self.ips = {}

	def run_ips(self):

		thread = threading.Thread(target=self.clear)
		thread.start()		

	def clear(self):

		while True:

			self.lock.acquire()
			self.ips.clear()
			self.lock.release()
			time.sleep(self.timeout)

parser = argparse.ArgumentParser()
parser.add_argument("-a", "--address", help="ip address", type=str, default="localhost")
parser.add_argument("-d", "--directory", help="root directory", type=str, default="")
parser.add_argument("-l", "--length", help="message length (bytes)", type=int, default=4096)
parser.add_argument("-r", "--requests", help="requests per ip", type=int, default=10)
parser.add_argument("-p", "--port", help="port address", type=int, default=8000)
parser.add_argument("-t", "--timeout", help="ip list timeout (sec)", type=int, default=300)
args = parser.parse_args()

server = Server(address=args.address, 
				directory=args.directory, 
				length=args.length,
				port=args.port, 
				requests=args.requests,
				timeout=args.timeout)
server.run_ips()
server.serve_forever()

