using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerCore
{
    class Program
    {
        static void MainThread(Object state)
        {
            for(int i=0; i<5; i++)
                Console.WriteLine("Hello Thread!");
        }




        volatile static bool _stop = false; // 릴리즈모드로 실행시 어셈블리단에서 코드를 자동 최적화 시켜주는데 이걸 방지하는 키워드가 volatile이다. C++과 C#에서 작동 방식이 다르다.
                                            // C#에서는 권장하지 않음

        static void ThreadMain()
        {
            Console.WriteLine("쓰레드 시작!");

            while(_stop == false)
            {
                
            }

            Console.WriteLine("쓰레드 종료!");
        }



        static int x = 0;
        static int y = 0;
        static int r1 = 0;
        static int r2 = 0;

        static void Thread_1()
        {
            y = 1; // Store >> 변수에 직접 값을 넣어주는것 

            Thread.MemoryBarrier(); // 이렇게 해주면 하드웨어가 두 코드가 연관이 없어서 마음대로 순서를 바꾸는 경우를 방지해준다. 

            r1 = x; // Load >> 변수에 변수를 대입하는 것
        }

        static void Thread_2()
        {
            x = 1; 

            Thread.MemoryBarrier(); // 이렇게 해주면 하드웨어가 두 코드가 연관이 없어서 마음대로 순서를 바꾸는 경우를 방지해준다. 

            r2 = y;
        }

        static void Main(string[] args)
        {
            #region 쓰레드 생성

            //Task t = new Task(()=> { while (true) { } },TaskCreationOptions.LongRunning); // thread와 기능은 같으나 longRunning으로 오래걸릴것같은 작업은 따로 쓰레드 풀에서 하나 제외시켜 돌린다?
            //t.Start();
            //
            //ThreadPool.SetMinThreads(1, 1);
            //ThreadPool.SetMaxThreads(5, 5);
            //
            //for (int i = 0; i < 5; i++)
            //    ThreadPool.QueueUserWorkItem((obj) => { while (true) { } });
            //ThreadPool.QueueUserWorkItem(MainThread); // 백그라운드에서 실행되는 쓰레드 풀 

            //Thread t = new Thread(MainThread);
            //t.IsBackground = true; // 백그라운드에서 실행되므로 메인함수가 종료되면 바로 종료됨 
            //t.Name = "Test Thread";
            //t.Start();
            //Console.WriteLine("Waiting for Thread!");

            //t.Join();

            //Console.WriteLine("Hello World!");
            //while(true)
            //{
            //
            //}

            //Task t1 = new Task(ThreadMain);
            //t1.Start();

            //Thread.Sleep(1000);

            //_stop = true;

            //Console.WriteLine("Stop 호출");
            //Console.WriteLine("종료 대기중");
            //t1.Wait(); // >> thread의 join과 같은 함수 
            //Console.WriteLine("종료 성공");
            #endregion


            #region 메모리 배리어 

            int count = 0;

            while(true)
            {
                count++;
                x = y = r1 = r2 = 0;

                Task t1 = new Task(Thread_1);
                Task t2 = new Task(Thread_2);
                t1.Start();
                t2.Start();

                Task.WaitAll(t1, t2);

                if (r1 == 0 && r2 == 0)
                    break;
            }

            Console.WriteLine($"{count}번만에 빠져나옴!");
            // r1 이랑 r2 가 0이되는 경우는 없어야 하는데 로직상으로는.. 멀티쓰레드에서는 그렇지 않기 때문에 메모리 배리어를 사용 ?

            //메모리 배리어 
            //1. 코드 재배치 억제 >> 코드가 연관이 없다 생각되면 하드웨어도 자체적으로 최적화를 진행하여 순서가 꼬일 수 있는 경우가 있다 .
            //2. 가시성 

            //Full Memory Barrier >> store , load 둘다 막는다.  >> 대부분 이거 사용 
            //Store Memory Barrier >> store 만 막는다. >> 이런게 있다 정도만 
            //Load Memory Barrier >> load 만 막는다. >> 이런게 있다 정도만 
            #endregion

        }
    }
}
