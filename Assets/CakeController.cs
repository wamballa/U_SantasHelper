using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;
using TMPro;

public class CakeController : MonoBehaviour
{
    public enum CakeState
    {
        IsStarting,
        IsPaused,
        IsOnConveyorBelt,
        //IsOffConveyorBelt,
        IsReadyForPickUp,
        //IsReadyToLoadOnTruck,
        HasBeenPickedUp,
        IsInOven,
        IsLoadingOnTruck,
        IsFalling,
        IsOnFloor,
        IsPacked,
        IsCooking
    }
    public CakeState currentState;

    public ConveyorBelt currentConveyorBelt;

    public List<Transform> dropOffPointTransforms = new List<Transform>();
    public List<Vector3> truckPackingPositions = new List<Vector3>();   

    public int currentDropOffNumber = 0;

    public TMP_Text debugText;

    // Bools
    public bool isOnConveryorBelt;
    public bool isAtEndOfBelt;

    public bool IsOverlappingMouse { get; set; }

    SpriteRenderer spriteRenderer;

    //private GameManager gameManager;

    // Conveyor belt
    public int conveyorLevel;

    // Cake sprites
    SpriteRenderer[] cakeSprites;
    private int curretCakeID;

    bool isPacked;
    private bool isPaused;

    void Start()
    {
        InititiateEventing();
        Inititiate();
        InititiatePackingPositions();
    }



    void Update()
    {
        debugText.text = currentState.ToString();
        //CheckIfLastPlatform();
    }

    public IEnumerator WaitAndChangeSprite(Vector3 ovenPosition, bool movingRight)
    {
        // Wait until the cake is completely behind the oven
        if (movingRight)
        {
            while (transform.position.x < ovenPosition.x)
            {
                yield return null;
            }
        }
        else
        {
            while (transform.position.x > ovenPosition.x)
            {
                yield return null;
            }
        }
        //yield return new WaitForSeconds(0.5f);
        ShowCakeSprite(++curretCakeID);
    }

    private void ShowCakeSprite(int v)
    {
        //print("Show cake number " + v);
        for (int i = 0; i < 6; i++)
        {
            if (i == v)
            {
                cakeSprites[i].enabled = true;
            }
            else cakeSprites[i].enabled = false;
        }
    }

    private void Inititiate()
    {
        currentState = CakeState.IsStarting;

        GameObject[] _positions = GameObject.FindGameObjectsWithTag("DropOffPoint");
        foreach (GameObject _position in _positions)
        {
            dropOffPointTransforms.Add(_position.transform);
        }

        // Sort the list based on vertical screen position (lowest first)
        Camera mainCamera = Camera.main;
        dropOffPointTransforms.Sort((a, b) =>
        {
            float aScreenY = mainCamera.WorldToScreenPoint(a.position).y;
            float bScreenY = mainCamera.WorldToScreenPoint(b.position).y;
            return aScreenY.CompareTo(bScreenY); // Ascending order
        });

        //gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
        //spriteRenderer = transform.GetComponentInChildren<SpriteRenderer>();

        //cakeSprites = transform.GetComponentsInChildren<SpriteRenderer>();
        //ShowCakeSprite(0);


    }

    private void InititiateEventing()
    {
        //GameManager.instance.OnPauseGamePlay += OnPauseGamePlay;
        //GameManager.instance.OnResumeGamePlay += OnResumeGamePlay;
        //GameManager.instance.OnLevelComplete += OnLevelComplete;
        //GameManager.instance.OnGameOver += OnGameOver;
    }

    private void InititiatePackingPositions()
    {
        GameObject[] _positions = GameObject.FindGameObjectsWithTag("PackingPosition");

        // Sort positions by GameObject name
        Array.Sort(_positions, (a, b) => string.Compare(a.name, b.name, StringComparison.Ordinal));


        foreach (GameObject _position in _positions)
        {
            truckPackingPositions.Add(_position.transform.position);
        }
    }

    private void OnGameOver(object sender, EventArgs e)
    {
        isPaused = true;
    }

    private void OnLevelComplete(object sender, EventArgs e)
    {
        //Debug.Log("CAKE::: OnLevelComplete " + transform.name);
        //if (transform == null) return;

        isPaused = true;
        //if (transform.CompareTag("Cake")) 
        //{
        //    Debug.Log("Destroy names / tag = " + transform.name+" / "+transform.tag);
        //    Destroy(gameObject);
        //}
    }

    private void OnResumeGamePlay(object sender, EventArgs e)
    {
        isPaused = false;
    }

    private void OnPauseGamePlay(object sender, EventArgs e)
    {
        isPaused = true;
    }

}
