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
        slot = tracker.get_slot('obj')
        #dispatcher.utter_message(text=slot)
        slot = "gehen+" + slot
        sendToUnity(slot)
        return []

class ActionOpen(Action):
    def name(self) -> Text:
         return "action_open"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        slot = "open+" + tracker.get_slot('obj')
        sendToUnity(slot)
        return []

class ActionClose(Action):
    def name(self) -> Text:
         return "action_close"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        slot = "close+" + tracker.get_slot('obj')
        sendToUnity(slot)
        return []

class ActionPet(Action):
    def name(self) -> Text:
         return "action_pet"
    def run(self, dispatcher, tracker:Tracker, domain:"DomainDict") -> List[Dict[Text, Any]]:
        slot = "pet+" + tracker.get_slot('obj')
        sendToUnity(slot)
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
