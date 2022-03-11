using UnityEngine;
using UnityEngine.UI;

public class HealthBarUI : MonoBehaviour
{
    public GameObject healthUIPrefab;

    public Transform barPos;
    //³¤¾Ã¿É¼û
    public bool alwaysVisble;
    public float visibleTime;
    private float currentTime;
    Transform UIbar;
    Image healthSlider;
    Transform cam;

    public CharacterState currentStats;

    private void Awake()
    {
        currentStats = GetComponent<CharacterState>();
        currentStats.UpdateHealthBarOnAttack += UpdateHealthBar;
        //UIbar =transform.
    }

    private void OnEnable()
    {
        cam = Camera.main.transform;

        foreach (Canvas canvas in FindObjectsOfType<Canvas>())
        {
            if (canvas.renderMode == RenderMode.WorldSpace)
            {
                UIbar = Instantiate(healthUIPrefab, canvas.transform).transform;
                healthSlider = UIbar.GetChild(0).GetComponent<Image>();
                UIbar.transform.position = barPos.position;
                UIbar.gameObject.SetActive(alwaysVisble);
            }
        }
    }

    private void UpdateHealthBar(int currentHealth, int maxHealth)
    {
        if (currentHealth <= 0)
        {
            Destroy(UIbar.gameObject);
        }

        UIbar.gameObject.SetActive(true);
        currentTime = visibleTime;
        float sliderPercent = (float)currentHealth / maxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    private void LateUpdate()
    {
        if (UIbar != null)
        {
            UIbar.position = barPos.position;
            UIbar.forward = -cam.forward;
            if (currentTime <= 0 && !alwaysVisble)
            {
                UIbar.gameObject.SetActive(false);
            }
            else
            {
                currentTime -= Time.deltaTime;
            }
        }
    }
}
