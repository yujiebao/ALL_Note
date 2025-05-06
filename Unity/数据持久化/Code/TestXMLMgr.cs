using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class TestLesson99
{
    public int test1 = 1;
    public string test2 = "test2";
    public SerializerDictionary<string, int> test3;
    public int test4 = 4;

    public override string ToString()
    {
        return "test1:"+test1+" test2:"+test2+" test3:"+test3+" test4:"+test4;
    }
}
public class Test : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        TestLesson99 lesson99 = new TestLesson99();
        lesson99.test3 = new SerializerDictionary<string, int>();
        lesson99.test3.Add("test1", 1);
        lesson99.test3.Add("test2", 2);
        lesson99.test3.Add("test3", 3);
        lesson99.test3.Add("test4", 4);
        XMLDataMgr.Instance.SaveData(lesson99, "lesson99");

        lesson99 = XMLDataMgr.Instance.LoadData(typeof(TestLesson99), "lesson99") as TestLesson99;
        print(lesson99);
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
