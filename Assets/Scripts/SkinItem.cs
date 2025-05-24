using UnityEngine;

[System.Serializable]
public class SkinItem {
    public string skinName;
    public Texture skinTexture;
    public string tier;
    public int cost;
    public bool isOwned = false;
}
