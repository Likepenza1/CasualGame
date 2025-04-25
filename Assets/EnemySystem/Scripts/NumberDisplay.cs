using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class NumberDisplay : MonoBehaviour
{
    public TextMeshProUGUI textMeshPro;
    public int killEnemy;

    private void Update()
    {
        int nevValue = killEnemy;
        textMeshPro.text = nevValue.ToString();
    }
}
