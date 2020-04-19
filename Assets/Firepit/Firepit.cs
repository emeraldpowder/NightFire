using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using Random = UnityEngine.Random;

public class Firepit : MonoBehaviour
{
    [Header("Settings")]
    public int MaxHealthSeconds = 120;
    [Range(0, 1)] public float Health = 1;
    public AnimationCurve DifficultyCurve;

    [Header("UI")] public Text HpText;
    public Text TimeText;
    public Image HpBar;
    public HpAddedText HpAddedTextPrefab;
    public HpAddedText MaxHpAddedTextPrefab;
    public GameOverScreen GameOverScreen;

    [Header("Audio")] public AudioSource WoodBurntSound;
    public AudioSource GameOverSound;
    public AudioSource FireLoopSound;
    
    [Header("Stuff")]
    public GameObject[] Logs;
    public Light FirepitLight;

    public static Firepit Instance;
    public static float TimePassed; 
    
    private Vector3 lightInitialPosition;
    private ParticleSystem[] LogParticles;
    private Collider[] colliders = new Collider[10];

    private float lastHpBarFill;

    private void Start()
    {
        Instance = this;
        
        lightInitialPosition = FirepitLight.transform.position;

        LogParticles = Logs.Select(l => l.GetComponentInChildren<ParticleSystem>()).ToArray();

        lastHpBarFill = Health;
        
        TimePassed = 0;
    }

    private void Update()
    {
        FireLoopSound.volume = Mathf.Lerp(0.2f, 0.8f, Health);
        
        FirepitLight.intensity = Mathf.Lerp(1, 4, Health) + Mathf.Sin(Time.time * 3 + Random.value * .5f) * .2f;
        FirepitLight.transform.position = lightInitialPosition + new Vector3(
                                              Mathf.Sin(Time.time * 2) + Mathf.Sin(Time.time * Mathf.PI * 1.2f),
                                              Mathf.Sin(Time.time * 2.2f) + Mathf.Sin(Time.time * Mathf.PI * 1.1f) +
                                              Mathf.Sin(Time.time * 3.123f),
                                              Mathf.Sin(Time.time * 2.4f) + Mathf.Sin(Time.time * Mathf.PI)
                                          ) * .05f; // For nice light cracking animation

        if (!MainMenu.IsGameStarted) return;

        TimePassed += Time.deltaTime;

        int minutes = (int) TimePassed / 60;
        int seconds = (int) TimePassed % 60;
        TimeText.text = $"{minutes:00}:{seconds:00}";

        Health -= Time.deltaTime / MaxHealthSeconds * DifficultyCurve.Evaluate(TimePassed);
        if (Health <= 0.001f)
        {
            GameOverScreen.gameObject.SetActive(true);
            GameOverSound.Play();
        }
        
        UpdateLogs();
        UpdateUI();
        CheckAnyFuelThrowed();
    }

    private void UpdateLogs()
    {
        for (int i = 0; i < Logs.Length; i++)
        {
            bool active = (float) i / Logs.Length < Health;

            Vector3 target = Vector3.up * (active ? 0 : -1.25f);
            Logs[i].transform.localPosition = Vector3.MoveTowards(Logs[i].transform.localPosition, target,
                Time.deltaTime * 2);

            if (active && !LogParticles[i].isPlaying) LogParticles[i].Play();
            else if (!active && LogParticles[i].isPlaying) LogParticles[i].Stop();
        }
    }

    private void UpdateUI()
    {
        int health = (int) (MaxHealthSeconds * Health);
        HpText.text = $"{health}<size=30> / {MaxHealthSeconds}</size>";
        lastHpBarFill = Mathf.Lerp(lastHpBarFill, Health, Time.deltaTime*2);
        HpBar.fillAmount = lastHpBarFill;
    }

    private void CheckAnyFuelThrowed()
    {
        int mask = LayerMask.GetMask("Firewood", "Tree", "Mushroom");
        int count = Physics.OverlapSphereNonAlloc(transform.position + Vector3.up, 3f, colliders, mask);
        for (int i = 0; i < count; i++)
        {
            var fireThrowable = colliders[i].attachedRigidbody.GetComponent<FireThrowable>();
            if (fireThrowable != null)
            {
                StartCoroutine(ConsumeObject(fireThrowable, colliders[i]));
            }
        }
    }

    private void OnDrawGizmosSelected()
    {
        Gizmos.DrawWireSphere(transform.position + Vector3.up, 3f);
    }

    public IEnumerator ConsumeObject(FireThrowable target, Collider colliderInFire)
    {
        if (colliderInFire.attachedRigidbody.isKinematic)
        {
            // It's currently grabbed
            yield break;
        }

        Destroy(colliderInFire);
        Destroy(colliderInFire.attachedRigidbody);
        
        WoodBurntSound.Play();
        
        AddHealth(target.HealthAddAmount, target.MaxHealthAddAmount);

        if (target.ThrowPrefab != null)
        {
            Instantiate(target.ThrowPrefab, transform.position+Vector3.up, Quaternion.identity);
        }

        Vector3 moveTo = transform.position + Vector3.down * 12;
        Vector3 velocity = Vector3.zero;

        while (Vector3.Distance(target.transform.position, moveTo) > 1f)
        {
            target.transform.position =
                Vector3.SmoothDamp(target.transform.position, moveTo, ref velocity, .2f, 10);
            yield return null;
        }
        
        Destroy(target.gameObject);
    }

    public void AddHealth(float amount, int maxAmount)
    {
        if (amount > 0)
        {
            Health += amount;
            if (Health > 1) Health = 1;

            HpAddedText hpAddedText = Instantiate(HpAddedTextPrefab, HpText.transform.root);
            hpAddedText.SetText((int) (amount * MaxHealthSeconds));
        }

        if (maxAmount > 0)
        {
            MaxHealthSeconds += maxAmount;

            HpAddedText maxHpAddedText = Instantiate(MaxHpAddedTextPrefab, HpText.transform.root);
            maxHpAddedText.SetText(maxAmount);
        }
    }
}