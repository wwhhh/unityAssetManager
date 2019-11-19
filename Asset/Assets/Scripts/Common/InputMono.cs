using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InputMono : MonoBehaviour
{

    public static float ANGLE_NONE = 999f;

    public float ASDWAngle = ANGLE_NONE;

#if UNITY_EDITOR || UNITY_STANDALONE_WIN
    public void Update()
    {
        bool A = Input.GetKey(KeyCode.A);
        bool S = Input.GetKey(KeyCode.S);
        bool D = Input.GetKey(KeyCode.D);
        bool W = Input.GetKey(KeyCode.W);

        if (A || S || D || W)
        {
            int x = -(A ? 1 : 0) + (D ? 1 : 0);
            int y = -(S ? 1 : 0) + (W ? 1 : 0);
            ASDWAngle = Mathf.Atan2(x, y) * 180f / Mathf.PI;
        }
        else
        {
            ASDWAngle = ANGLE_NONE;
        }

        InputManager.I.ASDWAngle = ASDWAngle;
        InputManager.I.JoystickUse = ASDWAngle != ANGLE_NONE;
    }
#endif

}
