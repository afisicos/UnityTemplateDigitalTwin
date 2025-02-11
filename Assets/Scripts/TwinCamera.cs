using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class TwinCamera : MonoBehaviour
{
    public Transform m_oCameraTransform;
    static private Camera m_oCamera;
    Vector3 m_vTargetPos;
    public GameObject m_oReference;
    public float m_fMinZoom, m_fMaxZoom;
    public float m_fMinxRot, m_fMaxxRot;
    public float m_fXRot, m_fYRot, m_fZPos;
    float m_fXPan, m_fYPan, m_fXPanK, m_fYPanK;
    public float m_fMoveSpeed;
    public bool m_bControlEnabled, m_bIsRotating, m_bIsMousePanning;
    InputController m_oInputController;
    private List<TwinCameraBlocker> m_tBlockers = new List<TwinCameraBlocker>();

    void Start()
    {
        m_oInputController = FindObjectOfType<InputController>();
        m_oCamera = GetComponentInChildren<Camera>();
        m_oCamera.transform.LookAt(transform.position);
        m_vTargetPos = transform.position;
        m_fXRot = transform.localEulerAngles.x;
        m_fYRot = transform.localEulerAngles.y;
        m_fZPos = m_oCamera.transform.localPosition.z;


        m_oInputController.m_oRightMouseButtonHold += Orbit;
        m_oInputController.m_oRightMouseButtonUp += () => m_bIsRotating = false;
        m_oInputController.m_oLeftMouseButtonPressed += SetMousePanning;
        m_oInputController.m_oLeftMouseButtonHold += Panning;
        m_oInputController.m_oLeftMouseButtonUp += ResetMousePanning;
        m_oInputController.m_oMouseDeltaX += UpdateMouseXDelta;
        m_oInputController.m_oMouseDeltaY += UpdateMouseYDelta;
        m_oInputController.m_oHorizontalAxis += UpdateKeyboardXDelta;
        m_oInputController.m_oVerticalAxis += UpdateKeyboardYDelta;
        m_oInputController.m_oTouchPinch += DoZoom;
        m_oInputController.m_oMouseScroll += DoZoom;

        m_oReference.transform.localEulerAngles = new Vector3(-m_fXRot, 0, 0);

        foreach (TwinCameraBlocker oBlocker in FindObjectsOfType<TwinCameraBlocker>(true))
        {
            m_tBlockers.Add(oBlocker);
        }

    }

    public void CheckBlockStatus()
    {
        foreach (TwinCameraBlocker oBlocker in m_tBlockers)
        {
            if (oBlocker.m_bEnabled && oBlocker.m_bBlocked)
            {
                m_bControlEnabled = false;
                return;
            }
        }
        m_bControlEnabled = true;
    }

    public void ForcePosition(Vector3 v)
    {
        m_vTargetPos = v;
    }

    public void ToggleLayerMask(int _iLayer) // Toggle del bit específico de la capa que queremos cambiar
    {
        m_oCamera.cullingMask ^= 1 << _iLayer;
    }

    public void RemoveLayerMask(int _iLayer)
    {
        m_oCamera.cullingMask |= 1 << _iLayer;
    }
    public void AddLayerMask(int _iLayer)
    {
        m_oCamera.cullingMask &= ~(1 << _iLayer);
    }
    public bool IsLayerRendered(int layer)
    {
        return ((m_oCamera.cullingMask & (1 << layer)) != 0);
    }

    public void Reorientation()
    {
        m_fYRot = 0;
        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
    }

    public void ForceFocusOnPoint(Vector3 pos)
    {
        m_vTargetPos = pos;
        transform.position = pos;
    }


    public void FocusOn(Vector3 _oPoint)
    {
        m_vTargetPos = _oPoint;
    }

 

    void ResetMousePanning()
    {
        m_bIsMousePanning = false;
        m_fXPan = 0;
        m_fYPan = 0;
    }

    void SetMousePanning()
    {
        m_bIsMousePanning = true;
    }

    void UpdateKeyboardXDelta(float f)
    {
        m_fXPanK = f;
        Panning();
    }
    void UpdateKeyboardYDelta(float f)
    {
        m_fYPanK = f;
        Panning();
    }
    void UpdateMouseXDelta(float f)
    {
        if (m_bIsRotating)
            m_fYRot += f;
        if (m_bIsMousePanning)
            m_fXPan = -f;
    }
    void UpdateMouseYDelta(float f)
    {
        if (m_bIsRotating)
            m_fXRot -= f;
        if (m_bIsMousePanning)
            m_fYPan = -f;
    }

    void Orbit()
    {
        if (!m_bControlEnabled)
            return;

        if (!m_bIsRotating)
            m_bIsRotating = true;

        if (m_fXRot > m_fMaxxRot) m_fXRot = m_fMaxxRot;
        if (m_fXRot < m_fMinxRot) m_fXRot = m_fMinxRot;

        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
        m_oReference.transform.localEulerAngles = new Vector3(-m_fXRot, 0, 0);
    }

    public void Rotate()
    {
        transform.eulerAngles = new Vector3(m_fXRot, m_fYRot, 0);
    }


    void Panning()
    {

        if (!m_bControlEnabled)
            return;

        float fFinalMoveSpeed = 0;

        if ((m_fXPanK != 0 || m_fYPanK != 0) || m_fXPan != 0 || m_fYPan != 0)
        {

            float fFactor = ZoomFactor() / (float)200;
            if (Screen.height > Screen.width)
            {
                fFactor *= 0.2f;
            }
            fFinalMoveSpeed = m_fMoveSpeed * fFactor;
        }

        m_vTargetPos = m_vTargetPos + fFinalMoveSpeed * (m_fXPanK + m_fXPan) * m_oReference.transform.right + fFinalMoveSpeed * (m_fYPanK + m_fYPan) * m_oReference.transform.forward;
        transform.position = Vector3.Lerp(transform.position, m_vTargetPos, Time.deltaTime * 10);


    }

    void DoZoom(float f)
    {
        if (!m_bControlEnabled)
            return;

        float fDelta = f * ZoomFactor();

        if ((fDelta > 0 && m_fZPos < -m_fMinZoom) || fDelta < 0 && m_fZPos > -m_fMaxZoom)
        {
            m_fZPos = m_fZPos + fDelta;

            if (m_fZPos < -m_fMaxZoom)
                m_fZPos = -m_fMaxZoom;
            if (m_fZPos > -m_fMinZoom)
                m_fZPos = -m_fMinZoom;
        }
        UpdateZoom();
    }

    float ZoomFactor()
    {
        return (-m_oCamera.transform.localPosition.z);
    }

    public void UpdateZoom()
    {
        float fZSmooth = Mathf.Lerp(m_oCamera.transform.localPosition.z, m_fZPos, Time.deltaTime * 5);
        Vector3 vNewPos = m_oCamera.transform.localPosition;
        vNewPos.z = fZSmooth;
        m_oCamera.transform.localPosition = vNewPos;
    }


    private void Update()
    {
        CheckBlockStatus();
    }

}
