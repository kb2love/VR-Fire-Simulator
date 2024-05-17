using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{


    void Start()
    {
        Invoke("OpenStep00", 3.0f);
    }
    void OpenStep00()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
