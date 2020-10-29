using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;

public class Web : MonoBehaviour
{
    void Start()
    {
        //StartCoroutine(GetUsers("http://localhost/UnityBackendTutorial/GetUsers.php"));
        //StartCoroutine(Login("testuser", "123456"));
        //StartCoroutine(RegisterUser("testuser3", "123456"));
    }

    // public void ShowUserItems()
    // {
    //     StartCoroutine(GetItemsIDs(Main.Instance.UserInfo.UserID, ));
    // }

    IEnumerator GetDate(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    IEnumerator GetUsers(string uri)
    {
        using (UnityWebRequest webRequest = UnityWebRequest.Get(uri))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            string[] pages = uri.Split('/');
            int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log(pages[page] + ": Error: " + webRequest.error);
            }
            else
            {
                Debug.Log(pages[page] + ":\nReceived: " + webRequest.downloadHandler.text);
            }
        }
    }
    public IEnumerator Login(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/UnityBackendTutorial/Login.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text );
                Main.Instance.UserInfo.SetCredentials(username, password);
                Main.Instance.UserInfo.SetId(www.downloadHandler.text);
                
                if (www.downloadHandler.text.Contains("Wrong Credentials") ||
                    www.downloadHandler.text.Contains("Username does not exists"))
                {
                    Debug.Log("Try Again");
                }
                else {
                    //If we logged in correctly
                    Main.Instance.UserProfile.SetActive(true);
                    Main.Instance.Login.gameObject.SetActive(false);
                }
            }
        }
    }
    public IEnumerator RegisterUser(string username, string password)
    {
        WWWForm form = new WWWForm();
        form.AddField("loginUser", username);
        form.AddField("loginPass", password);

        using (UnityWebRequest www = UnityWebRequest.Post("http://127.0.0.1/UnityBackendTutorial/RegisterUser.php", form))
        {
            yield return www.SendWebRequest();

            if (www.isNetworkError || www.isHttpError)
            {
                Debug.Log(www.error);
            }
            else
            {
                Debug.Log(www.downloadHandler.text);
            }
        }
    }
    
    public IEnumerator GetItemsIDs(string userID, System.Action<string> callback) {
        WWWForm form = new WWWForm();
        form.AddField("userID", userID);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1/UnityBackendTutorial/GetItemsIDs.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // string[] pages = uri.Split('/');
            // int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log( "Received: " + webRequest.downloadHandler.text);
                string jsonArray = webRequest.downloadHandler.text;
                
                callback(jsonArray);
            }
        }
    }
    
    public IEnumerator GetItem(string itemID, System.Action<string> callback) {
        WWWForm form = new WWWForm();
        form.AddField("itemID", itemID);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1/UnityBackendTutorial/GetItem.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            // string[] pages = uri.Split('/');
            // int page = pages.Length - 1;

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log( "Received: " + webRequest.downloadHandler.text);
                string jsonArray = webRequest.downloadHandler.text;

                callback(jsonArray);
            }
        }
    }
    
    public IEnumerator SellItem(string ID, string itemID, string userID) {
        WWWForm form = new WWWForm();
        form.AddField("ID", ID);
        form.AddField("itemID", itemID);
        form.AddField("userID", userID);
        
        using (UnityWebRequest webRequest = UnityWebRequest.Post("http://127.0.0.1/UnityBackendTutorial/SellItem.php", form))
        {
            // Request and wait for the desired page.
            yield return webRequest.SendWebRequest();

            if (webRequest.isNetworkError)
            {
                Debug.Log("Error: " + webRequest.error);
            }
            else
            {
                Debug.Log( "Received: " + webRequest.downloadHandler.text);
            }
        }
    }
}
