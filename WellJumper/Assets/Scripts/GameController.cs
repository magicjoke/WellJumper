using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{

    public float coins;
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
        Debug.Log(maxHeight);
        maxScoreText.GetComponent<Text>().text = "Best " + maxHeight.ToString("F0");
        spawnMaxHeight();
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
       Time.timeScale = 0;
    }
    public void activateGameOver(){ 
       gameOverOverlay.SetActive(true);
       Time.timeScale = 0;
    }
    public void deActivatePause(){ 
        pauseOverlay.SetActive(false); 
        Time.timeScale = 1;
    }
    public void deActivateGameOver(){ 
        gameOverOverlay.SetActive(false); 
        Time.timeScale = 1;
    }


    public void saveMaxHeight(float height){

        PlayerPrefs.SetFloat("MaxHeight", height);

    }

}
