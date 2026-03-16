using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class UISwitch : MonoBehaviour
{
    public enum Direction
    {
        Right,
        Down,
        Left,
        Up
    }

    public Direction currentDirection = Direction.Right;

    public Image arrowImage;

    public SwitchPuzzleUI puzzle;

    private bool rotating = false;

    void Start()
    {
        UpdateArrowInstant();
    }

    public void RotateSwitch()
    {
        if (rotating) return;

        currentDirection = (Direction)(((int)currentDirection + 1) % 4);

        StartCoroutine(RotateArrow());

        puzzle.CheckPuzzle();
    }

    IEnumerator RotateArrow()
    {
        rotating = true;

        Quaternion startRot = arrowImage.rectTransform.rotation;
        Quaternion endRot = Quaternion.Euler(0, 0, -(int)currentDirection * 90);

        float t = 0;

        while (t < 1)
        {
            t += Time.unscaledDeltaTime * 6f;
            arrowImage.rectTransform.rotation = Quaternion.Lerp(startRot, endRot, t);
            yield return null;
        }

        arrowImage.rectTransform.rotation = endRot;

        rotating = false;
    }

    void UpdateArrowInstant()
    {
        arrowImage.rectTransform.rotation = Quaternion.Euler(0, 0, -(int)currentDirection * 90);
    }
}
