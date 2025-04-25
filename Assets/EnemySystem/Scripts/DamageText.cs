using TMPro;
using UnityEngine;

public class DamageText : MonoBehaviour
{
    public TextMeshProUGUI textMesh;

    public void Setup(int damage, bool isCrit)
    {
        textMesh.text = isCrit ? $"<color=yellow><b>{damage}</b></color>" : damage.ToString();
    }

    public void DestroyAfter(float seconds)
    {
        Destroy(gameObject, seconds);
    }
}