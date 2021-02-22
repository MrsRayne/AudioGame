using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerScript : MonoBehaviour
{
    public static PlayerScript instance;
    public GameObject currentPositionObject;
    private void Start()
    {
        instance = this;
    }
}
