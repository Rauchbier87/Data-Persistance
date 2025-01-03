using System.Collections;
using System.Collections.Generic;
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
        //PlayerName = (TMP_InputField)GameObject.Find("Canvas").GetComponent("Name");
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
}
