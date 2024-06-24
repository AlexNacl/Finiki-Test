using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 

public class GameManager : MonoBehaviour
{
    public static GameManager current;
    [SerializeField]
    public Text counter;
    [SerializeField]
    public UIController UIController;
    [SerializeField]
    public GameObject RoadGenerator;
    public bool gameEnded { get; private set; }
    private int coins = 0;

    private void Awake() 
    {
        current = this;
    }

    private void Start() 
    {
        GameEvents.current.onEndGame += EndGame;
        GameEvents.current.onCoinCollect += CoinCollect;

        RoadGenerator.GetComponent<RoadGenerator>().StartLevel();
    }

    private void EndGame() {
        gameEnded = true;
    }

    private void CoinCollect() {
        coins++;
        UIController.CoinCollect(coins);
    }

    private void OnDestroy() {
        GameEvents.current.onEndGame -= EndGame;
        GameEvents.current.onCoinCollect -= CoinCollect;
    }

    private void Update() {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Application.Quit();
        }
    }
}
