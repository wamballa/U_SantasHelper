using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CakeCollisionHandler : MonoBehaviour
{

    CakeMovement cakeMovement;
    CakeController cakeController;

    private void Start()
    {
        cakeMovement = GetComponent<CakeMovement>();
        cakeController = GetComponent<CakeController>();
    }


    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ConveyorBelt")
            && cakeController.currentState != CakeController.CakeState.IsReadyForPickUp
            && cakeController.currentState != CakeController.CakeState.IsFalling
            && cakeController.currentState != CakeController.CakeState.IsLoadingOnTruck
            && cakeController.currentState != CakeController.CakeState.IsPacked)
        {
            cakeController.currentState = CakeController.CakeState.IsOnConveyorBelt;
            cakeController.currentConveyorBelt = collision.transform.GetComponent<ConveyorBelt>();
            //print(">> Is final belt? " + cakeController.currentConveyorBelt.isFinalConveyorBelt);

        }

        if (collision.transform.CompareTag("Floor")
            && cakeController.currentState == CakeController.CakeState.IsFalling)
        {
            cakeController.currentState = CakeController.CakeState.IsOnFloor;
        }

    }

    private void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.transform.CompareTag("ConveyorBelt")
            && cakeController.currentState != CakeController.CakeState.IsStarting
            && cakeController.currentState != CakeController.CakeState.IsFalling
            && cakeController.currentState != CakeController.CakeState.IsLoadingOnTruck
            && cakeController.currentState != CakeController.CakeState.IsPacked)
        {
            //print(">>>>>>>>>>> Exit conveyor belt");
            cakeController.currentState = CakeController.CakeState.IsReadyForPickUp;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {

        if (collision.transform.CompareTag("Player"))
        {
            if (cakeController.currentState != CakeController.CakeState.IsFalling
                && cakeController.currentState != CakeController.CakeState.IsPacked)
            {
                if (cakeController.currentConveyorBelt.isFinalConveyorBelt)
                {
                    print("IsLoadingOnTruck");
                    cakeController.currentState = CakeController.CakeState.IsLoadingOnTruck;


                }
                else
                {
                    print("HasBeenPickedUp");
                    cakeController.currentState = CakeController.CakeState.HasBeenPickedUp;
                }
            }
        }

        if (collision.transform.CompareTag("Oven"))
        {
            //HandleOvenCollision(collision.transform);
            cakeMovement.currentOven = collision.transform;
            cakeController.currentState = CakeController.CakeState.IsMovingIntoOven;

        }
    }

    //    private void HandleOvenCollision(Transform ovenTransform)
    //    {
    //        print("> Cake Collision > OnTriggerEnter2D > Cake Moving Toward Oven");
    //        StartCoroutine(MoveTowardsOven(ovenTransform));
    //    }

    //    private IEnumerator MoveTowardsOven(Transform ovenTransform)
    //    {
    //        float thresholdDistance = 0.5f; // Distance at which the cake is "close enough" to the oven
    //        float speed = cakeMovement.current; // Assuming ConveyorSpeed exists in CakeMovement

    //        while (Vector2.Distance(transform.position, ovenTransform.position) > thresholdDistance)
    //        {
    //            // Move the cake closer to the oven
    //            transform.position = Vector2.MoveTowards(transform.position, ovenTransform.position, speed * Time.deltaTime);
    //            yield return null;
    //        }

    //        // Once the cake is close enough, update its state
    //        print("> Cake Collision > Cake Reached Oven > ISCOOKING");
    //        cakeController.currentState = CakeController.CakeState.IsCooking;
    //    }

    //}
}
