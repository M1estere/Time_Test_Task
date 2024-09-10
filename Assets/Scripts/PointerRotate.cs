using UnityEngine;
using UnityEngine.EventSystems;

public class PointerRotate : MonoBehaviour, IDragHandler
{
    public enum ArrowStyle
    {
        Hour,
        Minute,
    }
    
    [SerializeField] private ArrowStyle _arrowStyle;

    private AlarmAnalogController _alarmAnalogController;
    private Camera _mainCamera;
    
    private float _stepSize;

    private void Awake()
    {
        _alarmAnalogController = GetComponentInParent<AlarmAnalogController>();
        _mainCamera = FindObjectOfType<Camera>();
        _stepSize = _arrowStyle == ArrowStyle.Hour ? 360 / 12 : 360 / 60;
    }

    public void OnDrag(PointerEventData eventData)
    {
        Vector3 mousePosition = Input.mousePosition;

        Vector3 direction = mousePosition - _mainCamera.WorldToScreenPoint(transform.position);
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg;
        angle -= 90f;

        float snappedRotation = Mathf.Round(angle / _stepSize) * _stepSize;
        transform.eulerAngles = new Vector3(0, 0, snappedRotation);
        
        float time = (float) Mathf.FloorToInt((-1 * snappedRotation + 360) % 360 / _stepSize);
        
        _alarmAnalogController.UpdateTime(_arrowStyle, time);
    }
}