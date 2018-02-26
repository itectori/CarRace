using System.Collections;
using System.Collections.Generic;
using System;

public class Tools
{
    public static double Sigmoide(double x)
    {
        return 1d / (1d + Math.Exp(-x));
    }

    public static double D_Sigmoide(double x)
    {
        return x * (1d - x);
    }
}
