using UnityEngine;
using UnityEngine.UI;

public class HealthUI : MonoBehaviour
{
    [SerializeField] private HealthData healthData;
    [SerializeField] private Transform livesContainer;
    [SerializeField] private GameObject lifePrefab;

    private Image[] _lives;

    void Start()
    {
        BuildLives();
        Refresh();
    }

    private void BuildLives()
    {
        _lives = new Image[healthData.maxHealth];
        for (int i = 0; i < healthData.maxHealth; i++)
            _lives[i] = Instantiate(lifePrefab, livesContainer).GetComponent<Image>();
    }

    public void Refresh()
    {   
        for (int i = 0; i < _lives.Length; i++)
            _lives[i].color = i < healthData.CurrentHealth ? Color.white : new Color(0f, 0f, 0f, 0.25f);
    }
}
