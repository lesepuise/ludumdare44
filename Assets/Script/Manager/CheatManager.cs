using CleverCode;
using UnityEngine;

public class CheatManager : Singleton<CheatManager>
{
    private void Update()
    {
        CheckTimeCheat(KeyCode.Alpha1, 0.0f);
        CheckTimeCheat(KeyCode.Alpha2, 0.1f);
        CheckTimeCheat(KeyCode.Alpha3, 0.4f);
        CheckTimeCheat(KeyCode.Alpha4, 1.0f);
        CheckTimeCheat(KeyCode.Alpha5, 2.0f);
        CheckTimeCheat(KeyCode.Alpha6, 5.0f);
    }

    private void CheckTimeCheat(KeyCode keyCode, float scale)
    {
        if (Input.GetKeyDown(keyCode))
        {
            Time.timeScale = scale;
        }
    }
}