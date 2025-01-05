using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainManager : MonoBehaviour
{
    public Brick BrickPrefab;
    public int LineCount = 6;
    public Rigidbody Ball;
    public GameObject BallModel;

    public Text ScoreText;
    public Text bestScore;
    public TMP_InputField tMP_Inputfield;
    public GameObject GameOverText;
    public Text PlayerText;
    public NameManager nameManager;
    
    private bool m_Started = false;
    private int m_Points;
    
    private bool m_GameOver = false;

    private string nameHigh;
    private int pointHigh;
    public int brickNum = 36;
    
    // Start is called before the first frame update
    void Start()
    {
        //null reference below-----------------------------------------
        PlayerText = GameObject.Find("PlayerText").GetComponent<Text>();
        nameManager = GameObject.Find("NameHolder").GetComponent<NameManager>();

        PlayerText.text += " " + nameManager.playerName;
        const float step = 0.6f;
        int perLine = Mathf.FloorToInt(4.0f / step);
        
        int[] pointCountArray = new [] {1,1,2,2,5,5};
        for (int i = 0; i < LineCount; ++i)
        {
            for (int x = 0; x < perLine; ++x)
            {
                Vector3 position = new Vector3(-1.5f + step * x, 2.5f + i * 0.3f, 0);
                var brick = Instantiate(BrickPrefab, position, Quaternion.identity);
                brick.PointValue = pointCountArray[i];
                brick.onDestroyed.AddListener(AddPoint);
            }
        }
        LoadHigh();
        if (nameHigh.Equals("")) 
        {
            bestScore.text = "High Score: ";
        }
        else { bestScore.text = "High Score: " + nameHigh + "- " + pointHigh; }
    }

    private void Update()
    {
        if (brickNum == 0) //bricks not accurately detected when hit-----------------------
        {
            GameOver();
            Destroy(BallModel);
        }
        if (!m_Started)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                m_Started = true;
                float randomDirection = Random.Range(-1.0f, 1.0f);
                Vector3 forceDir = new Vector3(randomDirection, 1, 0);
                forceDir.Normalize();

                Ball.transform.SetParent(null);
                Ball.AddForce(forceDir * 2.0f, ForceMode.VelocityChange);
            }
        }
        else if (m_GameOver)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
    }

    void AddPoint(int point)
    {
        m_Points += point;
        ScoreText.text = $"Score : {m_Points}";
    }

    public void GameOver()
    {
//call savehigh() and enter name if high score-----------------------------
        m_GameOver = true;
        SaveHigh();
        GameOverText.SetActive(true);
    }

    [System.Serializable]
    class SaveScore
    {
        public string nameHigh;
        public int pointHigh;
    }

    public void SaveHigh()
    {
        SaveScore data = new SaveScore();

//save only if greater than current      
            if (m_Points > pointHigh)
            {
//code to get player name to save
                data.nameHigh = nameManager.playerName;
                data.pointHigh = m_Points;
                string json = JsonUtility.ToJson(data);
                File.WriteAllText(Application.persistentDataPath + "savefile.json", json);
            }
    }

    public void LoadHigh()
    {
        string path = Application.persistentDataPath + "savefile.json";
        if (File.Exists(path))
        {
            string json = File.ReadAllText(path);
            SaveScore data = JsonUtility.FromJson<SaveScore>(json);
            nameHigh = data.nameHigh;
            pointHigh = data.pointHigh;
        }
    }

    public void Menu()
    {
        SceneManager.LoadScene(0); 
    }
}
