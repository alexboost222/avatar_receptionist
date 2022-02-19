import zmq
import json

ENCODING = "utf-8"

context = zmq.Context()

#  Socket to talk to server
print("Connecting to hello world server…")
socket = context.socket(zmq.REQ)
socket.connect("tcp://localhost:5555")

# Do JSON request
request = { "msg": "Стас на час - это пивас! Стас на час - холодный квас! Стас на час - и кровь из глаз! Стас на час - не унитаз!"}
request = json.dumps(request)
print(f"Sending request {request}")
socket.send(request.encode(ENCODING))

response = socket.recv()
print(f"Received response {response}")