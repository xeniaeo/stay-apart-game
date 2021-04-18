using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Tutorial : MonoBehaviour
{
    public Button GoBtn;

    // Start is called before the first frame update
    void Start()
    {
        GoBtn.GetComponent<Button>().onClick.AddListener(() => LoadGame());
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void LoadGame()
    {
        SceneManager.LoadScene("Main", LoadSceneMode.Single);
    }
}
