using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Home : MonoBehaviour
{
    public Button StartGameBtn;
    public Button EndBtn;
    public Button AboutBtn;

    public Image ExitCharacter;
    private Vector3 characterStartPos;

    // Start is called before the first frame update
    void Start()
    {
        StartGameBtn.GetComponent<Button>().onClick.AddListener(() => StartGame());
        EndBtn.GetComponent<Button>().onClick.AddListener(() => End());
        AboutBtn.GetComponent<Button>().onClick.AddListener(() => About());

        characterStartPos = ExitCharacter.transform.position;
    }

    private void StartGame()
    {
        SceneManager.LoadScene("Tutorial", LoadSceneMode.Single);
    }

    private void End()
    {
        StartCoroutine(Exit());
    }

    private void About()
    {
        SceneManager.LoadScene("About", LoadSceneMode.Single);
    }

    IEnumerator Exit()
    {
        float startTime = Time.time;
        while (Time.time < startTime + 2.0f)
        {
            ExitCharacter.transform.position = Vector3.Lerp(characterStartPos, new Vector3(1566.0f, -800.0f, 0.0f), (Time.time - startTime) / 2.0f);
            yield return null;
        }
        //yield return new WaitForSeconds(3.0f);
        Application.Quit();
    }
}
