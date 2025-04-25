using UnityEngine;

public class ScreenSetup : MonoBehaviour
{
    void Start() 
    { 
        // Установите оконный режим и желаемое разрешение 
        Screen.SetResolution(1080, 1920, false); 
    }
}