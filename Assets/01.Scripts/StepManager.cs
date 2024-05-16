using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class StepManager : MonoBehaviour
{

    [SerializeField] Transform playerTr;

    void Start()
    {
        Invoke("OpenStep00", 3.0f);
        playerTr.position = Vector3.zero;
    }
    void OpenStep00()
    {
        transform.GetChild(0).gameObject.SetActive(true);
    }
}
