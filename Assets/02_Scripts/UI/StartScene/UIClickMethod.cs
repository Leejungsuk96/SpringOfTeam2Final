using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIClickMethod : MonoBehaviour
{
    public Button startBtn;
    public Button exitBtn;
    
    public void ClickStartBtn()
    {
        StartCoroutine(LoadSceneAsync("MainScene"));
    }

    IEnumerator LoadSceneAsync(string sceneName)
    {
        // �ε� ���μ����� ��Ÿ���� AsyncOperation ��ü ����
        AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(sceneName);

        // ���� Ȱ��ȭ�� ������ ��ٸ�
        asyncOperation.allowSceneActivation = false;

        // �ε尡 �Ϸ�Ǿ����� Ȯ��
        while (!asyncOperation.isDone)
        {
            // �ʿ��� ��� �ε� ����� ǥ��
            float progress = Mathf.Clamp01(asyncOperation.progress / 0.9f); // ���� ������ �ε�� ���� �Ϸ� ���� 0.9�Դϴ�.

            Debug.Log("�ε� �����: " + (progress * 100) + "%");

            // �ε尡 ���� �Ϸ�Ǿ����� Ȯ��
            if (asyncOperation.progress >= 0.9f)
            {
                // ���� �Ϸ�Ǿ��ٸ� ���� Ȱ��ȭ�� �� �ֽ��ϴ�.
                asyncOperation.allowSceneActivation = true;
            }

            yield return null;
        }
    }

    public void ClickExitBtn()
    {
        #if UNITY_EDITOR
        UnityEditor.EditorApplication.isPlaying = false;
        #else
        Application.Quit();
        #endif
    }
}
