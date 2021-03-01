using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class DoorInteractable : BaseInteractable
{
    [SerializeField]
    protected DoorInteractable travelDestination;

    protected PolygonCollider2D floorLimits;
    protected CinemachineConfiner cameraBound;

    protected Timer camFollowTimer;

    protected override void Start()
    {
        base.Start();

        camFollowTimer = TimerManager.Instance.CreateNewTimer(0.5f);
        cameraBound = FindObjectOfType<CinemachineConfiner>();
    }

    protected override void DoInteractionAction()
    {
        PlayerController.Instance.transform.position = travelDestination.transform.position;
        travelDestination.SendCameraToThisDoor();
    }

    public void SendCameraToThisDoor()
    {
        cameraBound.m_BoundingShape2D = floorLimits;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.CompareTag("CamBox"))
        {
            floorLimits = (PolygonCollider2D)collision;
        }
    }
}
