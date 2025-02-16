using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class FullscreenController : MonoBehaviour
{
    public Toggle toggle;
    public TMP_Dropdown resolucionesDropdown;
    Resolution[] resoluciones;

    void Start()
    {
        // Verificar si la pantalla está en modo fullscreen y establecer el estado del toggle
        if (Screen.fullScreen)
        {
            toggle.isOn = true;
        }
        else
        {
            toggle.isOn = false;
        }

        // Obtener y mostrar las resoluciones disponibles
        RevisarResolucion();

        // Establecer la resolución por defecto
        EstablecerResolucionPorDefecto();
    }

    public void ActivarPantallaCompleta(bool pantallaCompleta)
    {
        Screen.fullScreen = pantallaCompleta;
    }

    public void RevisarResolucion()
    {
        resoluciones = Screen.resolutions;
        resolucionesDropdown.ClearOptions();
        List<string> opciones = new List<string>();
        int resolucionActual = 0;

        for (int i = 0; i < resoluciones.Length; i++)
        {
            string opcion = resoluciones[i].width + " x " + resoluciones[i].height;
            opciones.Add(opcion);

            if (Screen.fullScreen && resoluciones[i].width == Screen.currentResolution.width && resoluciones[i].height == Screen.currentResolution.height)
            {
                resolucionActual = i;
            }
        }
        resolucionesDropdown.AddOptions(opciones);
        resolucionesDropdown.value = resolucionActual;
        resolucionesDropdown.RefreshShownValue();
    }

    public void CambiarResolucion(int indiceResolucion)
    {
        Resolution resolucion = resoluciones[indiceResolucion];
        Screen.SetResolution(resolucion.width, resolucion.height, Screen.fullScreen);
    }

    private void EstablecerResolucionPorDefecto()
    {
        bool encontrada1920x1080 = false;

        // Buscar la resolución 1920x1080 y establecerla como la resolución por defecto si está disponible
        for (int i = 0; i < resoluciones.Length; i++)
        {
            if (resoluciones[i].width == 1920 && resoluciones[i].height == 1080)
            {
                resolucionesDropdown.value = i;
                CambiarResolucion(i); // Aplicar la resolución por defecto
                encontrada1920x1080 = true;
                break;
            }
        }

        // Si no se encontró la resolución 1920x1080, seleccionar la resolución más alta disponible
        if (!encontrada1920x1080 && resoluciones.Length > 0)
        {
            int indiceResolucionMasAlta = resoluciones.Length - 1;
            resolucionesDropdown.value = indiceResolucionMasAlta;
            CambiarResolucion(indiceResolucionMasAlta); // Aplicar la resolución más alta disponible
        }
    }
}
