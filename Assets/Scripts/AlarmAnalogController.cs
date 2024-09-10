using UnityEngine;

public class AlarmAnalogController : MonoBehaviour
{
    [SerializeField] private Transform _hourHand;
    [SerializeField] private Transform _minuteHand;
    
    private AlarmController _alarmController;

    private float _hours;
    private float _minutes;
    
    private void Awake()
    {
        _alarmController = GetComponentInParent<AlarmController>();
    }

    public void UpdateTime(PointerRotate.ArrowStyle arrowStyle, float value)
    {
        if (arrowStyle == PointerRotate.ArrowStyle.Hour) _hours = value;
        else _minutes = value;
        
        _alarmController.SetTime(0, _hours, _minutes);
    }

    public void SetTime(float hours, float minutes)
    {
        hours %= 12;
        minutes %= 60;

        // Вычисляем угол для часов и минут
        float hourAngle = hours * 30f;
        float minuteAngle = minutes * 6f;
        hourAngle *= -1;
        minuteAngle *= -1;

        // Вращаем стрелки
        _hourHand.eulerAngles = new Vector3(0, 0, hourAngle);
        _minuteHand.eulerAngles = new Vector3(0, 0, minuteAngle);
    }
}