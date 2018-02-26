using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Xor : MonoBehaviour
{
    Network n;
    double[][] inputs = new double[][] {
                                 new double[] { 1, 1 },
                                 new double[] { 0, 1 },
                                 new double[] { 0, 0 },
                                 new double[] { 1, 0 } };
    int[] outputs = new int[] { 0, 1, 0, 1 };

    void Start()
    {
        n = new Network(2, 3, 2);
    }
    void Update()
    {
        var delta = 1d;
        for (int i = 0; i < 10; i++)
            delta = n.Teach(inputs, outputs);
        print("-------------------\ndelta = " + delta);
        print("(0, 1) -> " + n.Compute(new double[] { 0, 1 }));
        print("(1, 1) -> " + n.Compute(new double[] { 1, 1 }));
        print("(0, 0) -> " + n.Compute(new double[] { 0, 0 }));
        print("(1, 0) -> " + n.Compute(new double[] { 1, 0 }));
    }
}
