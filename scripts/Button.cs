using UnityEngine;
using UnityEngine.UI;

public class ButtonHoverExample : MonoBehaviour
{
    public Button button;
    public Color normalColor = Color.white;
    public Color highlightColor = new Color(0.5f, 0.5f, 1f); // Azul claro
    public Text buttonText; // Variable para almacenar el componente Text

    void Start()
    {
        // Asignar colores iniciales
        button.image.color = normalColor;

        // Verificar si se asign� correctamente el componente Text
        if (buttonText != null)
        {
            buttonText.color = Color.black; // Color de texto normal
        }
        else
        {
            Debug.LogError("No se ha asignado el componente Text para el bot�n.");
        }
    }

    public void OnPointerEnter()
    {
        // Cambiar a color destacado cuando el rat�n entra
        button.image.color = highlightColor;

        // Cambiar color de texto a blanco si el componente Text est� asignado
        if (buttonText != null)
        {
            buttonText.color = Color.white;
        }
        else
        {
            Debug.LogError("No se ha asignado el componente Text para el bot�n.");
        }
    }

    public void OnPointerExit()
    {
        // Volver al color normal cuando el rat�n sale
        button.image.color = normalColor;

        // Restaurar color de texto normal si el componente Text est� asignado
        if (buttonText != null)
        {
            buttonText.color = Color.black;
        }
        else
        {
            Debug.LogError("No se ha asignado el componente Text para el bot�n.");
        }
    }
}
