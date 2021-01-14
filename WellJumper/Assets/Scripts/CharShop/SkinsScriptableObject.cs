using System.Collections;
using System.Collections.Generic;
using UnityEngine;
[CreateAssetMenu(fileName = "SkinsScriptableObject", menuName = "SkinsScriptableObject/SkinObject")]
public class SkinsScriptableObject : ScriptableObject
{
    public int id;
    public string skinName;
    public Sprite skinSprite;
    public RuntimeAnimatorController skinAnim;
    public int skinPrice;
}
