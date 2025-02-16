using UnityEngine;
using UnityEngine.UI;

public class ButtonEscapeHandler : MonoBehaviour
{
    public Button targetButton; // Referencia al botón que queremos manejar

    void Start()
    {
        if (targetButton == null)
        {
            Debug.LogError("El botón de destino no está asignado en el script ButtonEscapeHandler.");
            return;
        }
    }

    void Update()
    {
        // Detecta la pulsación de la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Simula un clic en el botón
            targetButton.onClick.Invoke();
        }
    }
}
