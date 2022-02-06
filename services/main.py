import json
from service import Service

def init():
    tts = Service("tts")

    # tts.run(["python", "tts.py"])
    tts.run()

    inp = input("Svc input: ")

    while(inp != 'q'):
        out = tts.get({"msg": inp})
        print("Svc output: " + json.dumps(out))
        inp = input("Svc input: ")

    tts.stop()



if __name__ == "__main__":
    init()