using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasicWall : MonoBehaviour
{
    private float maxDuravity = 40f;
    public float currentDuravity = 40f;

    private float recoveryTime;
    private bool isDamaged;

    public void DamagedWall(float dmg)
    {
        isDamaged = true;
        currentDuravity -= dmg;
        if (currentDuravity <= maxDuravity/2)
        {
            // �̹��� �ٲ��ֱ�
        }
        else if(currentDuravity <= 0)
        {
            Destroy(gameObject);
            // ���� ������Ʈ Pool�� ���� �����ϸ� SetActive(false)��.
        }
        RecoveryDmg();
    }

    private void RecoveryDmg()
    {
        if (currentDuravity < maxDuravity)
        {
            recoveryTime = 0f;
            recoveryTime += Time.deltaTime;// �ڷ�ƾ����
            if (isDamaged && recoveryTime >= 5f)
            {
                currentDuravity = maxDuravity;
                isDamaged = false;
                recoveryTime = 0;
                // �̹��� ó�� ���� �ٲ��ֱ�.
            }
        }
    }
}
