using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActionEncoder : MonoBehaviour
{
    [SerializeField] List<GameObject> gameObjects = new List<GameObject>();
    GameObject selectedGameObject; 
    bool toReturn;
    enum ActionTypes {Oeffnen, Schliessen, Gehen, Betrachten};
    ActionTypes actionTypes;
    public static ActionEncoder instance;
    private void Start()
    {
        instance = this;
    }
    public void ActionDecoder(string message)
    {
        print(message);
        if (message.Split('+').Length>1)
        { 
        string[] messageParts = new string[] { message.Split('+')[0], message.Split('+')[1] };
        print(messageParts[0] + " " + messageParts[1]);
        
        switch (messageParts[1])
        {
            case "tuer":
                selectedGameObject = gameObjects[0];
                break;
            case "schrank":
                selectedGameObject = gameObjects[1];
                break;
            default:
                break;
        }
            switch (messageParts[0])
            {
                case "oeffnen":
                    actionTypes = ActionTypes.Oeffnen;
                    toReturn = !selectedGameObject.GetComponent<ObjectProperties>().open;
                    if (toReturn)
                    {
                        selectedGameObject.GetComponent<ObjectProperties>().open = true;
                    }
                    break;
                case "schliessen":
                    actionTypes = ActionTypes.Schliessen;
                    toReturn = selectedGameObject.GetComponent<ObjectProperties>().open;
                    if (toReturn)
                    {
                        selectedGameObject.GetComponent<ObjectProperties>().open = false;
                    }
                    break;
                case "gehen":
                    actionTypes = ActionTypes.Gehen;
                    toReturn = selectedGameObject != PlayerScript.instance.currentPositionObject ? true : false;
                    if (toReturn)
                    {
                        PlayerScript.instance.currentPositionObject = selectedGameObject;
                        PlayerScript.instance.gameObject.transform.position = selectedGameObject.GetComponent<ObjectProperties>().position;
                    }
                    break;
                case "betrachten":
                    actionTypes = ActionTypes.Betrachten;
                    break;
                default:
                    break;
            }
        }
        NetworkManager.instance.SendMessageToRasa(toReturn.ToString());
    }
}
