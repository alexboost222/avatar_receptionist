from transport import Transport
import sys
import json

def kek(inp):
    return { "answer": inp["msg"] }

t = Transport(kek)

t.arg_parse(sys.argv)

t.work()