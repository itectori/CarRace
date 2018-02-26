using System;
using UnityEngine;

public class Neuron
{
    static int id = 0;
    static System.Random rand = new System.Random();
    public const double app = 0.1d;

    Neuron[] prev;
    Neuron[] next;
    double[] links_prev;

    double value = 0;
    double delta = 0;
    int index;
    int id_;

    public Neuron(int size_prev, int size_next, int index, bool random_init = true)
    {
        id_ = id;
        id++;
        prev = new Neuron[size_prev];
        next = new Neuron[size_next];
        links_prev = new double[size_prev];
        if (random_init)
            for (int i = 0; i < size_prev; i++)
                links_prev[i] = rand.Next(-10, 10) / 10f;

        this.index = index;
    }

    public void Set_Prev(Neuron[] neurons, int start_index)
    {
        for (int i = 0; i < prev.Length; i++)
            prev[i] = neurons[i + start_index];
    }

    public void Set_Next(Neuron[] neurons, int start_index)
    {
        for (int i = 0; i < next.Length; i++)
            next[i] = neurons[i + start_index];
    }

    public double Get_Value()
    {
        return value;
    }

    public void Set_Delta(double d)
    {
        delta = d;
    }

    public double Get_Delta()
    {
        return delta;
    }

    public void Set_Value(double d)
    {
        value = d;
    }

    public double Get_Link(int index)
    {
        return links_prev[index];
    }

    public void Set_Link(int index_prev, float value)
    {
        links_prev[index_prev] = value;
    }

    public int Get_Id()
    {
        return id_;
    }

    public void Compute()
    {
        double sum = 0;
        for (int i = 0; i < prev.Length; i++)
            sum += prev[i].Get_Value() * links_prev[i];
        value = Tools.Sigmoide(sum);
    }

    public void Compute_Delta()
    {
        double sum = 0;
        for (int i = 0; i < next.Length; i++)
            sum += next[i].Get_Delta() * next[i].Get_Link(index);
        delta = Tools.D_Sigmoide(value) * sum;
    }

    public void Compute_New_Prev_Links()
    {
        for (int i = 0; i < prev.Length; i++)
            links_prev[i] = links_prev[i] + app * prev[i].Get_Value() * delta;
    }

    public void Print()
    {
        var s = "(" + id_ + ") = " + value;
        for (int i = 0; i < next.Length; i++)
            s += "\n    " + id_ + " -> " + next[i].Get_Id() + " : " + next[i].Get_Link(index);
        Debug.Log(s);
    }
}