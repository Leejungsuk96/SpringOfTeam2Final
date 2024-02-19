using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EffectPool : MonoBehaviour
{
    private List<GameObject>[] effectPool;

    private void Awake()
    {
        effectPool = new List<GameObject>[EffectManager.instance.effectMappings.Length];

        for(int i = 0; i < effectPool.Length; i++)
        {
            effectPool[i] = new List<GameObject>();
        }
    }

    public void EffectIndex(EffectManager.EffectType effectType, Vector3 spawnPosition, Vector3 direction)
    {
        GameObject selectEffect = null;

        // ������ Ǯ�� ��Ȱ��ȭ ������Ʈ�� ����
        foreach (GameObject effect in effectPool[(int)effectType])
        {
            if (!effect.activeSelf)
            {
                selectEffect = effect;
                selectEffect.transform.position = spawnPosition;

                // ���� ���͸� ����Ͽ� ���� ���
                float angle = -Mathf.Atan2(direction.y, direction.x);

                ParticleSystem particleSystem = selectEffect.GetComponent<ParticleSystem>();

                if (particleSystem != null)
                {
                    // Start Rotation ���� ����
                    var mainModule = particleSystem.main;
                    mainModule.startRotation = new ParticleSystem.MinMaxCurve(angle + 0.785f);
                }
                selectEffect.SetActive(true);
                break;
            }
        }
        if (!selectEffect)
        {
            selectEffect = Instantiate(EffectManager.instance.GetEffectPrefab(effectType), transform);
            selectEffect.transform.position = spawnPosition;

            // ���� ���͸� ����Ͽ� ���� ���
            float angle =  -Mathf.Atan2(direction.y, direction.x);
            
            ParticleSystem particleSystem = selectEffect.GetComponent<ParticleSystem>();

            if (particleSystem != null)
            {
                // Start Rotation ���� ����
                var mainModule = particleSystem.main;
                mainModule.startRotation = new ParticleSystem.MinMaxCurve(angle + 0.785f);
            }

            effectPool[(int)effectType].Add(selectEffect);
        }
    }
}
