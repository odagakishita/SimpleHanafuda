using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using UnityEngine.SceneManagement;
using TMPro;
public class SceneDirector : MonoBehaviour
{
    [SerializeField] FudasDirector fDirector;

    [SerializeField] Text textYaku;
    [SerializeField] Text turn;

    [SerializeField] TextMeshProUGUI oneYakuZyoukyou;
    [SerializeField] TextMeshProUGUI twoYakuZyoukyou;
    [SerializeField] Image yakuComplete;
    [SerializeField] Button buttonRestart; 
    [SerializeField] Button buttonKoikoi;
    [SerializeField] Button buttonAgari;


    List<FudaController> fudas;
    Vector3 fillpos = new Vector3(0.0f, 0.0f, 0.0f);
    //Vector3 fillpos2 = new Vector3(0.0f, 0.0f, 0.0f);
    
    
    //private Animator anim2;
    List<FudaController> hand1p, hand2p;

    List<FudaController> layout;

    List<FudaController> mochi1p, mochi2p;

    FudaController selectFuda;
    FudaController drawedFuda;

    int dealFudaCount;

    int fieldNum = 0;

    float cpuTimer;

    float cpuTimer2;

    bool isdrawable = false;

    bool isturn1p;

    bool flowone = true;
    float layoutx = -2.2f ;
    float layoutz = 2.01f;

    //bool overlap = true;

    const int LeadCardNo = 100;//選択できない

    const float CpuRandomTimerMin = 1;
    const float CpuRandomTimerMax = 2;

    const float CpuRandomTimerMin2 = 3;
    const float CpuRandomTimerMax2 = 4;

    const float RestartMoveSpeed = 0.8f;
    const float RestartRotateSpeed = 0.2f;

    const float DealCardMoveSpeed = 0.5f;

    const float PlayCardMoveSpeed = 0.8f;

    const float HandPositionX = 0.15f;
    const float HandPositionZ = -0.1f;

    const float LayoutPositionX = -0.12f;
    const float LayoutPositionZ = -0.13f;

    const float LeadPositionX = -0.05f;

    const float StackCardHeight = 0.0001f;

    const float SortHandTime = 0.5f;
    public Animator anim1;
    public Animator anim2;

    Animator anim3;

    int hmagari = 0;
    int hmagari2 = 0;
    public static int kasu1p = 0;
    int tan1p = 0;
    public static int tane1p = 0;
    int hikari1p = 0;
    int kasu2p = 0;
    int tan2p = 0;
    int tane2p = 0;
    int hikari2p = 0;

    int gokou1p;
    int gokou2p;
    int sikou1p;
    int sikou2p;
    int amesikou1p;
    int amesikou2p;
    int sankou1p;
    int sankou2p;
    int tsukimi1p = 0;
    int hanami1p = 0;
    int tsukimi2p = 0;
    int hanami2p = 0;
    public static int point1p = 0;
    public int point2p = 0;

    int pointlog = 0;

    int pointlog2 = 0;
    void Start()
    {
        
        isturn1p = true;
        fudas = fDirector.GetShuffleFudas();
        hand1p = new List<FudaController>();
        hand2p = new List<FudaController>();
        mochi1p = new List<FudaController>();
        mochi2p = new List<FudaController>();
        layout = new List<FudaController>();
        textYaku.text = "";
        turn.text = "";
        oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
        twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;

        restartGame(false);
        cpuTimer = Random.Range(CpuRandomTimerMin,CpuRandomTimerMax);
        cpuTimer2 = Random.Range(CpuRandomTimerMin2,CpuRandomTimerMax2);
        anim1 = gameObject.transform.GetChild(0).GetComponent<Animator>();
        anim2 = gameObject.transform.GetChild(1).GetComponent<Animator>();
        anim3 = gameObject.transform.GetChild(2).GetComponent<Animator>();
        yakuComplete.gameObject.SetActive(false);

        //setButtonsInPlay(false);
        
    }

