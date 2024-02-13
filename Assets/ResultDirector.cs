using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class ResultDirector : MonoBehaviour
{
    [SerializeField] Text point1ptext;
    [SerializeField] Text resulttext;

    [SerializeField] Button ButtonRestart;
    int point1p = SceneDirector.point1p;
    int kasu1p = SceneDirector.kasu1p;
    int tane1p = SceneDirector.tane1p;
    // Start is called before the first frame update
    void Start()
    {
        point1ptext.text = "カス : " + "  " + "枚" + "\nタネ : "+"  "+"枚";
        ButtonRestart.gameObject.SetActive(false);
        

        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(ResultCoroutine());
     

        
    }

    public void OnClickRestart()
    {
        SceneManager.LoadScene("education");
    }

    private IEnumerator ResultCoroutine()
    {
        for (int i = 0; i < 20; i++)
        {
             yield return null;
        }
        point1ptext.text = "カス : " + "  " + "枚" + "\nタネ : "+"  "+"枚";
        for (int i = 0; i < 40; i++)
        {
             yield return null;
        }
        point1ptext.text = "カス : " + kasu1p + "枚" + "\nタネ : "+"  "+"枚";
        for (int i = 0; i < 40; i++)
        {
             yield return null;
        }
        point1ptext.text = "カス : " + kasu1p + "枚" + "\nタネ : "+tane1p+"枚";
        for (int i = 0; i < 40; i++)
        {
             yield return null;
        }
        resulttext.text = "1Pの得点は：";
        for (int k = 0; k < 60; k++)
        {
             yield return null;
        }
        resulttext.text = "1Pの得点は： " + point1p +"点";
        ButtonRestart.gameObject.SetActive(true);


        
        yield return null;
    }
}
