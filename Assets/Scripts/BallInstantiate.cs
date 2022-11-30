using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInstantiate : MonoBehaviour
{
    public GameObject ballPrefab = default;

    // 生成時に画像の設定
    public Sprite[] ballSprites = default;

    public Sprite bomb = default;



    public IEnumerator Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-0.3f, 0.3f), 10f);
            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);

            // 画像の設定↓
            int ballID = Random.Range(0, ballSprites.Length);  // -1を実装＆-1はボムとする
            // IDが-1の時ボムを発動、それ以外のＩＤは変わらない
            if (Random.Range(0, 100) < 5)
            {
                ballID = -1;
                ball.GetComponent<SpriteRenderer>().sprite = bomb;
            }
            else
            {
                ball.GetComponent<SpriteRenderer>().sprite = ballSprites[ballID];
            }
            ball.GetComponent<Ball>().id = ballID;
            yield return new WaitForSeconds(0.05f);
        }       
    }


}        