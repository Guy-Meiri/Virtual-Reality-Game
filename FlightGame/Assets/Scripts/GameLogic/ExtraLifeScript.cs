using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ExtraLifeScript : MonoBehaviour
{
    [SerializeField]
    private GameObject m_ExtraLifeManagerObject;

    private ExtraLifeManager m_ExtraLifeManagerScript;

    void Start()
    {
        if (m_ExtraLifeManagerObject != null)
        {
            m_ExtraLifeManagerScript = m_ExtraLifeManagerObject.GetComponent<ExtraLifeManager>();
        }
    }

    public GameObject ExtraLifeManagerObject
    {
        get
        {
            return m_ExtraLifeManagerObject;
        }
        set
        {
            m_ExtraLifeManagerObject = value;
            m_ExtraLifeManagerScript = m_ExtraLifeManagerObject.GetComponent<ExtraLifeManager>();
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Contains("MainCamera"))
        {
            m_ExtraLifeManagerScript.OnExtraLifeTaken(this);
        }
    }
}
