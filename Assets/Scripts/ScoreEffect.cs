using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScoreEffect : MonoBehaviour
{
    // スコアに応じて表示を変更
    // 上に上げる

    [SerializeField] Text text;

    public void Show(int score)
    {
        text.text = score.ToString();
        StartCoroutine(MoveUp());
    }


    IEnumerator MoveUp()
    {
        for (int i = 0; i < 15; i++)
        {
            yield return new WaitForSeconds(0.01f);
            transform.Translate(0, 0.1f, 0);
            Destroy(text,0.5f);
        }
    }
}
