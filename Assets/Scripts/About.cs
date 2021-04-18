using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class About : MonoBehaviour
{
    public Button BackBtn;

    // Start is called before the first frame update
    void Start()
    {
        BackBtn.GetComponent<Button>().onClick.AddListener(() => Back());
    }

    private void Back()
    {
        SceneManager.LoadScene("Home", LoadSceneMode.Single);
    }
}
