using UnityEngine;

public class TogglePlayerStats : MonoBehaviour
{
    public GameObject playerStats;  // Referencia al GameObject PlayerStats que quieres mostrar/ocultar
    public GameObject shopDef;      // Referencia al GameObject ShopDef que quieres verificar
    public GameObject noShop;       // Referencia al GameObject NoShop que quieres mostrar/ocultar

    private bool statsVisible = false;  // Variable para controlar si las estad�sticas est�n visibles o no

    void Update()
    {
        // Verificar si se ha presionado la tecla Tabulador
        if (Input.GetKeyDown(KeyCode.Tab))
        {
            // Verificar si ShopDef est� activo
            bool shopDefActive = shopDef.activeSelf;

            // Cambiar el estado de visibilidad de las estad�sticas
            statsVisible = !statsVisible;
            playerStats.SetActive(statsVisible);

            // Pausar o reanudar el tiempo del juego seg�n el estado de visibilidad
            if (!shopDefActive)
            {
                Time.timeScale = statsVisible ? 0f : 1f; // Pausar el tiempo si statsVisible es true, de lo contrario reanudarlo
            }

            // Cambiar el estado de visibilidad de NoShop solo si ShopDef no est� activo
            noShop.SetActive(!statsVisible && !shopDefActive);
        }
    }
}
