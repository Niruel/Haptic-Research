using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using UnityEngine;
using UnityEngine.UI;

public class UIController : MonoBehaviour
{
    Canvas UI_control;
    Vector3 _transform = new Vector3(-0.0212000012f, -0.162200004f, -0.207000002f);


    public GameObject[] cups =new GameObject[4];
    // Start is called before the first frame update
    void Start()
    {
        UI_control = GetComponentInChildren<Canvas>();
        if (UI_control)
        {
            UI_control.enabled = true;
        }
     
    }

    public void PolkaDot(bool _active)
    {
        cups[0].SetActive(_active);

        if (!cups[0].activeSelf)
        {
            cups[0].transform.position = _transform; 
        }

    }
    public void Stripe(bool _active)
    {
        cups[1].SetActive(_active);

        if (!cups[1].activeSelf)
        {
            cups[1].transform.position = _transform;
        }
    }
    public void Route66(bool _active)
    {
        cups[2].SetActive(_active);

        if (!cups[2].activeSelf)
        {
            cups[2].transform.position = _transform;
        }
    }
    public void Micky(bool _active)
    {
        cups[3].SetActive(_active);

        if (!cups[3].activeSelf)
        {
            cups[3].transform.position = _transform;
        }
    }
    
    
}


