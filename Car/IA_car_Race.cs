using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_car_Race : I_IA
{
    static int count_car = 0;
    static int count_collided = 0;

    InfoCar car;
    CarController controller;
    Vector3 pos_init;
    Quaternion rot_init;

    float time_Alive = 0;
    bool collided = false;
    void Start()
    {
        count_car++;
        pos_init = transform.position;
        rot_init = transform.rotation;
        network = new Network(3, 5, 1);
        car = GetComponent<InfoCar>();
        controller = GetComponent<CarController>();
    }

    void Update()
    {
        if (!collided)
        {
            time_Alive += Time.deltaTime;
            network.Compute(car.Get_Dist_Front(),
                            car.Get_Dist_Left(),
                            car.Get_Dist_Right());
            var output = network.Get_Output();
            controller.Set_steering((output[0] - 0.5f) * 2);
            controller.Set_motor(1);
        }
    }

    override public void Reset()
    {
        transform.position = pos_init;
        transform.rotation = rot_init;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
        GetComponent<Rigidbody>().angularVelocity = Vector3.zero;
        time_Alive = 0;
        collided = false;
        count_collided = 0;
    }

    public override float Get_Efficiency()
    {
        return time_Alive;
    }

    void OnCollisionEnter(Collision other)
    {
        if (!collided)
        {
            collided = true;
            count_collided++;
            if (count_collided == count_car)
                RaceManager.Net_Turn();
        }
    }
}
