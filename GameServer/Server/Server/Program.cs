using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    class Program
    {
        static void Main(string[] args)
        {
            #region 캐시 테스트
            int[,] arr = new int[10000, 10000];
            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[y, x] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(y,x) 순서 걸린 시간 {end-now}");
                //같은 공간을 참조하므로 시간이 적게 걸린다. 
                //ex y,x이므로 0,1 0,2 0,3 .... 이므로 이미 0이 참조된 상태로 같은 공간에 있으므로 다음 인덱스를 참조할것이다라고 예상하여 미리 캐싱해놓는다?
            }

            {
                long now = DateTime.Now.Ticks;
                for (int y = 0; y < 10000; y++)
                    for (int x = 0; x < 10000; x++)
                        arr[x, y] = 1;
                long end = DateTime.Now.Ticks;
                Console.WriteLine($"(x,y) 순서 걸린 시간 {end-now}");
                //다른 공간을 참조하므로 시간이 더걸린다. 
                //ex x,y이므로 1,0 2,0 3,0 .... 이므로 같은 공간일줄알고 다음 인덱스들을 캐싱해놨는데 공간을 벗어나므로 시간이 좀더 걸린다? 
            }
            #endregion
        }
    }
}
