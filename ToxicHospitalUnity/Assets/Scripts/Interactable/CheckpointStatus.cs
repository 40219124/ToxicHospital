using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointStatus
{
    public static bool Initialised;


    //store key player data
    public static float PlayerInfectionLevel;
    public static float PlayerHealthPercentage;

    //store the loaction, scale, and rotation at time of checkpoint
    public static Transform PlayerTransform;


    //store inventory contents
    public static List<AudioLore> Recordings = new List<AudioLore>();
    public static List<VisualLore> Letters = new List<VisualLore>();
    public static List<VisualLore> Reports = new List<VisualLore>();


    // get a reference to all interactables and their transforms(for the resetting movable ones) and their active/inactive bool.
    public static BaseInteractable[] AllInteractables;
    public static List<Transform> AllInteractablesTransforms = new List<Transform>();

    public static List<bool> ActivyStatuses = new List<bool>();

    // Assume will need a list of enemy health, transforms, and whether or not they are active gameobjects 
    //public static List<Enemy> Enemies = new List<Enemy>();
    //public static List<EnemyStatus> MoveableInteractableTransforms = new List<Transform>();
}

