using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallInstantiate : MonoBehaviour
{
    public GameObject ballPrefab = default;

    // �������ɉ摜�̐ݒ�
    public Sprite[] ballSprites = default;

    public Sprite bomb = default;



    public IEnumerator Spawn(int count)
    {
        for (int i = 0; i < count; i++)
        {
            Vector2 pos = new Vector2(Random.Range(-0.3f, 0.3f), 10f);
            GameObject ball = Instantiate(ballPrefab, pos, Quaternion.identity);

            // �摜�̐ݒ聫
            int ballID = Random.Range(0, ballSprites.Length);  // -1��������-1�̓{���Ƃ���
            // ID��-1�̎��{���𔭓��A����ȊO�̂h�c�͕ς��Ȃ�
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