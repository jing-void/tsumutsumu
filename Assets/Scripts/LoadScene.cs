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

            //Rayでクリックしたオブジェクトを取得
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit = new RaycastHit();

            if (Physics.Raycast(ray, out hit))  // (rayの開始地点, 方向)
            {

                //clickedGameObjectがクリックされたオブジェクト
                clickedGameObject = hit.collider.gameObject;

                //マテリアルを変更
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
