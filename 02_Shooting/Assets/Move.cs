using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 1.0f;
    // ����Ƽ �̺�Ʈ �Լ� : ����Ƽ�� Ư�� Ÿ�ֿ̹� ���� ��Ű�� �Լ�

    /// <summary>
    /// Start �Լ�. ������ ���۵� ��(ù��° Update �Լ��� ȣ��Ǳ� ������ ȣ��Ǵ� �Լ�) ȣ��Ǵ� �Լ�
    /// </summary>
    private void Start()
    {
        Debug.Log("Hello Unity");
    }

    /// <summary>
    /// Update �Լ�. �� �����Ӹ��� ȣ��Ǵ� �Լ�. ���������� ����Ǵ� ���� ���� �� ����ϴ� �Լ�.
    /// </summary>
    private void Update()
    {
        // Vector3 : ���͸� ǥ���ϱ� ���� ����ü. ��ġ�� ǥ���� ���� ���� ����Ѵ�.
        // ���� : ���� ����� ũ�⸦ ��Ÿ���� ����

        //transform.position += new Vector3(1, 0, 0);        

        //new Vector3(1, 0, 0);   // ������ Vector3.right;
        //new Vector3(-1, 0, 0);  // ����   Vector3.left;
        //new Vector3(0, 1, 0);   // ����   Vector3.up;
        //new Vector3(0, -1, 0);  // �Ʒ��� Vector3.down;

        //transform.position += (Vector3.down * speed );   // �Ʒ��� �������� speed ��ŭ ��������(�� �����Ӹ���)

        // Time.deltaTime : ���� �����ӿ��� ���� �����ӱ��� �ɸ� �ð� => 1�����Ӵ� �ɸ� �ð�
        //transform.position += (Vector3.down * speed * Time.deltaTime);  // �Ʒ��� �������� speed ��ŭ ��������(�� �ʸ���)
        transform.position += (speed * Time.deltaTime * Vector3.down);

    }
}
