using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.U2D;

public class TestSpriteAtlas : MonoBehaviour
{

    #region Field

    public SpriteRenderer sr;

    #endregion


    #region Methods

    private void OnGUI()
    {
        if (GUI.Button(new Rect(0, 0, 100, 100), "test_SBP"))
        {
            var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/S_Atlas/bundle");

            var asset = ab.LoadAsset<SpriteAtlas>("Assets/atlas.spriteatlas");

            sr.sprite = asset.GetSprite("1");
        }

        if (GUI.Button(new Rect(0, 200, 100, 100), "test_Legacy"))
        {

            var ab = AssetBundle.LoadFromFile(Application.streamingAssetsPath + "/L_Atlas/atlas_ab");

            var asset = ab.LoadAsset<SpriteAtlas>("atlas.spriteatlas");

            sr.sprite = asset.GetSprite("1");
        }
    }

    #endregion
}


