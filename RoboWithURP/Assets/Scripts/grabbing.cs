using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class grabbing : MonoBehaviour
{

    GameObject obj;
    void OnCollisionEnter(Collision collision)
    {
       



            collision.gameObject.transform.SetParent(gameObject.transform);

            if (collision.gameObject.transform.parent != null)
            {
                Debug.Log(collision.gameObject.transform.parent.name);
                obj = collision.gameObject;
           

            }

        


    }
    private void FixedUpdate()
    {
        
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (obj.transform.parent != null)
            {
                Debug.Log("Pressed key");
                obj.transform.SetParent(null);
            }
            

        }
    }
}
