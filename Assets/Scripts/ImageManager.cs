using System;
using System.IO;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleJSON;

public class ImageManager : MonoBehaviour
{
    public static ImageManager Instance;

    private string _basePath;
    string _versionJsonPath;
    private JSONObject _versionJson;
    
    //TODO:
    // 1. Create Version Dictionary (JSON)
    // 2. Add Version in DB
    // 3. Get Version in our Item info  (PHP)
    // 4. Save and Compare Version
    // 5. Check Version Json and decide if we should download or load from device
    
    // 6. Save and Load Json File

    private void Start()
    {
        if (Instance != null)
        {
            GameObject.Destroy(this);
            return;
        }
        Instance = this;

        _basePath = Application.persistentDataPath + "/Images/";
        if (!Directory.Exists(_basePath)) {
            Directory.CreateDirectory(_basePath);
        }
        
        _versionJson = new JSONObject();
        _versionJsonPath = _basePath + "VersionJson";

        //Load Json
        if (File.Exists(_versionJsonPath))
        {
            string jsonString = File.ReadAllText(_versionJsonPath);
            _versionJson = JSON.Parse(jsonString) as JSONObject;
        }
    }

    bool ImageExists(string name)
    {
        return File.Exists(_basePath + name);
    }

    public void SaveImage(string name, byte[] bytes, int imgVer) {
        File.WriteAllBytes(_basePath + name, bytes);
        UpdateVersionJson(name, imgVer);
    }

    /// <summary>
    /// Returns empty if image is not found or if not up to date
    /// </summary>
    /// <param name="name"></param>
    /// <param name="imgVer"></param>
    /// <returns></returns>
    public byte[] LoadImage(string name, int imgVer)
    {
        byte[] bytes = new byte[0];
        
        //TODO : Compare version
        if (!IsImageUpToDate(name, imgVer))
        {
            return bytes;
        }
        
        if (ImageExists(name))
        {
            return File.ReadAllBytes(_basePath + name);
        } return bytes;
    }

    public Sprite BytesToSprite(byte[] bytes)
    {
        //Create texture2D
        Texture2D texture = new Texture2D(2, 2);
        texture.LoadImage(bytes);
                
        //Create sprite (to be placed in UI)
        Sprite sprite = Sprite.Create(texture, new Rect(0, 0, texture.width, texture.height), new Vector2(0.5f,0.5f));

        return sprite;
    }

    void UpdateVersionJson(string name, int ver)
    {
        _versionJson[name] = ver;
    }

    bool IsImageUpToDate(string name, int ver)
    {
        if (_versionJson[name] != null)
        {
            return _versionJson[name].AsInt == ver;
        }

        return false;
    }

    public void SaveVersionJson()
    {
        File.WriteAllText(_versionJsonPath, _versionJson.ToString());
    }
}
