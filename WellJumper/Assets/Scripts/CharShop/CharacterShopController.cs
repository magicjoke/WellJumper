using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class CharacterShopController : MonoBehaviour
{
    
    public List<SkinsScriptableObject> skinsList;
    public int skinsNumber;
    public int currentSkinNumber;
    public SkinsScriptableObject currentSkin;
    
    // Skin price \ text
    public int coins = 0;
    public Text coinsText;
    //public int skinPrice;
    public Text skinPriceText;
    public GameObject skinPriceLabel;
    [SerializeField] private SpriteRenderer playerSkinGo;


    // buttons 
    [SerializeField] private GameObject selectBuyButton;
    [SerializeField] private Image prevButton;
    [SerializeField] private Image nextButton;



    public void Start(){

        string currentSkinName = PlayerPrefs.GetString("CurrentSkinName");
        Debug.Log(currentSkinName);
        for(int i = 0; i < skinsList.Count; i++){
            if(skinsList[i].skinName == currentSkinName){
                currentSkin = skinsList[i];
                playerSkinGo.sprite = skinsList[i].skinSprite;
                skinPriceText.text = skinsList[i].skinPrice.ToString();
                currentSkinNumber = i;
            }
        }
        skinsNumber = skinsList.Count - 1;

        coins = PlayerPrefs.GetInt("Coins");
        changePreview();
    }

    public void Update(){
        if(currentSkinNumber == 0){
            //Color currentColor = prevButton.color;
            prevButton.color = new Color(prevButton.color.r, prevButton.color.g, prevButton.color.b, 0.5f);
        } else if(currentSkinNumber == skinsNumber){
            nextButton.color = new Color(nextButton.color.r, nextButton.color.g, nextButton.color.b, 0.5f);
        } else {
            prevButton.color = new Color(prevButton.color.r, prevButton.color.g, prevButton.color.b, 1f);
            nextButton.color = new Color(nextButton.color.r, nextButton.color.g, nextButton.color.b, 1f);
        }

        if(Input.GetKeyDown(KeyCode.L)){
            PlayerPrefs.DeleteAll();
        }
    }

    public void backButton(){
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("MainMenu");
    }

    public void nextSkin(){
        if(currentSkinNumber < skinsNumber){
            currentSkinNumber++;

            changePreview();

        }
        Debug.Log("Skin Number " + currentSkinNumber);
    }

    public void prevSkin(){
        if(currentSkinNumber != 0){
            currentSkinNumber--;
            changePreview();
        }
        Debug.Log("Skin Number " + currentSkinNumber);
    }

    public void changePreview(){
        int skinUnlockCheck = PlayerPrefs.GetInt(skinsList[currentSkinNumber].skinName);
        string currSkinName = PlayerPrefs.GetString("CurrentSkinName");
        Debug.Log(currSkinName + " " + skinsList[currentSkinNumber].skinName);

        if(skinUnlockCheck == 1){
            // show price
            skinPriceLabel.SetActive(false);
            selectBuyButton.GetComponent<Text>().text = "Select";
            if(currSkinName == skinsList[currentSkinNumber].skinName){
                skinPriceLabel.SetActive(false);
                selectBuyButton.GetComponent<Text>().text = "Selected";
            }
        } else {
            // hide price
            skinPriceLabel.SetActive(true);
            selectBuyButton.GetComponent<Text>().text = "Buy";
        }

        skinPriceText.text = skinsList[currentSkinNumber].skinPrice.ToString();
        playerSkinGo.sprite = skinsList[currentSkinNumber].skinSprite;
    }

    public void selectSkin(){
        //PlayerPrefs.SetString("CurrentSkinName", "Rabbit");
        //PlayerPrefs.SetString("CurrentSkinName", skinsList[currentSkinNumber].skinName);
        // Buy
        int skinUnlockCheck = PlayerPrefs.GetInt(skinsList[currentSkinNumber].skinName);
        // Если нет скинна
        if(skinUnlockCheck == 0){
            // Проверяем хватает ли монет для скина
            if(coins >= skinsList[currentSkinNumber].skinPrice){
                // Если хватает - покупаем
                PlayerPrefs.SetInt("Coins", PlayerPrefs.GetInt("Coins") - skinsList[currentSkinNumber].skinPrice);
                updateCoins();
                PlayerPrefs.SetInt(skinsList[currentSkinNumber].skinName, 1);
            } else {
                // Если не хватает - показываем оповещение.
                Debug.Log("No money");
            }
        } else {
            //Если есть скин - применяем
            PlayerPrefs.SetString("CurrentSkinName", skinsList[currentSkinNumber].skinName);
        }
        //PlayerPrefs.SetInt(skinsList[currentSkinNumber].skinName, 1);
        changePreview();
    }

    public void updateCoins(){
        int newCoins = PlayerPrefs.GetInt("Coins");
        coinsText.text = newCoins.ToString();
    }

}
