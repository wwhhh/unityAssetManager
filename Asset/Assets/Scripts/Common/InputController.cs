using UnityEngine;

class InputManager : Singleton<InputManager>
{

    public static float ANGLE_NONE = 999f;
    
    public float ASDWAngle = ANGLE_NONE;
    public bool JoystickUse;
    
    private Vector3 delta = new Vector3();
    public Vector3 GetMoveForward()
    {
        float angle = 0f;
        if (ASDWAngle != ANGLE_NONE) angle += ASDWAngle;
        else return Vector3.zero;
    
        angle *= Mathf.Deg2Rad;
        float x = Mathf.Sin(angle);
        float z = Mathf.Cos(angle);
    
        delta.x = x;
        delta.z = z;
        return delta;
    }

}