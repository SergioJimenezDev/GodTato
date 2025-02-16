using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class QualityController : MonoBehaviour
{
    public TMP_Dropdown dropdown;
    public int calidad;

    void Start()
    {
        calidad = PlayerPrefs.GetInt("NumeroDeCalidad", 3);
        dropdown.value = calidad;
        AjustarCalidad();
    }

    public void AjustarCalidad()
    {
        QualitySettings.SetQualityLevel(dropdown.value);
        PlayerPrefs.SetInt("NumeroDeCalidad", dropdown.value);
        calidad = dropdown.value;
    }
}
