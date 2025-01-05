using BBBirder.UnityInjection;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class FirstPatch
{
    public static void Print(object obj1, object obj2)
    {
        for (int i = 0; i < 100; i++)
        {
            Debug.Log(obj1);
            Debug.Log(obj2);
        }
    }

    internal class MethodReplacer : IInjectionProvider
    {
        public IEnumerable<InjectionInfo> ProvideInjections()
        {
            yield return new InjectionInfo<Action<object, object>>(
                Print,         // replace it
                raw => (obj1, obj2) =>  // with me
                {
                    Debug.Log($"=====开始统计");
                    System.Diagnostics.Stopwatch stopwatch = new System.Diagnostics.Stopwatch();
                    raw("[my log] " + obj1, obj2);
                    stopwatch.Stop();
                    //raw(obj1, obj2);
                    Debug.Log($"=====结束统计,耗时：{stopwatch.ElapsedMilliseconds}ms");
                }
            );
        }
    }
}

public class NewBehaviourScript : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        FirstPatch.Print("111", "222");
    }
}
