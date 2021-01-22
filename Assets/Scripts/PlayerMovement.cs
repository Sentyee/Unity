using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class PlayerMovement : MonoBehaviour
{
    public GameObject player;
    public GameObject jump;
    public Rigidbody playerRB;
    public TextMeshProUGUI scoreText;
    public TextMeshProUGUI winText;
    public Button Button;
    public float transitionSpeed = 0.4f;

    private Vector3 originPos, finalPos, rotate;
    private int score;
    private bool moving = false;
    private bool allowInput = true;

    public void Start()
    {
        score = 0;

        SetScoreText();
    }

    void SetScoreText()
    {
        scoreText.text = score.ToString() + "/3";
    }

    // Update is called once per frame
    void Update()
    {
        if (!Physics.Raycast(transform.position,Vector3.down,5))
        {
            allowInput = false;
        }

        if (allowInput == true)
        {

            if (Input.GetKey(KeyCode.W) && !moving)
            {
                StartCoroutine(MovePlayer(Vector3.forward));
                rotate.Set(0, -90, 0);
                player.transform.eulerAngles = rotate;
            }

            if (Input.GetKey(KeyCode.A) && !moving)
            {
                StartCoroutine(MovePlayer(Vector3.left));
                rotate.Set(0, 180, 0);
                player.transform.eulerAngles = rotate;
            }


            if (Input.GetKey(KeyCode.S) && !moving)
            {
                StartCoroutine(MovePlayer(Vector3.back));
                rotate.Set(0, 90, 0);
                player.transform.eulerAngles = rotate;
            }


            if (Input.GetKey(KeyCode.D) && !moving)
            {
                StartCoroutine(MovePlayer(Vector3.right));
                rotate.Set(0, 0, 0);
                player.transform.eulerAngles = rotate;
            }

        }


        if (score >= 3)
        {
            allowInput = false;
            Button.gameObject.SetActive(true);
            winText.gameObject.SetActive(true);
            
        }
    }

    private IEnumerator MovePlayer(Vector3 direction)
    {
        moving = true;

        float timeSpentMoving = 0;

        originPos = transform.position;
        finalPos = originPos + direction;

        while (timeSpentMoving < transitionSpeed)
        {
            transform.position = Vector3.Lerp(originPos, finalPos, (timeSpentMoving / transitionSpeed));
            timeSpentMoving += Time.deltaTime;
            yield return null;
        }

        transform.position = finalPos;

        moving = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("PickUp"))
        {
            other.gameObject.SetActive(false);
            score++;

            SetScoreText();
        }

        
    }
    private void OnTriggerStay(Collider other)
    {
        if (other.gameObject.CompareTag("Jump"))
        {
            if (Input.GetKey(KeyCode.Space) && !moving)
            {
                playerRB.AddForce(7, 7, 0);

            }
        }
        if (other.gameObject.CompareTag("Finish"))
        {
            if (Input.GetKey(KeyCode.Space) && !moving)
            {
                playerRB.AddForce(0, 12, 11);

            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.CompareTag("Jump") || other.gameObject.CompareTag("Finish"))
        {
            moving = false;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("GrassPH"))
        {
            allowInput = true;
        }
    }

}
