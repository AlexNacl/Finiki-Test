using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameEvents : MonoBehaviour
{
    public static GameEvents current;

    private void Awake() 
    {
        current = this;
    }

    public event Action onCoinCollect;
    public void CoinCollect()
    {
        if (onCoinCollect != null)
        {
            onCoinCollect();
        }
    }

    public event Action onEndGame;
    public void EndGame()
    {
        if (onEndGame != null)
        {
            onEndGame();
        }
    }
}
