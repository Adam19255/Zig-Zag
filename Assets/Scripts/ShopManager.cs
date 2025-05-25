using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopItemPrefab; // Assign prefab
    public Transform shopGridParent;  // Assign ShopGrid
    public List<SkinItem> skinItems;

    // Icon sprites
    private Sprite iconEquipped, iconEquip, iconBuy;

    private List<TMP_Text> allButtonTexts = new List<TMP_Text>();
    private List<Image> allButtonImages = new List<Image>();
    private List<SkinItem> allSkinItems = new List<SkinItem>();
    private List<Image> allIconImages = new List<Image>();

    void Awake()
    {
        // Load icons from Resources/Images/Icons/
        iconEquipped = Resources.Load<Sprite>("Icons/v");
        iconEquip    = Resources.Load<Sprite>("Icons/play");
        iconBuy      = Resources.Load<Sprite>("Icons/GemImage");
    }

    void Start()
    {
        PopulateShopItems();
    }

    void PopulateShopItems()
    {
        foreach (var item in skinItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopGridParent);

            // Background tint by tier
            Image bgImage = obj.GetComponent<Image>();
            Color tierColor = Color.white;
            switch (item.tier.ToLower())
            {
                case "common":    tierColor = new Color(0.8f, 0.8f, 0.8f); break;
                case "rare":      tierColor = new Color(0.4f, 0.6f, 1f); break;
                case "epic":      tierColor = new Color(0.7f, 0.3f, 0.9f); break;
                case "legendary": tierColor = new Color(1f, 0.65f, 0f); break;
            }
            bgImage.color = tierColor;

            // UI References
            RawImage preview = obj.transform.Find("SkinPreview").GetComponent<RawImage>();
            TMP_Text tierText = obj.transform.Find("TierText").GetComponent<TMP_Text>();
            TMP_Text buttonText = obj.transform.Find("BuyButton/BuyButtonText").GetComponent<TMP_Text>();
            Button buyButton = obj.transform.Find("BuyButton").GetComponent<Button>();
            Button previewButton = obj.transform.Find("SkinPreview").GetComponent<Button>();
            Image buttonImage = buyButton.GetComponent<Image>();
            Image iconImage = obj.transform.Find("BuyButton/StateIcon").GetComponent<Image>();

            // Setup visuals
            preview.texture = item.skinTexture;
            tierText.text = item.tier;

            item.isOwned = PlayerPrefs.GetInt("OwnedSkin_" + item.skinName, 0) == 1;
            string equipped = PlayerPrefs.GetString("EquippedSkin", "");

            if (item.isOwned)
            {
                if (item.skinName == equipped)
                {
                    buttonText.text = "Equipped";
                    buttonImage.color = Color.green;
                    iconImage.sprite = iconEquipped;
                }
                else
                {
                    buttonText.text = "Equip";
                    buttonImage.color = new Color(0.2f, 0.4f, 1f);
                    iconImage.sprite = iconEquip;
                }
            }
            else
            {
                buttonText.text = $"{item.cost} x Gems";
                buttonImage.color = Color.white;
                iconImage.sprite = iconBuy;
            }

            iconImage.enabled = true;

            // Button actions
            buyButton.onClick.AddListener(() => BuySkin(item, buttonText));
            previewButton.onClick.AddListener(() => BuySkin(item, buttonText));

            // Track for refresh
            allButtonTexts.Add(buttonText);
            allButtonImages.Add(buttonImage);
            allIconImages.Add(iconImage);
            allSkinItems.Add(item);
        }
    }

    void BuySkin(SkinItem item, TMP_Text buttonText)
    {
        int currentGems = PlayerPrefs.GetInt("Gems", 0);

        if (!item.isOwned && currentGems >= item.cost)
        {
            // Buy skin
            currentGems -= item.cost;
            PlayerPrefs.SetInt("Gems", currentGems);
            PlayerPrefs.SetInt("OwnedSkin_" + item.skinName, 1);
            PlayerPrefs.Save();

            item.isOwned = true;
            PowerUpUI.Instance.UpdateGems(currentGems);
        }

        if (item.isOwned)
        {
            // Equip skin
            PlayerPrefs.SetString("EquippedSkin", item.skinName);
            PlayerPrefs.Save();

            for (int i = 0; i < allSkinItems.Count; i++)
            {
                bool isEquipped = allSkinItems[i].skinName == item.skinName;
                bool isOwned = allSkinItems[i].isOwned;

                if (isEquipped)
                {
                    allButtonTexts[i].text = "Equipped";
                    allButtonImages[i].color = Color.green;
                    allIconImages[i].sprite = iconEquipped;
                }
                else if (isOwned)
                {
                    allButtonTexts[i].text = "Equip";
                    allButtonImages[i].color = new Color(0.2f, 0.4f, 1f);
                    allIconImages[i].sprite = iconEquip;
                }
                else
                {
                    allButtonTexts[i].text = $"{allSkinItems[i].cost} x Gems";
                    allButtonImages[i].color = Color.white;
                    allIconImages[i].sprite = iconBuy;
                }

                allIconImages[i].enabled = true;
            }

            Debug.Log($"Equipped {item.skinName}");
        }
        else
        {
            Debug.Log("Not enough gems!");
        }
    }
}
