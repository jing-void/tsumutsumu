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

    // �{�[���̓��X�g�Ŏ󂯎���Ă��邽�߁A���̂��̂ł͎󂯎��Ȃ� ���Ԃ�
    Ball ball;

    ScoreManagement scoreManagement;
    
    // �X�R�A�G�t�F�N�g�@�l���X�R�A��s�x�\���i�\���シ�������j
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
            return;       // ���̊֐��ł̏����͂����őS�ďI��
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
        // �}�E�X�ɂ��I�u�W�F�N�g�̔���
        Vector2 mousePoint = Camera.main.ScreenToWorldPoint(Input.mousePosition);
        RaycastHit2D hit = Physics2D.Raycast(mousePoint, Vector2.zero);

        if (hit && hit.collider.GetComponent<Ball>())
        {
            // �{���Ȃ���͂��܂߂Ĕ��j
            // ����ȊO�͒ʏ�ʂ�
            Ball ball = hit.collider.GetComponent<Ball>();
            if (ball.id == -1) // (ball.IsBomb())
                               // �ꌩ-1���ƕ�����ɂ��� ���̏ꍇBool�֐����g��-1���ǂ����𔻒肷��A�Ƃ��������ɂ��������悢
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
            // �E������ށ��������߂�������List�ɒǉ�
            // ���ƁH ���@���݃h���b�O���Ă���I�u�W�F�N�g��

            Ball ball = hit.collider.GetComponent<Ball>();

            // �������
            if (ball.id == currentDraggingBall.id)
            {
                // �������߂�
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
        // �{���̏����B�N���b�N����͂̃{�[������������Ŕ��j
        // ����ȊO�͒ʏ�ʂ�        
        int ballCount = removeBalls.Count;


        // �I�𒆂̃{�[�����R�ȏ�̏ꍇ
        if (ballCount >= 3)
        {
            for (int i = 0; i < ballCount; i++)
            {              
        //        Destroy(removeBalls[i]);
                removeBalls[i].Explosion();           // �G�t�F�N�g                                                
            }
            StartCoroutine(ballInstantiate.Spawn(ballCount));
            int score = ballCount * ParamsSO.Entity.score;
            AddScore(score);

            ScoreEffect(removeBalls[removeBalls.Count-1].transform.position, score);
            SoundController.instance.PlaySE(SoundController.SelectSE.Destroy);
        }

        // �I�𒆂̂��ׂẴ{�[���T�C�Y��߂�
        for (int i = 0; i < ballCount; i++)
        {
            removeBalls[i].transform.localScale = Vector3.one;
            removeBalls[i].GetComponent<SpriteRenderer>().color = Color.white;
        }
        removeBalls.Clear();
        IsDragging = false;
    }


    // ���X�g�Ƀ{�[�����Ȃ���Βǉ�
    void RemoveBallList(Ball ball)
    {
       
        currentDraggingBall = ball;
        // ball�͊��Ƀ����_���֐����ݒ肳��Ă���       
        if (removeBalls.Contains(ball) == false)
        {
            SoundController.instance.PlaySE(SoundController.SelectSE.Touch);
            // ���X�g�ǉ����A�{�[����傫������
            // �F��ς���
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
            Ball ball = hitObj[i].GetComponent<Ball>();    // �{�[���������甚�j���X�g�ɒǉ�
            if (ball)
            {
                explosionList.Add(ball);
            }
        }

        // ���j
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

    // �G�t�F�N�g�ɕK�v�ȏ��@�ʒu�E���_�i�����j
    void ScoreEffect(Vector2 position,int score)
    {
       // �G�t�F�N�g�������ɃQ�[���I�u�W�F�N�g���擾
       // �܂肻�̃R���|�[�l���g���擾�ł���


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
                gameOver = true;    // ���Q�[���I�[�o�[�ɂȂ�̂�
                
            }
        }
    }
}
