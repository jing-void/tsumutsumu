using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{    

    public enum SelectSound
    {
        // 予め番号が割り振られている 配列で受け取る際は(int)でint型に変換
        Title,    // 0 
        Main      // 1
    };

    public enum SelectSE
    {
        Touch,
        Destroy
    };

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;   // 複数の曲を扱うため配列を使う
    [SerializeField] AudioSource seSource;
    [SerializeField] AudioClip[] seClips;
    
    SoundController soundController;
    

    private void Start()
    {
        PlayBGM(SelectSound.Title);
        soundController = GetComponent<SoundController>();
    }

   

    // シングルトンを使用
    // ゲーム内にただ１つだけのもの
    // シーンが変わっても破壊されない
    // どのコードからもアクセスしやすい

    public static SoundController instance;

    
    private void Awake()
    {
        if(instance == null)
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    public void PlaySE(SelectSE se)
    {
        seSource.PlayOneShot(seClips[(int)se]);
        
    }


    public void PlayBGM(SelectSound bgm)
    {
        
        audioSource.clip = audioClips[(int)bgm];
                
        audioSource.Play();
    }

    public void StopMainBGM()
    {
        audioSource.Stop();
    }
  
}
