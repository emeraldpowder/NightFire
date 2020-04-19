using UnityEngine;
using UnityEngine.UI;

public class Compass : MonoBehaviour
{
    public Transform Player;

    public float StartAppearingRange = 10;
    public float EndAppearingRange = 100;

    private Image imageComponent;

    private void Start()
    {
        imageComponent = GetComponent<Image>();
    }

    private void Update()
    {
        float distance = Player.position.magnitude;
        float a = Mathf.Clamp01((distance - StartAppearingRange) / (EndAppearingRange - StartAppearingRange));
        imageComponent.color = new Color(0.96f, 0.77f, 0.41f, a);

        transform.localRotation = Quaternion.Euler(0,0, Vector3.SignedAngle(Vector3.back, Player.position, Vector3.down));
    }
}
