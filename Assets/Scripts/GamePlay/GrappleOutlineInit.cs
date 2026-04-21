using UnityEngine;

public class GrappleOutlineInit : MonoBehaviour
{
    void Awake()
    {
        Outline outline = GetComponent<Outline>();
        if (outline != null)
            outline.enabled = false;
    }
}