    // Update is called once per frame
    void Update()
    {
        if(Input.GetMouseButtonUp(1))
        {
           Debug.Log(kasu1p);
           Debug.Log(kasu2p);

        }
        
        if(isturn1p == true)
        {
            turn.text = "あなたのターン";

            //Debug.Log("１ｐのターンです。");
            if(Input.GetMouseButtonUp(0))
            {
                if(!isdrawable)
                {
                    playerSelectCard();
                }
                else
                {
                    //Debug.Log("drawable");
                    drawFuda();
                }  
            }        
        

        }
        else
        {
            turn.text = "2P選択中・・・";
            
            cpuTimer -= Time.deltaTime;
            cpuTimer2 -= Time.deltaTime;
            // if (0 > cpuTimer)
            // {
            //     cpuSelectFuda(hand2p);
            //     cpuDrawFuda();
            //     cpuTimer = Random.Range(CpuRandomTimerMin,CpuRandomTimerMax);
            //     Debug.Log("2pターンエンド");   
            // }    
            if(cpuTimer < 0 && flowone)
            {
                Debug.Log("???");
                cpuSelectFuda(hand2p);
                flowone = false;
            }
            else if(cpuTimer2 < 0)
            {
                openDraw(cpuDrawFuda());
                Debug.Log("2pターンエンド");  
                isdrawable = false;
                //isturn1p = true;
                flowone = true;
                cpuTimer = Random.Range(CpuRandomTimerMin,CpuRandomTimerMax);
                cpuTimer2 = Random.Range(CpuRandomTimerMin2,CpuRandomTimerMax2);
            }
            
            
            
                //cpuTimer = Random.Range(CpuRandomTimerMin,CpuRandomTimerMax);
            

        }
    }

    FudaController cpuDrawFuda()
    {
        
        FudaController fuda = fudas[dealFudaCount++];
        //fudas.Remove(fuda);
        return fuda;
    }

