using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class I_IA : MonoBehaviour
{
    protected Network network;

    public Network Get_Network()
    {
        return network;
    }
    public void Set_Network(Network n)
    {
        network = n;
    }

    public abstract void Reset();
    public abstract float Get_Efficiency();
}
