using UnityEngine;
using System.Collections;

[ExecuteInEditMode]
public class WorldCurver : MonoBehaviour
{
	[Range(-0.1f, 0.1f)]
	public float curveStrength = 0.01f;
    int m_CurveStrengthID;

    public void OnEnable()
    {
        m_CurveStrengthID = Shader.PropertyToID("_CurveStrength");
        Shader.SetGlobalFloat(m_CurveStrengthID, curveStrength);
    }
}
