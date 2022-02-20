import unittest
from .. import tts
import os

MSG_KEY = "msg"

RESULT_OK_KEY = "ok"
RESULT_RESPONSE_KEY = "response"
RESULT_ERROR_KEY = "error"

class TestTTS(unittest.TestCase):
    def test_result_params(self):
        input = { MSG_KEY: "Тестовое сообщение"}
        result = tts.handle(input)

        self.assertTrue(len(result) == 3)
        self.assertTrue(RESULT_OK_KEY in result)
        self.assertTrue(RESULT_RESPONSE_KEY in result)
        self.assertTrue(RESULT_ERROR_KEY in result)

    def test_correct_input(self):
        input = { MSG_KEY: "Тестовое сообщение" }
        result = tts.handle(input)
        self.assertTrue(result[RESULT_OK_KEY])
        self.assertTrue(os.path.exists(result[RESULT_RESPONSE_KEY]))
        self.assertTrue(os.path.isfile(result[RESULT_RESPONSE_KEY]))

    def test_incorrect_input(self):
        input = { }
        result = tts.handle(input)
        self.assertFalse(result[RESULT_OK_KEY])

if __name__ == '__main__':
    unittest.main()