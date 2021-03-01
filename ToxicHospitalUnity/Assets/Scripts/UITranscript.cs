using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;


public class UITranscript : MonoBehaviour
{
    static UITranscript instance;
    public static UITranscript Instance { get { return instance; } }



    private UISlider popup;
    private Button closeButton;
    private TextMeshProUGUI transcriptText;


    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }

        popup = gameObject.GetComponent<UISlider>();
        transcriptText = gameObject.GetComponentInChildren<TextMeshProUGUI>();
        closeButton = gameObject.GetComponentInChildren<Button>();

        closeButton.onClick.AddListener(popup.Hide);
    }

    public void Show(AudioLore lore)
    {
        transcriptText.text = lore.transcript;
        popup.Show();
        closeButton.Select();
    }

    public bool ShowingTranscript { get { return popup.IsShowing; } }
}
