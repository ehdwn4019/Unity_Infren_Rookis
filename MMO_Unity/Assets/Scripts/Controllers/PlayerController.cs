using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

#region Vector구현 실습
//1. 위치 벡터
//2. 방향 벡터
//struct MyVector
//{
//    public float x;
//    public float y;
//    public float z;
//
//    //피타고라스 정리 
//    public float magnitude //공식
//    {
//        get
//        {
//            return Mathf.Sqrt(x*x+y*y+z*z); //피타고라스는 c제곱 = a제곱 + b제곱 인데 , 3d 에서는 점하나가 더생겨서 z제곱도 더해준다? / 마지막에 루트 씌워주기
//        }
//    }
//
//    public MyVector normalized //공식
//    {
//        get
//        {
//            return new MyVector(x / magnitude, y / magnitude, z / magnitude);
//        }
//    }
//
//    public MyVector(float x, float y, float z)
//    {
//        this.x = x;
//        this.y = y;
//        this.z = z;
//    }
//
//    public static MyVector operator +(MyVector a, MyVector b)
//    {
//        return new MyVector(a.x + b.x, a.y + b.y , a.z + b.z);
//    }
//
//    public static MyVector operator -(MyVector a, MyVector b)
//    {
//        return new MyVector(a.x - b.x, a.y - b.y, a.z - b.z);
//    }
//
//    public static MyVector operator *(MyVector a, float d)
//    {
//        return new MyVector(a.x * d, a.y * d, a.z * d);
//    }
//}

#endregion

public class PlayerController : BaseController
{
    bool _stopSkill = false;
    int _mask = (1 << (int)Define.Layer.Ground) | (1 << (int)Define.Layer.Moster);

    PlayerStat _stat;

    // Start is called before the first frame update
    public override void Init()
    {
        WorldObjectType = Define.WorldObejct.Player;
        _stat = gameObject.GetComponent<PlayerStat>();

        #region Vector구현 실습
        //MyVector pos = new MyVector(0.0f, 10.0f, 0.0f);
        //pos += new MyVector(0.0f, 2.0f, 0.0f);
        //
        //MyVector to = new MyVector(10.0f, 0.0f, 0.0f);
        //MyVector from = new MyVector(5.0f, 0.0f,0.0f);
        //MyVector dir = to - from; //목적지에서 내위치 빼기 
        //
        //dir = dir.normalized;
        ////normalized >> 단위벡터  , 1로 만들어준다. / dir = (1.0f, 0.0f , 0.0f);
        //
        //MyVector newPos = from + dir * _speed;

        //방향 벡터 
        // 1.거리(크기) >> magnitude 
        // 2. 실제 방향
        #endregion
        //Managers.Input.KeyAction -= OnKeyboard; //두번호출할수도 있으니 한번빼주고 시작
        //Managers.Input.KeyAction += OnKeyboard;
        Managers.Input.MouseAction -= OnMouseEvent;
        Managers.Input.MouseAction += OnMouseEvent;

        //Managers.Resource.Instantiate("UI/UI_Button");
        if (gameObject.GetComponentInChildren<UI_HPBar>() == null)
            Managers.UI.MakeWorldSpaceUI<UI_HPBar>(transform);
    }

    protected override void UpdateMoving()
    {
        if(_lockTarget !=null)
        {
            _destPos = _lockTarget.transform.position;
           float distance = (_destPos - transform.position).magnitude;
           if(distance <= 1)
            {
                State = Define.State.Skill;
                return;
            }
        }

        Vector3 dir = _destPos - transform.position;
        dir.y = 0;
        if (dir.magnitude < 0.1f)
        {
            State = Define.State.Idle;
        }
        else
        {
            Debug.DrawRay(transform.position, dir.normalized, Color.green);
            if (Physics.Raycast(transform.position + Vector3.up * 0.5f, dir, 1.0f, LayerMask.GetMask("Block")))
            {
                if(Input.GetMouseButton(0) == false)
                    State = Define.State.Idle;
                return;
            }

            //transform.position += dir.normalized * moveDist;

            float moveDist = Mathf.Clamp(_stat.MoveSpeed * Time.deltaTime, 0, dir.magnitude);
            transform.position += dir.normalized * moveDist;
            transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(dir), 20 * Time.deltaTime);
        }

