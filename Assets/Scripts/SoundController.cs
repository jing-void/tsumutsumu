using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundController : MonoBehaviour
{    

    public enum SelectSound
    {
        // �\�ߔԍ�������U���Ă��� �z��Ŏ󂯎��ۂ�(int)��int�^�ɕϊ�
        Title,    // 0 
        Main      // 1
    };

    public enum SelectSE
    {
        Touch,
        Destroy
    };

    [SerializeField] AudioSource audioSource;
    [SerializeField] AudioClip[] audioClips;   // �����̋Ȃ��������ߔz����g��
    [SerializeField] AudioSource seSource;
    [SerializeField] AudioClip[] seClips;
    
    SoundController soundController;
    

    private void Start()
    {
        PlayBGM(SelectSound.Title);
        soundController = GetComponent<SoundController>();
    }

   

    // �V���O���g�����g�p
    // �Q�[�����ɂ����P�����̂���
    // �V�[�����ς���Ă��j�󂳂�Ȃ�
    // �ǂ̃R�[�h������A�N�Z�X���₷��

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
