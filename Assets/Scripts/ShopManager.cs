using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ShopManager : MonoBehaviour
{
    public GameObject shopItemPrefab; // Assign prefab
    public Transform shopGridParent;  // Assign ShopGrid
    public List<SkinItem> skinItems;

    private List<TMP_Text> allButtonTexts = new List<TMP_Text>();
    private List<Image> allButtonImages = new List<Image>();
    private List<SkinItem> allSkinItems = new List<SkinItem>();
    private List<Image> allIconImages = new List<Image>();

    void Start()
    {
        PopulateShopItems();
    }

    void PopulateShopItems()
    {
        foreach (var item in skinItems)
        {
            GameObject obj = Instantiate(shopItemPrefab, shopGridParent);

            // UI References
            RawImage preview = obj.transform.Find("ShopItem/SkinPreview").GetComponent<RawImage>();
            TMP_Text buttonText = obj.transform.Find("ShopItem/BuyButtonText").GetComponent<TMP_Text>();
            Button buyButton = obj.transform.Find("ShopItem").GetComponent<Button>();
            Button previewButton = obj.transform.Find("ShopItem/SkinPreview").GetComponent<Button>();
            Image buttonImage = buyButton.GetComponent<Image>();
            Image iconImage = obj.transform.Find("StateIcon").GetComponent<Image>();

            // Setup visuals
            preview.texture = item.skinTexture;

            item.isOwned = PlayerPrefs.GetInt("OwnedSkin_" + item.skinName, 0) == 1;
            string equipped = PlayerPrefs.GetString("EquippedSkin", "");

            if (item.isOwned)
            {
                if (item.skinName == equipped)
                {
                    buttonText.text = "Equipped";
                    buttonImage.color = new Color(0f, 0.792f, 1f);
                    iconImage.enabled = false;
                }
                else
                {
                    buttonText.text = "Equip";
                    buttonImage.color = new Color(0.9725f, 0.3294f, 0.9137f);
                    iconImage.enabled = false;
                }
            }
            else
            {
                buttonText.text = $"{item.cost}x";
                buttonImage.color = Color.white;
            }

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
            SoundManager.Instance.ButtonClickSound();
        }

        if (item.isOwned) {
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
                    allButtonImages[i].color = new Color(0f, 0.792f, 1f);
                    allIconImages[i].enabled = false;
                }
                else if (isOwned)
                {
                    allButtonTexts[i].text = "Equip";
                    allButtonImages[i].color = new Color(0.9725f, 0.3294f, 0.9137f);
                    allIconImages[i].enabled = false;
                }
                else
                {
                    allButtonTexts[i].text = $"{allSkinItems[i].cost}x";
                    allButtonImages[i].color = Color.white;
                }
            }
            SoundManager.Instance.ButtonClickSound();
        }
        else {
            SoundManager.Instance.NoMoneySound();
        }
    }
}
