using System;
using System.Text.RegularExpressions;
using TMPro;
using UnityEngine;

public class AlarmDigitalController : MonoBehaviour
{
    [SerializeField] private TMP_InputField _inputField;

    private AlarmController _alarmController;

    private void Awake()
    {
        _alarmController = GetComponentInParent<AlarmController>();
    }

    public void SetTime(float hours, float minutes)
    {
        _inputField.text = $"{hours:00}:{minutes:00}";
    }

    public void CheckSequence(string seq)
    {
        seq = seq.Trim();
        if (seq.Length < 5) return;
        if (Regex.IsMatch(seq, "[0-1][0-9]:[0-9][0-9]") == false)
        {
            _alarmController.DisplayError(2);
            return;
        }

        float hours = (float) Convert.ToDouble(seq.Split(":")[0]);
        float minutes = (float) Convert.ToDouble(seq.Split(":")[^1]);

        if (minutes > 60 || hours > 24)
        {
            _alarmController.DisplayError(2);
            return;
        }
        
        _alarmController.SetTime(1, hours, minutes);
    }
}