        //애니메이션 처리 코드, 블렌딩으로 하는법 
        //wait_run_ratio = Mathf.Lerp(wait_run_ratio, 1, 10.0f * Time.deltaTime);
        //Animator anim = GetComponent<Animator>();
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
        

    }

    void OnRunEvent()
    {
        Debug.Log("뚜벅 뚜벅~~");
    }

    void UpdateIdle()
    {
        //애니메이션 처리 코드, 블렌딩으로 하는법 
        //wait_run_ratio = Mathf.Lerp(wait_run_ratio, 0, 10.0f * Time.deltaTime);
        //Animator anim = GetComponent<Animator>();
        //anim.SetFloat("wait_run_ratio", wait_run_ratio);
        //anim.Play("WAIT_RUN");
    }

    protected override void UpdateSkill()
    {
        if(_lockTarget !=null)
        {
            Vector3 dir = _lockTarget.transform.position - transform.position;
            Quaternion quat = Quaternion.LookRotation(dir);
            transform.rotation = Quaternion.Lerp(transform.rotation, quat, 20 * Time.deltaTime);
        }
    }

    void OnHitEvent()
    {
        if(_lockTarget !=null)
        {
            Stat targetStat = _lockTarget.GetComponent<Stat>();
            targetStat.OnAttacked(_stat);
        }

        if(_stopSkill)
        {
            State = Define.State.Idle;
        }
        else
        {
            State = Define.State.Skill;
        }
        
    }

    //void OnKeyboard()
    //{
    //    if (Input.GetKey(KeyCode.W))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.forward), 0.2f);
    //        transform.position += Vector3.forward * Time.deltaTime * _speed;
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed); // slerp 가 아닌 lerp 로 사용할떄는 translate함수를 사용한다?
    //    }
    //
    //    if (Input.GetKey(KeyCode.S))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.back), 0.2f);
    //        transform.position += Vector3.back * Time.deltaTime * _speed;
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //    }
    //
    //    if (Input.GetKey(KeyCode.A))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.left), 0.2f);
    //        transform.position += Vector3.left * Time.deltaTime * _speed;
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //    }
    //
    //    if (Input.GetKey(KeyCode.D))
    //    {
    //        transform.rotation = Quaternion.Slerp(transform.rotation, Quaternion.LookRotation(Vector3.right), 0.2f);
    //        transform.position += Vector3.right * Time.deltaTime * _speed;
    //        //transform.Translate(Vector3.forward * Time.deltaTime * _speed);
    //    }
    //
    //    _moveToDest = false;
    //}

    void OnMouseEvent(Define.MouseEvent evt)
    {
        switch(State)
        {
            case Define.State.Idle:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Moving:
                OnMouseEvent_IdleRun(evt);
                break;
            case Define.State.Skill:
                {
                    if (evt == Define.MouseEvent.PointerUp)
                        _stopSkill = true;
                }
                break;
            case Define.State.Die:
                break;

        }

        //  밑에 줄들이랑 동일 하다

        //Debug.DrawRay(Camera.main.transform.position, ray.direction * 100.0f, Color.red, 1.0f);

        //int mask = (1 << 8); // 비트 연산자 사용 (1 << 8) | (1 << 9)  하면 8번,9번 레이어 사용한다.

        //Vector3 mousePos = Camera.main.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, Camera.main.nearClipPlane));
        //Vector3 dir = mousePos - Camera.main.transform.position;
        //dir = dir.normalized;
        //
        //Debug.DrawRay(Camera.main.transform.position, dir * 100.0f, Color.red, 1.0f);
        //
        //if (Physics.Raycast(Camera.main.transform.position, dir, out hit,100.0f))
        //{
        //    Debug.Log($"Raycast Camera @ {hit.collider.gameObject.name}");
        //}
    }

    void OnMouseEvent_IdleRun(Define.MouseEvent evt)
    {

        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        bool rayCastHit = Physics.Raycast(ray, out hit, 100.0f, _mask);

        switch (evt)
        {
            case Define.MouseEvent.PointerDown:
                {
                    if (rayCastHit)
                    {
                        _destPos = hit.point;
                        State = Define.State.Moving;
                        _stopSkill = false;

                        if (hit.collider.gameObject.layer == (int)Define.Layer.Moster)
                        {
                            _lockTarget = hit.collider.gameObject;
                        }
                        else
                        {
                            _lockTarget = null;
                        }
                    }
                }
                break;
            case Define.MouseEvent.Press:
                {
                    if (_lockTarget == null && rayCastHit)
                        _destPos = hit.point;
                }
                break;
            case Define.MouseEvent.PointerUp:
                _stopSkill = true;
                break;
        }
    }
}
