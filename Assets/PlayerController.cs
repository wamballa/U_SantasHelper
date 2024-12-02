using NUnit.Framework;
using System;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public List<Transform> RightTransforms = new List<Transform>();
    public int NumOfRightPositions { get; private set; }
    public int RightPositionCurrent { get; set; }
    public int RightPositionTarget { get; set; }

    public List<Transform> LeftTransforms = new List<Transform>();
    public int NumOfLeftPositions { get; private set; }
    public int LeftPositionCurrent { get; set; }
    public int LeftPositionTarget { get; set; }

    void Awake()
    {
        InitaliseRightPlayer();
        InitaliseLeftPlayer();
    }



    // Update is called once per frame
    void Update()
    {
        
    }

    private void InitaliseLeftPlayer()
    {
        LeftPositionCurrent = 0;
        // Find objects with the tag and add their transforms to the list
        GameObject[] _positions = GameObject.FindGameObjectsWithTag("PlayerLeftPositions");
        foreach (GameObject position in _positions)
        {
            LeftTransforms.Add(position.transform);
        }

        // Sort the list based on vertical screen position (lowest first)
        Camera mainCamera = Camera.main;
        LeftTransforms.Sort((a, b) =>
        {
            float aScreenY = mainCamera.WorldToScreenPoint(a.position).y;
            float bScreenY = mainCamera.WorldToScreenPoint(b.position).y;
            return aScreenY.CompareTo(bScreenY); // Ascending order
        });

        NumOfLeftPositions = LeftTransforms.Count - 1;

    }

    private void InitaliseRightPlayer()
    {



        RightPositionCurrent = 0;
        // Find objects with the tag and add their transforms to the list
        GameObject[] _positions = GameObject.FindGameObjectsWithTag("PlayerRightPositions");
        foreach (GameObject position in _positions)
        {
            RightTransforms.Add(position.transform);
        }

        // Sort the list based on vertical screen position (lowest first)
        Camera mainCamera = Camera.main;
        RightTransforms.Sort((a, b) =>
        {
            float aScreenY = mainCamera.WorldToScreenPoint(a.position).y;
            float bScreenY = mainCamera.WorldToScreenPoint(b.position).y;
            return aScreenY.CompareTo(bScreenY); // Ascending order
        });

        NumOfRightPositions = RightTransforms.Count - 1;

    }
}
