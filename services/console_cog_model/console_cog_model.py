import zmq
import json

MSG_KEY = "msg"

RESULT_OK_KEY = "ok"
RESULT_RESPONSE_KEY = "response"
RESULT_ERROR_KEY = "error"

ENCODING = "utf-8"

def handle(request):
    result = { RESULT_OK_KEY: True, RESULT_RESPONSE_KEY: bytes("", encoding="UTF-8"), RESULT_ERROR_KEY: "" }

    if not MSG_KEY in request:
        result[RESULT_OK_KEY] = False
        result[RESULT_ERROR_KEY] = f"The key {MSG_KEY} is not in input"
        return result
    
    text = request[MSG_KEY]
    print(f"User says: {text}")
    result[RESULT_RESPONSE_KEY] = input("Your answer: ")

    return result

if __name__ == "__main__":
    context = zmq.Context()
    socket = context.socket(zmq.REP)
    socket.bind("tcp://*:5556")

    while True:
        #  Wait for next request from client
        message = socket.recv()
        print(f"Received request: {message} typeof {type(message)}")

        #  Do some 'work'
        result = handle(json.loads(message.decode(ENCODING)))

        #  Send reply back to client
        socket.send(json.dumps(result).encode(ENCODING))