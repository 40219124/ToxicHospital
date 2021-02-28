using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class PageController : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI textBox;

    void Start()
    {
        textBox.pageToDisplay = 1;
    }

    public void NextPage()
    {
        textBox.pageToDisplay += 1;
    }

    public void PreviousPage()
    {
        textBox.pageToDisplay -= 1;
    }
}
