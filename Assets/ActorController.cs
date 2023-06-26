using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActorController : MonoBehaviour
{
    public GameObject model;
    public PlayerInput PI;
    public float walkSpeed;
    public float runMultiplier;
    public float jumpVelocity;
    public float rollVelocity;

    [SerializeField]
    private Animator anim;
    private Rigidbody rigid;
    private Vector3 planarVec;
    //冲量向量
    private Vector3 thrustVec;

    private bool lockPlanar = false;

    // Start is called before the first frame update
    void Awake()
    {
        PI = GetComponent<PlayerInput>();
        anim = model.GetComponent<Animator>();
        //rigid 不能在Update中调用，要在FixUpdate中调用,Update函数的每秒刷新次数为60次,FixedUpdate函数每秒刷新次数为50次;
        rigid = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()   //Time.deltaTime 1/60
    {
        anim.SetFloat("forward", PI.Dmag * Mathf.Lerp(anim.GetFloat("forward"), ((PI.run) ? 2.0f : 1.0f), 0.5f));
        if (rigid.velocity.magnitude > 1.0f) 
        {
            anim.SetTrigger("roll");
        }
        if (PI.jump)
        {
            anim.SetTrigger("jump");
        }
        if (PI.Dmag > 0.1f) 
        {
            //Slerp 球形插值
            model.transform.forward = Vector3.Slerp(model.transform.forward, PI.Dvec, 0.1f);
        }
        if (lockPlanar == false)
        {
            planarVec = PI.Dmag * model.transform.forward * walkSpeed * ((PI.run) ? runMultiplier : 1.0f);
        }
    }

    void FixedUpdate()   //Time.fixedDeltaTime   1/50
    {
        //直接改位置
        //rigid.position += planarVec * Time.fixedDeltaTime;

        //直接改变速度
        rigid.velocity = new Vector3(planarVec.x, rigid.velocity.y, planarVec.z) + thrustVec;
        thrustVec = Vector3.zero;
    }

    /// 
    /// Message processing block
    /// 

    public void OnJumpEnter()
    {
        PI.inputEnable = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, jumpVelocity, 0);
    }

    //public void OnJumpExit()
    //{
    //    PI.inputEnable = true;
    //    lockPlanar = false;
    //}

    public void IsGround()
    {
        anim.SetBool("isGround",true);
    }

    public void IsNotGround()
    {
        anim.SetBool("isGround", false);
    }

    public void OnGroundEnter()
    {
        PI.inputEnable = true;
        lockPlanar = false;
    }

    public void OnFallEnter()
    {
        PI.inputEnable = false;
        lockPlanar = true;
    }

    public void OnRollEnter()
    {
        PI.inputEnable = false;
        lockPlanar = true;
        thrustVec = new Vector3(0, rollVelocity, 0);
    }

    public void OnJabEnter()
    {
        PI.inputEnable = false;
        lockPlanar = true;
    }

    public void OnJabUpdate()
    {
        thrustVec = model.transform.forward * anim.GetFloat("jabvelocity");
    }

}
