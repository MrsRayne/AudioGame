# This files contains your custom actions which can be used to run
# custom Python code.
#
# See this guide on how to implement these action:
# https://rasa.com/docs/rasa/custom-actions


# This is a simple example for a custom action which utters "Hello World!"

from typing import Any, Text, Dict, List

from rasa_sdk import Action, Tracker
from rasa_sdk.executor import CollectingDispatcher
import os, aiohttp, asyncio, json, socket, binascii

TCP_IP = '127.0.0.1'
TCP_PORT = 60600
BUFFER_SIZE = 1024
rasa_url = 'http://localhost:5005/webhooks/rest/webhook'

def sendToUnity(textToParse):
# send to unity server
    s = socket.socket(socket.AF_INET, socket.SOCK_STREAM)
    s.connect((TCP_IP, TCP_PORT))
    binarymsg = bytearray(textToParse[:BUFFER_SIZE],"utf8")
    s.send(binarymsg)
    # dont comment back in: data = s.recv(BUFFER_SIZE)
    s.close()


class ActionCheckPosition(Action):
    def name(self) -> Text:
         return "action_check_position"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        dispatcher.utter_message(text="checking position")
        sendToUnity(text=f"gehen+{object}")
        return []

class ActionMovePlayer(Action):
    def name(self) -> Text:
         return "action_move_player"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        dispatcher.utter_message(text="moving player")
        sendToUnity("hello")
        return []

class ActionExecutingAction(Action):
    def name(self) -> Text:
         return "action_execute_action"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        dispatcher.utter_message(text="executing action")
        sendToUnity("hello")
        return []

# class ActionHelloWorld(Action):
#
#     def name(self) -> Text:
#         return "action_hello_world"
#
#     def run(self, dispatcher: CollectingDispatcher,
#             tracker: Tracker,
#             domain: Dict[Text, Any]) -> List[Dict[Text, Any]]:
#
#         dispatcher.utter_message(text="Hello World!")
#
#         return []
