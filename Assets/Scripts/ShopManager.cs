using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopItemPrefab; // Assign prefab
    public Transform shopGridParent;  // Assign ShopGrid
    public List<SkinItem> skinItems;

    void Start()
    {
        PopulateShopItems();
    }

    void PopulateShopItems()
    {
        foreach (var item in skinItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopGridParent);
            
            RawImage preview = obj.transform.Find("SkinPreview").GetComponent<RawImage>();
            TMP_Text tierText = obj.transform.Find("TierText").GetComponent<TMP_Text>();
            TMP_Text priceText = obj.transform.Find("PriceText").GetComponent<TMP_Text>();
            TMP_Text buttonText = obj.transform.Find("BuyButton/BuyButtonText").GetComponent<TMP_Text>();
            Button buyButton = obj.transform.Find("BuyButton").GetComponent<Button>();

            preview.texture = item.skinTexture;
            tierText.text = item.tier;
            priceText.text = $"{item.cost} Gems";

            item.isOwned = PlayerPrefs.GetInt("OwnedSkin_" + item.skinName, 0) == 1;

            if (item.isOwned)
            {
                string equipped = PlayerPrefs.GetString("EquippedSkin", "");
                buttonText.text = item.skinName == equipped ? "Equipped" : "Equip";
            }
            else
            {
                buttonText.text = "Buy";
            }

            buyButton.onClick.AddListener(() => BuySkin(item, buttonText));
        }
    }

    void BuySkin(SkinItem item, TMP_Text buttonText)
    {
        int currentGems = PlayerPrefs.GetInt("Gems", 0);

        if (!item.isOwned && currentGems >= item.cost)
        {
            currentGems -= item.cost;
            PlayerPrefs.SetInt("Gems", currentGems);
            PlayerPrefs.Save();
            PowerUpUI.Instance.UpdateGems(currentGems);

            item.isOwned = true;
            PlayerPrefs.SetInt("OwnedSkin_" + item.skinName, 1);
            PlayerPrefs.Save();

            buttonText.text = "Equip";
            Debug.Log($"Bought {item.skinName}");
        }
        else if (item.isOwned)
        {
            PlayerPrefs.SetString("EquippedSkin", item.skinName);
            PlayerPrefs.Save();
            buttonText.text = "Equipped";
            Debug.Log($"Equipped {item.skinName}");
        }
        else
        {
            Debug.Log("Not enough gems!");
        }
    }
}
