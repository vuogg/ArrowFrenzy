using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class BuffText : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI buffText;
    //[SerializeField] private int currentBuff;

    public void OnInit(int value)
    {
        //buffText.SetText($"x{currentBuff}");
        buffText.SetText($"x{value}");
    }
}
