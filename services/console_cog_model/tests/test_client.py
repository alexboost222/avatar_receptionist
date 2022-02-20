import zmq
import json

ENCODING = "utf-8"

context = zmq.Context()

#  Socket to talk to server
print("Connecting to hello world server…")
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:5556")

# Do JSON request
request = { "msg": "Добрый день"}
request = json.dumps(request)
print(f"Sending request {request}")
socket.send(request.encode(ENCODING))

response = socket.recv()
print(f"Received response {response}")