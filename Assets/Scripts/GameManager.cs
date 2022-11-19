using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using System.IO;
using UnityEngine.SceneManagement;
using System;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;

    [SerializeField] TextMeshProUGUI bestScoreText;
    [SerializeField] TMP_InputField nameInput;

    public string name;
    public string bestScoreName;
    public int bestScore;

    private SaveData saveData;

    private void Awake()
    {
        if (Instance is null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }

        Load();
    }

    [Serializable]
    public class SaveData
    {
        public string name;
        public string bestScoreName;
        public int bestScore = 0;
    }

    public void StartGame()
    {
        name = nameInput.text;
        SceneManager.LoadScene(1);
    }

    public void Save(int score)
    {
        saveData.name = name;

        if (score > bestScore)
        {
            bestScore = score;
            bestScoreName = name;
            saveData.bestScoreName = name;
            saveData.bestScore = score;
        }

        string json = JsonUtility.ToJson(saveData);

        File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void Load()
    {
        string path = Application.persistentDataPath + "/savefile.json";

        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            saveData = JsonUtility.FromJson<SaveData>(json);

            bestScoreText.SetText($"Best Score : {saveData.bestScoreName} : {saveData.bestScore}");
            nameInput.text = saveData.name;
            bestScoreName = saveData.bestScoreName;
            bestScore = saveData.bestScore;
        }
        else
        {
            saveData = new SaveData();
            bestScoreText.SetText($"Best Score : : 0");
        }
    }

    public void Quit()
    {
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }
}
