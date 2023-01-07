using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager main;
    private void Awake()
    {
        main = this;
    }

    [SerializeField]
    private UIHealth uiPlayerHealth;

    public void RegisterPlayerHealth(EntityHealth playerHealth)
    {
        uiPlayerHealth.Initialize(playerHealth);
    }

}
