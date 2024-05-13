using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Test : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "LeftCol")
            Debug.Log(other.gameObject.name);
        else if(other.gameObject.tag == "RightCol")
            Debug.Log(other.gameObject.name);
    }
}
