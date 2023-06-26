using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInput : MonoBehaviour
{
    [Header("===== Keys settings =====")]
    public string keyUp;
    public string keyDown;
    public string keyLeft;
    public string keyRight;
    public string keyA;
    public string keyB;
    public string keyC;
    public string keyD;
    public string keyJRight;
    public string keyJLeft;
    public string keyJUp;
    public string keyJDown;

    [Header("===== Output signals =====")]
    public float Dup;
    public float Dright;
    public float Dmag;
    public Vector3 Dvec;
    public float Jup;
    public float Jright;
    public bool run;
    public bool jump;   
    private bool lastJump;

    [Header("===== Others =====")]
    public bool inputEnable;
    private float targetDup;
    private float targetDright;
    private float velocityDup;
    private float velocityDright;
    

    // Start is called before the first frame update
    void Start()
    {
        inputEnable = true;
    }

    // Update is called once per frame
    void Update()
    {
        Jup = (Input.GetKey(keyJUp) ? 1.0f : 0)- (Input.GetKey(keyJDown) ? 1.0f : 0);
        Jright = (Input.GetKey(keyJRight) ? 1.0f : 0) - (Input.GetKey(keyJLeft) ? 1.0f : 0);

        targetDup = (Input.GetKey(keyUp) ? 1.0f : 0) - (Input.GetKey(keyDown) ? 1.0f : 0);
        targetDright = (Input.GetKey(keyRight) ? 1.0f : 0) - (Input.GetKey(keyLeft) ? 1.0f : 0);

        if (!inputEnable)
        {
            targetDup = 0;
            targetDright = 0;
        }


        //ref 传参不传值 velocity 由函数自动改变
        Dup = Mathf.SmoothDamp(Dup, targetDup, ref velocityDup, 0.1f);
        Dright = Mathf.SmoothDamp(Dright, targetDright, ref velocityDright, 0.1f);

        Vector2 tempDAxis = SquareToCircle(new Vector2(Dright, Dup));
        float Dright2 = tempDAxis.x;
        float Dup2 = tempDAxis.y;

        Dmag = Mathf.Sqrt(Mathf.Pow(Dup2, 2) + Mathf.Pow(Dright2, 2));
        Dvec = Dright * transform.right + Dup * transform.forward;

        run = Input.GetKey(keyA);

        bool tempJump = Input.GetKey(keyB);
        if (tempJump != lastJump)
        {
            jump = true;
        }
        else
        {
            jump = false;
        }
        lastJump = tempJump;
    }

    //处理斜向跑动速度不均问题，将平面点全部转为圆上的点
    private Vector2 SquareToCircle(Vector2 input)
    {
        Vector2 output = Vector2.zero;
        output.x = input.x * Mathf.Sqrt(1 - (input.y * input.y) / 2.0f);
        output.y = input.y * Mathf.Sqrt(1 - (input.x * input.x) / 2.0f);
        
        return output;
    }

}
