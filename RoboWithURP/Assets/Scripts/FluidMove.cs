using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FluidMove : MonoBehaviour
{
    public HapticPlugin hp;

    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;

   
    Renderer rend;
    HapticMaterial h_mat;
  
    Vector3 lastPos;
    Vector3 velocity;
    Vector3 lastRot;
    Vector3 angularVelocity;

    float wobbleAmountX;
    float wobbleAmountZ;
    float wobbleAmountToAddX;
    float wobbleAmountToAddZ;
    float pulse;
    float time = 0.5f;
    float fill=.606f;
   

    Rigidbody rigidBody;
    public GameObject ob;
    

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_fill", fill);
        rigidBody = ob.GetComponent<Rigidbody>();
        h_mat = ob.GetComponent<HapticMaterial>();
        

       
    }
    private void Update()
    {
        //fill about adjust
       // rend.material.SetFloat("_fill", fill);

        time += Time.deltaTime;
        // decrease wobble over time
        wobbleAmountToAddX = Mathf.Lerp(wobbleAmountToAddX, 0, Time.deltaTime * (Recovery));
        wobbleAmountToAddZ = Mathf.Lerp(wobbleAmountToAddZ, 0, Time.deltaTime * (Recovery));

        // make a sine wave of the decreasing wobble
        pulse = 2 * Mathf.PI * WobbleSpeed;
        wobbleAmountX = wobbleAmountToAddX * Mathf.Sin(pulse * time);
        wobbleAmountZ = wobbleAmountToAddZ * Mathf.Sin(pulse * time);

        // send it to the shader
        rend.material.SetFloat("_WobbleX", wobbleAmountX);
        rend.material.SetFloat("_WobbleZ", wobbleAmountZ);


        // velocity
        velocity = (lastPos - transform.position) / Time.deltaTime;
        angularVelocity = transform.rotation.eulerAngles - lastRot;
        //Debug.Log(velocity);


        // add clamped velocity to wobble
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.2f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;


         SimulateSpill();
       //check how the cup is 
       //while at a certian point check the rotation 
       //drop the fill amount
        Debug.Log("X = "+ ob.transform.rotation.x + " Z =" + ob.transform.rotation.z);
        Debug.Log(fill);
        if (fill　>.58f  && ob.transform.rotation.x > .12f || ob.transform.rotation.x < -.12f || ob.transform.rotation.z > .12f || ob.transform.rotation.z < -.12f)
        {
            
            {
                fill -= 0.005f * Time.deltaTime;
                h_mat.hMass-=0.005f*Time.deltaTime;
                rend.material.SetFloat("_fill", fill);
            }
        }
      
      
    }
   
    IEnumerator CheckSpill()
    {
        float magnitude;
        magnitude = Vector3.Magnitude(ob.transform.position);
        Debug.Log(magnitude);

        if (magnitude > .25f)
        {
            fill -= 0.005f * Time.deltaTime;
            rend.material.SetFloat("_fill", fill);
        }
        yield return new WaitForFixedUpdate();
    }

    void SimulateSpill()
    {

        if (hp.bIsGrabbing)
        {
            StartCoroutine("CheckSpill");
        }
        if (!hp.bIsGrabbing)
        {
            StopCoroutine("CheckSpill");
        }
    }
   
}

