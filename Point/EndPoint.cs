using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndPoint : MonoBehaviour
{
    float initial_y;

    void Start()
    {
        initial_y = transform.position.y;
    }

    void Update()
    {
        transform.position = new Vector3(transform.position.x,
                             initial_y + Mathf.Cos(Time.time * 3) / 10,
                             transform.position.z);
        transform.eulerAngles += new Vector3(0, Time.deltaTime * 50, 0);
    }
}
