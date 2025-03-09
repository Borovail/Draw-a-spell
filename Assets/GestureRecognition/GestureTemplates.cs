using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Assets.Scripts;
using UnityEngine;

public class CoroutineMonoBehaviour : Singleton<CoroutineMonoBehaviour>
{

}

[Serializable]
public class GestureTemplates
{
    private static GestureTemplates instance;

    public static GestureTemplates Get()
    {
        if (instance == null)
        {
            instance = new GestureTemplates();
            instance.Load();
        }

        return instance;
    }


    public List<RecognitionManager.GestureTemplate> RawTemplates = new List<RecognitionManager.GestureTemplate>();
    public List<RecognitionManager.GestureTemplate> ProceedTemplates = new List<RecognitionManager.GestureTemplate>();

    public List<RecognitionManager.GestureTemplate> GetTemplates()
    {
        return ProceedTemplates;
    }

    public void RemoveAtIndex(int indexToRemove)
    {
        ProceedTemplates.RemoveAt(indexToRemove);
        RawTemplates.RemoveAt(indexToRemove);
    }

    public RecognitionManager.GestureTemplate[] GetRawTemplatesByName(string name)
    {
        return RawTemplates.Where(template => template.Name == name).ToArray();
    }

    public void Save()
    {
        string path = Application.persistentDataPath + "/SavedTemplates.json";
        string potion = JsonUtility.ToJson(this);
        File.WriteAllText(path, potion);
    }

    private void Load()
    {
        string path;

#if UNITY_WEBGL
        path = Application.streamingAssetsPath + "/SavedTemplates.json";
#else
    path = Application.persistentDataPath + "/SavedTemplates.json";
#endif

        Debug.Log("Loading file from: " + path);
        CoroutineMonoBehaviour.Instance.StartCoroutine(LoadFromWeb(path));
    }

    private IEnumerator LoadFromWeb(string path)
    {
        using (UnityEngine.Networking.UnityWebRequest www = UnityEngine.Networking.UnityWebRequest.Get(path))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityEngine.Networking.UnityWebRequest.Result.Success)
            {
                string json = www.downloadHandler.text;
                GestureTemplates data = JsonUtility.FromJson<GestureTemplates>(json);

                RawTemplates = data.RawTemplates ?? new List<RecognitionManager.GestureTemplate>();
                ProceedTemplates = data.ProceedTemplates ?? new List<RecognitionManager.GestureTemplate>();

                Debug.Log("Templates loaded successfully!");
            }
            else
            {
                Debug.LogError("Failed to load templates: " + www.error);
            }
        }
    }

}