using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class GameController : MonoBehaviour
{

    public float coins;
    public GameObject coinsText;
    public float maxHeight;
    [SerializeField] private GameObject pauseOverlay;
    [SerializeField] private GameObject gameOverOverlay;
    [SerializeField] private GameObject maxScoreText;
    public Vector3 lastBlockPos;

    public GameObject[] startSpikes;

    public State currentState;
    public enum State {
        idle,
        active
    }

    // spawn max height
    public GameObject recordGo;



    public void Start(){
        currentState = State.idle;
        pauseOverlay = GameObject.FindGameObjectWithTag("PauseOverlay");
        gameOverOverlay = GameObject.FindGameObjectWithTag("GameOverOverlay");
        deActivatePause();
        deActivateGameOver();
        
        // Max height get pref
        maxHeight = PlayerPrefs.GetFloat("MaxHeight");
        Debug.Log("Max heigth: " + maxHeight);
        maxScoreText.GetComponent<Text>().text = "Best " + maxHeight.ToString("F0");
        spawnMaxHeight();

        // Coins
        coins = PlayerPrefs.GetInt("Coins");
        Debug.Log("Max coins:" + coins);
        coinsText.GetComponent<Text>().text = coins.ToString();
    }

    public void spawnMaxHeight(){

        Instantiate(recordGo, new Vector3(0,maxHeight,0), Quaternion.identity);

    }

    public void Update(){
        if(currentState == State.active){
            for(int i = 0; i < startSpikes.Length; i++){
                startSpikes[i].GetComponent<Animator>().SetTrigger("ActivateSpikes");
            }
        }
    }

    public void changeToActiveState(){
        currentState = State.active;
    }

    public void activatePause(){ 
       pauseOverlay.SetActive(true);
       currentState = GameController.State.idle;
       Time.timeScale = 0;
    }
    public void activateGameOver(){ 
       gameOverOverlay.SetActive(true);
       currentState = GameController.State.idle;
       Time.timeScale = 0;
    }
    public void deActivatePause(){ 
        pauseOverlay.SetActive(false); 
        //currentState = GameController.State.active;
        Time.timeScale = 1;
    }
    public void deActivateGameOver(){ 
        gameOverOverlay.SetActive(false); 
        //currentState = GameController.State.active;
        Time.timeScale = 1;
    }

    public float getMaxHeigth(){
        return PlayerPrefs.GetFloat("MaxHeight");
    }
    public void saveMaxHeight(float height){
        PlayerPrefs.SetFloat("MaxHeight", height);
    }

    // Coins
    public void updateCoins(int coinAmount){
        coins += coinAmount;
        PlayerPrefs.SetInt("Coints", (int)coins);
        // Stretch coin img
        GameObject coinUI = GameObject.FindGameObjectWithTag("CoinIMG");
        //Debug.Log(coinUI);
        Sequence sequence = DOTween.Sequence()
            .Join(coinUI.transform.DOScale(new Vector3(0.7f, 1.4f), 0.1f).OnComplete(() => coinUI.transform.DOScale(new Vector3(1f, 1f), 0.1f)));
        coinsText.GetComponent<Text>().text = coins.ToString();
    }

    public void saveCoins(){
        PlayerPrefs.SetInt("Coins", (int)coins);
    }

    public void OnApplicationQuit(){
        PlayerPrefs.SetInt("Coins", (int)coins);
    }

}
