using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EntityHealth : MonoBehaviour
{
    public int Max { get; private set; }
    public int Current { get; private set; }
    public bool Depleted { get { return Current <= 0; } }
    public float Percentage { get { return Current / Max; } }

    void Start()
    {
        if (tag == "Player")
        {
            Initialize(GameManager.main.Difficulty.Health);
            UIManager.main.RegisterPlayerHealth(this);
        }
    }

    public void Initialize(int max)
    {
        Max = max;
        Current = max;
    }
    public void Modify(int change)
    {
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
