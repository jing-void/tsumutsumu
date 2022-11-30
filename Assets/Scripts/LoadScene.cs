using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LoadScene : MonoBehaviour
{
    Material MaterialA;
    GameObject clickedGameObject;

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            clickedGameObject = null;

            //Ray�ŃN���b�N�����I�u�W�F�N�g���擾
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))  // (ray�̊J�n�n�_, ����)
            {

                //clickedGameObject���N���b�N���ꂽ�I�u�W�F�N�g
                clickedGameObject = hit.collider.gameObject;

                //�}�e���A����ύX
                clickedGameObject.GetComponent<Renderer>().material = MaterialA;
            }
        }

        if (Input.GetMouseButtonUp(0))
        {
            SceneManager.LoadScene("Main");
        }
    }
    

   

    public void DraggingColor()
    {
        gameObject.GetComponent<Renderer>().material.color = Color.white;
    }

}
