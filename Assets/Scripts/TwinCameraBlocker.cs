using UnityEngine;

public class TwinCameraBlocker : MonoBehaviour
{
    RectTransform m_oRectTransform;
    public bool m_bBlocked;
    public bool m_bEnabled;

    private void Start()
    {
        m_oRectTransform = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        m_bEnabled = true;
    }

    private void OnDisable()
    {
        m_bEnabled = false;
    }

    private void Update()
    {
        if (ExtensionMethods.PointInsideRect(Input.mousePosition, m_oRectTransform))
        {
            m_bBlocked = true;
        }
        else
        {
            m_bBlocked = false;
        }
    }
}
