using System;
using UnityEngine;

public class InputController : MonoBehaviour
{
    public float m_oMouseSensibility;
    private float m_fLastClikTime;
    private float m_fLastRightClikTime;
    public float m_fDoubleClickTime;
    public Action m_oKeyEscapePressed;
    public Action m_oKeyF1Pressed;
    public Action m_oMiddleMouseButtonHold;
    public Action m_oMiddleMouseButtonUp;
    public Action m_oRightMouseButtonPressed;
    public Action m_oRightMouseButtonHold;
    public Action m_oRightMouseButtonUp;
    public Action m_oLeftMouseButtonPressed;
    public Action m_oLeftMouseButtonHold;
    public Action m_oLeftMouseButtonUp;
    public Action<float> m_oMouseDeltaX;
    public Action<float> m_oMouseDeltaY;
    public Action<float> m_oMouseScroll;
    public Action<float> m_oTouchPinch;
    public Action<float> m_oHorizontalAxis;
    public Action<float> m_oVerticalAxis;
    public Action<Vector2> m_oDoubleClick;
    public Action<Vector2> m_oDoubleRightClick;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            m_oKeyEscapePressed?.Invoke();
        }
        if (Input.GetKeyDown(KeyCode.F1))
        {
            m_oKeyF1Pressed?.Invoke();
        }
        if (Input.GetButton("Fire3"))
        {
            m_oMiddleMouseButtonHold?.Invoke();
        }
        if (Input.GetButtonUp("Fire3"))
        {
            m_oMiddleMouseButtonUp?.Invoke();
        }
        if (Input.GetButton("Fire2"))
        {
            m_oRightMouseButtonHold?.Invoke();
        }
        if (Input.GetButtonDown("Fire2"))
        {
            m_oRightMouseButtonPressed?.Invoke();
        }
        if (Input.GetButtonUp("Fire2"))
        {
            if (Time.time < m_fLastRightClikTime + m_fDoubleClickTime)
            {
                m_oDoubleRightClick?.Invoke(Input.mousePosition);
            }
            m_fLastRightClikTime = Time.time;
            m_oRightMouseButtonUp?.Invoke();
        }
        if (Input.GetButton("Fire1"))
        {
            m_oLeftMouseButtonHold?.Invoke();
        }
        if (Input.GetButtonDown("Fire1"))
        {
            m_oLeftMouseButtonPressed?.Invoke();
        }
        if (Input.GetButtonUp("Fire1"))
        {
            if(Time.time < m_fLastClikTime + m_fDoubleClickTime && (Input.mousePosition.x < Screen.width - 50))
            {
                m_oDoubleClick?.Invoke(Input.mousePosition);
            }
            m_fLastClikTime = Time.time;
            m_oLeftMouseButtonUp?.Invoke();
        }
        if (Input.touchCount == 2)
        {
            Touch touchZero = Input.GetTouch(0);
            Touch touchOne = Input.GetTouch(1);

            Vector2 touchZeroPrevPos = touchZero.position - touchZero.deltaPosition;
            Vector2 touchOnePrevPos = touchOne.position - touchOne.deltaPosition;

            float prevMagnitude = (touchZeroPrevPos - touchOnePrevPos).magnitude;
            float currentMagnitude = (touchZero.position - touchOne.position).magnitude;

            float difference = currentMagnitude - prevMagnitude;

            m_oTouchPinch?.Invoke(difference * 0.002f);
        }
        m_oMouseDeltaX?.Invoke(Input.GetAxis("Mouse X") * m_oMouseSensibility);
        m_oMouseDeltaY?.Invoke(Input.GetAxis("Mouse Y") * m_oMouseSensibility);
        m_oMouseScroll?.Invoke(Input.GetAxis("Mouse ScrollWheel"));
        m_oHorizontalAxis?.Invoke(Input.GetAxis("Horizontal"));
        m_oVerticalAxis?.Invoke(Input.GetAxis("Vertical"));
    }
}
