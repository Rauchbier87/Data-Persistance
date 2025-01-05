using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CanvasMenu : MonoBehaviour
{
    public TMP_InputField PlayerName;
    public NameManager nameManager;
    private string saveName;
    // Start is called before the first frame update
    void Start()
    {
        nameManager = GameObject.Find("NameHolder").GetComponent<NameManager>();
    }

    
    public void NameEntered()
    {
//code executed for endEdit() for input
        saveName = PlayerName.text;
    }

    public void StartClicked()
    {
        nameManager.playerName = saveName;
        SceneManager.LoadScene(1);
    }

    public void Quit()
    {
#if UNTIY_STANDALONE
        Application.Quit();
#else
        //UnityEditor.EditorApplication.isPlaying = false; //find out why this wont compile----------
#endif
    }

    [System.Serializable]
    class SaveScore
    {
        public string nameHigh;
        public int pointHigh;
    }

    public void ResetHigh()
    {
        SaveScore data = new SaveScore();

        data.nameHigh = "";
        data.pointHigh = 0;
        string json = JsonUtility.ToJson(data);
        File.WriteAllText(Application.persistentDataPath + "savefile.json", json);
    }
}
