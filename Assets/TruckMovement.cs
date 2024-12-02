using System;
using UnityEngine;
using System.Collections;

public class TruckMovement : MonoBehaviour
{
    public TruckController truckController;
    private Rigidbody2D rigidbody;

    public Transform startPosition;
    public Transform endPosition;

    private float duration = 3f;

    void Start()
    {
        Initiate();
    }

    private void Initiate()
    {
        rigidbody = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        CheckControllerState();
    }

    private void CheckControllerState()
    {
        switch (truckController.currentState)
        {
            case TruckController.TruckState.IsEntering:
                StartCoroutine(MoveToPosition(endPosition.position, 0, false));
                break;
            case TruckController.TruckState.IsExiting:
                break;
        }
    }

    private IEnumerator MoveToPosition(Vector2 _targetPos, float _pause, bool _levelComplete)
    {

        Vector2 _startPosition = rigidbody.position;
        //print("TRUCK: TruckMovesAway " + _targetPos);
        //Vector2 targetPosition = new Vector2(rb.position.x - 6f, rb.position.y);
        float timer = 0f;

        yield return new WaitForSeconds(_pause);

        while (timer < duration)
        {
            timer += Time.deltaTime;
            float t = timer / duration;

            // Use SmoothStep for built-in ease-in-out interpolation
            float smoothT = Mathf.SmoothStep(0, 1, t);

            Vector2 nextPosition = Vector2.Lerp(_startPosition, _targetPos, smoothT);
            rigidbody.MovePosition(nextPosition);

            yield return null;
        }

        if (_levelComplete)
        {
            print("> TRUCK > > TruckMovesAway: START NEXT LEVEL");
            truckController.currentState = TruckController.TruckState.IsOnGameScreen;
            //GameManager.instance.RestartLevel();
        }
    }


}
