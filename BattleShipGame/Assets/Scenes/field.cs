using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class field : MonoBehaviour
{
	[SerializeField]
	private GameObject masu;
	[SerializeField]
	private GameObject koma;
    void Start()
    {
		int id = 0;
		var sz = masu.GetComponent<SpriteRenderer>().sprite.texture;
		Debug.Log(sz.width);
		
		// マス目の作成
		for (int y = 0; y < 10; ++y)
		{
			for(int x = 0; x < 10; ++x)
			{
				var obj = GameObject.Instantiate(masu, new Vector3(x*0.9f+0.75f+4.25f, -y*0.9f-0.75f, 1),Quaternion.Euler(0, 0, 0));
				obj.GetComponent<button>().SetID = id;
				++id;
			}
		}

        // 駒とりあえずひとつ
        var hune = GameObject.Instantiate(koma, new Vector3(0.75f + 4.25f, -0.75f, 0), Quaternion.Euler(0, 0, 0));
        hune.GetComponent<koma>().SetID = id;
    }

    // Update is called once per frame
    void Update()
    {
    }
}
