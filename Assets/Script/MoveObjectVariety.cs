using UnityEngine;

public class MoveObjectVariety : MonoBehaviour
{
    public enum moveOptions // Create options for movement in the inspector
    {
        MoveNormal,
        MoveInstant,
        MoveOnPoint,
        MoveWithCollision,
        MoveOnClick
    }
    public moveOptions moveChoice;

    [SerializeField] Transform[] positions;
    [SerializeField] float objectSpeed, moveDelay, rotSpeed;
    [SerializeField] bool destroyAfter, randomNextPos;
    [SerializeField] Rigidbody2D rb2D;

    float moveTimer;
    int nextPosIndex;
    Transform nextPos;
    // Start is called before the first frame update
    void Start()
    {
        // Check if Move On Point is selected or not and assign its starting position.
        // If Move On Point is selected, assign objects own transform to prevent object from moving in the start of the game
        // If Move On Point is not selected, then assign position index 0 as its starting point.
        nextPos = moveChoice == moveOptions.MoveOnPoint ? transform : positions[0];
    }

    // Update is called once per frame
    void Update()
    {
        if (moveChoice == moveOptions.MoveInstant)
        {
            MoveInstantly();
        }
        else if (moveChoice == moveOptions.MoveOnPoint)
        {
            MoveOnPoint();
        }
        else if (moveChoice == moveOptions.MoveWithCollision)
        {
            MoveUsingRigidbody();
        }
        else if (moveChoice == moveOptions.MoveOnClick) 
        {
            MoveOnClick();
        }
        else
        {
            MoveGameObject();
        }
    }

    void MoveGameObject() // Simple movement
    {
        if (transform.position == nextPos.position)
        {
            if (randomNextPos) // Check if random position is ON
            {
                nextPosIndex = Random.Range(0, positions.Length);
            }
            else
            {
                nextPosIndex++;
            }

            if (nextPosIndex < positions.Length)
            {
                nextPos = positions[nextPosIndex];
            }
            else
            {
                if (destroyAfter) // Check if object should be destroyed in the last position
                {
                    if (nextPosIndex >= positions.Length - 1)
                    {
                        gameObject.SetActive(false);
                        //Destroy(gameObject); // If you really need to destroy the object
                    }
                }
                else
                {
                    nextPosIndex = 0;
                    nextPos = positions[nextPosIndex];
                }
            }

        }
        else
        {
            LookAtPos();
            transform.position = Vector3.MoveTowards(transform.position, nextPos.position, objectSpeed * Time.deltaTime);
        }
    }

    void MoveInstantly()
    {
        if (moveTimer >= moveDelay)
        {
            if (randomNextPos)
            {
                nextPosIndex = Random.Range(0, positions.Length);
            }
            else
            {
                nextPosIndex++;
            }

            if (nextPosIndex < positions.Length)
            {
                nextPos = positions[nextPosIndex];
            }
            else
            {
                if (destroyAfter)
                {
                    if (nextPosIndex >= positions.Length - 1)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    nextPosIndex = 0;
                    nextPos = positions[nextPosIndex];
                }
            }

            moveTimer = 0;
            transform.position = nextPos.position;
        }
        else
        {
            moveTimer += Time.deltaTime;
        }
    }

    void MoveOnPoint()
    {
        if (transform.position == nextPos.position)
        {
            if (moveTimer >= moveDelay)
            {
                if (Input.GetMouseButtonDown(0))
                {
                    Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                    RaycastHit hit;

                    if (Physics.Raycast(ray, out hit, 100))
                    {
                        nextPos = hit.transform;
                    }
                    moveTimer = 0;
                }
            }
            else
            {
                moveTimer += Time.deltaTime;
            }
        }
        else
        {
            LookAtPos();
            transform.position = Vector3.MoveTowards(transform.position, nextPos.position, objectSpeed * Time.deltaTime);
        }
    }

    void MoveUsingRigidbody()
    {
        if (transform.position == nextPos.position)
        {
            if (randomNextPos)
            {
                nextPosIndex = Random.Range(0, positions.Length);
            }
            else
            {
                nextPosIndex++;
            }

            if (nextPosIndex < positions.Length)
            {
                nextPos = positions[nextPosIndex];
            }
            else
            {
                if (destroyAfter)
                {
                    if (nextPosIndex >= positions.Length - 1)
                    {
                        Destroy(gameObject);
                    }
                }
                else
                {
                    nextPosIndex = 0;
                    nextPos = positions[nextPosIndex];
                }
            }

        }
        else
        {
            LookAtPos();

            if (rb2D != null)
            {
                Vector2 newPosition = Vector3.MoveTowards(transform.position, nextPos.position, objectSpeed * Time.deltaTime);
                rb2D.MovePosition(newPosition);
            }
        }
    }

    void MoveOnClick() 
    {
        if (Input.anyKey)
        {
            if (transform.position == nextPos.position)
            {
                if (randomNextPos)
                {
                    nextPosIndex = Random.Range(0, positions.Length);
                }
                else
                {
                    nextPosIndex++;
                }

                if (nextPosIndex >= positions.Length)
                {
                    nextPosIndex = 0;
                }

                nextPos = positions[nextPosIndex];
            }
        }

        LookAtPos();
        transform.position = Vector3.MoveTowards(transform.position, nextPos.position, objectSpeed * Time.deltaTime);
    }

    void LookAtPos() 
    {
        Vector2 lookPos = nextPos.position - transform.position;
        transform.up = lookPos;

        /*Quaternion targetRotation = Quaternion.LookRotation(Vector3.forward, lookPos);
        transform.rotation = Quaternion.RotateTowards(transform.rotation, targetRotation, rotSpeed * Time.deltaTime);*/
    }
}
