using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bird : MonoBehaviour
{
    float xMinPos, yMinPos, xMaxPos, yMaxPos;
    Vector3 initialPosition;
    Vector3 diffBtnInitialFinalPos;
    float waitingTime = 0f;
    bool isGameStarted = false;
    [SerializeField] float padding = 2.5f;
    [SerializeField] float SpeedOfBird = 13000f;
    [SerializeField] float timeToWait = 3f;

    private void Awake()
    {
        initialPosition = gameObject.transform.position;
    }
    private void Start()
    {
        setUpBoundaries();
    }

    private void Update()
    {
        gameObject.GetComponent<LineRenderer>().SetPosition(0,initialPosition);
        gameObject.GetComponent<LineRenderer>().SetPosition(1, transform.position);


        setupReplay();
        if (isGameStarted && gameObject.GetComponent<Rigidbody2D>().velocity.magnitude <= 0.1)
        {
            waitingTime += Time.deltaTime;
        }
    }

    private void OnMouseDown()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.red;
        gameObject.GetComponent<LineRenderer>().enabled = true;
    }

    private void OnMouseUp()
    {
        gameObject.GetComponent<SpriteRenderer>().color = Color.white;
        gameObject.GetComponent<Rigidbody2D>().AddForce(diffBtnInitialFinalPos * Time.deltaTime * SpeedOfBird);
        gameObject.GetComponent<Rigidbody2D>().gravityScale = 1;
        isGameStarted = true;
        gameObject.GetComponent<LineRenderer>().enabled = false ;
    }

    private void OnMouseDrag()
    {
        Vector3 BirdPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        gameObject.transform.position = new Vector3(BirdPosition.x,BirdPosition.y,0);
        diffBtnInitialFinalPos = initialPosition - transform.position;
    }

    private void setUpBoundaries()
    {
        xMinPos = Camera.main.ViewportToWorldPoint(new Vector3(-1, 0, 0)).x - padding;
        xMaxPos = Camera.main.ViewportToWorldPoint(new Vector3(1, 0, 0)).x + padding;
        yMinPos = Camera.main.ViewportToWorldPoint(new Vector3(0, -1, 0)).y - padding;
        yMaxPos = Camera.main.ViewportToWorldPoint(new Vector3(0, 1, 0)).y + padding;
    }

    private void setupReplay()
    {
        if(transform.position.x <= xMinPos || 
            transform.position.x >= xMaxPos ||
            transform.position.y <= yMinPos ||
            transform.position.y >= yMaxPos ||
            waitingTime>=timeToWait
            )
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
    }
}
