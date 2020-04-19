using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HpAddedText : MonoBehaviour
{
    public Text HpText;

    public void SetText(int value)
    {
        HpText.text = "+" + value;
    }

    private void Start()
    {
        StartCoroutine(Animation());
    }

    private IEnumerator Animation()
    {
        RectTransform rectTransform = (RectTransform) transform;
        Vector2 startPosition = rectTransform.anchoredPosition;
        for (float i = 0; i < 1; i += Time.deltaTime)
        {
            float t = 1 - (1 - i) * (1 - i);
            rectTransform.anchoredPosition = startPosition + t * 30 * Vector2.down;
            
            yield return null;
        }
        yield return new WaitForSeconds(1f);
        
        Destroy(gameObject);
    }
}
