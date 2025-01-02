using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class LevelText : MonoBehaviour
{
    public TextMeshProUGUI levelText;

    public void LevelMainMenu(int level)
    {
        if (level >= 10)
        {
            levelText.SetText($"Level {level}");
        }
        else
        {
            levelText.SetText($"Level 0{level}");
        }
    }

    public void LevelGamePlay(int level)
    {
        if(level >= 10)
        {
            levelText.SetText($"{level}");
        }
        else
        {
            levelText.SetText($"0{level}");
        }
    }
}
