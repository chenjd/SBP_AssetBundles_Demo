using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestInstantiate : MonoBehaviour {


	private void OnGUI()
	{
		if(GUI.Button(new Rect(0, 0, 100, 100), "test_SBP"))
		{
			var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/S_cube/ab_2");

			var asset = ab.LoadAsset<GameObject>("Assets/Cube_S.prefab");
			GameObject gameObject = Instantiate(asset);

		}

		if (GUI.Button(new Rect(0, 200, 100, 100), "test_Legacy"))
        {
			var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/L_cube/ab_1");

			var asset = ab.LoadAsset<GameObject>("Cube_L");
			GameObject gameObject = Instantiate(asset);
        }
	}
}


