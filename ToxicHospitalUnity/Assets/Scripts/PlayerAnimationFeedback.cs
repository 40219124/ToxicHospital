using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationFeedback : MonoBehaviour
{
    private PlayerController player;

    private void Start()
    {
        player = GetComponentInParent<PlayerController>();
    }
    public void FanimSwapCharacter()
    {
        player.FanimSwapCharacter();
    }
}
