using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FudasDirector : MonoBehaviour
{
    [SerializeField] List<GameObject> prefabJanuary;
    [SerializeField] List<GameObject> prefabFebruary;
    [SerializeField] List<GameObject> prefabMarch;
    [SerializeField] List<GameObject> prefabApril;
    [SerializeField] List<GameObject> prefabMay;
    [SerializeField] List<GameObject> prefabJune;
    [SerializeField] List<GameObject> prefabJuly;
    [SerializeField] List<GameObject> prefabAugust;
    [SerializeField] List<GameObject> prefabSeptember;
    [SerializeField] List<GameObject> prefabOctober;
    [SerializeField] List<GameObject> prefabNovember;
    [SerializeField] List<GameObject> prefabDecember;

    
    // Start is called before the first frame update
    void Start()
    {
        
        
    }

    
    public List<FudaController> GetShuffleFudas()
    {
        List<FudaController> ret = new List<FudaController>();

        ret.AddRange(createFudas(HanaType.January));
        ret.AddRange(createFudas(HanaType.February));
        ret.AddRange(createFudas(HanaType.March));
        ret.AddRange(createFudas(HanaType.April));//リストに対してAddRangeすることで複数のアイテムを追加できる
        ret.AddRange(createFudas(HanaType.May));
        ret.AddRange(createFudas(HanaType.June));
        ret.AddRange(createFudas(HanaType.July));
        ret.AddRange(createFudas(HanaType.August));
        ret.AddRange(createFudas(HanaType.September));
        ret.AddRange(createFudas(HanaType.October));
        ret.AddRange(createFudas(HanaType.November));
        ret.AddRange(createFudas(HanaType.December));
        ShuffleFudas(ret);

        return ret;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void ShuffleFudas(List<FudaController> fudas)//引数でカードののリストを取得
    {
        for (int i = 0; i < fudas.Count; i++)
        {
            int rnd = Random.Range(0, fudas.Count);
            FudaController tmp = fudas[i];

            fudas[i] = fudas[rnd];
            fudas[rnd] = tmp;

        }
    }

    List<FudaController> createFudas(HanaType hanatype)
    {
        List<FudaController> ret = new List<FudaController>();

        //札の種類（デフォルト）
        List<GameObject> prefabfudas = prefabJanuary;

        if(HanaType.February == hanatype)
        {
            prefabfudas = prefabFebruary;
        }
        else if(HanaType.March == hanatype)
        {
            prefabfudas = prefabMarch;
        }
        else if(HanaType.April == hanatype)
        {
            prefabfudas = prefabApril;
        }
        else if(HanaType.May == hanatype)
        {
            prefabfudas = prefabMay;
        }
        else if(HanaType.June == hanatype)
        {
            prefabfudas = prefabJune;
        }
        else if(HanaType.July == hanatype)
        {
            prefabfudas = prefabJuly;
        }
        else if(HanaType.August == hanatype)
        {
            prefabfudas = prefabAugust;
        }
        else if(HanaType.September == hanatype)
        {
            prefabfudas = prefabSeptember;
        }
        else if(HanaType.October == hanatype)
        {
            prefabfudas = prefabOctober;
        }
        else if(HanaType.November == hanatype)
        {
            prefabfudas = prefabNovember;
        }
        else if(HanaType.December == hanatype)
        {
            prefabfudas = prefabDecember;
        }

        for (int i = 0; i < prefabfudas.Count; i++)
        {
            GameObject obj = Instantiate(prefabfudas[i]);

            BoxCollider bc = obj.AddComponent<BoxCollider>();

            Rigidbody rb = obj.AddComponent<Rigidbody>();

            bc.isTrigger = true;
            rb.isKinematic = true;//カード同士の当たり判定無効化
            bc.size = new Vector3(9.0f,14.5f,1.0f);

            //カードにデータをセット
            FudaController ctrl = obj.AddComponent<FudaController>();

            ctrl.Suit = hanatype;
            //ctrl.SuitColor = suitcolor;
            ctrl.PlayerNo = -1;
            ctrl.No = i + 1;

            ret.Add(ctrl);


        }
        return ret;

    }
}
