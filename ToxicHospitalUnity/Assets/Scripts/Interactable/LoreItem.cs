using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public abstract class LoreItem : ScriptableObject
{
    public string objectName;
    public Sprite objectSprite;
    public LoreType classification;
}

public enum LoreType
{
    AudioLog,
    Letter,
    Report

}
