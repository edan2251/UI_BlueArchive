using UnityEngine;
using UnityEngine.UI;

public class ShapedButton : MonoBehaviour
{
    void Start()
    {
        GetComponent<Image>().alphaHitTestMinimumThreshold = 0.1f;
    }
}
