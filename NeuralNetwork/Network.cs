using System;
using UnityEngine;

public class Network
{
    public const float proba_mutation = 0.1f;

    Neuron[] neurons;
    int[] layers;
    int[] sum_layers;
    int total = 0;

    public Network(Network n1, Network n2)
    {
        if (n1.total != n2.total && n1.layers.Length != n2.layers.Length)
            throw new Exception("Networks don't have the same size");
        layers = new int[n1.layers.Length];
        sum_layers = new int[layers.Length];
        n1.layers.CopyTo(layers, 0);
        total = n1.total;
        sum_layers[0] = layers[0];
        for (int i = 1; i < layers.Length; i++)
            sum_layers[i] = sum_layers[i - 1] + layers[i];
        neurons = new Neuron[total];
        init(false);
        int count_mut = 0;
        int count_n_mut = 0;
        for (int i = 1; i < layers.Length; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                for (int k = 0; k < layers[i - 1]; k++)
                {
                    float f1 = (float)n1.neurons[j + sum_layers[i - 1]].Get_Link(k);
                    float f2 = (float)n2.neurons[j + sum_layers[i - 1]].Get_Link(k);
                    if (UnityEngine.Random.Range(0f, 1f) < proba_mutation)
                    {
                        count_mut++;
                        neurons[j + sum_layers[i - 1]].Set_Link(k, f1 + UnityEngine.Random.Range(-5f, 5f) / 10f);
                    }
                    else
                    {
                        count_n_mut++;
                        float f = f1 + (f1 > f2 ? 0.1f : -0.1f);
                        neurons[j + sum_layers[i - 1]].Set_Link(k, f);
                    }
                }
            }
        }
    }

    public Network(params int[] layers)
    {
        if (layers.Length < 2)
            throw new Exception("Network must have at least 2 layers");
        this.layers = layers;
        sum_layers = new int[layers.Length];
        sum_layers[0] = layers[0];
        for (int i = 1; i < layers.Length; i++)
            sum_layers[i] = sum_layers[i - 1] + layers[i];
        total = sum_layers[sum_layers.Length - 1];
        neurons = new Neuron[total];
        init();
    }

    private void init(bool random_init = true)
    {
        int index = 0;
        for (int i = 0; i < layers[0]; i++)
            neurons[i] = new Neuron(0, layers[1], i, random_init);

        index += layers[0];
        for (int i = 1; i < layers.Length - 1; i++)
        {
            for (int j = 0; j < layers[i]; j++)
                neurons[index + j] = new Neuron(layers[i - 1], layers[i + 1], j, random_init);
            index += layers[i];
        }

        for (int i = 0; i < layers[layers.Length - 1]; i++)
            neurons[index + i] = new Neuron(layers[layers.Length - 2], 0, i, random_init);

        index = 0;
        for (int i = 0; i < layers[0]; i++)
            neurons[i].Set_Next(neurons, layers[0]);
        index += layers[0];
        for (int i = 1; i < layers.Length - 1; i++)
        {
            for (int j = 0; j < layers[i]; j++)
            {
                neurons[index + j].Set_Next(neurons, index + layers[i]);
                neurons[index + j].Set_Prev(neurons, index - layers[i - 1]);
            }
            index += layers[i];
        }

        for (int i = 0; i < layers[layers.Length - 1]; i++)
            neurons[i + index].Set_Prev(neurons, index - layers[layers.Length - 2]);
    }

    private void set_input(params double[] inputs)
    {
        if (inputs.Length != layers[0])
            throw new Exception("incorrect number of inputs");
        for (int i = 0; i < layers[0]; i++)
            neurons[i].Set_Value(inputs[i]);
    }

    public int Compute(params double[] inputs)
    {
        set_input(inputs);
        for (int i = layers[0]; i < total; i++)
            neurons[i].Compute();

        double max = 0;
        int max_index = 0;

        for (int i = total - layers[layers.Length - 1]; i < total; i++)
        {
            if (neurons[i].Get_Value() > max)
            {
                max = neurons[i].Get_Value();
                max_index = i;
            }
        }

        return max_index - total + layers[layers.Length - 1];
    }

    public float[] Get_Output()
    {
        var res = new float[layers[layers.Length - 1]];
        for (int i = total - layers[layers.Length - 1]; i < total; i++)
            res[i - total + layers[layers.Length - 1]] = (float)neurons[i].Get_Value();
        return res;
    }

    public double Teach(double[][] inputs, int[] results)
    {
        double delta_max = 0;

        for (int i = 0; i < inputs.Length; i++)
        {
            int res = Compute(inputs[i]);
            double delta = 1d - neurons[results[i] + total - layers[layers.Length - 1]].Get_Value();
            if (delta > delta_max)
                delta_max = delta;
            for (int j = 0; j < layers[layers.Length - 1]; j++)
            {
                if (j == results[i])
                    neurons[total - layers[layers.Length - 1] + j].Set_Delta(1 - neurons[total - layers[layers.Length - 1]].Get_Value());
                else if (j == res)
                    neurons[total - layers[layers.Length - 1] + j].Set_Delta(0 - neurons[total - layers[layers.Length - 1]].Get_Value());
                else
                    neurons[total - layers[layers.Length - 1] + j].Set_Delta(0);
            }
            for (int j = total - layers[layers.Length - 1] - 1; j >= layers[0]; j--)
                neurons[j].Compute_Delta();
            for (int j = layers[0]; j < total; j++)
                neurons[j].Compute_New_Prev_Links();
        }

        return delta_max;
    }

    public void Print()
    {
        foreach (var n in neurons)
            n.Print();
    }
}
