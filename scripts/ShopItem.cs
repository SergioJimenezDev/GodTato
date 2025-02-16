using UnityEngine;

[CreateAssetMenu(fileName = "NewShopItem", menuName = "Shop/ShopItem")]
public class ShopItem : ScriptableObject
{
    public string title;
    public Texture2D itemImage;
    public string tier;
    public string stats;
    public string description;
    public int baseCost;
}
