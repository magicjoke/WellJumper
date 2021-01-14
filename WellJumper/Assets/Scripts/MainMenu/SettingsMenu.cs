using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SettingsMenu : MonoBehaviour
{
    [Header ("space betwe menu items")]
    [SerializeField] Vector2 spacing;

    [Space]
    [Header("Main Button Rotation")]
    [SerializeField] float rotationDuration;
    [SerializeField] Ease rotationEase;

    [Space]
    [Header("Animation")]
    [SerializeField] float expandDuration;
    [SerializeField] float collapseDuration;
    [SerializeField] Ease expandedEase;
    [SerializeField] Ease collapseEase;

    [Space]
    [Header("Fading")]
    [SerializeField] float expandFadeDuration;
    [SerializeField] float collapseFadeDuration;


    Button mainButton;
    SettingsMenuItem[] menuItems;
    bool isExpanded = false;

    Vector2 mainButtonPosition;
    int itemsCount;

    public void Start(){
        itemsCount = transform.childCount -1;
        menuItems = new SettingsMenuItem[itemsCount];
        for (int i = 0; i< itemsCount; i++){
            menuItems [i] = transform.GetChild(i+1).GetComponent<SettingsMenuItem>();

        }

        mainButton = transform.GetChild(0).GetComponent<Button>();
        mainButton.onClick.AddListener(ToggleMenu);
        mainButton.transform.SetAsLastSibling();

        mainButtonPosition = mainButton.transform.position;

        ResetPositions();
    }

    void ResetPositions(){
        for(int i =0; i< itemsCount; i++){
            menuItems [i].trans.position = mainButtonPosition;
        }
    }

    void ToggleMenu(){
        isExpanded = !isExpanded;

        if(isExpanded){
            for(int i =0; i< itemsCount; i++){
                menuItems [i].trans.DOMove(mainButtonPosition + spacing * (i + 1), expandDuration).SetEase(expandedEase);
                menuItems [i].img.DOFade(1f, expandFadeDuration).From(0f);
            }
        } else {
            for(int i =0; i< itemsCount; i++){
                menuItems [i].trans.DOMove(mainButtonPosition, collapseDuration).SetEase(collapseEase);
                menuItems [i].img.DOFade(0f, collapseFadeDuration);
            }
        }

        //rotate button
        mainButton.transform
            .DORotate(Vector3.forward*360f,rotationDuration)
            .From(Vector3.zero)
            .SetEase(rotationEase);
    }

    public void OnItemClick(int index){
        switch(index){
            case 0:
                Debug.Log("Music");
                changeSprite(index);
                break;
            case 1:
                Debug.Log("Sound");
                changeSprite(index);
                break;
            case 2:
                Debug.Log("Vibration");
                changeSprite(index);
                break;
        }
    }

    public void changeSprite(int index){
        Image currImg = menuItems[index].GetComponent<Image>();
        Sprite onSprite = menuItems[index].GetComponent<SettingsMenuItem>().onImg;
        Sprite offSprite = menuItems[index].GetComponent<SettingsMenuItem>().offImg;

        if(currImg.sprite == onSprite){
            currImg.sprite = offSprite;
        } else {
            currImg.sprite = onSprite;
        }
         Sequence sequence = DOTween.Sequence()
                .Join(currImg.transform.DOScale(new Vector3(0.8f, 1.2f), 0.1f).OnComplete(() => currImg.transform.DOScale(new Vector3(1f, 1f), 0.1f)));
    }

    void OnDestroy(){
          mainButton.onClick.RemoveListener(ToggleMenu);
    }
}
