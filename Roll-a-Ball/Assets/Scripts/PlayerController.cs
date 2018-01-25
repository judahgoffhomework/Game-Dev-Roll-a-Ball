﻿using System.Collections;
using UnityEngine.UI;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
	public float speed;
	public Text countText;
	public Text winText;
    public Text timerText;

    private int maxSeconds = 30;
    private Color redScale;
    private Rigidbody rb;
	private int count;
    private float elapsedTime;
    private bool gameOver;

    void Start()
	{
		rb = GetComponent<Rigidbody> ();
        StartNewGame();
	}

    void StartNewGame()
    {
        {
            print("Started new game.");
            count = 0;
            setCountText();
            winText.text = "";
            gameOver = false;
            elapsedTime = 0;
            redScale = new Color(0, 0, 0);
            
            GameObject[] pickups = Resources.FindObjectsOfTypeAll(typeof(GameObject)) as GameObject[];
            foreach (GameObject g in pickups)
            {
                if (g.CompareTag("Pick Up") || g.CompareTag("Bonus Pickup"))
                    g.SetActive(true);
            }
            
            Vector3 resetPosition = new Vector3(0, (float)0.5, 0);
            rb.MovePosition(resetPosition);
            rb.Sleep();
            rb.WakeUp();
        }
    }

    private void Update()
    {
        if (!gameOver)
        {
            elapsedTime += Time.deltaTime;
            float timeLeft = maxSeconds - elapsedTime;
            string seconds = (timeLeft % 60).ToString("f1");
            
            if ((count * 1.875) > elapsedTime)
            {
                redScale.r = 0;
                redScale.g = 0;
                redScale.b = elapsedTime * (float)0.03;
            }
            else
            {
                redScale.r = elapsedTime * (float)0.03;
                redScale.g = 0;
                redScale.b = 0;
            }

            timerText.text = seconds;
            timerText.color = redScale;

            if (timeLeft <= 0)
            {
                gameOver = true;
                winText.text = "You Lose!";
            }
        }

        // Did user press N
        if (Input.GetKeyDown(KeyCode.N))
        {
            StartNewGame();
        }
    }

    private void FixedUpdate()
    {
		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");

		Vector3 movement = new Vector3 (moveHorizontal, 0, moveVertical);

		rb.AddForce (movement * speed);
    }

	void OnTriggerEnter(Collider other)
	{
        if (!gameOver)
        {
            if (other.gameObject.CompareTag("Pick Up"))
            {
                other.gameObject.SetActive(false);
                count++;
                setCountText();
            }
            else if (other.gameObject.CompareTag("Bonus Pickup"))
            {
                other.gameObject.SetActive(false);
                count = count + 5;
                setCountText();
            }
        }
	}

    //

    void setCountText()
    {
        countText.text = "Count: " + count.ToString();
        if (count >= 16 && winText.text == "")
        {
            winText.text = "You Win!";
            timerText.color = Color.blue;
            gameOver = true;
        }
    }
}