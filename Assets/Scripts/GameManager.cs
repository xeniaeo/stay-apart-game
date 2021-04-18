using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instance
    {
        get
        {
            if (_instance == null)
                Debug.LogError("The GameManager is null.");
            return _instance;
        }
    }

    private void Awake()
    {
        _instance = this;
    }

    public GameObject CharacterPrefab;
    public struct Character
    {
        public bool isSelected;
        public float StartPosX;
        public float StartPosY;
        public GameObject CharacterPrefab;
        public int itemNeeded;

        public Character(float x, float y, GameObject prefab, int itemID)
        {
            this.isSelected = false;
            this.StartPosX = x;
            this.StartPosY = y;
            this.CharacterPrefab = prefab;
            this.itemNeeded = itemID;
        }
    }
    public Sprite[] CharacterSprites;

    private List<Character> Characters = new List<Character>();

    // Define levels
    public struct Level
    {
        public int levelID;
        public int characterCount;
        public int finishScore;

        public Level(int levelID, int characterCount, int finishScore)
        {
            this.levelID = levelID;
            this.characterCount = characterCount;
            this.finishScore = finishScore;
        }
    }
    private List<Level> Levels = new List<Level>();
    public int itemRetrievedCount = 0;
    private int _currentLevelIdx;

    // Static variables
    public static int MaxLevel;
    public static bool AllLevelsCleared;
    public static int LevelReached;
    public static int TotalScore;

    // For level transition
    public GameObject LevelCleared;
    private float _previousRisk = 0f;

    // Start is called before the first frame update
    void Start()
    {
        Level level1 = new Level(1, 3, 100);
        Levels.Add(level1);

        Level level2 = new Level(2, 6, 200);
        Levels.Add(level2);

        Level level3 = new Level(3, 8, 300);
        Levels.Add(level3);

        Level level4 = new Level(4, 9, 400);
        Levels.Add(level4);

        Level level5 = new Level(5, 11, 500);
        Levels.Add(level5);

        Level level6 = new Level(6, 12, 600);
        Levels.Add(level6);

        Level level7 = new Level(7, 13, 800);
        Levels.Add(level7);

        MaxLevel = Levels.Count;
        AllLevelsCleared = false;
        TotalScore = 0;

        _currentLevelIdx = 0;
        LoadLevel(_currentLevelIdx);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            Application.Quit();

        if (Input.GetMouseButtonDown(0))
        {
            Vector2 ray = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            RaycastHit2D hit = Physics2D.Raycast(ray, Vector2.zero);

            if (hit && hit.collider.gameObject.tag == "Character")
            {
                if (!hit.collider.GetComponent<CharacterContoller>().selected)
                {
                    // Deselect every character first
                    for (int i = 0; i < Characters.Count; i++)
                    {
                        Characters[i].CharacterPrefab.GetComponent<CharacterContoller>().selected = false;
                        // Characters[i].CharacterPrefab.transform.Find("SelectedIcon").GetComponent<SpriteRenderer>().enabled = false;
                        Characters[i].CharacterPrefab.GetComponent<SpriteRenderer>().color = Color.white;
                    }
                    hit.collider.GetComponent<CharacterContoller>().selected = true; // And then select this character
                    // hit.transform.Find("SelectedIcon").GetComponent<SpriteRenderer>().enabled = true;
                    hit.collider.GetComponent<SpriteRenderer>().color = new Color(0.9642182f, 0.9716981f, 0.7104397f);
                }
            }
        }

        // When player finish retrieving all items, calculate score and check if there's next level to load
        if (itemRetrievedCount == Levels[_currentLevelIdx].characterCount)
        {
            CalculateScore(_currentLevelIdx);
            AudioManager.Instance.PlayLevelUpSound(_currentLevelIdx);

            _currentLevelIdx += 1;
            if (Levels.Count > _currentLevelIdx)
            {
                StartCoroutine(WaitAndLoadLevel(1.8f, _currentLevelIdx));
            }
            else
                GameOver("Finish");
        }
    }

    IEnumerator WaitAndLoadLevel(float sec, int level)
    {
        LevelCleared.SetActive(true);
        yield return new WaitForSeconds(sec);
        LevelCleared.SetActive(false);
        LoadLevel(level);
    }

    private void LoadLevel(int index)
    {
        Reset();
        int characterCount = Levels[index].characterCount;
        LevelReached = Levels[index].levelID;

        for (int i = 0; i < characterCount; i++)
        {
            // Instantiate characters in specified range (X: -7.0f ~ 7.0f; Y: -2.5f ~ 2.5f)
            float x = -7.0f + (Random.Range((14.0f / characterCount) - 0.2f, (14.0f / characterCount) + 0.2f) * i);
            float y;
            if (i % 3 == 0)
                y = Random.Range(-2.5f, -1.0f);
            else if (i % 3 == 1)
                y = Random.Range(-1.0f, 1.0f);
            else
                y = Random.Range(1.0f, 2.5f);

            GameObject newCharacterPrefab = Instantiate(CharacterPrefab, new Vector3(x, y, 0), Quaternion.identity);
            newCharacterPrefab.gameObject.GetComponent<SpriteRenderer>().sprite = CharacterSprites[i % CharacterSprites.Length]; // Assign character sprite

            // Randomly choose one item that the character needs, and assign it to the character
            int itemID = ItemManager.Instance.AssignItem();
            newCharacterPrefab.gameObject.GetComponent<CharacterContoller>().AssignItemToCharacter(itemID);

            Character newCharacter = new Character(x, y, newCharacterPrefab, itemID);
            Characters.Add(newCharacter);
        }
        ItemManager.Instance.SpawnItem();

        AudioManager.Instance.BackgroundMusicState(true);
    }

    public void GameOver(string EndState)
    {
        if (EndState == "Lose")
        {
            // Debug.Log("Virus spread, game over!");
        }
        else if (EndState == "Finish")
        {
            AllLevelsCleared = true;
        }
        StartCoroutine(WaitAndLoadGameOver());
    }

    IEnumerator WaitAndLoadGameOver()
    {
        yield return new WaitForSeconds(1.0f);
        SceneManager.LoadScene("GameOver", LoadSceneMode.Single);
    }

    private void Reset()
    {
        // Clear Characters list
        Characters.Clear();

        // Clear Item list
        ItemManager.Instance.ResetItem();

        // Reset number of items retrieved
        itemRetrievedCount = 0;

        // Destroy all active objects with tag "Character"
        GameObject[] characterObj;
        characterObj = GameObject.FindGameObjectsWithTag("Character");

        if (characterObj != null)
        {
            for (var i = 0; i < characterObj.Length; i++)
                Destroy(characterObj[i]);
        }
    }

    private void CalculateScore(int levelIdx)
    {
        float _currentRisk = UIManager.Instance.GetRiskBarValue();
        float _riskIncreased = Mathf.Round((_currentRisk - _previousRisk) * 100.0f) * 0.01f;
        _previousRisk = _currentRisk;
        int riskControlBonus = Mathf.RoundToInt((1.0f - _riskIncreased) * 500.0f);

        UIManager.Instance.PostToRiskUpdate("Risk increased: " + _riskIncreased.ToString(), 1.7f);
        UIManager.Instance.PostToScoreUpdate("Risk control bonus +" + riskControlBonus.ToString(), 1.7f);

        // TotalScore += Levels[levelIdx].finishScore;
        TotalScore += riskControlBonus;
    }

    public void AddScore(int score)
    {
        TotalScore += score;
    }
}
