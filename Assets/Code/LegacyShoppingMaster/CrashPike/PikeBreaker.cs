using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PikeBreaker : MonoBehaviour
{

    // ���� ������ ������ ������ �� ����� ������ ���� (����� ������� ��� ����� ��� ���) ��� ����� ���� ������ ���� ������ � ���� �������
  
    private void OnCollisionEnter(Collision collision)
    {


        DangerWall dangerProduct = collision.gameObject.GetComponent<DangerWall>();

        // �������� ������ DANGERPRODUCT �� ������� �� ������� ����������� ����
        if (dangerProduct != null)
        {
            CrashPike();
        }

    }

    private void CrashPike()
    {
        // ������� ����
        int i = 0;
        GameObject[] allChildren = new GameObject[transform.childCount];
        // ������� ���� ����� � ��������� � ������

        foreach (Transform child in transform)
        {
            allChildren[i] = child.gameObject;
            i += 1;
        }


        foreach (GameObject child in allChildren)
        {
            child.transform.parent = null;
            child.GetComponent<Rigidbody>().isKinematic = false;
            // ���������� ���� ����� ����� ���������

        }
        // ����������� ���� ����� ����� ������ ���� ������������
        gameObject.GetComponent<Rigidbody>().isKinematic = false;
        gameObject.transform.parent = null;

    }
}
