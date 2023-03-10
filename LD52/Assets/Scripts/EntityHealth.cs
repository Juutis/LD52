using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public static EntityHealth player;
    void Awake()
    {
        if (tag == "Player")
        {
            player = this;
        }
    }
    public int Max { get; private set; }
    public int Current { get; private set; }
    public bool Depleted { get { return Current <= 0; } }
    public float Percentage { get { return Current / Max; } }

    void Start()
    {
        if (tag == "Player")
        {
            Initialize();
        }
    }


    public void Initialize()
    {
        Max = GameManager.main.Difficulty.Health;
        Current = Max;
        UIManager.main.RegisterPlayerHealth(this);
    }
    public void Modify(int change)
    {
        if (tag == "Player" && change < 0)
        {
            UIManager.main.ShowBloodEffect();
        }
        Current += change;
        if (Current <= 0)
        {
            Current = 0;
            if (tag == "Player")
            {
                Debug.Log("<color=red>YOU ARE DEAD!!!!</color>");
                GameManager.main.GameOver();
            }
        }
        if (Current > Max)
        {
            Current = Max;
        }
    }
}
