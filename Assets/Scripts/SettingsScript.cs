using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class SettingsScript : MonoBehaviour
{
    public static string playerName;
    public static Color playerColor;

    public GameObject inputName;
    public GameObject inputColor;
    public Button buttonApply;

    // Start is called before the first frame update
    void Start()
    {
        inputName.GetComponent<InputField>().text = playerName;
        inputColor.GetComponent<InputField>().text = playerColor.ToString();
        buttonApply.onClick.AddListener(delegate
        {
            playerName = inputName.GetComponent<InputField>().text;
            SceneManager.LoadScene(0, LoadSceneMode.Single);
        });
    }
}
