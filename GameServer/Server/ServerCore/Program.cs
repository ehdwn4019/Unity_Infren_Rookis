using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;
using System.Net.Sockets;
using System.Net;

namespace ServerCore
{
    class SessionManager
    {
        static object _lock = new object();

        public static void TestSession()
        {
            lock(_lock)
            {

            }
        }

        public static void Test()
        {
            lock(_lock)
            {
                UserManager.TestUser();
            }
        }
    }

    class UserManager
    {
        static object _lock = new object();

        public static void Test()
        {
            lock(_lock)
            {
                SessionManager.TestSession();
            }
        }

        public static void TestUser()
        {
            lock(_lock)
            {

            }
        }
    }

    //SpinLock 구현하기 
    //class SpinLock
    //{
    //    volatile int _locked = 0;
    //
    //    public void Acquire()
    //    {
    //        while(true)
    //        {
    //            //EXCHANGE
    //            //int original = Interlocked.Exchange(ref _locked, 1); // 원래의 값을 반환, 1이 된다면 이미 다른 스레드가 접근했다는 의미 
    //            //if (original == 0)
    //            //    break;
    //
    //            //COMPAREEXCHANGE
    //            int expected = 0;
    //            int desired = 1;
    //            int original = Interlocked.CompareExchange(ref _locked, desired, expected); // 원래의 값을 반환, ref의 값과 expected의 값이 같다면 desired값을 넣어준다 
    //            if (original == 0)
    //                break;
    //        }
    //        
    //
    //    }
    //
    //    public void Release()
    //    {
    //        _locked = 0;
    //    }
    //}

    //class Lock
    //{
    //    AutoResetEvent _available = new AutoResetEvent(true); //true, false에 따라서 문이 열린상태 또는 닫힌 상태로 결정 // 자동문 같은 개념  
    //
    //    ManualResetEvent _avilable_ver2 = new ManualResetEvent(true);// 수동문 같은개념 
    //
    //    public void Acquire()
    //    {
    //        _available.WaitOne(); // 입장 시도 
    //
    //        _avilable_ver2.Reset(); // 문을 직접 닫아줌, 수동문 이기 때문, lock 구현시에는 ManualResetEvent이 아닌 AutoResetEvent 사용 
    //    }
    //
    //    public void Release()
    //    {
    //        _available.Set();
    //    }
    //}

    class Program
    {

        #region 쓰레드 생성 
        static void MainThread(Object state)
        {
            for(int i=0; i<5; i++)
                Console.WriteLine("Hello Thread!");
        }

        #endregion


        #region 메모리 배리어 
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
        #endregion

        #region Interlocked, lock
        //static volatile int number = 0;
        static int number = 0;
        static object obj = new object();

        static void Thread1()
        {
            for(int i=0; i<10000000; i++)
            {
                //Interlocked.Increment(ref number); //변수의 원자성 보존, 성능에 문재 존재, 내부에서 메모리 배리어 사용하기 때문에 volatile 필요성이 없어짐 

                //lock과 같은 느낌, C++에서는 CriticalSection 또는 Std::Mutex 
                //Monitor.Enter(obj);
                //
                //number++;
                //
                //Monitor.Exit(obj); // Exit 처리가 안된다면 데드락인 상황 

                //try catch finally로도 가능, 예상치 못한 익셉션의 경우에 
                //try
                //{
                //    Monitor.Enter(obj);
                //    number++;
                //
                //    return;
                //}
                //finally
                //{
                //    Monitor.Exit(obj); 
                //}

                //보통은 lock 사용, 내부는 Monitor로 구현되있음 
                //lock(obj)
                //{
                //    number++;
                //}

                SessionManager.Test();
            }
        }

        static void Thread2()
        {
            for (int i = 0; i < 10000000; i++)
            {
                //Interlocked.Decrement(ref number); //변수의 원자성 보존, 성능에 문재 존재 

                //Monitor.Enter(obj);
                //
                //number--;
                //
                //Monitor.Exit(obj);
                //
                //lock(obj)
                //{
                //    number++;
                //}

                UserManager.Test();
            }
        }

        #endregion

        //spinlock은 기본적으로 구현되어있음 
        #region spinlock

        //static int _num = 0;
        //static SpinLock _spinLock = new SpinLock();
        //static Lock _lock = new Lock();
        //static Mutex _mutex = new Mutex(); //AutoResetEvent와 사용법 비슷, 커널 쪽 까지 가기 때문에 좀 느리다, 좀더 많은 정보를 가짐 

        //static void Thread_1_1()
        //{
        //    for(int i=0; i<100000; i++)
        //    {
        //        _spinLock.Acquire();
        //        _num++;
        //        _spinLock.Release();
        //
        //
        //        _mutex.WaitOne();
        //        _num++;
        //        _mutex.ReleaseMutex();
        //    }
        //}
        //
        //static void Thread_2_1()
        //{
        //    for(int i=0; i<100000; i++)
        //    {
        //        _spinLock.Acquire();
        //        _num--;
        //        _spinLock.Release();
        //
        //        _mutex.WaitOne();
        //        _num--;
        //        _mutex.ReleaseMutex();
        //    }
        //}



        //static object _lock = new object();
        //static SpinLock _lock2 = new SpinLock();
        //static Mutex _lock3 = new Mutex();

