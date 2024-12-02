using UnityEngine;
using System;
using System.Collections;


public class CakeMovement : MonoBehaviour
{
    private Rigidbody2D rb;
    public Vector2 packmePos;
    public bool IsOnConveyorBelt { get; set; }
    public bool IsAtEndOfBelt { get; set; }
    public bool IsPaused { get; set; }
    public bool IsPacked { get; set; }
    private bool IsFalling { get; set; }

    //public Vector2 ConveyorDirection { get; set; }
    //public float ConveyorSpeed { get; set; }
    public int ConveyorLevel { get; set; }
    private bool CanPackTruck { get; set; }

    // Timer
    private float pauseTimer = 0;
    private float cakePauseDelay = 1f;
    // Players
    private PlayerController playerRcontroller;
    private PlayerController playerLcontroller;
    private PlayerController playerController;

    public CakeController cakeController;
    public Quaternion originalRotationValue; // declare this as a Quaternion

    void Awake()
    {
        Initiate();
    }

    private void Initiate()
    {
        rb = GetComponent<Rigidbody2D>();
        pauseTimer = cakePauseDelay;

        originalRotationValue = transform.localRotation;

        //playerRcontroller = GameObject.Find("PlayerR").GetComponent<PlayerController>();
        //playerLcontroller = GameObject.Find("PlayerL").GetComponent<PlayerController>();
        //playerController = GameObject.Find("PlayerController").GetComponent<PlayerController>();
        //if (playerController == null) Debug.LogError("ERROR: cannot find player controller");
        //playerRcontroller = playerLcontroller = playerController;
        //packmePos = GameObject.Find("5").transform.position;
    }

    private void IntitiateEventing()
    {
        //GameManager.instance.OnPauseGamePlay += OnPauseGamePlay;
        //GameManager.instance.OnResumeGamePlay += OnResumeGamePlay;
        //GameManager.instance.OnLevelComplete += OnLevelComplete;
        //GameManager.instance.OnGameOver += OnGameOver;
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        IsPaused = true;
    }

    private void OnLevelComplete(object sender, EventArgs e)
    {
        //if (transform.CompareTag("Cake")) IsPaused = true;
    }

    void Update()
    {
        HandleMovement();
        if (Input.GetKeyDown(KeyCode.Space))
        {
            IsPaused = !IsPaused;
        }
    }

    private void HandleMovement()
    {
        if (IsFalling) { return; }

        switch (cakeController.currentState)
        {
            case CakeController.CakeState.IsStarting:
                rb.gravityScale = 1.0f;
                rb.angularVelocity = 0.0f;
                //rb.linearVelocity = Vector2.zero;
                break;
            case CakeController.CakeState.IsOnConveyorBelt:
                Vector2 _conveyorDirection = cakeController.currentConveyorBelt.direction;
                float _conveyorSpeed = cakeController.currentConveyorBelt.speed;
                rb.linearVelocity = _conveyorDirection * _conveyorSpeed;
                break;
            case CakeController.CakeState.IsReadyForPickUp:
                StartCoroutine(PauseAtEndOfBelt());
                break;
            case CakeController.CakeState.IsPaused:
                rb.linearVelocity = Vector2.zero;
                break;
            case CakeController.CakeState.HasBeenPickedUp:
                cakeController.currentState = CakeController.CakeState.IsStarting;
                rb.linearVelocity = Vector2.zero;
                gameObject.transform.position = cakeController.dropOffPointTransforms[cakeController.currentDropOffNumber].position;
                transform.localRotation = originalRotationValue;
                cakeController.currentDropOffNumber++;
                break;
            case CakeController.CakeState.IsLoadingOnTruck:
                PackCake();
                break;
            case CakeController.CakeState.IsCooking:
                StartCoroutine(CookCake());
                break;


                //if (IsPaused || IsPacked )
                if (IsPaused && !IsPacked)
                {
                    rb.linearVelocity = Vector2.zero;
                    return;
                }
                //else if (IsOnConveyorBelt)
                //{
                //    rb.linearVelocity = ConveyorDirection * ConveyorSpeed;
                //}
                else if (IsAtEndOfBelt)
                {
                    //StartCoroutine(PauseAtEndOfBelt());
                    IsAtEndOfBelt = false;
                }
        }
    }

    IEnumerator CookCake()
    {
        Vector2 orginalVelocity = rb.linearVelocity;
        rb.linearVelocity = Vector2.zero;
        gameObject.GetComponent<SpriteRenderer>().enabled = false;

        yield return new WaitForSeconds(0.5f);
        gameObject.GetComponent<SpriteRenderer>().enabled = false;
        rb.linearVelocity = orginalVelocity;
    }

