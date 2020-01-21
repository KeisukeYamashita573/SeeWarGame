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
    [SerializeField]
    private GameObject userMng = default;

	private bool namespaceFlag;

    // Start is called before the first frame update
    void Start()
    {
        login.SetActive(true);
        newMake.SetActive(true);
		nameField.gameObject.SetActive(false);

		newMake.GetComponent<Button>().onClick.AddListener(CreateUer);
		login.GetComponent<Button>().onClick.AddListener(Login);

		namespaceFlag = false;
	}

    // Update is called once per frame
    void Update()
    {
		// 新規作成が押されたら
		if (namespaceFlag)
		{
			if (Input.GetKeyDown(KeyCode.Return))
			{
				//  ユーザーネームが入力されていれば通す(空白は通さない)
				if (!string.IsNullOrWhiteSpace(nameField.text))
				{
					userMng.GetComponent<user>().SetUserName(nameField.text);
					login.SetActive(true);
					newMake.SetActive(true);
					nameField.gameObject.SetActive(false);
					namespaceFlag = false;
				}
			}
		}
    }

	public void CreateUer()
	{
		login.SetActive(false);
		newMake.SetActive(false);
		nameField.gameObject.SetActive(true);
		namespaceFlag = true;
	}

	public void Login()
	{
		SceneManager.LoadScene("GamePlaying");

	}
}
