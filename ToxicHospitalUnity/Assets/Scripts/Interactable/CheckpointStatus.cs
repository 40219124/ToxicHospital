using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class CheckpointStatus
{
    //store key player data
    public static float PlayerInfectionLevel;
    public static float PlayerHealthPercentage;

    //store inventory contents
    public static List<AudioLore> Recordings = new List<AudioLore>();
    public static List<VisualLore> Letters = new List<VisualLore>();
    public static List<VisualLore> Reports = new List<VisualLore>();


    //store the loaction, scale, and rotation at time of checkpoint
    public static Transform Transform;

    // store transforms of all Movable interactables and enemies currently in the scene
    public static List<Transform> MoveableInteractableTransforms = new List<Transform>();

    // Assume will need a list of enemy health, transforms, and whether or not they are active gameobjects 
    //public static List<Enemy> Enemies = new List<Enemy>();
    //public static List<EnemyStatus> MoveableInteractableTransforms = new List<Transform>();
}

