using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "NewVisualLore", menuName = "ScriptableObjects/VisualLore", order = 1)]
public class VisualLore : LoreItem
{
    [TextArea(3, 10)]
    public string objectText;
}
