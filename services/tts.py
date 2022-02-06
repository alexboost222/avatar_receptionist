from transport import Transport
import sys
import json
import requests
import jwt
import time

ROOT = "./services/tts"
CONFIG_FILE_RELATIVE_PATH = f"{ROOT}/config.pem"
OAUTH_PRIVATE_KEY_FILE_PATH = f"{ROOT}/private.pem"
SPEECH_FOLDER_PATH = f"{ROOT}/speech"

SERVICE_ACCOUNT_ID_KEY = "service_account_id"
OAUTH_KEY_ID_KEY = "key_id"
IAM_TOKEN_KEY = "iamToken"
IAM_TOKEN_EXPIRES_AT_KEY = "expiresAt"

IAM_TOKEN_CREATE_URL = "https://iam.api.cloud.yandex.net/iam/v1/tokens"
SYNTHESIZE_SPEECH_URL = "https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize"

NEXT_DEFAULT = "none"

service_account_id = ""
oauth_key_id = ""

setup_dirty_flag = False

# TODO switch to json config file
def setup_config():
    # TODO rewrite global to smth better
    global service_account_id
    global oauth_key_id

    with open(CONFIG_FILE_RELATIVE_PATH, "r") as config:
        config_lines = config.readlines()

        service_account_id_line = next((l for l in config_lines if l.startswith(SERVICE_ACCOUNT_ID_KEY)), NEXT_DEFAULT)
        if service_account_id_line != NEXT_DEFAULT:
            service_account_id = service_account_id_line.strip().split()[1]

        oauth_key_id_line = next((l for l in config_lines if l.startswith(OAUTH_KEY_ID_KEY)), NEXT_DEFAULT)
        if oauth_key_id_line != NEXT_DEFAULT:
            oauth_key_id = oauth_key_id_line.strip().split()[1]

def get_auth_jwt():
    with open(OAUTH_PRIVATE_KEY_FILE_PATH, "r") as private:
        private_key = private.read() # Reading the private key from the file.

    now = int(time.time())
    payload = {
            "aud": IAM_TOKEN_CREATE_URL,
            "iss": service_account_id,
            "iat": now,
            "exp": now + 360}

    # JWT generation.
    auth_jwt = jwt.encode(
        payload,
        private_key,
        algorithm="PS256",
        headers={ "kid": oauth_key_id }
        )

    return auth_jwt

def create_iam_token(auth_jwt):
    params = { "jwt": auth_jwt }
    response = requests.post(IAM_TOKEN_CREATE_URL, params=params)
    decoded_response = response.content.decode("UTF-8")
    response_json = json.loads(decoded_response)
    iam_token = response_json.get(IAM_TOKEN_KEY)
    expires_at = response_json.get(IAM_TOKEN_EXPIRES_AT_KEY)
    return (iam_token, expires_at)

def synthesize(text, emotion, iam_token):
    headers = {
        "Authorization": f"Bearer {iam_token}",
    }

    data = {
        "text": text,
        "lang": "ru-RU",
        "speed": 1.0,
        "emotion": emotion,
    }

    response = requests.post(SYNTHESIZE_SPEECH_URL, headers=headers, data=data, stream=True)
    if response.status_code != 200:
        raise RuntimeError("Invalid response received: code: %d, message: %s" % (response.status_code, response.text))
    
    return response.content

def handle(input):
    # TODO use class and methods instead of functions
    global setup_dirty_flag

    if not setup_dirty_flag:
        setup_config()
        setup_dirty_flag = True
    
    text = input["msg"]
    auth_jwt = get_auth_jwt()
    # TODO use expires at
    (iam_token, expires_at) = create_iam_token(auth_jwt)
    synt_result = synthesize(text, "good", iam_token)

    synt_file_path = f"{SPEECH_FOLDER_PATH}/{hash(text)}.ogg"

    with open(synt_file_path, "wb") as syn_result:
        syn_result.write(synt_result)

    return { "avatar_speech_filepath": synt_file_path }

t = Transport(handle, "tts")

t.arg_parse(sys.argv)

t.work()
