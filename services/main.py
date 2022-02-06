import json
import sys
from service import Service
import argparse


parser = argparse.ArgumentParser(description='Run unity avatar with')
parser.add_argument('integers', metavar='N', type=int, nargs='+',
                    help='an integer for the accumulator')
parser.add_argument('--sum', dest='accumulate', action='store_const',
                    const=sum, default=max,
                    help='sum the integers (default: find the max)')

args = parser.parse_args()



def init(python_path):
    tts = Service("tts")

    tts.run([python_path, "services/tts.py"])
    # tts.run()

    inp = input("Svc input: ")

    while(inp != 'q'):
        out = tts.get({"msg": inp})
        print("Svc output: " + json.dumps(out))
        inp = input("Svc input: ")

    tts.stop()


if __name__ == "__main__":
    init(sys.argv[2])