using System;
using UnityEngine;

public class TruckEvents : MonoBehaviour
{

    public TruckController truckController;

    void Start()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnLevelComplete += OnLevelComplete;
            GameManager.instance.OnLevelStart += OnLevelStart;
        }
        //truckController = GetComponent<TruckController>();
        if (truckController == null) Debug.LogError("ERROR: cannot find TruckController");
    }

    private void OnLevelStart(object sender, EventArgs e)
    {
        //print("> TRUCK > OnLevelStart");
        truckController.currentState = TruckController.TruckState.CanEnter;
    }

    // Called when the level is complete
    private void OnLevelComplete(object sender, EventArgs e)
    {
        //if (gameObject != null)
        //{
        //    print("TRUCK alive");
        //}
        //print("> TRUCK > OnLevelComplete");
        truckController.currentState = TruckController.TruckState.IsExiting;
    }

    void OnDestroy()
    {
        if (GameManager.instance != null)
        {
            GameManager.instance.OnLevelComplete -= OnLevelComplete;
        }
    }

}
