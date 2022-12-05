using UnityEditor;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class FluidMove : MonoBehaviour
{
    
    public HapticPlugin hp;
    public GameObject cup_obj;
    [Range(.0001f,.0009f)]
    public float MaxWobble = 0.0003f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;
    public Color color;

    [SerializeField] float decrement_mass = 0.005f;
    [SerializeField] float decrement_fill = 0.005f;
    [SerializeField] float max_Mass;
    [SerializeField] float min_Mass;
   
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
   

    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_fill", fill);
        rend.material.SetColor("_color", color);
        h_mat = cup_obj.GetComponent<HapticMaterial>();
        h_mat.hMass = max_Mass;
        

       
    }
    private void FixedUpdate()
    {
        #region Slosh effect
        

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
        #endregion

        SimulateSpill();

    }
   
    IEnumerator CheckSpill()
    {
        
       
        float hp_x =  Mathf.Abs(hp.CurrentVelocity.x);
        float hp_y =  Mathf.Abs(hp.CurrentVelocity.y);
        float hp_z =  Mathf.Abs(hp.CurrentVelocity.z);
        

            if ( hp_x > 50f || hp_z> 50f)
            {

               
                fill -=  decrement_fill * Time.deltaTime;
                rend.material.SetFloat("_fill", fill);
                if (h_mat.hMass > min_Mass)
                {
                    h_mat.hMass -= decrement_mass * Time.deltaTime;
                }
                else
                {
                    h_mat.hMass = min_Mass;
                }
                
                
            }
      
        
        yield return new WaitForFixedUpdate();
    }

    IEnumerator CheckRotation()
    {
        if (cup_obj.transform.rotation.x > .12f || cup_obj.transform.rotation.x < -.12f || cup_obj.transform.rotation.z > .12f || cup_obj.transform.rotation.z < -.12f)
        {


            fill -= decrement_fill * Time.deltaTime;
            rend.material.SetFloat("_fill", fill);
            if (h_mat.hMass > min_Mass)
            {
                h_mat.hMass -= decrement_mass * Time.deltaTime;
            }
            else
            {
                h_mat.hMass = min_Mass;
            }
        }


        yield return new WaitForFixedUpdate();
    }

    void SimulateSpill()
    {
        
        if (hp.bIsGrabbing)
        {
            StartCoroutine(CheckRotation());
            StartCoroutine(CheckSpill());
        }
        if (!hp.bIsGrabbing)
        {
            StopCoroutine(CheckRotation());
            StopCoroutine(CheckSpill());
        }
    }
   
}

