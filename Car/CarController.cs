using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[System.Serializable]
public class AxleInfo
{
    public WheelCollider leftWheel;
    public WheelCollider rightWheel;
    public bool motor; // is this wheel attached to motor?
    public bool steering; // does this wheel apply steer angle?
}

public class CarController : MonoBehaviour
{
    public List<AxleInfo> axleInfos; // the information about each individual axle
    public float maxMotorTorque; // maximum torque the motor can apply to wheel
    public float maxSteeringAngle; // maximum steer angle the wheel can have

    Transform[] wheels = new Transform[4];

    [SerializeField]
    bool manual;

    float motor;
    float steering;

    void Start()
    {
        wheels[0] = transform.Find("Wheels/Front_left/model");
        wheels[1] = transform.Find("Wheels/Front_right/model");
        wheels[2] = transform.Find("Wheels/Back_left/model");
        wheels[3] = transform.Find("Wheels/Back_right/model");
    }

    void move_wheel(Transform wheel, WheelCollider w_c)
    {
        Vector3 pos;
        Quaternion rot;
        w_c.GetWorldPose(out pos, out rot);
        wheel.SetPositionAndRotation(pos, rot);
    }

    public void FixedUpdate()
    {
        if (manual)
        {
            motor = maxMotorTorque * (Input.GetKey(KeyCode.Z) ? 1 : (Input.GetKey(KeyCode.S) ? -1 : 0));
            steering = maxSteeringAngle * (Input.GetKey(KeyCode.D) ? 1 : (Input.GetKey(KeyCode.Q) ? -1 : 0));
        }
        float brake = (Mathf.Abs(motor) < maxMotorTorque / 10 ) ? maxMotorTorque : 0;

        int i = 0;
        foreach (AxleInfo axleInfo in axleInfos)
        {
            axleInfo.leftWheel.brakeTorque = brake;
            axleInfo.rightWheel.brakeTorque = brake;
            if (axleInfo.steering)
            {
                axleInfo.leftWheel.steerAngle = steering;
                axleInfo.rightWheel.steerAngle = steering;
            }
            if (axleInfo.motor)
            {
                axleInfo.leftWheel.motorTorque = motor;
                axleInfo.rightWheel.motorTorque = motor;
            }
            move_wheel(wheels[i], axleInfo.leftWheel);
            i++;
            move_wheel(wheels[i], axleInfo.rightWheel);
            i++;
        }
    }

    public void Set_motor(float m)
    {
        if (!manual)
            motor = m * maxMotorTorque;
    }

    public void Set_steering(float s)
    {
        if (!manual)
            steering = s * maxSteeringAngle;
    }
}