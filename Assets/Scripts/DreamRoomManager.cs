using UnityEngine;
using System.Collections.Generic;

public class DreamRoomManager : MonoBehaviour
{
    [System.Serializable]
    public class DecorationItem
    {
        public string id;
        public string name;
        public GameObject prefab;
        public int cost;
        public bool isUnlocked;
    }

    public List<DecorationItem> availableDecorations = new List<DecorationItem>();
    public List<GameObject> placedItems = new List<GameObject>();

    public void UnlockItem(string id)
    {
        var item = availableDecorations.Find(x => x.id == id);
        if (item != null && !item.isUnlocked)
        {
            if (GameManager.Instance.HasEnoughDust(item.cost))
            {
                GameManager.Instance.SpendDreamDust(item.cost);
                item.isUnlocked = true;
                Debug.Log($"Unlocked: {item.name}");
            }
        }
    }

    public void PlaceItem(string id, Vector3 position)
    {
        var item = availableDecorations.Find(x => x.id == id);
        if (item != null && item.isUnlocked)
        {
            GameObject obj = Instantiate(item.prefab, position, Quaternion.identity);
            placedItems.Add(obj);
            Debug.Log($"Placed: {item.name}");
        }
    }

    public void ClearRoom()
    {
        foreach (var item in placedItems)
        {
            Destroy(item);
        }
        placedItems.Clear();
    }
}
