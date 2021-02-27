using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraColliderCollection : MonoBehaviour
{
    private static CameraColliderCollection instance;
    public static CameraColliderCollection Instance
    {
        get { return instance; }
    }

    public Collider2D BottomFloor;
    public Collider2D MiddleFloor;
    public Collider2D TopFloor;

    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
        }
    }
}
