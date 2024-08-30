using System.Collections;
using System.Collections.Generic;
using Cinemachine;
using UnityEngine;

public class CameraTarget : MonoBehaviour
{
    public CinemachineFreeLook thirdPersonCam;

    // Update is called once per frame
    void Update()
    {
        this.transform.rotation *= Quaternion.AngleAxis(thirdPersonCam.m_YAxis.m_InputAxisValue, Vector3.up);
        this.transform.rotation *= Quaternion.AngleAxis(thirdPersonCam.m_XAxis.m_InputAxisValue, Vector3.right);

    }
}
