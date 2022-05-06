using System;
using System.Collections.Generic;
using System.Text;

namespace CSharp
{
    //옵저버 패턴 : 이렇게 event로 구독? 신청을 하면 알림을 뿌려주는 기능 
    class InputManager
    {
        //delegate , event  : 델리게이트를 외부에서 함부러 호출 못하게 은닉화 시킨게 이벤트
        public delegate void OnInputKey();
        public event OnInputKey InputKey;

        public void Update()
        {

            if (Console.KeyAvailable == false) // 아무키도 누르지 않았을때 
                return;

            ConsoleKeyInfo info = Console.ReadKey();
            if (info.Key == ConsoleKey.A)
            {
                InputKey();
            }
        }
    }
}
