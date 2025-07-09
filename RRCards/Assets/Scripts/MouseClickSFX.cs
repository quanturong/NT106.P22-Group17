using UnityEngine;

public class MouseClickSFX : MonoBehaviour
{
    void Update()
    {
        if (Input.GetMouseButtonDown(0)) 
        {
            SFXManager.Instance?.PlayClick();
        }
    }
}