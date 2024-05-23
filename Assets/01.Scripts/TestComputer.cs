using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestComputer : MonoBehaviour
{
    [SerializeField] Step00 step00;
    [SerializeField] Step01 step01;
    [SerializeField] Step02 step02;
    [SerializeField] Step03 step03;
    [SerializeField] Step04 step04;
    [SerializeField] Step05 step05;

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
        else if (Input.GetKeyDown(KeyCode.Alpha4))
            step03.GetComponent<Step03>().OnClickMethods();
        else if(  Input.GetKeyDown(KeyCode.Alpha5))
            step04.GetComponent<Step04>().OnClickMethods();
        else if (Input.GetKeyDown(KeyCode.Alpha6))
            step05.GetComponent<Step05>().OnClickMethods();


    }
}
