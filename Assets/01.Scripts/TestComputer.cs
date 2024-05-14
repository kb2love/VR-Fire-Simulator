using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComputer : MonoBehaviour
{
    [SerializeField] Step00 step00;
    [SerializeField] Step01 step01;
    [SerializeField] Step02 step02;


    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Alpha1))
        {
            step00.GetComponent<Step00>().OnClickMethods();
            Debug.Log("alpha1");
        }
        else if(Input.GetKeyDown(KeyCode.Alpha2))
            step01.GetComponent<Step01>().OnClickMethods();
        else if(Input.GetKeyDown(KeyCode.Alpha3))
            step02.GetComponent<Step02>().OnClickMethods();
    }
}
