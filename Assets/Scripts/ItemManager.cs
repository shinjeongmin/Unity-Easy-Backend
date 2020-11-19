using System;
using System.Collections;
using SimpleJSON;
using UnityEngine;
using UnityEngine.UI;

public class ItemManager : MonoBehaviour
{ 
    Action<string> _createItemsCallback;
    
    // Use this for initialization
    void Start()
    {
        _createItemsCallback = (jsonArrayString) =>
        {
            StartCoroutine(CreateItemsRoutine(jsonArrayString)); 
        };
        
        CreateItems();
    }

    public void CreateItems()
    {
        string userId = Main.Instance.UserInfo.UserID;
        StartCoroutine(Main.Instance.Web.GetItemsIDs(userId, _createItemsCallback));
    }

    IEnumerator CreateItemsRoutine(string jsonArrayString)
    {
        //Parsing json array string as an array
        JSONArray jsonArray = JSON.Parse(jsonArrayString) as JSONArray;

        for (int i = 0; i < jsonArray.Count; i++)
        {
            //Create local variables
            bool isDone = false; // are we done downloading?
            string itemId = jsonArray[i].AsObject["itemID"];
            string id = jsonArray[i].AsObject["ID"];
            
            JSONObject itemInfoJson = new JSONObject();    
            
            //Create a callback to get the information from Web.cs
            //DB서버에서 아이템 정보를 성공적으로 불러왔는지 여부를 isDone으로 알려줌
            Action<string> getItemInfoCallback = (itemInfo) =>
            {
                isDone = true;
                JSONArray tempArray = JSON.Parse(itemInfo) as JSONArray;
                itemInfoJson = tempArray[0].AsObject;
            };
            
            //Wait until Web.cs calls the callback we passed as parameter
            //Web.cs스크립트에서 아이템 정보를 서버에서 불러오도록 호출한다.
            StartCoroutine(Main.Instance.Web.GetItem(itemId, getItemInfoCallback));
            
            //Wait until the callbac1k is called from WEB (info finished downloading)
            //아이템 정보를 성공적으로 불러와서 isDone이 true가 될 때까지 기다림. 
            yield return new WaitUntil(() => isDone == true);
            
            //Instantiate GameObject (item prefab)
            GameObject itemGo = Instantiate(Resources.Load("Prefabs/Item") as GameObject);
            Item item = itemGo.AddComponent<Item>();

            item.ID = id;
            item.ItemID = itemId;
            
            itemGo.transform.SetParent(this.transform);
            itemGo.transform.localScale = Vector3.one;
            itemGo.transform.localPosition = Vector3.zero;
            
            //Fill Information
            itemGo.transform.Find("Name").GetComponent<Text>().text = itemInfoJson["name"];
            itemGo.transform.Find("Price").GetComponent<Text>().text = itemInfoJson["price"];
            itemGo.transform.Find("Description").GetComponent<Text>().text = itemInfoJson["description"];
            
            // TODO:
            // 1. Get image version and send it to Image Manager

            int imgVer = itemInfoJson["imgVer"].AsInt;
            
            byte[] bytes = ImageManager.Instance.LoadImage(itemId, imgVer);

            // Download from web
            if (bytes.Length == 0)
            {
                //Create a callback to get the SPRITE from Web.cs
                Action<byte[]> getItemIconCallback = (downloadedBytes) =>
                {
                    Sprite sprite = ImageManager.Instance.BytesToSprite(downloadedBytes);
                    itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
                    ImageManager.Instance.SaveImage(itemId, downloadedBytes, imgVer);
                    ImageManager.Instance.SaveVersionJson();
                };
                StartCoroutine(Main.Instance.Web.GetItemIcon(itemId, getItemIconCallback));   
            }
            //Load from device
            else {
                Sprite sprite = ImageManager.Instance.BytesToSprite(bytes);
                itemGo.transform.Find("Image").GetComponent<Image>().sprite = sprite;
            }

            //Set Sell Button
            itemGo.transform.Find("SellButton").GetComponent<Button>().onClick.AddListener(() =>
            {
                string iId = itemId;
                string idInInventory = id;
                string userId = Main.Instance.UserInfo.UserID;
                StartCoroutine(Main.Instance.Web.SellItem(idInInventory,itemId, userId));
            });
            
            //continue to the next item
            //반복문으로 다른 ID의 아이템들을 호출
        }


        yield return null;
    }
}
