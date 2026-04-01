using UnityEngine;

public class Water : MonoBehaviour
{
    public float moveSpeed = 2f;

    private float targetY;

    void Start()
    {
        targetY = transform.position.y;
    }

    void Update()
    {
        Vector3 pos = transform.position;

        pos.y = Mathf.MoveTowards(pos.y, targetY, moveSpeed * Time.deltaTime);

        transform.position = pos;
    }

    public void SetWaterHeight(float newY)
    {
        targetY = newY;
    }
}





