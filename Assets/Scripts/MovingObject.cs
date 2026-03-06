using UnityEngine;

public class MovingObject : MonoBehaviour
{
    public float speed = 2f;
    public Vector3 moveDirection;

    private bool active = false;

    void Update()
    {
        if(active)
        {
            transform.Translate(moveDirection * speed * Time.deltaTime);
        }
    }

    public void SetActiveState(bool state)
    {
        active = state;
    }
}