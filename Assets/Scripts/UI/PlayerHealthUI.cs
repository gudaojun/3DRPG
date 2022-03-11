using UnityEngine;
using UnityEngine.UI;
public class PlayerHealthUI : MonoBehaviour
{
    Image healthSlider;
    Image expSlider;
    Text levelText;
    void Awake()
    {
        healthSlider = transform.GetChild(0).GetChild(0).GetComponent<Image>();
        expSlider = transform.GetChild(1).GetChild(0).GetComponent<Image>();
        levelText = transform.GetChild(2).GetComponent<Text>();
    }

    private void Update()
    {
        UpdateHealth();
        UpdateExp();
        levelText.text = "Level   " + GameManager.Instance.playerState.characterData.currentLevel.ToString("00");
    }

    void UpdateHealth()
    {
        float sliderPercent = (float)GameManager.Instance.playerState.CurrentHealth / GameManager.Instance.playerState.MaxHealth;
        healthSlider.fillAmount = sliderPercent;
    }
    void UpdateExp()
    {
        float sliderPercent = (float)GameManager.Instance.playerState.characterData.currentExp / GameManager.Instance.playerState.characterData.baseExp;
        expSlider.fillAmount = sliderPercent;
    }
}
