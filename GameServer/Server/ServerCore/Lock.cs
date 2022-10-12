using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Threading;

namespace ServerCore
{
    class Lock
    {
        const int EMPTY_FLAG = 0x00000000;
        const int WRITE_MASK = 0x7FFF0000;
        const int READ_MASK = 0x0000FFFF;
        const int MAX_SPIN_COUNT = 5000;

        int _flag = EMPTY_FLAG;
        int _writeCount = 0;

        public void WriteLock()
        {
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if(Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                _writeCount++;
                return;
            }

            while(true)
            {
                int desired = (Thread.CurrentThread.ManagedThreadId << 16) & WRITE_MASK;
                for(int i=0; i<MAX_SPIN_COUNT; i++)
                {
                    //성공 했는지 확인 
                    if (Interlocked.CompareExchange(ref _flag, desired, EMPTY_FLAG) == EMPTY_FLAG)
                    {
                        _writeCount = 1;
                        return;
                    }
                }

                Thread.Yield();
            }
        }

        public void WriteUnlock()
        {
            //초기화 
            int lockCount = --_writeCount;
            if(lockCount == 0)
                Interlocked.Exchange(ref _flag, EMPTY_FLAG);
        }

        public void ReadLock()
        {
            int lockThreadId = (_flag & WRITE_MASK) >> 16;
            if (Thread.CurrentThread.ManagedThreadId == lockThreadId)
            {
                Interlocked.Increment(ref _flag);
                return;
            }

            while (true)
            {
                for(int i=0; i<MAX_SPIN_COUNT; i++)
                {
                    int expected = (_flag & READ_MASK);
                    //WRITE_ROCK 획득하지 않은 경우 
                    if(Interlocked.CompareExchange(ref _flag, expected+1, expected) == expected)
                    {

                    }
                }

                Thread.Yield();
            }
        }

        public void ReadUnLock()
        {
            Interlocked.Decrement(ref _flag);
        }
    }
}
