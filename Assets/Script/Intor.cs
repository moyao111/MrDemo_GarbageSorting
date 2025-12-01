using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 打开或者关闭介绍
/// </summary>
public class Intor : MonoBehaviour
{
    [Header("打字速度")]
    public float Speed = 15;

    Text text;

    private bool intor;
    //介绍
    public void Introduction()
    {
        intor = !intor;
        gameObject.SetActive(intor);
        //打字机
    }
    //激活后调用
  
    void Start()
    {
        text = this.transform.GetChild(0).GetComponent<Text>();
        if (gameObject.name == "green")
        {
            Run("厨余垃圾俗称“湿垃圾”，包括居民家庭产生的剩菜剩饭、菜根菜叶、瓜果皮核渣、动物内脏、过期食品等废物及农贸市场的有机垃圾。", text);
        }
        if (gameObject.name == "grey")
        {
            Run("其他垃圾是包括除其他几类垃圾之外的砖瓦陶瓷、尘土、卫生间废纸、纸巾等难以回收以及暂无回收利用价值的废弃物。", text);
        }
        if (gameObject.name == "red")
        {
            Run("有毒有害垃圾是指垃圾中对人体健康或自然环境造成直接或潜在危害的物质。包括灯管、废水银温度计、废油漆桶、过期药品等日常用品", text);
        }
        if (gameObject.name == "blue")
        {
            Run("可回收物是指经过加工可以成为生产原料或者经过整理可以再利用的物品，主要包括废纸、塑料、金属、玻璃等。", text);
        }

    }
    public void Run(string textToType, Text textLabel)
    {
        StartCoroutine(TypeText(textToType, textLabel));
    }
    IEnumerator TypeText(string textToType, Text textLabel)
    {
        float t = 0;//经过的时间
        int charIndex = 0;//字符串索引值
        while (charIndex < textToType.Length)
        {
            t += Time.deltaTime * Speed;//简单计时器赋值给t
            charIndex = Mathf.FloorToInt(t);//把t转为int类型赋值给charIndex
            charIndex = Mathf.Clamp(charIndex, 0, textToType.Length);
            textLabel.text = textToType.Substring(0, charIndex);

            yield return null;
        }
        textLabel.text = textToType;
    }




}
