using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI; 
using DG.Tweening;

public class UIController : MonoBehaviour
{
    public static UIController current;
    [SerializeField]
    public GameObject coinModel;
    [SerializeField]
    public Text coinText;
    [SerializeField]
    public float coinRotationSpeed = 5f;

    [SerializeField]
    public GameObject gameEndText;
    [SerializeField]
    public Image FaderImg;

    private void Awake() 
    {
        current = this;
    }

    private void Start() {
        GameEvents.current.onEndGame += EndGame;
        FaderImg.DOFade(0, 2f);
        DOTween.Init(false, true, LogBehaviour.ErrorsOnly);
        Vector3 vec = new Vector3(0.0f, 0.0f, 180.0f);
        coinModel.transform.DOLocalRotate(vec, 2).SetEase(Ease.InOutSine).SetLoops(-1, LoopType.Yoyo);
    }

    private void OnDestroy() {
        GameEvents.current.onEndGame -= EndGame;
    }

    private void EndGame() {
        FaderImg.DOFade(1, 2f);
        gameEndText.SetActive(true);
    }

    public void CoinCollect(int coins) {
        coinText.text = "Coins: " + coins;
    }

    void Update() {
        coinModel.transform.Rotate(0, 0, 0);
    }
}
