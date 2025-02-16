using UnityEngine;
using UnityEngine.UI;

public class ButtonEscapeHandler : MonoBehaviour
{
    public Button targetButton; // Referencia al bot�n que queremos manejar

    void Start()
    {
        if (targetButton == null)
        {
            Debug.LogError("El bot�n de destino no est� asignado en el script ButtonEscapeHandler.");
            return;
        }
    }

    void Update()
    {
        // Detecta la pulsaci�n de la tecla Escape
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            // Simula un clic en el bot�n
            targetButton.onClick.Invoke();
        }
    }
}
