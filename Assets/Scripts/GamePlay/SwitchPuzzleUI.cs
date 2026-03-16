using UnityEngine;

public class SwitchPuzzleUI : MonoBehaviour
{
    public UISwitch[] switches;

    public UISwitch.Direction[] correctDirections;

    public GeneratorLever generator;

    public void CheckPuzzle()
    {
        for (int i = 0; i < switches.Length; i++)
        {
            if (switches[i].currentDirection != correctDirections[i])
            {
                return;
            }
        }

        SolvePuzzle();
    }

    void SolvePuzzle()
    {
        Debug.Log("Generator puzzle solved");

        generator.ActivateGenerator();
    }
}
