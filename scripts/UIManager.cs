using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI totalCoinsText;

    void Start()
    {
        // Asegurarse de que totalCoinsText esté asignado en el Inspector
        if (totalCoinsText == null)
        {
            Debug.LogError("TotalCoinsText no está asignado en el inspector.");
            return;
        }

        // Llamar a la función del CoinManager para actualizar el texto total de monedas
        if (CoinManager.Instance != null)
        {
            CoinManager.Instance.UpdateTotalCoinsText(totalCoinsText);
        }
        else
        {
            Debug.LogError("CoinManager instance not found!");
        }
    }
}
