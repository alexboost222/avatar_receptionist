import json
import os
import socket


class Transport:
    ENCODING = "utf-8"
    BUFFER_SIZE = 2048
    LOGS_PATH = "./logs"

    def __init__(self, main_method, name="some-name"):
        if not os.path.exists(self.LOGS_PATH):
            os.makedirs(self.LOGS_PATH)

        self.log_file = open(f"{self.LOGS_PATH}/{name}.log", "w+")
        self.main_method = main_method

        self.host = ""
        self.port = ""

    def log(self, text):
        self.log_file.writelines(text + "\n")

    def arg_parse(self, args):
        for i in range(len(args)):
            if args[i] == "--host":
                self.host = args[i + 1]
            if args[i] == "--port":
                self.port = args[i + 1]

        if self.host == "" or self.port == "":
            raise Exception(f"Empty host '{self.host}' or port '{self.port}'.")

    def work(self):
        while True:
            with socket.socket(socket.AF_INET, socket.SOCK_STREAM) as s:
                s.bind((self.host, int(self.port)))
                s.listen()
                conn, addr = s.accept()
                with conn:
                    self.log(f"Connected by {addr}")
                    while True:
                        # TODO make buffer size variable
                        data = conn.recv(self.BUFFER_SIZE)

                        if not data:
                            break

                        request = data.decode(self.ENCODING)
                        response = json.dumps(self.main_method(json.loads(request)))

                        conn.sendall(response.encode(self.ENCODING))
