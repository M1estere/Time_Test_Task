using System;
using System.Collections.Generic;
using System.Linq;
using Api;
using General;
using Newtonsoft.Json;
using UnityEngine;

public class RequestController : MonoBehaviour
{
    private TimeDisplayController _timeDisplayController;

    private void Awake()
    {
        _timeDisplayController = FindObjectOfType<TimeDisplayController>();
        
        SendRequest();
    }

    public void SendRequest()
    {
        JsonTestApiCall();
    }

    private void TimeApiCall()
    {
        // time api
        StartCoroutine(TimeApi.GetRequest(Constants.TIMEAPI_REQUEST, delegate(string data)
        {
            Dictionary<string, string> jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            Dictionary<string, float> target = jsonData
                .Where(item => Constants.KEYS.Contains(item.Key))
                .ToDictionary(item => item.Key, item => (float) Convert.ToDouble(item.Value));
            
            _timeDisplayController.SetTime(target);
        }));
    }

    private void JsonTestApiCall()
    {
        // json test api
        StartCoroutine(JsonTestApi.GetRequest(Constants.JSONTEST_REQUEST, delegate(string data)
        {
            Dictionary<string, string> jsonData = JsonConvert.DeserializeObject<Dictionary<string, string>>(data);
            Dictionary<string, float> target = jsonData
                .Where(item => Constants.KEYS.Contains(item.Key))
                .ToDictionary(item => item.Key, item => (float) Convert.ToDouble(item.Value));
            
            _timeDisplayController.SetTime((long) target["milliseconds_since_epoch"]);
            
            TimeApiCall();
        }));
    }
}