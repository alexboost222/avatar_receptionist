import json
import subprocess
import os


class Transport:
    logs_path = "./logs"

    def __init__(self, main_method, name="some-name"):

        if not os.path.exists(self.logs_path):
            os.makedirs(self.logs_path)

        self.log_file = open(f"{self.logs_path}/{name}.log", "w+")
        self.main_method = main_method

    def log(self, text):
        self.log_file.writelines(text + "\n")

    def arg_parse(self, args):
        in_pipe_name = ""
        out_pipe_name = ""

        for i in range(len(args)):
            if args[i] == "in-pipe":
                in_pipe_name = args[i + 1]
            if args[i] == "out-pipe":
                out_pipe_name = args[i + 1]

        if in_pipe_name == "" or out_pipe_name == "":
            raise Exception()

        self.in_pipe = open(in_pipe_name, "r")
        self.out_pipe = open(out_pipe_name, "w")

    def work(self):
        while True:
            line = self.in_pipe.readline()
            res = self.main_method(json.loads(line))

            self.out_pipe.writelines([json.dumps(res) + "\n"])
            self.out_pipe.flush()


    def __del__(self):
        self.in_pipe.close()
        self.out_pipe.close()


