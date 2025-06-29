using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class Ball : MonoBehaviour
{
    bool isGameOver;
    BallStatus ballStatus = BallStatus.Bouncy;
    BallMovement currMovement = BallMovement.Backward;
    bool isMoving = false;
    public Image ballImage;
    public Sprite rock, bouncy;
    public float movementSpeed;
    public float bouncySpeed;

    enum BallStatus
    {
        Rock,
        Bouncy

    }
    enum BallMovement
    {
        Farward,
        Backward
    }
    private void Awake()
    {
        Debug.Log("awake called");
    }
    void Start()
    {
        Debug.Log("start called");
    }
    private void OnEnable()
    {
        Debug.Log("enable called");
    }
    private void OnDisable()
    {
        Debug.Log("disable called");
    }

    void Update()
    {
        if(isGameOver) return;

        if(isMoving)
        {
            var lastPosition = transform.position;
            if (currMovement == BallMovement.Farward)
            {
                transform.position += new Vector3(Time.deltaTime * movementSpeed, 0, 0);
            }
            if (currMovement == BallMovement.Backward)
            {
                transform.position -= new Vector3(Time.deltaTime * movementSpeed, 0, 0);
            }
            if(transform.position.x >= Screen.width*0.3f || transform.position.x <= Screen.width*0.1f)
            {
                transform.position = lastPosition;
                isMoving = false;
            }
        }
        if (ballStatus == BallStatus.Rock) 
        {
            transform.position -= new Vector3(0, Time.deltaTime * bouncySpeed, 0);
        }
        if (ballStatus == BallStatus.Bouncy)
        {
            Debug.Log("ball position " + transform.position.y+ " : "+transform.position.y );

            var lastPos = transform.position;
            transform.position += new Vector3(0, Time.deltaTime * bouncySpeed, 0);
            if (transform.position.y > Screen.height * 0.9f) transform.position = lastPos;
        }
        if (transform.position.y < Screen.height * 0.2f)
        {
            isGameOver = true;
            Debug.Log("game over");
        }
    }
    public void MoveObjectToForWardOrBackWard(Text text)
    {
        isMoving = true;
        if(currMovement == BallMovement.Farward)
        {
            currMovement = BallMovement.Backward;
            text.text = "Forward";
        }
        else if(currMovement == BallMovement.Backward)
        {
            currMovement = BallMovement.Farward;
            text.text = "Backward";
        }
    }
    public void ChangeBallStatus(Text text)
    {
        if (ballStatus == BallStatus.Rock)
        {
            ballStatus = BallStatus.Bouncy;
            ballImage.sprite = bouncy;
            text.text = "Rock";

        }
        else if (ballStatus == BallStatus.Bouncy)
        {
            ballStatus = BallStatus.Rock;
            ballImage.sprite = rock;
            text.text = "Bouncy";

        }
    }
    
}
