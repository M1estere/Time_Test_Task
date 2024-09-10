using TMPro;
using UnityEngine;

public class DigitalClock : MonoBehaviour
{
    [SerializeField] private TMP_Text _timeText;
    
    public void SetTime(float hours, float minutes, float seconds)
    {
        _timeText.SetText($"{hours:00}:{minutes:00}:{seconds:00}");
    }
}