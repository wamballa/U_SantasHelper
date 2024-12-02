using Unity.VisualScripting;
using UnityEngine;

public class PlayerKeyboardInput : MonoBehaviour
{
    public PlayerController playerController;

    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        HandleKeyboardInput();
    }

    private void HandleKeyboardInput()
    {
        //if (!canMove) { return; }

        //if (!playerMovement.CanAcceptInput) return;

        if (Input.GetKeyUp(KeyCode.P))
        {
            if (playerController.RightPositionCurrent < playerController.NumOfRightPositions)
            {
                playerController.RightPositionTarget++;
            }
        }
        if (Input.GetKeyUp(KeyCode.L))
        {
            if (playerController.RightPositionCurrent > 0)
            {
                playerController.RightPositionTarget--;
            }
        }
        if (Input.GetKeyUp(KeyCode.Q))
        {
            if (playerController.LeftPositionTarget < playerController.NumOfLeftPositions)
            {
                playerController.LeftPositionTarget++;
            }
        }
        if (Input.GetKeyUp(KeyCode.A))
        {
            if (playerController.LeftPositionCurrent > 0)
            {
                playerController.LeftPositionTarget--;
            }
        }

    }

}
