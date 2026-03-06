using UnityEngine;

public class SlidingDoor : MonoBehaviour
{
    public float openHeight = 4f;
    public float speed = 2f;

    private Vector3 closedPos;
    private Vector3 openPos;
    private bool opening = false;

    void Start()
    {
        closedPos = transform.position;
        openPos = closedPos + Vector3.up * openHeight;
    }

    void Update()
    {
        if (opening)
        {
            transform.position = Vector3.MoveTowards(
                transform.position,
                openPos,
                speed * Time.deltaTime
            );

            if (transform.position == openPos)
            {
                opening = false;
            }
        }
    }

    public void OpenDoor()
    {
        opening = true;
    }
}