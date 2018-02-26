using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class IA_car_Point : I_IA
{
    [SerializeField]
    EndPoint end_point;

    float angle;

    InfoCar car;
    CarController controller;

    Vector3 pos_init;
    Quaternion rot_init;
    int catch_point = 0;

    void Start()
    {
        pos_init = transform.position;
        rot_init = transform.rotation;
        network = new Network(3, 5, 2);
        car = GetComponent<InfoCar>();
        controller = GetComponent<CarController>();
    }

    void Update()
    {
        angle = Vector3.Angle(transform.TransformDirection(Vector3.forward), end_point.transform.position - transform.position) / 180;
        network.Compute(//angle,
                        car.Get_Dist_Front(),
                        car.Get_Dist_Left(),
                        car.Get_Dist_Right()
                        );// car.Get_Speed());
        var output = network.Get_Output();
        controller.Set_motor((output[0] - 0.5f) * 2);
        controller.Set_steering((output[1] - 0.5f) * 2);
    }

    override public void Reset()
    {
        transform.position = pos_init;
        transform.rotation = rot_init;
        GetComponent<Rigidbody>().velocity = Vector3.zero;
    }

    override public float Get_Efficiency()
    {
        return catch_point + 1 - Vector3.Distance(transform.position, end_point.transform.position) /
                                 Vector3.Distance(pos_init, end_point.transform.position);
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.GetComponentInParent<EndPoint>() == end_point)
        {
            catch_point++;
            pos_init = transform.position;
            rot_init = transform.rotation;
            other.transform.parent.parent.position = new Vector3(Random.Range(-10f, 10f), 0, Random.Range(-10f, 10f));
        }
    }

    void OnDrawGizmos()
    {
        Gizmos.color = new Color(angle, angle, 1);
        Gizmos.DrawLine(transform.position, end_point.transform.position);
    }
}
