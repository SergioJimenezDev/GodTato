using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class ShopManager : MonoBehaviour
{
    public static ShopManager Instance { get; private set; }

    public ShopItem[] ShopItems;
    public ShopTemplate[] shopPanels;
    public Button[] myPurchaseBtns;

    private PlayerMovement playerMovement; // Referencia al script PlayerMovement
    private HealthSystem healthSystem; // Referencia al sistema de salud
    private Gun gun; // Referencia a la pistola

    void Start()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }
        Instance = this;

        playerMovement = FindObjectOfType<PlayerMovement>(); // Encontrar el script PlayerMovement en la escena

        healthSystem = HealthSystem.Instance; // Obtener la referencia al sistema de salud
        gun = FindObjectOfType<Gun>(); // Encontrar la instancia de Gun en la escena

        RefreshShopItems(); // Mostrar ShopItems en la UI
    }

    public void RefreshShopItems()
    {
        // Mostrar todos los ShopItems en orden
        for (int i = 0; i < ShopItems.Length; i++)
        {
            int shopItemIndex = i;
            int shopPanelIndex = i;

            // Verificar si el índice está dentro de los límites y si los elementos no son nulos
            if (shopItemIndex < ShopItems.Length && shopPanelIndex < shopPanels.Length &&
                ShopItems[shopItemIndex] != null && shopPanels[shopPanelIndex] != null)
            {
                // Asignar los datos del ShopItem al ShopTemplate correspondiente
                shopPanels[shopPanelIndex].titleText.text = ShopItems[shopItemIndex].title;
                shopPanels[shopPanelIndex].tierText.text = ShopItems[shopItemIndex].tier;
                shopPanels[shopPanelIndex].statsText.text = ShopItems[shopItemIndex].stats;
                shopPanels[shopPanelIndex].descriptionText.text = ShopItems[shopItemIndex].description;
                shopPanels[shopPanelIndex].costText.text = "" + ShopItems[shopItemIndex].baseCost.ToString();

                if (ShopItems[shopItemIndex].itemImage != null)
                {
                    shopPanels[shopPanelIndex].itemImage.texture = ShopItems[shopItemIndex].itemImage;
                }

                shopPanels[shopPanelIndex].gameObject.SetActive(true);
            }
            else
            {
                Debug.LogError("Invalid index or null reference at index " + shopItemIndex);
            }
        }

        // Desactivar los paneles restantes si hay más paneles que items
        for (int i = ShopItems.Length; i < shopPanels.Length; i++)
        {
            if (shopPanels[i] != null)
            {
                shopPanels[i].gameObject.SetActive(false);
            }
        }

        CheckPurchaseable();
    }

    public void CheckPurchaseable()
    {
        int coins = CoinManager.Instance.GetCoins();
        for (int i = 0; i < ShopItems.Length; i++)
        {
            if (myPurchaseBtns[i] != null) // Asegurarse de que myPurchaseBtns[i] no sea nulo
            {
                if (coins >= ShopItems[i].baseCost)
                    myPurchaseBtns[i].interactable = true;
                else
                    myPurchaseBtns[i].interactable = false;
            }
        }
    }

    public void PurchaseItem(int btnNo)
    {
        int coins = CoinManager.Instance.GetCoins();
        if (btnNo >= 0 && btnNo < ShopItems.Length && coins >= ShopItems[btnNo].baseCost)
        {
            CoinManager.Instance.SpendCoins(ShopItems[btnNo].baseCost);
            RefreshShopItems(); // Actualizar la visualización después de la compra

            if (btnNo == 22)
            {
                playerMovement.IncreaseSpeed();
                HealthSystem.Instance.UpdateUIStats();
            }
            else
            {
                // Resto de efectos de ítems como están actualmente
                if (btnNo == 0)
                {
                    healthSystem.IncreaseDamage(5f); // 1-1
                }
                else if (btnNo == 1)
                {
                    gun.IncreaseNormalFireRate(0.5f);
                }
                else if (btnNo == 2)
                {
                    healthSystem.IncreaseMaxHealth(1f);
                }
                else if (btnNo == 3)
                {
                    healthSystem.IncreaseArmor(1f);
                }
                else if (btnNo == 4)
                {
                    healthSystem.IncreaseDamage(8.5f); // 2-1
                }
                else if (btnNo == 5)
                {
                    gun.IncreaseNormalFireRate(1f);
                }
                else if (btnNo == 6)
                {
                    healthSystem.IncreaseMaxHealth(2f);
                }
                else if (btnNo == 7)
                {
                    healthSystem.IncreaseArmor(2f);
                }
                else if (btnNo == 8)
                {
                    healthSystem.IncreaseDamage(13f); // 3-1
                }
                else if (btnNo == 9)
                {
                    gun.IncreaseNormalFireRate(1.5f);
                }
                else if (btnNo == 10)
                {
                    healthSystem.IncreaseMaxHealth(3f);
                }
                else if (btnNo == 11)
                {
                    healthSystem.IncreaseArmor(3f);
                }
                else if (btnNo == 12)
                {
                    healthSystem.IncreaseDamage(18f); // 4-1
                }
                else if (btnNo == 13)
                {
                    gun.IncreaseNormalFireRate(2f);
                }
                else if (btnNo == 14)
                {
                    healthSystem.IncreaseMaxHealth(4f);
                }
                else if (btnNo == 15)
                {
                    healthSystem.IncreaseArmor(4f);
                }
                else if (btnNo == 16)
                {
                    healthSystem.IncreaseDamage(25f);// 5-1
                    gun.IncreaseNormalFireRate(-3.5f);
                }
                else if (btnNo == 17)
                {
                    gun.IncreaseNormalFireRate(3.5f);
                    healthSystem.IncreaseDamage(-25f);
                }
                else if (btnNo == 18)
                {
                    healthSystem.IncreaseMaxHealth(10f);
                    healthSystem.IncreaseArmor(-7f);
                }
                else if (btnNo == 19)
                {
                    healthSystem.IncreaseArmor(7f);
                    healthSystem.IncreaseMaxHealth(-10f);
                }
                else if (btnNo == 20)
                {
                    healthSystem.criticalChancePercentage += 1f;
                }
                else if (btnNo == 21)
                {
                    gun.IncreaseBoostDuration(0.5f);
                }
            }

            // Actualizar UI de PlayerStats después de la compra
            healthSystem.UpdateGraphics(); // Actualiza las gráficas (si es necesario)
            healthSystem.UpdateUIStats(); // Actualiza los textos de UI
        }
    }
}
