using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class BaseController : MonoBehaviour
{
    [SerializeField]
    protected Vector3 _destPos;

    [SerializeField]
    protected Define.State _state = Define.State.Idle;

    [SerializeField]
    protected GameObject _lockTarget;

    float wait_run_ratio = 0.0f;
    //float _yAngle = 0.0f;

    public Define.WorldObejct WorldObjectType { get; protected set; } = Define.WorldObejct.UnKnown;

    public virtual Define.State State
    {
        get { return _state; }
        set
        {
            _state = value;

            Animator anim = GetComponent<Animator>();
            switch (_state)
            {
                case Define.State.Die:
                    //anim.SetBool("attack",false);
                    break;
                case Define.State.Idle:
                    anim.CrossFade("WAIT", 0.1f);
                    //anim.Play("WAIT");
                    //anim.SetFloat("speed", 0);
                    //anim.SetBool("attack", false);
                    break;
                case Define.State.Moving:
                    anim.CrossFade("RUN", 0.1f);//애니메이션 부드럽게 재생
                    //anim.Play("RUN");
                    //anim.SetFloat("speed", _stat.MoveSpeed);
                    //anim.SetBool("attack", false);
                    break;
                case Define.State.Skill:
                    anim.CrossFade("ATTACK", 0.1f);
                    //anim.Play("ATTACK");
                    //anim.SetBool("attack", true);
                    break;
            }
        }
    }

    private void Start()
    {
        Init();
    }

    // Update is called once per frame
    void Update()
    {
        #region Vector구현 실습
        // world >> 유니티 자체의 좌표 
        // local >> 플레이어 기준으로 회전되어있다면 회전값에 맞는 좌표 ex) 45도 틀어져있다면 좌표도 45도 틀어져있다. 그래서 정면을 맞춰준다.

        // x,y,z 가 1이면 그 방향을 나타내고 2이상이면 방향과 크기를 나타낸다?

        //transform.TransformDirection() >> local에서 World로 변환해주는 것 
        //transform.InverseTransformDirection() >> World에서 local로 변환해주는 것

        //translate는 local 좌표계를 따른다?

        // 거리 = (방향 * )  시간 * 속도 ; 

        #endregion

        #region Rotation 
        //_yAngle += Time.deltaTime * _speed;

        //절대 회전값 
        //transform.eulerAngles = new Vector3(0.0f, _yAngle, 0.0f);

        //+- delta 값 
        //transform.Rotate(new Vector3(0.0f, _yAngle, 0.0f));

        //transform.rotation = Quaternion.Euler(new Vector3(0.0f, _yAngle, 0.0f));
        #endregion

        //if(_moveToDest)
        //{
        //    Vector3 dir = _destPos - transform.position;
        //    if(dir.magnitude < 0.0001f)
        //    {
        //        _moveToDest = false;
        //    }
        //    else
        //    {
        //        float moveDist = Mathf.Clamp(_speed * Time.deltaTime,0,dir.magnitude);
        //
        //        transform.position += dir.normalized* moveDist;
        //
        //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 10 * Time.deltaTime);
        //    }
        //}

        //if(_moveToDest)
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //    Animator anim = GetComponent<Animator>();
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("WAIT_RUN");
        //}
        //else
        //{
        //    wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //    Animator anim = GetComponent<Animator>();
        //    anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //    anim.Play("WAIT_RUN");
        //}


        switch (_state)
        {
            case Define.State.Die:
                UpdateDie();
                break;
            case Define.State.Moving:
                UpdateMoving();
                break;
            case Define.State.Idle:
                UpdateIdle();
                break;
            case Define.State.Skill:
                UpdateSkill();
                break;
        }
    }

    public abstract void Init();

    protected virtual void UpdateDie() { }
    protected virtual void UpdateMoving() { }
    protected virtual void UpdateIdle() { }
    protected virtual void UpdateSkill() { }
}
