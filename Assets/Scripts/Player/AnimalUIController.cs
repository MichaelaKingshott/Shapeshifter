using UnityEngine;
using TMPro;
using System.Collections.Generic;

public class AnimalUIController : MonoBehaviour
{
    public ShapeshifterController shapeshifter;

    [System.Serializable]
    public class AnimalUIItem
    {
        public AnimalForm form;
        public TextMeshProUGUI text;
    }

    public AnimalUIItem[] uiItems;

    void Update()
    {
        UpdateUI();
    }

    void UpdateUI()
    {
        foreach (var item in uiItems)
        {
            bool unlocked = shapeshifter.IsFormUnlocked(item.form);
            KeyCode key = shapeshifter.GetKeyForForm(item.form);

            string keyLabel = key != KeyCode.None
                ? key.ToString().Replace("Alpha", "") // Get the key for form
                : "-";

            // Displaying key and form
            item.text.text = $"[{keyLabel}] {item.form}";

            // Visual feedback
            if (shapeshifter.GetCurrentForm() == item.form)
            {
                item.text.color = Color.green; // current form
            }
            else if (unlocked)
            {
                item.text.color = Color.white; // unlocked
            }
            else
            {
                item.text.color = Color.gray; // locked
            }
        }
    }
}