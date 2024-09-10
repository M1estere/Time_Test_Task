using System;
using System.Collections;
using System.Collections.Generic;
using General;
using TMPro;
using UnityEngine;

public class AlarmController : MonoBehaviour
{
    private Dictionary<int, string> _errors = new()
    {
        {0, "сохранено"},
        {1, "проверьте данные"},
        {2, "проверьте формат"},
    };
    
    [SerializeField] private TMP_Text _warningText;
    [Space(5)] 
    
    [SerializeField] private TMP_Text _alarmCornerText;
    
    [SerializeField] private TMP_InputField _timeInputField;

    private AlarmAnalogController _alarmAnalogController;
    private AlarmDigitalController _alarmDigitalController;
    
    private Coroutine _errorDisplayCoroutine;
    
    private void Awake()
    {
        _alarmAnalogController = GetComponentInChildren<AlarmAnalogController>();
        _alarmDigitalController = GetComponentInChildren<AlarmDigitalController>();
        
        _warningText.gameObject.SetActive(false);
    }

    private void Start()
    {
        CheckAlarmStatus();
    }

    public void SetTime(int where, float hours, float minutes)
    {
        if (where == 0) // place on digital
        {
            _alarmDigitalController.SetTime(hours, minutes);
        }
        else // place on analog
        {
            _alarmAnalogController.SetTime(hours, minutes);
        }
    }
    
    public void SetAlarm()
    {
        string time = _timeInputField.text;
        float hours = (float) Convert.ToDouble(time.Split(":")[0]);
        float minutes = (float)Convert.ToDouble(time.Split(":")[^1]);
        
        PlayerPrefs.SetFloat(Constants.ALARM_STORAGE_NAME_HOURS, hours);
        PlayerPrefs.SetFloat(Constants.ALARM_STORAGE_NAME_MINUTES, minutes);
        
        DisplayError(0);
        CheckAlarmStatus();
    }
    
    public void DisplayError(int code)
    {
        if (_errorDisplayCoroutine != null) StopCoroutine(_errorDisplayCoroutine);
        _errorDisplayCoroutine = StartCoroutine(ErrorDisplay(code));
    }

    private IEnumerator ErrorDisplay(int code)
    {
        _warningText.SetText(_errors[code]);
        _warningText.gameObject.SetActive(true);
        yield return new WaitForSecondsRealtime(2);
        _warningText.gameObject.SetActive(false);
    }

    private void CheckAlarmStatus()
    {
        if (PlayerPrefs.HasKey(Constants.ALARM_STORAGE_NAME_HOURS) &&
            PlayerPrefs.HasKey(Constants.ALARM_STORAGE_NAME_MINUTES))
        {
            float hours = PlayerPrefs.GetFloat(Constants.ALARM_STORAGE_NAME_HOURS);
            float minutes = PlayerPrefs.GetFloat(Constants.ALARM_STORAGE_NAME_MINUTES);
            
            _alarmCornerText.SetText($"Будильник установлен на <b>{hours:00}:{minutes:00}</b>");
            _alarmCornerText.gameObject.SetActive(true);
        }
        else
        {
            _alarmCornerText.gameObject.SetActive(false);
        }
    }
}