    IEnumerator PauseAtEndOfBelt()
    {
        rb.gravityScale = 0;
        rb.linearVelocity = Vector3.zero;
        rb.freezeRotation = true;
        //print(">Pause at end of Belt");
        yield return new WaitForSeconds(pauseTimer);

        //Falling to the floor

        if (!CanPackTruck || !IsPacked)
        {
            //print("CAKES: Falling to floor " + transform.name);
            //GameManager.instance.HandleCakeDroppedEvent();
            cakeController.currentState = CakeController.CakeState.IsFalling;
            cakeController.currentConveyorBelt = null;
            rb.freezeRotation = false;
            rb.gravityScale = 1;
        }
    }

    //Use this to trigger the packing process, adjusting as necessary to match your game's logic
    public void PackCake()
    {
        //print("PackCake called " + transform.name)/*;*/

        if (GameManager.instance.CakesPacked < 8)
        {
            cakeController.currentState = CakeController.CakeState.IsPacked;
            //print("PackCake " + transform.name);
            //IsFalling = false;
            //rb.gravityScale = 0;
            transform.tag = "PackedCake";



            //rb.position = packmePos;

            Vector2 targetSlotPosition = cakeController.truckPackingPositions[GameManager.instance.CakesPacked];
            float peakHeight = 1f;
            float duration = 1f;


            StartCoroutine(MoveCakeToPackPosition(targetSlotPosition, peakHeight, duration));
        }

    }

    private IEnumerator MoveCakeToPackPosition(Vector2 targetSlotPosition, float peakHeight, float duration)
    {
        Vector2 startPosition = rb.position;
        float elapsed = 0f;

        //while (elapsed < duration)
        //{
        //    elapsed += Time.deltaTime;
        //    float t = elapsed / duration;
        //    Vector2 nextPosition = Vector2.Lerp(startPosition, targetSlotPosition, t);
        //    nextPosition.y += peakHeight * (1 - Mathf.Pow(2 * t - 1, 2)); // Parabola for arc movement

        //    rb.MovePosition(nextPosition);
        //    yield return null;
        //}

        transform.position = targetSlotPosition;
        yield return null;

        rb.angularVelocity = 0f;
        rb.gravityScale = 0f;
        rb.linearVelocity = Vector3.zero;
        transform.localRotation = originalRotationValue;
        //transform.parent = cakeController.truckPackingPositions[GameManager.instance.CakesPacked];
        GameManager.instance.AddPackedCakes();
        //rb.isKinematic = true;
        //IsPacked = true;

    }



    //bool PlayerMovesCake(bool isRightPlayer)
    //{
    //    if (isRightPlayer)
    //    {
    //        if (ConveyorLevel == -1 && playerRcontroller.RightPositionCurrent == 0)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[0].position;
    //            rb.gravityScale = 1;
    //            IsAtEndOfBelt = false;
    //            return true;
    //        }
    //        if (ConveyorLevel == playerRcontroller.RightPositionCurrent)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[ConveyorLevel + 1].position;
    //            IsAtEndOfBelt = false;
    //            rb.gravityScale = 1;
    //            return true;
    //        }
    //        if (ConveyorLevel == 3 && playerRcontroller.RightPositionCurrent == 2)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[ConveyorLevel + 1].position;
    //            IsAtEndOfBelt = false;
    //            rb.gravityScale = 1;
    //            return true;
    //        }
    //    }
    //    else
    //    {
    //        if (ConveyorLevel == playerLcontroller.LeftPositionCurrent)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[ConveyorLevel + 1].position;
    //            rb.gravityScale = 1;
    //            IsAtEndOfBelt = false;
    //            return true;
    //        }
    //        if (ConveyorLevel == 2 && playerLcontroller.LeftPositionCurrent == 1)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[ConveyorLevel + 1].position;
    //            rb.gravityScale = 1;
    //            IsAtEndOfBelt = false;
    //            return true;
    //        }
    //        if (ConveyorLevel == 4 && playerLcontroller.LeftPositionCurrent == 2)
    //        {
    //            transform.position = GameAssets.instance.wayPoints[ConveyorLevel + 1].position;
    //            rb.gravityScale = 1;
    //            IsAtEndOfBelt = false;
    //            //CanPackTruck = true;
    //            PackCake();

    //            return true;
    //        }
    //    }
    //    return false;
    //}

    private void OnResumeGamePlay(object sender, EventArgs e)
    {
        IsPaused = false;
    }

    private void OnPauseGamePlay(object sender, EventArgs e)
    {
        IsPaused = true;
    }

}// class end