        #endregion

        static object _lock = new object();

        class Reward
        {

        }

        //ReaderWriterLock 보다 최신버전 >> ReaderWriterLockSlim
        static ReaderWriterLockSlim _lock3 = new ReaderWriterLockSlim(); 

        static Reward GetRewardById(int id)
        {
            _lock3.EnterReadLock();

            _lock3.ExitReadLock();

            return null;
        }

        static void AddReward(Reward reward)
        {
            _lock3.EnterWriteLock();

            _lock3.ExitWriteLock();
        }

        #region TLS 
        // 전역 변수이나 스레드 마다 고유한 공간을 보유, 전역 변수를 지역변수처럼 사용?
        static ThreadLocal<string> ThreadName = new ThreadLocal<string>(()=> { return $"My Name Is {Thread.CurrentThread.ManagedThreadId}"; }); 

        static void WhoAmI()
        {
            bool repeat = ThreadName.IsValueCreated;
            if(repeat)
                Console.WriteLine(ThreadName.Value+"(repeat)");
            else
                Console.WriteLine(ThreadName.Value);
        }
        #endregion


        static volatile int count = 0;
        static Lock _lock2 = new Lock();

        #region 소켓 프로그래밍 
        static Listener _listener = new Listener();
        static void OnAcceptHandler(Socket clientSocket)
        {
            try
            {
                Session session = new Session();
                session.Start(clientSocket);

                byte[] sendBuff = Encoding.UTF8.GetBytes("Welcome to MMORPG Server !");
                session.Send(sendBuff);

                Thread.Sleep(1000);

                session.Disconnect();

            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }

        #endregion

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

            //int count = 0;
            //
            //while(true)
            //{
            //    count++;
            //    x = y = r1 = r2 = 0;
            //
            //    Task t1 = new Task(Thread_1);
            //    Task t2 = new Task(Thread_2);
            //    t1.Start();
            //    t2.Start();
            //
            //    Task.WaitAll(t1, t2);
            //
            //    if (r1 == 0 && r2 == 0)
            //        break;
            //}
            //
            //Console.WriteLine($"{count}번만에 빠져나옴!");
            // r1 이랑 r2 가 0이되는 경우는 없어야 하는데 로직상으로는.. 멀티쓰레드에서는 그렇지 않기 때문에 메모리 배리어를 사용 ?

            //메모리 배리어 
            //1. 코드 재배치 억제 >> 코드가 연관이 없다 생각되면 하드웨어도 자체적으로 최적화를 진행하여 순서가 꼬일 수 있는 경우가 있다 .
            //2. 가시성 

            //Full Memory Barrier >> store , load 둘다 막는다.  >> 대부분 이거 사용 
            //Store Memory Barrier >> store 만 막는다. >> 이런게 있다 정도만 
            //Load Memory Barrier >> load 만 막는다. >> 이런게 있다 정도만 
            #endregion

            #region Interlocked, lock
            //Task t1 = new Task(Thread1);
            //Task t2 = new Task(Thread2);
            //t1.Start();
            //t2.Start();
            //
            //Task.WaitAll(t1, t2);
            //
            //Console.WriteLine(number);
            #endregion

            #region spinlock
            //Task t1 = new Task(Thread_1_1);
            //Task t2 = new Task(Thread_2_1);
            //
            //t1.Start();
            //t2.Start();
            //
            //Task.WaitAll(t1, t2);
            //
            //Console.WriteLine(_num);



            //lock (_lock)
            //{
            //
            //}
            //
            //bool lockTaken = false;
            //
            //try
            //{
            //    _lock2.Enter(ref lockTaken);
            //}
            //finally
            //{
            //    if (lockTaken)
            //        _lock2.Exit();
            //}

            #endregion

            Task t1 = new Task(delegate ()
            {
                for(int i=0; i<100000; i++)
                {
                    _lock2.WriteLock();
                    count++;
                    _lock2.WriteUnlock();
                }
            });

            Task t2 = new Task(delegate ()
            {
                for(int i=0; i< 100000; i++)
                {
                    _lock2.WriteLock();
                    count--;
                    _lock2.WriteUnlock();
                }
            });

            t1.Start();
            t2.Start();

            Task.WaitAll(t1, t2);

            Console.WriteLine(count);

            #region TLS 

            ThreadPool.SetMinThreads(1, 1);
            ThreadPool.SetMaxThreads(3, 3);
            Parallel.Invoke(WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI, WhoAmI);

            ThreadName.Dispose();
            #endregion

            #region 소켓 프로그래밍

            // DNS >> Domain Name System 
            // www.kdj.com >> 도메인 등록, IP가 아닌 string 값으로 관리, IP를 적어넣으면 하드코딩 

            string host = Dns.GetHostName();
            IPHostEntry ipHost =  Dns.GetHostEntry(host);
            IPAddress ipAddr = ipHost.AddressList[0]; // google 같이 많이 접속하는 사이트는 여러개의 IP를 가지고 있다. 그래서 list로 받음 
            IPEndPoint endPoint = new IPEndPoint(ipAddr, 7777);

            _listener.Init(endPoint, OnAcceptHandler);

            while (true)
            {
                //임시로 서버 계속 실행시키기 위한 무한루프 
            }
            #endregion
        }
    }
}
