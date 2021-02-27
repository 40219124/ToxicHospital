using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitboxChooser : MonoBehaviour
{
    [SerializeField]
    private GameObject millieStand;
    [SerializeField]
    private GameObject millieJump;
    [SerializeField]
    private GameObject carsonStand;
    [SerializeField]
    private GameObject carsonCrawl;

    List<GameObject> hitboxes = new List<GameObject>();
    GameObject lastBox = null;
    // Start is called before the first frame update
    void Start()
    {
        hitboxes.Add(millieStand);
        hitboxes.Add(millieJump);
        hitboxes.Add(carsonStand);
        hitboxes.Add(carsonCrawl);

        EnableOne(millieStand);
    }

    public void ChooseHitbox(eInteractionRequirement character, ePlayerAction action)
    {
        GameObject choice = null;
        if (character == eInteractionRequirement.journalist)
        {
            if ((action & ePlayerAction.isJustMovement) != ePlayerAction.none || action == ePlayerAction.none)
            {
                choice = carsonStand;
            }
            else
            {
                choice = carsonCrawl; // ~~~ needs to explicitly link to crawling action
            }
        }
        else
        {
            if (action == ePlayerAction.jumping)
            {
                choice = millieJump;
            }
            else
            {
                choice = millieStand;
            }
        }
        EnableOne(choice);
    }

    private void EnableOne(GameObject toEnable)
    {
        if (toEnable == lastBox)
        {
            return;
        }
        foreach (GameObject item in hitboxes)
        {
            item.SetActive(toEnable.Equals(item));
        }
        lastBox = toEnable;
    }
}
