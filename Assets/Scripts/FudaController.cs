using System.Collections;
using System.Collections.Generic;
using UnityEngine;

//柄
public enum HanaType
{
    January,
    February,
    March,
    April,
    May,
    June,
    July,
    August,
    September,
    October,
    November,
    December,

}
public class FudaController : MonoBehaviour
{
    public const float Width = 0.7f;
    public const float Height = 0.09f;
    //このカードのマーク
    public HanaType Suit;
    //月ごとの架空に振り分けた番号
    public int No;
    //どのプレイヤーのカードか
    public int PlayerNo;

    public int Index;

    public Vector3 HandPosition;
    //public Vector3 handpos = new Vector3(0 , 0,);
    public Vector2Int IndexPosition;


    public Color HanaColor;//使うかどうか

    public bool isFrontUp;//表向きかどうかの判定

    public bool selecting = false;

    public bool select = false;

    
    // Start is called before the first frame update
    void Start()
    {
        // GameObject selectlight = transform.Find("Front/Triangle").gameObject;
        // selectlight.gameObject.SetActive(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnMouseEnter()
    {
        
        //Debug.Log(Suit);
        if(selecting) return;
        if(Input.mousePosition.y < 213 && Input.mousePosition.x < 1379)
        {
            selecting = true;
            Vector3 pos = this.transform.position;
            pos.z += 0.2f;
            this.transform.position = pos;
        } 
        
        // if(Input.mousePosition.y > 250) return;
        // selecting = true;
        // Vector3 pos = this.transform.position;
        // pos.z += 0.2f;
        // this.transform.position = pos;
        // GameObject selectlight = transform.Find("Front/Triangle").gameObject;
        // selectlight.gameObject.SetActive(true);

        
    }

    // void OnMouseDown()
    // {
    //     if(selecting)
    //     {
    //         selecting = false;
    //     }
    //     else if(!selecting)
    //     {
    //         selecting = true;
    //     }
    // }

    void OnMouseExit()
    {
        // Debug.Log("hogehoge");
        // Debug.Log(select);
        if(!selecting && !select) return;
        if(selecting && select) return;
        
        //if(Input.mousePosition.y > 250) return;
        //Debug.Log(this.transform.position);
        Vector3 pos = this.transform.position;
        
        pos.z -= 0.2f;
        this.transform.position = pos;
        //Debug.Log(this.transform.position);
        selecting = false;
        // GameObject selectlight = transform.Find("Front/Triangle").gameObject;
        // selectlight.gameObject.SetActive(false);
        
        
    }

    public void FlipFuda(bool frontup = true)
    {
        float anglex = -90;
        
        // if(this.transform.position.z < 1.35)
        // {
        //    anglez = 0;
        //    Debug.Log(this.transform.position.z);
        // }
        if(!frontup)
        {
            anglex = 90;
            
        }
        //Debug.Log(this.transform.position);

        isFrontUp = frontup;//状態を記憶
        transform.eulerAngles = new Vector3(anglex,0,-180);
    }
    // public void Turn1p(bool vector = true)
    // {
    //     float anglez = 0;
    //     if(!vector)
    //     {
    //         anglez = 0;
    //         Debug.Log("Turn1p");
    //     }
    //     transform.eulerAngles = new Vector3(0,0,anglez);
        
        
    // }
}
