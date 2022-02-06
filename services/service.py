import json
import os, errno, subprocess

class Service:

    def __init__(self, name):
        self.service_name = name

    @property
    def name(self):
        return self.service_name

    @property
    def in_pipe_name(self):
        return f"pipes/{self.service_name}-in"

    @property
    def out_pipe_name(self):
        return f"pipes/{self.service_name}-out"

    def make_pipe(self, fifo):
        if not os.path.exists("pipes/"):
            os.mkdir("pipes/")
        try:
            os.mkfifo(fifo)
        except OSError as oe: 
            if oe.errno != errno.EEXIST:
                raise


    def initialize_pipes(self):
        self.make_pipe(self.in_pipe_name)
        self.make_pipe(self.out_pipe_name)

        self.in_pipe = open(self.in_pipe_name, "w")
        self.out_pipe = open(self.out_pipe_name, "r")

    @property
    def build_pipes_args(self):
        return ["in-pipe", self.in_pipe_name, 
                "out-pipe", self.out_pipe_name]

    def run(self, args=None):
        if args != None:
            subprocess.Popen(args + self.build_pipes_args)

        self.initialize_pipes()

    def get(self, j_input):
        self.in_pipe.writelines([json.dumps(j_input) + "\n"])
        self.in_pipe.flush()
        return json.loads(self.out_pipe.readline())


    def stop(self):
        self.out_pipe.close()
        self.in_pipe.close()

    def __del__(self):
        self.stop()