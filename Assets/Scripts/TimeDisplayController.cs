using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using General;
using UnityEngine;

public class TimeDisplayController : MonoBehaviour
{
    [SerializeField] private DigitalClock _digitalClock;
    [Space(5)]
    
    [SerializeField] private Transform _hourArrow;
    [SerializeField] private Transform _minuteArrow;
    [SerializeField] private Transform _secondArrow;

    private float _hours = 0;
    private float _minutes = 0;
    private float _seconds = 0;

    private Coroutine _mainCoroutine;
    
    public void SetTime(Dictionary<string, float> data)
    {
        _hours = data.ContainsKey("hour") ? data["hour"] : data["hours"];
        _minutes = data.ContainsKey("minute") ? data["minute"] : data["minutes"];
        _seconds = data["seconds"] + 1;
        
        _hourArrow
            .DORotate(new Vector3(0, 0, -1 * (360 / 12 * _hours)), .1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear);

        _minuteArrow
            .DORotate(new Vector3(0, 0, -1 * (360 / 60 * _minutes)), .1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear);

        if (_mainCoroutine != null) StopCoroutine(_mainCoroutine);
        _mainCoroutine = StartCoroutine(Time());
    }

    public void SetTime(long timeSinceEpoch)
    {
        DateTimeOffset dateTimeOffset = DateTimeOffset.FromUnixTimeMilliseconds(timeSinceEpoch);
        _hours = dateTimeOffset.Hour + 3;
        _minutes = dateTimeOffset.Minute;
        _seconds = dateTimeOffset.Second + 1;
        
        _hourArrow
            .DORotate(new Vector3(0, 0, -1 * (360 / 12 * _hours)), .1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear);

        _minuteArrow
            .DORotate(new Vector3(0, 0, -1 * (360 / 60 * _minutes)), .1f)
            .SetUpdate(true)
            .SetEase(Ease.Linear);
        
        if (_mainCoroutine != null) StopCoroutine(_mainCoroutine);
        _mainCoroutine = StartCoroutine(Time());
    }
    
    private IEnumerator Time()
    {
        while (true)
        {
            CheckAlarm();
            
            // новая минута
            if (_seconds >= 60)
            {
                _minutes++;
                _seconds = 0;
                
                _minuteArrow
                    .DORotate(new Vector3(0, 0, -1 * (360 / 60 * _minutes)), .1f)
                    .SetUpdate(true)
                    .SetEase(Ease.Linear);
            }

            // новый час
            if (_minutes >= 60)
            {
                _hours++;
                _minutes = 0;
                
                FindObjectOfType<RequestController>().SendRequest();
                yield return new WaitForSecondsRealtime(1);
                continue;
            }
            
            _secondArrow
                .DORotate(new Vector3(0, 0, -1 * (360 / 60 * _seconds++)), .1f)
                .SetUpdate(true)
                .SetEase(Ease.Linear);
            _digitalClock.SetTime(_hours, _minutes, _seconds);

            yield return new WaitForSecondsRealtime(1);
        }
    }

    private void CheckAlarm()
    {
        if (PlayerPrefs.HasKey(Constants.ALARM_STORAGE_NAME_HOURS) &&
            PlayerPrefs.HasKey(Constants.ALARM_STORAGE_NAME_MINUTES))
        {
            float storedHours = PlayerPrefs.GetFloat(Constants.ALARM_STORAGE_NAME_HOURS);
            float storedMinutes = PlayerPrefs.GetFloat(Constants.ALARM_STORAGE_NAME_MINUTES);

            if (_hours == storedHours && _minutes == storedMinutes)
            {
                // TODO: alarm action
                print("Alarm goes off");
                    
                PlayerPrefs.DeleteKey(Constants.ALARM_STORAGE_NAME_HOURS);
                PlayerPrefs.DeleteKey(Constants.ALARM_STORAGE_NAME_MINUTES);
            }
        }
    }
}