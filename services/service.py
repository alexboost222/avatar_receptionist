import json
import subprocess
import socket

class Service:
    ENCODING = "utf-8"
    BUFFER_SIZE = 2048

    def __init__(self, host, port):
        self.host = host
        self.port = port
        self.s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)

    @property
    def build_args(self):
        return ["--host", self.host, "--port", self.port]

    def run(self, args=None):
        if args != None:
            subprocess.Popen(args + self.build_args)

        self.s.connect((self.host, int(self.port)))


    def get(self, j_input):
        request = json.dumps(j_input)
        
        self.s.send(request.encode(self.ENCODING))
    
        response = self.s.recv(self.BUFFER_SIZE).decode(self.ENCODING)

        return json.loads(response)

    def stop(self):
        if self.s != None:
            self.s.close()

        self.s = None

    def __del__(self):
        self.stop()
