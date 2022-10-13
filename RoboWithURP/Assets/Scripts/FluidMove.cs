using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FluidMove : MonoBehaviour
{

    public float MaxWobble = 0.03f;
    public float WobbleSpeed = 1f;
    public float Recovery = 1f;

    [Range(.54f, 0.606f)] public float fill;
    public float h_vel_control_X;
    public float h_vel_control_Z;
    public HapticPlugin hPlugin = null;
    public GameObject cupObj;
    Renderer rend;
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
    float curFill;


    // Use this for initialization
    void Start()
    {
        rend = GetComponent<Renderer>();
        rend.material.SetFloat("_fill", fill);


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

        if (hPlugin != null)//&& hPlugin.bIsGrabbing
        {
            h_vel_control_X = hPlugin.CurrentVelocity.x;
            h_vel_control_Z = hPlugin.CurrentVelocity.z;
            Debug.Log( cupObj.transform.rotation.eulerAngles);
            fill -= .05f*Time.deltaTime;
            rend.material.SetFloat("_fill", fill);
            if (fill < 0)
            {
                fill = 0;
            }
        }
    }
}



