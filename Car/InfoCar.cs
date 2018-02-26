using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InfoCar : MonoBehaviour
{
    public float DIST_CHECK = 15f;

    Rigidbody rb;
    float dist_front;
    float dist_left;
    float dist_right;

    void Start()
    {
        rb = GetComponent<Rigidbody>();
    }

    void Update()
    {
        RaycastHit r_hf;
        Physics.Raycast(Get_Forward(), out r_hf,  DIST_CHECK, 1);
        dist_front = r_hf.distance == 0 || r_hf.collider.tag == "Point" ? 1 : r_hf.distance /  DIST_CHECK;
        RaycastHit r_hr;
        Physics.Raycast(Get_Right(), out r_hr,  DIST_CHECK, 1);
        dist_right = r_hr.distance == 0 || r_hr.collider.tag == "Point" ? 1 : r_hr.distance /  DIST_CHECK;
        RaycastHit r_hl;
        Physics.Raycast(Get_Left(), out r_hl,  DIST_CHECK, 1);
        dist_left = r_hl.distance == 0 || r_hl.collider.tag == "Point" ? 1 : r_hl.distance /  DIST_CHECK;
    }

    Vector3 Get_Front()
    {
        return transform.position + transform.forward;
    }
    Ray Get_Forward()
    {
        return new Ray(Get_Front(), new Vector3(transform.forward.x, 0, transform.forward.z));
    }
    Ray Get_Right()
    {
        var vect = transform.forward + transform.right;
        return new Ray(Get_Front(), new Vector3(vect.x, 0, vect.z));
    }
    Ray Get_Left()
    {
        var vect = transform.forward - transform.right;
        return new Ray(Get_Front(), new Vector3(vect.x, 0, vect.z));
    }

    public float Get_Dist_Front()
    {
        return dist_front;
    }
    public float Get_Dist_Left()
    {
        return dist_left;
    }
    public float Get_Dist_Right()
    {
        return dist_right;
    }
    public float Get_Speed()
    {
        return rb.velocity.magnitude / 10;
    }

    void OnDrawGizmos()
    {
        if (rb)
        {
            Gizmos.color = Color.green;
            Gizmos.DrawLine(transform.position, transform.position + rb.velocity);
        }
        var forward = Get_Forward();
        var right = Get_Right();
        var left = Get_Left();

        Gizmos.color = new Color(1, dist_front, dist_front);
        Gizmos.DrawLine(forward.origin, forward.origin + forward.direction *  DIST_CHECK);
        Gizmos.color = new Color(1, dist_right, dist_right);
        Gizmos.DrawLine(right.origin, right.origin + right.direction *  DIST_CHECK);
        Gizmos.color = new Color(1, dist_left, dist_left);
        Gizmos.DrawLine(left.origin, left.origin + left.direction *  DIST_CHECK);
    }
}
