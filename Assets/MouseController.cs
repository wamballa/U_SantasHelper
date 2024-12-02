using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem.LowLevel;

public class MouseController : MonoBehaviour
{
    public enum MouseState {
        Idle,
        HasEatenCake,
        IsChasingCake,
        IsReturning
    }
    public MouseState currentState;


    Vector3 startPos;

    bool isGettingCake;
    bool isReturningToStart;

    Rigidbody2D rb;
    float speed = 15f;


    public GameObject target;

    private bool isActive = true;

    void Start()
    {
        currentState = MouseState.Idle;
        startPos = transform.position;
        rb = GetComponent<Rigidbody2D>();
        GameManager.instance.OnLevelComplete += OnLevelComplete;

    }

    private void OnLevelComplete(object sender, EventArgs e)
    {
        isActive = false;
    }

    // Update is called once per frame
    void Update()
    {
        HandleMouseMovement();
    }

    void HandleMouseMovement()
    {
        if (isActive)
        {
            CheckForCakeToEat();
            ApplyMovement();
            CheckIfNearCake();
            CheckIfHome();
        }

    }

    void CheckForCakeToEat()
    {
        GameObject[] cakes = GameObject.FindGameObjectsWithTag("Cake");

        if (currentState == MouseState.Idle && cakes.Length >0)
        {
            foreach (GameObject cake in cakes)
            {
                if (cake.GetComponent<CakeController>().currentState == CakeController.CakeState.IsOnFloor)
                {
                    target = cake;
                    currentState = MouseState.IsChasingCake;
                }
            }
        }
    }

    void ApplyMovement()
    {
        if (currentState == MouseState.IsChasingCake)
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, target.transform.position, step);
        }
        if (currentState == MouseState.IsReturning )
        {
            float step = speed * Time.deltaTime;
            transform.position = Vector2.MoveTowards(transform.position, startPos, step);
        }
    }

    void CheckIfNearCake()
    {
        if (currentState == MouseState.IsChasingCake)
        {
            float deltaX = transform.position.x - target.transform.position.x;
            if (Mathf.Abs(deltaX) <= 0.5f)
            {
                EatCake();
            }
        }

    }
    private void EatCake()
    {
        if (currentState == MouseState.IsChasingCake)
        {
            // Debug.Log("EatCake");
            currentState = MouseState.IsReturning;
            //GameObject _explosion = Instantiate(GameAssets.instance.cakeExplosion, transform.position, Quaternion.identity);
            //Destroy(_explosion, 3f);

            Destroy(target);
            GameManager.instance.HandleMouseEatsCakeEvent();
        }
    }


    void CheckIfHome()
    {
        if (currentState == MouseState.IsReturning)
        {
            if (transform.position == startPos)
            {
                currentState = MouseState.Idle;
                rb.linearVelocity = new Vector2(0, 0);
            }
        }
    }
    private void OnGUI()
    {
        //GUIStyle _style = new GUIStyle();
        //_style.fontSize = 25;
        //_style.normal.textColor = Color.white;
        //float xPos = 300f;
        //int cakeNum = 0;
        //GUI.Label(new Rect(xPos, 0, 200, 100), "Mouse status ", _style);
        //foreach (GameObject obj in cakes)
        ////droppedCakeList.ForEach
        //{
        //    GUI.Label(new Rect(xPos, 25+(cakeNum*25), 200, 100), "DropList " + droppedCakeList[cakeNum], _style);
        //    cakeNum++;
        //}
    }
}
