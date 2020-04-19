using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(CanvasGroup))]
public class CanvasGroupAlphaSync : MonoBehaviour
{
    public Graphic Target;
    
    private CanvasGroup canvasGroupComponent;

    private void Start()
    {
        canvasGroupComponent = GetComponent<CanvasGroup>();
    }

    private void Update()
    {
        canvasGroupComponent.alpha = Target.canvasRenderer.GetAlpha() * 2.5f;
    }
}
