import requests
import json
from transport import Transport
import sys

TOKEN = ""

with(open("./services/bot/token.pem")) as token_file:
    TOKEN = token_file.read()

CHAT_ID = "-648360876"
BOT_URL = f"https://api.telegram.org/bot{TOKEN}"
QUESTION_COMMAND = "/q"
SEND_MESSAGE_URL = f"{BOT_URL}/sendMessage"
GET_UPDATES_URL = f"{BOT_URL}/getUpdates"

def handle(input):
    response = requests.post(f"{SEND_MESSAGE_URL}?chat_id={CHAT_ID}&text={input['msg']}")
    decoded_response = response.content.decode("UTF-8")
    response_json = json.loads(decoded_response)

    sent_timestamp = 0
    
    if response_json["ok"]:
        sent_timestamp = response_json["result"]["date"]
    else:
        return { "msg": "Ошибка отправки сообщения в телеграм!" }

    params = { "limit" : 1, "timeout": 10, "allowed_updates": "message" }

    msg = ""
    prev_response_json = None

    while True:
        response = requests.get(GET_UPDATES_URL, params=json.dumps(params))
        decoded_response = response.content.decode("UTF-8")
        response_json = json.loads(decoded_response)
        valid_updates = [u for u in response_json["result"] if validate_update(u, sent_timestamp)]
        
        a, b = json.dumps(response_json, sort_keys=True), json.dumps(prev_response_json, sort_keys=True)

        if len(valid_updates) != 0:
            msg = " ".join(valid_updates[-1]["message"]["text"].split()[1:])
            break
        elif prev_response_json != None and a != b:
            requests.post(f"{SEND_MESSAGE_URL}?chat_id={CHAT_ID}&text=Ожидается сообщение в формате '/q <Teкcт>'")

        prev_response_json = response_json

    return { "msg": msg }

def validate_update(update_to_validate, sent_timestamp):
    if not "message" in update_to_validate:
        return False

    if f"{update_to_validate['message']['chat']['id']}" != CHAT_ID:
        return False

    if not "entities" in update_to_validate["message"]:
        return False

    if not any(e["type"] == "bot_command" for e in update_to_validate["message"]["entities"]):
        return False

    if len(update_to_validate["message"]["text"].split()) < 2:
        return False
    
    if update_to_validate["message"]["date"] < sent_timestamp:
        return False

    return True

t = Transport(handle, "bot")

t.arg_parse(sys.argv)

t.work()
