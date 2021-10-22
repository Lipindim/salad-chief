using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikeBreaker : MonoBehaviour
{

    // ЭТОТ скрипт должен висеть на самом острие пики (можно создать там сферу или куб) Все части пики должны быть детьми к этом объекту
  
    private void OnCollisionEnter(Collision collision)
    {


        DangerWall dangerProduct = collision.gameObject.GetComponent<DangerWall>();

        // Повесить скрипт DANGERPRODUCT на объекты от которых разбивается пика
        if (dangerProduct != null)
        {
            CrashPike();
        }

    }

    private void CrashPike()
    {
        // ломание пики
        int i = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        // находим всех детей и сохраняем в массив

        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }


        foreach (GameObject child in allChildren)
        {
            child.transform.parent = null;
            child.GetComponent<Rigidbody>().isKinematic = false;
            // выкидываем всех детей вверх поерархии

        }
        // откоментить если нужно чтобы острие тоже отваливалось
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;

    }
}
