using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectProperties : MonoBehaviour
{
    public string description;
    public Vector3 position;
    public bool open = false;

    private void Start()
    {
        position = this.transform.position;
    }
}
