using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class GameSystem : MonoBehaviour
{
    bool gameOver = default;
    public BallInstantiate ballInstantiate = default;
    public List<Ball> removeBalls = new List<Ball>();
    bool IsDragging,IsMain;
    Ball currentDraggingBall;

    public Text timerText;

    // ボールはリストで受け取っているため、↓のものでは受け取れない たぶん
    Ball ball;

    ScoreManagement scoreManagement;
    
    // スコアエフェクト　獲得スコアを都度表示（表示後すぐ消去）
    public Text scoreText;
    public GameObject scoreEffect;
    private GameObject retryOrExit;
    int score,timeCount;

    void Start()
    {
        

        timeCount = 10;
        score = 0;
        retryOrExit = GameObject.Find("ResultPanel");
        retryOrExit.SetActive(false);

        
        SoundController.instance.PlayBGM(SoundController.SelectSound.Main);
               
        AddScore(0);
        StartCoroutine(ballInstantiate.Spawn(40));
        StartCoroutine(UpdateTimerText());
    }

    void Update()
    {
        

        if (gameOver)
        {
            return;       // この関数での処理はここで全て終了
        }

        if (Input.GetMouseButtonDown(0))
        {
            DragDown();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            DragUp();
        }
        else if (IsDragging)
        {
            Dragging();
        }
    }

    

    public void AddScore(int point)
    {
        score += point;
        string str = "Score:";
        scoreText.text = str + score.ToString(); 
    }

    void DragDown()
    {
        // マウスによるオブジェクトの判定
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

        if (hit && hit.collider.GetComponent<Ball>())
        {
            // ボムなら周囲を含めて爆破
            // それ以外は通常通り
            Ball ball = hit.collider.GetComponent<Ball>();
            if (ball.id == -1) // (ball.IsBomb())
                               // 一見-1だと分かりにくい この場合Bool関数を使い-1かどうかを判定する、という処理にした方がよい
            {
                Explosion(ball);

            }
            else
            {
                //removeBalls.Add(ball);
                RemoveBallList(ball);
                IsDragging = true;
            }
        }
    }

    void Dragging()
    {
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);
        if (hit && hit.collider.GetComponent<Ball>())
        {
            // ・同じ種類＆距離が近かったらListに追加
            // 何と？ →　現在ドラッグしているオブジェクトと

            Ball ball = hit.collider.GetComponent<Ball>();

            // 同じ種類
            if (ball.id == currentDraggingBall.id)
            {
                // 距離が近い
                float distance = Vector2.Distance(ball.transform.position, currentDraggingBall.transform.position);
                if (distance < 1.3)
                {
                    RemoveBallList(ball);
                }
            }
        }
    }

    void DragUp()
    {
        // ボムの処理。クリック後周囲のボールを巻き込んで爆破
        // それ以外は通常通り        
        int ballCount = removeBalls.Count;


        // 選択中のボールが３つ以上の場合
        if (ballCount >= 3)
        {
            for (int i = 0; i < ballCount; i++)
            {              
        //        Destroy(removeBalls[i]);
                removeBalls[i].Explosion();           // エフェクト                                                
            }
            StartCoroutine(ballInstantiate.Spawn(ballCount));
            int score = ballCount * ParamsSO.Entity.score;
            AddScore(score);

            ScoreEffect(removeBalls[removeBalls.Count-1].transform.position, score);
            SoundController.instance.PlaySE(SoundController.SelectSE.Destroy);
        }

        // 選択中のすべてのボールサイズを戻す
        for (int i = 0; i < ballCount; i++)
        {
            removeBalls[i].transform.localScale = Vector3.one;
            removeBalls[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
        removeBalls.Clear();
        IsDragging = false;
    }


    // リストにボールがなければ追加
    void RemoveBallList(Ball ball)
    {
       
        currentDraggingBall = ball;
        // ballは既にランダム関数が設定されている       
        if (removeBalls.Contains(ball) == false)
        {
            SoundController.instance.PlaySE(SoundController.SelectSE.Touch);
            // リスト追加時、ボールを大きくする
            // 色を変える
            currentDraggingBall.transform.localScale = Vector3.one * 1.3f;
            currentDraggingBall.GetComponent<SpriteRenderer>().color = Color.cyan;
            removeBalls.Add(ball);
        }
    }

    void Explosion(Ball bomb)
    {
        List<Ball> explosionList = new List<Ball>();
        Collider2D[] hitObj = Physics2D.OverlapCircleAll(bomb.transform.position, 2);
        for (int i = 0; i < hitObj.Length; i++)
        {
            Ball ball = hitObj[i].GetComponent<Ball>();    // ボールだったら爆破リストに追加
            if (ball)
            {
                explosionList.Add(ball);
            }
        }

        // 爆破
        for (int i = 0; i < explosionList.Count; i++)
        {
            explosionList[i].Explosion();
            
        }
        StartCoroutine(ballInstantiate.Spawn(explosionList.Count));
        int score = explosionList.Count * ParamsSO.Entity.score;
        AddScore(score);
        ScoreEffect(bomb.transform.position,score);
        SoundController.instance.PlaySE(SoundController.SelectSE.Destroy);
    }

    // エフェクトに必要な情報　位置・得点（数字）
    void ScoreEffect(Vector2 position,int score)
    {
       // エフェクト生成時にゲームオブジェクトを取得
       // つまりそのコンポーネントも取得できる


       GameObject obj = Instantiate(scoreEffect, position, Quaternion.identity);
       ScoreEffect effect = obj.GetComponent<ScoreEffect>();
        effect.Show(score);
    }

    IEnumerator UpdateTimerText()
    {
        while (timeCount > 0)
        {
            yield return new WaitForSeconds(1);
            timeCount--;
            timerText.text = timeCount.ToString();

            if(timeCount == 0)
            {
                Debug.Log("time up");
                retryOrExit.SetActive(true);
                gameOver = true;    // いつゲームオーバーになるのか
                
            }
        }
    }
}
