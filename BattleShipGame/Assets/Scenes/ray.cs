using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ray : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
		GameObject clickedGameObject;

		if (Input.GetMouseButtonDown(0))
		{
			clickedGameObject = null;

			Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
			RaycastHit hit = new RaycastHit();

			if (Physics.Raycast(ray, out hit))
			{
				clickedGameObject = hit.collider.gameObject;
				//hit.collider.GetComponent<button>().Return();
				if (hit.collider.GetComponent<koma>() != null)
				{
					hit.collider.GetComponent<koma>().Return();
				}
				else
				{
					hit.collider.GetComponent<button>().Return();
					hit.collider.GetComponent<koma>().SetPos = hit.collider.GetComponent<button>().GetPos;
				}
				
			}
		}
	}
}
