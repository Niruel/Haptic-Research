using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FluidMove : MonoBehaviour
{
    public HapticPlugin hp;

    [Range(.0001f,.0009f)]public float MaxWobble = 0.0003f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    [SerializeField] float decrement_mass = 0.005f;
    [SerializeField] float decrement_fill = 0.005f;

   
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
    float time = 0.005f;
    float fill=.6f;
   

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
        wobbleAmountToAddX += Mathf.Clamp((velocity.x + (angularVelocity.z * 0.0002f)) * MaxWobble, -MaxWobble, MaxWobble);
        wobbleAmountToAddZ += Mathf.Clamp((velocity.z + (angularVelocity.x * 0.0002f)) * MaxWobble, -MaxWobble, MaxWobble);

        // keep last position
        lastPos = transform.position;
        lastRot = transform.rotation.eulerAngles;


         SimulateSpill();
        //check how the cup is 
        //while at a certian point check the rotation 
        //drop the fill amount
        //Debug.Log("X = " + ob.transform.rotation.x + " Z =" + ob.transform.rotation.z);
        //Debug.Log(fill);


        Debug.Log(h_mat.hMass);
      // if (fill > .589f) //&& h_mat.hMass > 0
        {


            if (ob.transform.rotation.x > .12f || ob.transform.rotation.x < -.12f || ob.transform.rotation.z > .12f || ob.transform.rotation.z < -.12f)
            {

                
                    fill -= decrement_fill * Time.deltaTime;
                    rend.material.SetFloat("_fill", fill);
                    if (h_mat.hMass>0)
                    {
                        h_mat.hMass -= decrement_mass * Time.deltaTime;
                    }
                    else
                    {
                        h_mat.hMass = 0;
                    }  
            }
        }
      

  
    }
   
    IEnumerator CheckSpill()
    {
        
        float magnitude;
        magnitude = Vector3.Magnitude(ob.transform.position);
        Debug.Log(h_mat.hMass);

        if (fill > .589f) // && h_mat.hMass>0
        {


            if (magnitude > .25f && magnitude < .28)
            {
                //Debug.Log("Magnitude " + magnitude + " fill " + fill);
                fill -= decrement_fill * Time.deltaTime;
                rend.material.SetFloat("_fill", fill);
                if (h_mat.hMass > 0)
                {
                    h_mat.hMass -= decrement_mass * Time.deltaTime;
                }
                else
                {
                    h_mat.hMass = 0;
                }
                
                
            }
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

