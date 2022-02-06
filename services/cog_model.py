import sys
from transport import Transport

def execute(j):
    print("User says: " + str(j))
    msg = input("Your answer: ")
    return {"msg": msg}
#    answer = {"msg" : msg}

#    if msg == "exit":
#        answer.update({"stop_flag" : True})

#    return answer

t = Transport(execute)
t.arg_parse(sys.argv)
t.work()