    void drawFuda()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit)) return;
        string objectname = hit.collider.gameObject.name;
        if(objectname == "Deck")
        {
            isdrawable = false;
            FudaController draw = fudas[dealFudaCount++];
            openDraw(draw);
        }
    }
    bool isYakuComplete(List<FudaController> mochi)
    {
        if(mochi == mochi1p)
        {
            if(kasu1p<10 && tane1p<5 && tan1p<5 && tsukimi1p<2 && hanami1p<2 && gokou1p<5 && sikou1p<4 && amesikou1p<4 && sankou1p<3) return false;
            point1p = Mathf.Max(kasu1p - 9,0)+Mathf.Max(tane1p - 4,0)+Mathf.Max(tan1p - 4,0)+Mathf.Max((tsukimi1p - 1)*5,0)+Mathf.Max((hanami1p - 1)*5,0)+Mathf.Max((gokou1p - 4)*10,0)+Mathf.Max((sikou1p - 3)*8,0)+Mathf.Max((amesikou1p - 3)*7,0)+Mathf.Max((sankou1p - 2)*5,0);
            if(hmagari == 0)
            {
                hmagari++;
                pointlog = point1p;
                return true;
            }
            else if(hmagari == 1 && point1p > pointlog)
            {
                hmagari++;
                return true;
            }
            else return false;


        }
        else if(mochi == mochi2p)
        {
            if(kasu2p<10 && tane2p<5 && tan2p<5 && tsukimi2p<2 && hanami2p<2 && gokou2p<5 && sikou2p<4 && amesikou2p<4 && sankou2p<3) return false;
            point1p = Mathf.Max(kasu2p - 9,0)+Mathf.Max(tane2p - 4,0)+Mathf.Max(tan2p - 4,0)+Mathf.Max((tsukimi2p - 1)*5,0)+Mathf.Max((hanami2p - 1)*5,0)+Mathf.Max(gokou2p - 4,0)*10+Mathf.Max(sikou2p - 3,0)*8+Mathf.Max(amesikou2p - 3,0)*7+Mathf.Max(sankou2p - 2,0)*5;
            if(hmagari2 == 0)
            {
                hmagari2++;
                pointlog2 = point2p;
                return true;
            }
            else if(hmagari2 == 1 && point2p > pointlog2)
            {
                hmagari2++;
                return true;
            }
            else return false;
        }
        return false;
        

    }


    void playerSelectCard()
    {
        
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if(!Physics.Raycast(ray, out RaycastHit hit)) return;
        if(hit.collider.gameObject.name == "Table" && selectFuda)
        {
            var screenPos = Input.mousePosition;   
            screenPos.z = 6.49f;
  
            var worldPos = Camera.main.ScreenToWorldPoint(screenPos);  
            worldPos.y = 0.51f;
            
            selectFuda.transform.DOMove(worldPos,SortHandTime);
            layout.Add(selectFuda);
            selectFuda.PlayerNo =2;
            isdrawable = true;
            selectFuda = null;
        }
        
        FudaController fuda = hit.collider.gameObject.GetComponent<FudaController>();
        if (!fuda || !fuda.isFrontUp) return;
        if(mochi1p.Contains(fuda) || mochi2p.Contains(fuda)) return;
        
        if(0 == fuda.PlayerNo)
        {
            setSelectFuda(fuda);
        }
        else if(selectFuda && 2 == fuda.PlayerNo)
        {
            
            if(selectFuda.Suit == fuda.Suit)
            {
                Vector3 pos = fuda.transform.position;
                fillpos = pos;
                
                
                addMochi(fuda);
                layout.Remove(fuda);
                addMochi(selectFuda);
                hand1p.Remove(selectFuda);
                sortMochi(mochi1p);
                isdrawable = true;
            }
            else
            {
                Debug.Log("同じ月の札を選択してください");
            }
            setSelectFuda();
        }
        else if(drawedFuda && 2 == fuda.PlayerNo)
        {
            if(drawedFuda.Suit == fuda.Suit)
            {
                Vector3 pos = fuda.transform.position;
                fillpos = pos;
                addMochi(fuda);
                layout.Remove(fuda);
                addMochi(drawedFuda);
                sortMochi(mochi1p);
                if(isYakuComplete(mochi1p))
                {
                    if(hmagari == 2)
                    {
                        StartCoroutine(AgariCoroutine());

                    }
                    else
                    {
                        yakuComplete.gameObject.SetActive(true);
                    }
                    
                }
                else
                {
                    drawedFuda = null;
                    layoutx = -2.2f;
                    layoutz = 2.01f;
                    fieldNum = 0;
                    foreach (var item in layout)
                    {
                        
                        sort(layout,item);
                    }
                    isturn1p = false;
                    anim2.SetTrigger("turn2");
                    oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
                    twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
                }
                
                
                return;
            }
            
            
        }


        //Vector3 pos = fuda.transform.position;
        //Debug.Log(pos);
    }
    void setSelectFuda(FudaController fuda = null)
    {
        if(selectFuda)
        {
            
            if(selectFuda.selecting && selectFuda.select)
            {
                //Debug.Log(selectFuda);
                Vector3 pos = selectFuda.gameObject.transform.position;
                pos.z -= 0.2f;
                selectFuda.gameObject.transform.position = pos;

                
                selectFuda.select = false;
                selectFuda.selecting = false;
                selectFuda = null;
            }
        }
        
        //
        if (!fuda) return;

        
        if(fuda.selecting)
        {
            fuda.select = true;
            selectFuda = fuda;
        }
    }
    void openDraw(FudaController fuda)
    {
        float z = 180.0f;
        float z2 = 0.23f;
        if(isturn1p == false)
        {
           
            z = 0.0f;
            z2 = 2.46f;
        }

        fuda.transform.DORotate(new Vector3(90,0,z), SortHandTime,RotateMode.FastBeyond360);
        fuda.transform.DOMove(new Vector3(-1.88f,1.95f,z2), SortHandTime)
        .OnComplete(() => 
        { 
            fuda.FlipFuda(); 
            sortDraw(fuda);
            
            
        });    
          
        

    
    }
    void sortDraw(FudaController fuda)
    {
        HanaType suit = fuda.Suit;
        //Debug.Log(suit  + "age");
        foreach(var item in layout)
        {
            if(item.Suit != suit) continue;
            if(!isturn1p)
            {
                fuda.transform.rotation = Quaternion.Euler(-90,0,0);
            }
            drawedFuda = fuda;
            isdrawable = false;
            

            if(!isturn1p)
            {
                addMochi2p(fuda);
                addMochi2p(item);
                layout.Remove(item);
                sortMochi(mochi2p);
                isturn1p = true;
                anim1.SetTrigger("turn");
                oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
                twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
            }
            return;
        }
        
            Vector3 pos = fillpos;
            if(isturn1p == false)
            {
                
                pos = fillpos;
            }
            fuda.transform.DOMove(pos,SortHandTime);
            if(fillpos.z > 0.77)
            {
                fuda.transform.rotation = Quaternion.Euler(-90,0,0);
                //Debug.Log("turn");
            }
            else
            {
                fuda.transform.rotation = Quaternion.Euler(-90,0,180);
                //Debug.Log("turn");
            }
            
            layout.Add(fuda);
            fuda.PlayerNo = 2;
            isdrawable = false;
            layoutx = -2.2f;
            layoutz = 2.01f;
            fieldNum = 0;
            if(isturn1p)
            {
                if(isYakuComplete(mochi1p))
                {
                    yakuComplete.gameObject.SetActive(true);

                }
                else
                {
                    anim2.SetTrigger("turn2");
                    oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
                    twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
                    foreach (var item in layout)
                    {
                        
                        sort(layout,item);
                    }
                    isturn1p = !isturn1p;
                }
                
            
            }
            else
            {
                if(isYakuComplete(mochi2p))
                {
                    textYaku.text = "2ｐさんこいこいしますか？";
                }
                else
                {
                    anim1.SetTrigger("turn");
                    oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
                    twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
                    foreach (var item in layout)
                    {
                        
                        sort(layout,item);
                    }
                    isturn1p = !isturn1p;
                }
            }       
    }


    FudaController addMochi(FudaController fuda)
    {
        HanaType suit = fuda.Suit;
        if(fuda.No == 3 || fuda.No == 4)
            {
                if(suit == HanaType.November && fuda.No == 3) tane1p++;
                else kasu1p++;
            }
            else if(fuda.No == 2)
            {
                if(suit == HanaType.August) tane1p++;
                else if(suit == HanaType.December) kasu1p++;
                else tan1p++;

            }
            else if(fuda.No == 1)
            {
                if(suit == HanaType.January)
                {
                    gokou1p++;
                    sikou1p++;
                    amesikou1p++;
                    sankou1p++;
                }
                else if(suit == HanaType.March)
                {
                    gokou1p++;
                    sikou1p++;
                    amesikou1p++;
                    sankou1p++;
                    hanami1p++;
                }
                else if(suit == HanaType.August)
                {
                    gokou1p++;
                    sikou1p++;
                    sankou1p++;
                    tsukimi1p++;
                }
                else if(suit == HanaType.November)
                {
                    gokou1p++;
                    amesikou1p++;
                }
                else if(suit == HanaType.December)
                {
                    gokou1p++;
                    sikou1p++;
                    amesikou1p++;
                }
                else if(suit == HanaType.September)
                {
                    tane1p++;
                    tsukimi1p++;
                }
                else
                {
                    tane1p++;
                } 
            }
            else hikari1p++;
            
        mochi1p.Add(fuda);
        return fuda;
    }
    FudaController addMochi2p(FudaController fuda)
    {
        HanaType suit = fuda.Suit;
        if(fuda.No == 3 || fuda.No == 4)
            {
                if(suit == HanaType.November && fuda.No == 3) tane2p++;
                else kasu2p++;
            }
            else if(fuda.No == 2)
            {
                if(suit == HanaType.August) tane2p++;
                else if(suit == HanaType.December) kasu2p++;
                else tan2p++;

            }
            else if(fuda.No == 1)
            {
                if(suit == HanaType.January || suit == HanaType.March ||suit == HanaType.August ||suit == HanaType.November ||suit == HanaType.December ) hikari1p++;
                else tane2p++;
            }
            else hikari2p++;
        fuda.FlipFuda();
        mochi2p.Add(fuda);
        return fuda;
    }
    void sortMochi(List<FudaController> mochi)
    {

        int dir = 1;

        // int englex = 90;
        int englez = 180;
        if(mochi == mochi2p)
        {
            dir = -1;
        }
        float x1 = 2.8f * dir;
        float z1 = 2.24f * dir;

        float x2 = 2.8f * dir;
        float z2 = 1.24f * dir;

        float x3 = 2.8f * dir;
        float z3 = 0.24f * dir;

        float x4 = 2.8f * dir;
        float z4 = -0.76f * dir;
        if(mochi == mochi2p)
        {
            // englex = -90;
            englez = 0;
            x1 = -3.6f;
            z1 = 0.6f;

            x2 = -3.6f;
            z2 = 1.6f;

            x3 = -3.6f;
            z3 = 2.6f;

            x4 = -3.6f;
            z4 = 3.6f;
        }

        
        foreach (var item in mochi)
        {
            HanaType suit = item.Suit;
            if(item.No == 3 || item.No == 4)
            {
                if(suit == HanaType.November && item.No == 3)
                {
                   
                    //Debug.Log("11月のタネを検知");
                    Vector3 posill = new Vector3(x2, 0.61f, z2);
                    // float dist = Vector3.Distance(item.transform.position, posill);
                    // float ry = Random.Range(-15.0f,15.0f);
                    // item.transform.DORotate(new Vector3(ry,0,0), SortHandTime);
                    item.transform.DOMove(posill, SortHandTime);
                     item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                     //item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                     item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x2 += 0.5f * dir;
                               
                }
                else
                {
                    
                    //Debug.Log("debug"+ suit + item.No);//カスかどうかを判定する処理でイレギュラーな１１と１２を調整
                    Vector3 pos = new Vector3(x4, 0.61f, z4);
                    item.transform.DOMove(pos, SortHandTime);
                     item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                     //item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                     item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x4 += 0.25f * dir;
                    
                    // else kasu2p++;
                    
                }
            }
            else if(item.No == 2)
            {
                if(suit == HanaType.August)
                {
                    if(isturn1p) tane1p++;
                    else tane2p++;
                    Vector3 postane = new Vector3(x2, 0.61f, z2);
                    item.transform.DOMove(postane, SortHandTime);
                    item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                    //item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                    item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x2 += 0.5f * dir;
                }
                else if(suit == HanaType.December)
                {
                    
                    Vector3 poskasu = new Vector3(x4, 0.61f, z4);
                    item.transform.DOMove(poskasu, SortHandTime);
                     item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                    // item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                    item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x4 += 0.25f * dir;
                    
                    //else kasu2p++;
                }
                else
                {
                    
                    Vector3 postan = new Vector3(x3, 0.61f, z3);
                    item.transform.DOMove(postan, SortHandTime);
                    item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                    //item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                    item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x3 += 0.5f * dir;
                }
                

            }
            else if(item.No == 1)
            {
                if(suit == HanaType.January || suit == HanaType.March ||suit == HanaType.August ||suit == HanaType.November ||suit == HanaType.December )
                {
                    Vector3 poshikari = new Vector3(x1, 0.61f, z1);
                    item.transform.DOMove(poshikari, SortHandTime);
                     item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                     //item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                     item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x1 += 0.5f * dir;
                    // if(isturn1p) hikari1p++;
                    // else hikari2p++;
                    
                }
                else
                {
                    
                    Vector3 postane = new Vector3(x2, 0.61f, z2);
                    item.transform.DOMove(postane, SortHandTime);
                     item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                    // item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                    item.transform.rotation = Quaternion.Euler(-90,0,englez);
                    x2 += 0.5f * dir;
                    

                }
            
            }
            else
            {
                Vector3 pos = new Vector3(x1, 0.61f, z1);
                item.transform.DOMove(pos, SortHandTime);
                 item.transform.DOScale(new Vector3(0.06f,0.06f,0.06f),SortHandTime);
                // item.transform.DORotate(new Vector3(englex,0,englez), SortHandTime);
                item.transform.rotation = Quaternion.Euler(-90,0,englez);
                x1 += 0.5f * dir;
                // if(isturn1p) hikari1p++;
                // else hikari2p++;
                
            }
            //表示位置へアニメーションして移動
            
            //item.transform.DOMove(pos, SortHandTime);

            
            
        }        
    }
    

    FudaController add(List<FudaController> hand)
    {
        
        int playerno = 2;//Fieldの札
        if(hand1p == hand)
        {
            playerno = 0;
        }
        else if(hand2p == hand)
        {
            playerno = 1;
        }
        
        FudaController fuda = fudas[dealFudaCount++];
        fuda.PlayerNo = playerno;
        
        hand.Add(fuda);
        
        //fudas.Remove(fuda);
        return fuda;
    }

    void open(FudaController fuda)
    {
        

        
        if(hand2p.Contains(fuda))
        {
            fuda.transform.DORotate(new Vector3(90,0,180), SortHandTime,RotateMode.FastBeyond360)
                      .OnComplete(() => { fuda.FlipFuda(false); });
        }
        else if(hand1p.Contains(fuda))
        {
            fuda.transform.DORotate(new Vector3(90,0,180), SortHandTime,RotateMode.FastBeyond360)
                      .OnComplete(() => { fuda.FlipFuda(); });

        }
        else
        {
           // Debug.Log("unchi");
            fuda.transform.DORotate(new Vector3(90,0,180), SortHandTime)
                      .OnComplete(() => { fuda.FlipFuda(); });
            
        }

        

    }
    void sort(List<FudaController> hand, FudaController fuda)
    {
        
        int dir = 1;
        if(layout == hand)
        {
            
            //Vector3(-2.21000004,0.50999999,2.01999998)
            
                
                if(fieldNum % 2 == 0)
                {
                    layoutz = 2.02f;
                    layoutx += FudaController.Width + 0.2f;
                    fuda.transform.DORotate(new Vector3(-90,0,0), SortHandTime);
                }
                else if(fieldNum % 2 ==1)
                {
                    layoutz = 0.67f;
                    fuda.transform.DORotate(new Vector3(-90,0,180), SortHandTime);
                }
                Vector3 pos = new Vector3(layoutx, 0.51f, layoutz);
                fuda.transform.DOMove(pos, SortHandTime);
                
                
                fieldNum++;

        }
        else
        {
            float x = - 5.0f;
            float z = -1.5f;
            dir = 1;
            if(hand2p == hand)
            {
                x = 5.8f;
                z = 4.2f;
                dir = -1;
            }
            
            //手札を枚数分並べる
            foreach (var item in hand)
            {
                //表示位置へアニメーションして移動
                Vector3 pos = new Vector3(x, 0.844f, z );
                item.transform.DOMove(pos, SortHandTime);

                x += FudaController.Width * dir + 0.02f * dir;
            }

        }
        
    }
    void restartGame(bool deal = true)
    {
        
        dealFudaCount = 0;
        hand1p.Clear();
        fDirector.ShuffleFudas(fudas);
        

        foreach (var item in fudas)
        {
            
            item.FlipFuda(false);

           
            item.transform.position = new Vector3(-2.72f,0.6f,1.41f);

            
        }

        if(!deal) return;

        for(int j = 0; j < 8; j++)
        {
           
            open(add(hand1p));
            open(add(hand2p));
            
            

        }

        sort(hand1p,null);
        sort(hand2p,null);
        StartCoroutine(SortCoroutine());
        
    }
    private IEnumerator AgariCoroutine()
    {
        anim3.SetTrigger("agari");
        for (int k = 0; k < 100; k++)
        {
             yield return null;
        }
        SceneManager.LoadScene("Result");
        
    }
    private IEnumerator SortCoroutine()
    {
        
        for(int i = 0; i < 8; i++)
        {
            //layout.Add(null);
            open(add(layout));
            
            
        }

        
        foreach (var item in layout)
        {
            
            sort(layout,item);
            for (int k = 0; k < 5; k++)
            {
                yield return null;
            }


        }
        
        
        anim1.SetTrigger("turn");
        oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
        twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
        
        //Debug.Log("hoge");
        
        
        
    }

    void cpuSelectFuda(List<FudaController> hand)
    {
        foreach (var select2p in hand)
        {
            foreach (var layout2p in layout)
            {
                if (!isMovable(select2p, layout2p)) continue;

                Vector3 pos = layout2p.transform.position;
                fillpos = pos;
                
                addMochi2p(select2p);
                addMochi2p(layout2p);   
                hand.Remove(select2p);
                layout.Remove(layout2p);
                sortMochi(mochi2p);
                Debug.Log("場捨てせず");
                return;

            }
        }
        int listcount = hand2p.Count;
        int cpuSelect = Random.Range(0,listcount - 1);
        FudaController fuda = hand2p[cpuSelect];
        setLayout(fuda);

    }
    void setLayout(FudaController fuda)
    {
        fuda.PlayerNo = 2;
        layout.Add(fuda);
        hand2p.Remove(fuda);
        
        fuda.transform.DOMove(new Vector3(2.0f,0.51f,2.0f),SortHandTime);
        fuda.transform.rotation = Quaternion.Euler(-90,0,0);
        
        fuda.FlipFuda();
        Debug.Log("場捨て");  
        isdrawable = false;
        

    }

    bool isMovable(FudaController moveselect, FudaController movelayout)
    {
        if(!moveselect || !movelayout) return false;
        if(DOTween.IsTweening(moveselect.transform) || DOTween.IsTweening(movelayout.transform)) return false;

        if(moveselect.Suit == movelayout.Suit)
        {
            //Debug.Log("hogehoge");
            return true;
            
        }
        return false;
    }

    public void OnClickPlay()
    {
        restartGame();  
        buttonRestart.gameObject.SetActive(false);
    }

    public void OnClickKoikoi()
    {
        layoutx = -2.2f;
        layoutz = 2.01f;
        fieldNum = 0;
        anim2.SetTrigger("turn2");
        oneYakuZyoukyou.text ="point1p : " +point1p+ "      "+"hanami : " + hanami1p + "      " +"tsukimi : " + tsukimi1p + "      " + "kasu : " + kasu1p + "      " + "tane : " + tane1p + "      " + "tan : " + tan1p;
        twoYakuZyoukyou.text ="point2p : " +point2p+ "      "+"hanami : " + hanami2p + "      " +"tsukimi : " + tsukimi2p + "      " + "kasu : " + kasu2p + "      " + "tane : " + tane2p + "      " + "tan : " + tan2p;
        foreach (var item in layout)
        {
                        
            sort(layout,item);
        }
        yakuComplete.gameObject.SetActive(false);
        isturn1p = !isturn1p;       

    }
    public void OnClickAgari()
    {
        SceneManager.LoadScene("Result");
    }
}
