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
            //bool movingRight = (cakeMovement.ConveyorDirection.x > 0);
            //StartCoroutine(cakeController.WaitAndChangeSprite(collision.transform.position, movingRight));
            print("> Cake Collision > OnTriggerEnter2D >  Cake Cooking");
            if (Mathf.Abs(transform.localPosition.x - collision.transform.localPosition.x) < 0.5f)
            {
                print("> ISCOOKING");
                cakeController.currentState = CakeController.CakeState.IsCooking;
            }

        }
    }
}
