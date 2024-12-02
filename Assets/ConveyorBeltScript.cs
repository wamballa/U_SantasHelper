using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ConveyorBelt : MonoBehaviour
{
    public Vector2 direction;
    public float speed;
    //public int levelPos;
    public bool isFinalConveyorBelt;

    //public int GetLevelPos()
    //{
    //    return levelPos;
    //}

    public Vector2 GetDirection()
    {
        return direction;
    }

    public float GetSpeed()
    {
        return speed;
    }

}
