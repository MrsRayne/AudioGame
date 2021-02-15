﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEncoder : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();
    GameObject selectedGameObject;
    bool toReturn;
    enum ActionTypes { Oeffnen, Schliessen, Gehen, Streicheln };
    ActionTypes actionTypes;
    public static ActionEncoder instance;
    [SerializeField] PlayerController playerController;
    private void Start()
    {
        instance = this;
    }
    public void ActionDecoder(string message)
    {
        if (message.Split('+').Length > 1)
        {
            string[] messageParts = new string[] { message.Split('+')[0], message.Split('+')[1] };
            print(messageParts[0] + " " + messageParts[1]);

            switch (messageParts[1])
            {
                case "katze":
                    selectedGameObject = gameObjects[0];
                    break;
                case "radio":
                    selectedGameObject = gameObjects[1];
                    break;
                case "fenster":
                    selectedGameObject = gameObjects[2];
                    break;
                default:
                    break;
            }
            switch (messageParts[0])
            {
                case "open":
                    actionTypes = ActionTypes.Oeffnen;
                    toReturn = !selectedGameObject.GetComponent<ObjectProperties>().open;
                    if (toReturn)
                    {
                        playerController.OpenWindow();
                        //selectedGameObject.GetComponent<ObjectProperties>().open = true;
                    }
                    break;
                case "close":
                    actionTypes = ActionTypes.Schliessen;
                    toReturn = selectedGameObject.GetComponent<ObjectProperties>().open;
                    if (toReturn)
                    {
                        playerController.CloseWindow();
                        //selectedGameObject.GetComponent<ObjectProperties>().open = false;
                    }
                    break;
                case "gehen":
                    actionTypes = ActionTypes.Gehen;
                    //toReturn = selectedGameObject != PlayerScript.instance.currentPositionObject ? true : false;
                    //if (toReturn)
                    //{
                    //PlayerScript.instance.currentPositionObject = selectedGameObject;
                    switch (messageParts[1])
                    {
                        case "katze":
                            playerController.CatPosition();
                            break;
                        case "radio":
                            playerController.TablePosition();
                            break;
                        case "fenster":
                            playerController.WindowPosition();
                            break;
                    }
                    //}
                    break;
                case "pet":
                    if (selectedGameObject == gameObjects[0])
                    {
                        actionTypes = ActionTypes.Streicheln;
                        playerController.PetCat();
                    }
                    break;
                default:
                    break;
            }
        }
    }
}
