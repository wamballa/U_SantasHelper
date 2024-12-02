using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public PlayerController playerController;
    public Transform rightPlayerTransform; 
    public Transform leftPlayerTransform;

    void Start()
    {
        Initialise();
    }

    void Initialise()
    {
        rightPlayerTransform.position =
        playerController.RightTransforms[
            playerController.RightPositionCurrent].position;
        leftPlayerTransform.position =
        playerController.LeftTransforms[
            playerController.LeftPositionCurrent].position;
    }

    // Update is called once per frame
    void Update()
    {
        HandlePlayerMove();
    }

    private void HandlePlayerMove()
    {
        // right
        if (playerController.RightPositionTarget != playerController.RightPositionCurrent)
        {
            // Move
            print(">> " + playerController.RightPositionTarget);
            rightPlayerTransform.position =
                playerController.RightTransforms[
                    playerController.RightPositionTarget].position;
            playerController.RightPositionCurrent = playerController.RightPositionTarget;
        }
        if (playerController.LeftPositionTarget != playerController.LeftPositionCurrent)
        {
            // Move
            leftPlayerTransform.position = playerController.LeftTransforms[playerController.LeftPositionTarget].position;
            playerController.LeftPositionCurrent = playerController.LeftPositionTarget;
        }
    }
}
