import json
import sys, copy
from service import Service
import argparse


parser = argparse.ArgumentParser(description='Run unity avatar with python services.')
parser.add_argument('-p', '--python', help="Path to python executeable", type=str, required=True)
parser.add_argument('-tts', '--text-to-speech', help="Is text to speech enabled", action='store_true')
parser.add_argument('-we', '--webcam-emotions', help="Is text to webcam emotion recognition enabled", action='store_true')
parser.add_argument('-stt', '--speech-to-text', help="Is text to speech recognition enabled", action='store_true')
parser.add_argument('--no-unity', help="Dry run without unity", action='store_true')


args = parser.parse_args()

def init():
    cog_model = Service("cog-model")
    cog_model.run([args.python, "services/cog_model.py"])

    if args.text_to_speech:
        tts = Service("tts")
        tts.run([args.python, "services/tts.py"])

    if not args.no_unity:
        unity = Service("unity")
        unity.run()

    from_unity = {"start_flag": True, "msg": "Startup"}
    to_unity = {}

    while "stop_flag" not in from_unity and "stop_flag" not in to_unity:
        to_unity = cog_model.get(from_unity)

        if "stop_flag" in to_unity:
            break

        if args.text_to_speech:
            to_unity.update(tts.get(to_unity))

        if not args.no_unity:
            from_unity = unity.get(to_unity)
        else:
            from_unity = copy.deepcopy(to_unity)

    cog_model.stop()
    if not args.no_unity:
        unity.stop()
    if args.text_to_speech:
        tts.stop()
    


if __name__ == "__main__":
    init()