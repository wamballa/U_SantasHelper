using System;
using System.Collections;
using UnityEngine;

public class TruckController : MonoBehaviour
{
    public enum TruckState
    {
        IsIdle,
        IsOnGameScreen, 
        IsEntering,
        IsExiting
    }
    public TruckState currentState;


    private bool CanMove { get; set; }
    private float duration = 3f;

    //public Transform gameTransform;
    //public Transform startTransform;

    // Physics
    Rigidbody2D rb;

    // Start is called before the first frame update
    void Start()
    {
        currentState = TruckState.IsEntering;


        CanMove = false;
        rb = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {

    }


    // OnDestroy is called when the MonoBehaviour will be destroyed


    //private void OnLevelStart(object sender, EventArgs e)
    //{
    //    print("> TRUCK > OnLevelStart");
    //    StartCoroutine(MoveToPosition(gameTransform.position, 0, false));
    //}

    // Called when the level is complete
    //private void OnLevelComplete(object sender, EventArgs e)
    //{
    //    if (gameObject != null)
    //    {
    //        print("TRUCK alive");
    //    }
    //    print("> TRUCK > OnLevelComplete");

    //    StartCoroutine(MoveToPosition(startTransform.position, 1, true));
    //}

    // Coroutine to move the truck away
    //private IEnumerator MoveToPosition(Vector2 _targetPos, float _pause, bool _levelComplete)
    //{
    //    print("TRUCK: TruckMovesAway");
    //    Vector2 _startPosition = rb.position;
    //    //Vector2 targetPosition = new Vector2(rb.position.x - 6f, rb.position.y);
    //    float timer = 0f;

    //    yield return new WaitForSeconds(_pause);

    //    while (timer < duration)
    //    {
    //        timer += Time.deltaTime;
    //        float t = timer / duration;
    //        Vector2 nextPosition = Vector2.Lerp(_startPosition, _targetPos, t);
    //        rb.MovePosition(nextPosition);
    //        yield return null;
    //    }

    //    if (_levelComplete)
    //    {
    //        print("> TRUCK > > TruckMovesAway: START NEXT LEVEL");
    //        //GameManager.instance.RestartLevel();
    //    }
    //}
}
