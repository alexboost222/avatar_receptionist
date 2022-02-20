import unittest
from .. import console_cog_model
import os
from unittest.mock import patch

MSG_KEY = "msg"

RESULT_OK_KEY = "ok"
RESULT_RESPONSE_KEY = "response"
RESULT_ERROR_KEY = "error"

TEST_INPUT = "Тестовый ввод"
class TestConcoleCogModel(unittest.TestCase):
    @patch('builtins.input', lambda *args: TEST_INPUT)
    def test_result_params(self):
        input = { MSG_KEY: "Тестовое сообщение"}
        result = console_cog_model.handle(input)

        self.assertTrue(len(result) == 3)
        self.assertTrue(RESULT_OK_KEY in result)
        self.assertTrue(RESULT_RESPONSE_KEY in result)
        self.assertTrue(RESULT_ERROR_KEY in result)

    @patch('builtins.input', lambda *args: TEST_INPUT)
    def test_correct_input(self):
        input = { MSG_KEY: "Тестовое сообщение" }
        result = console_cog_model.handle(input)
        self.assertTrue(result[RESULT_OK_KEY])
        self.assertEqual(result[RESULT_RESPONSE_KEY], TEST_INPUT)

    @patch('builtins.input', lambda *args: TEST_INPUT)
    def test_incorrect_input(self):
        input = { }
        result = console_cog_model.handle(input)
        self.assertFalse(result[RESULT_OK_KEY])

if __name__ == '__main__':
    unittest.main()