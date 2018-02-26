using System;
using System.Collections;
using System.Linq;
using System.Collections.Generic;
using UnityEngine;

public class RaceManager : MonoBehaviour
{
    private class SortNetwork
    {
        float v;
        Network net;
        int gen;

        public SortNetwork(float value, Network net, int gen)
        {
            this.v = value;
            this.net = net;
            this.gen = gen;
        }

        public int Get_Gen()
        {
            return gen;
        }

        public Network Get_Network()
        {
            return net;
        }

        public float Get_Value()
        {
            return v;
        }
        public void Set_Value(float value)
        {
            v = value;
        }

        public static bool operator <(SortNetwork n1, SortNetwork n2)
        {
            return n1.v < n2.v;
        }
        public static bool operator >(SortNetwork n1, SortNetwork n2)
        {
            return n1.v > n2.v;
        }
        public static int operator -(SortNetwork n1, SortNetwork n2)
        {
            return n1.v < n2.v ? 1 : -1;
        }
    }

    static int generation = 0;
    static RaceManager instance;
    public const float Time_Cycle = 100f; //seconds
    public const float D_LEADERBOARD = 1f;
    public const int NB_LEADERBOARD = 4;

    List<SortNetwork> ia_s = new List<SortNetwork>();
    float remainingTime = Time_Cycle;

    void Start()
    {
        instance = this;
    }

    void Update()
    {
        remainingTime -= Time.deltaTime;
        if (remainingTime <= 0)
        {
            new_cycle();
            generation++;
            remainingTime = Time_Cycle;
        }
    }

    void new_cycle()
    {
        var new_list = new List<SortNetwork>();
        foreach (var n in ia_s)
        {
            new_list.Add(n);
            new_list[new_list.Count - 1].Set_Value(new_list[new_list.Count - 1].Get_Value() * D_LEADERBOARD);
        }
        var all = FindObjectsOfType<I_IA>();
        foreach (var car in all)
        {
            new_list.Add(new SortNetwork(car.Get_Efficiency(), car.Get_Network(), generation));
            car.Reset();
        }
        new_list = new_list.OrderBy(item => -item.Get_Value()).ToList();

        if (ia_s.Count > NB_LEADERBOARD)
            new_list.RemoveRange(new_list.Count - all.Length, all.Length);

        foreach (var car in all)
            car.Set_Network(new Network(new_list[0].Get_Network(),
                                        new_list[UnityEngine.Random.Range(0, new_list.Count / 4)].Get_Network()));

        print("--Leader_board--");
        for (int i = new_list.Count - 1; i >= 0; --i)
            print("Generation : " + new_list[i].Get_Gen() + " = " + new_list[i].Get_Value());
        print("----------------");

        ia_s = new_list;
    }

    public static void Net_Turn()
    {
        instance.remainingTime = 0;
    }

    public static float Get_remainging_time()
    {
        return instance.remainingTime;
    }
}
