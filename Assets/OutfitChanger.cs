using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OutfitChanger : MonoBehaviour
{
    [Header("Sprite to Change")]
    public SpriteRenderer bodypart;

    [Header("Sprites to Cycle Through")]
    public List<Sprite> options = new List<Sprite>();
    // Start is called before the first frame update
    private int currentOption = 0;

    public void NextOption() {
        currentOption++; 
        if (currentOption >= options.Count) {
            currentOption = 0;
        }
        bodypart.sprite = options[currentOption];
    }

    public void PreviousOption() {
        currentOption--;
        if (currentOption <= 0) {
            currentOption = options.Count - 1;
        }
        bodypart.sprite = options[currentOption];
    }
}
