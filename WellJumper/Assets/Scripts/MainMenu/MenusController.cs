using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MenusController : MonoBehaviour
{
    // Start is called before the first frame update
    [SerializeField] private GameObject gameController;
    [SerializeField] private GameObject player;

    public void Start(){
        gameController = GameObject.FindGameObjectWithTag("GameController");
        player = GameObject.FindGameObjectWithTag("Player");
    }

    public void StartGame(){
        player.GetComponent<PlayerController>().changeToActiveState();
        GameObject gameStartOverlay = GameObject.FindGameObjectWithTag("GameStartOverlay");
        gameStartOverlay.SetActive(false);
    }

    public void restartGame(){
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(scene.name);
        Debug.Log("Restart Game");
    }

    public void backToMenu(){
        Time.timeScale = 1;
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainMenu");
    }

    public void gameStart(){
        Time.timeScale = 1;
        Debug.Log("GameStart");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("TestScene");
    }

    public void continueGame(){
        Time.timeScale = 1;
        gameController.GetComponent<GameController>().deActivatePause();
    }
    public void continueGameAds(){
        Time.timeScale = 1;
        player.transform.rotation = player.GetComponent<PlayerController>().startPos;
        player.transform.position = gameController.GetComponent<GameController>().lastBlockPos;
        player.transform.position = new Vector3(player.transform.position.x, player.transform.position.y +2f, player.transform.position.z);
        player.GetComponent<SpriteRenderer>().enabled = true;

        gameController.GetComponent<GameController>().deActivateGameOver();
        Debug.Log("Continue Game");
    }
}
