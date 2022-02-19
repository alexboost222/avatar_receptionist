from enum import Enum
import json
import requests
import jwt
import time
import os

TTS_FOLDER = os.path.dirname(os.path.abspath(__file__))

CONFIG_DIRECTORY = "config"
CONFIG_FILE_PATH = os.path.join(TTS_FOLDER, CONFIG_DIRECTORY, "config.pem")
OAUTH_PRIVATE_KEY_FILE_PATH = os.path.join(TTS_FOLDER, CONFIG_DIRECTORY, "private.pem")

SPEECH_DIRECTORY_PATH = os.path.join(TTS_FOLDER, "speech")

SERVICE_ACCOUNT_ID_KEY = "service_account_id"
OAUTH_KEY_ID_KEY = "key_id"
IAM_TOKEN_KEY = "iamToken"
IAM_TOKEN_EXPIRES_AT_KEY = "expiresAt"

IAM_TOKEN_CREATE_URL = "https://iam.api.cloud.yandex.net/iam/v1/tokens"
SYNTHESIZE_SPEECH_URL = "https://tts.api.cloud.yandex.net/speech/v1/tts:synthesize"

# TODO switch to json config file
def _get_auth_info():
    service_account_id = ""
    oauth_key_id = ""

    with open(CONFIG_FILE_PATH, "r") as config:
        config_lines = config.readlines()

        next_default = "none"

        service_account_id_line = next((l for l in config_lines if l.startswith(SERVICE_ACCOUNT_ID_KEY)), next_default)
        if service_account_id_line != next_default:
            service_account_id = service_account_id_line.strip().split()[1]

        oauth_key_id_line = next((l for l in config_lines if l.startswith(OAUTH_KEY_ID_KEY)), next_default)
        if oauth_key_id_line != next_default:
            oauth_key_id = oauth_key_id_line.strip().split()[1]
    
    return (service_account_id, oauth_key_id)

def _get_auth_jwt(service_account_id, oauth_key_id):
    with open(OAUTH_PRIVATE_KEY_FILE_PATH, "r") as private:
        private_key = private.read()

    now = int(time.time())
    payload = {
            "aud": IAM_TOKEN_CREATE_URL,
            "iss": service_account_id,
            "iat": now,
            "exp": now + 360}

    auth_jwt = jwt.encode(
        payload,
        private_key,
        algorithm="PS256",
        headers={ "kid": oauth_key_id }
        )

    return auth_jwt

def _get_iam_token(auth_jwt):
    params = { "jwt": auth_jwt }
    response = requests.post(IAM_TOKEN_CREATE_URL, params=params)
    decoded_response = response.content.decode("UTF-8")
    response_json = json.loads(decoded_response)
    iam_token = response_json.get(IAM_TOKEN_KEY)
    expires_at = response_json.get(IAM_TOKEN_EXPIRES_AT_KEY)
    return (iam_token, expires_at)

def _synthesize(text, format, iam_token):
    headers = {
        "Authorization": f"Bearer {iam_token}",
    }

    data = {
        "text": text,
        "lang": "ru-RU",
        "speed": 1.0,
        "format": format,
        "voice": "ermil:rc",
    }

    response = requests.post(SYNTHESIZE_SPEECH_URL, headers=headers, data=data, stream=True)
    if response.status_code != 200:
        raise RuntimeError("Invalid response received: code: %d, message: %s" % (response.status_code, response.text))
    
    return response.content

def handle(input):
    (service_account_id, oauth_key_id) = _get_auth_info()
    auth_jwt = _get_auth_jwt(service_account_id, oauth_key_id)
    # TODO use expires at
    (iam_token, expires_at) = _get_iam_token(auth_jwt)

    text = input["msg"]
    synt_result = _synthesize(text, "oggopus", iam_token)

    synt_file_path = f"{SPEECH_DIRECTORY_PATH}/{hash(text)}.ogg"
    synt_file_path = os.path.abspath(synt_file_path)

    with open(synt_file_path, "wb") as syn_result:
        syn_result.write(synt_result)

    return { "avatar_speech_filepath": synt_file_path }

print(__file__)
handle({ "msg": "Привет!"})
