using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Text;

public class userName : MonoBehaviour
{
    [SerializeField]
    private InputField nameField = default;
    [SerializeField]
    private GameObject login = default;
    [SerializeField]
    private GameObject newMake = default;

    // Start is called before the first frame update
    void Start()
    {
        login.SetActive(false);
        newMake.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Return))
        {
            if(nameField.text == "a")
            {
                //SceneManager.LoadScene("GamePlaying");
                login.SetActive(true);
                newMake.SetActive(true);
                nameField.gameObject.SetActive(false);
            }
        }
    }
}
