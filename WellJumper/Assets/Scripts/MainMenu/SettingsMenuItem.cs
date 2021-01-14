using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SettingsMenuItem : MonoBehaviour
{
    [HideInInspector] public Image img;
    [HideInInspector] public Transform trans;

    public Sprite onImg;
    public Sprite offImg;
    
    SettingsMenu settingsMenu;
    Button button;
    int index;
    void Awake()
    {
        img = GetComponent<Image>();
        trans = transform;

        settingsMenu = trans.parent.GetComponent<SettingsMenu>();
        index = trans.GetSiblingIndex() -1;

        button = GetComponent<Button>();
        button.onClick.AddListener(OnItemClick);
    }

    void OnItemClick(){
        settingsMenu.OnItemClick(index);
    }
    void OnDestroy(){
        button.onClick.RemoveListener(OnItemClick);
    }

}
