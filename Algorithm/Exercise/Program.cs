using System;
using System.Collections.Generic;

namespace Exercise
{
    class Graph
    {
        //2차원 배열 행렬을 이용하는 방법
        int[,] adj = new int[6, 6]
        {
            { -1,15,-1,35,-1,-1},
            { 15,-1,05,10,-1,-1},
            { -1,05,-1,-1,-1,-1},
            { 35,10,-1,-1,05,-1},
            { -1,-1,-1,05,-1,05},
            { -1,-1,-1,-1,05,-1},
        };

        //List를 이용하는 방법
        List<int>[] adj2 = new List<int>[]
        {
            new List<int>(){1,3},
            new List<int>(){0,2,3},
            new List<int>(){1},
            new List<int>(){0,1,4},
            new List<int>(){3,5},
            new List<int>(){4},
        };

        #region DFS

        bool[] visited = new bool[6];

        //==2차원 배열 행렬을 이용하는 방법==
        //1. now 부터 방문
        //2. now와 연결된 정점들을 하나씩 확인해서 아직 비방문 상태라면 방문
        public void DFS(int now)
        {
            Console.WriteLine(now);
            visited[now] = true; //방문 완료 

            for(int next = 0; next<6; next++)
            {
                if (adj[now, next] == 0) // 연결되어 있지 않으면 스킵.
                    continue;
                if (visited[next]) //이미 방문했으면 스킵.
                    continue;
                DFS(next);
            }
        }

        //==List를 이용하는 방법==
        public void DFS2(int now)
        {
            Console.WriteLine(now);
            visited[now] = true; //방문 완료 

            foreach(int next in adj2[now]) // 연결되어 있지 않으면 스킵. >> 연결되어 있는 애들만 데려옴 
            {
                if (visited[next])//이미 방문했으면 스킵.
                    continue;
                DFS2(next);
            }
        }

        public void SearchAll()
        {
            visited = new bool[6];
            for (int now = 0; now < 6; now++)
                if (visited[now] == false)
                    DFS(now);
        }

        #endregion

        #region BFS
        public void BFS(int start)
        {
            bool[] found = new bool[6];
            int[] parent = new int[6];
            int[] distance = new int[6];

            Queue<int> q = new Queue<int>();
            q.Enqueue(start);
            found[start] = true;
            parent[start] = start;
            distance[start] = 0;

            while(q.Count>0)
            {
                int now = q.Dequeue();
                Console.WriteLine(now);

                for(int next =0; next<6; next++)
                {
                    if (adj[now, next] == 0) // 인접하지 않았으면 스킵
                        continue;
                    if (found[next]) // 이미 발견 했으면 스킵
                        continue;
                    q.Enqueue(next);
                    found[next] = true;
                    parent[next] = now; // 현재 지점의 부모 찾기
                    distance[next] = distance[now]+1; // 현재지점의 시작점부터의 총거리 
                }
            }
        }
        #endregion

        #region Dijikstra
        public void Dijikstra(int start)
        {
            bool[] visited = new bool[6];
            int[] distance = new int[6];
            int[] parent = new int[6];
            Array.Fill(distance, Int32.MaxValue);

            

            distance[start] = 0;
            parent[start] = start;

            

            while (true)
            {
                // 가장 유력한 후보의 거리와 번호를 저장한다.
                int closest = Int32.MaxValue;
                int now = -1;

                // 제일 좋은 후보를 찾는다 (가장 가까이에 있는)
                for(int i=0; i<6; i++)
                {
                    // 이미 방문한 정점은 스킵 
                    if (visited[i])
                        continue;
                    // 아직 발견된 적이 없거나 , 기존 후보 보다 멀리 있으면 스킵 
                    if (distance[i] == Int32.MaxValue || distance[i] >= closest)
                        continue;
                    // 여태껏 발견한 가장 좋은 후보이니 정보 갱신 
                    closest = distance[i];
                    now = i;
                }

                // 다음 후보가 없다 
                if (now == -1)
                    break;

                //제일 좋은 후보를 찾음 
                visited[now] = true;

                //방문한 정점과 인접한 정점들을 조사해서, 상황에 따라 발견한 최단거리를 갱신한다.
                for (int next = 0; next < 6; next++)
                {
                    //연결되지 않으면 스킵
                    if (adj[now, next] == -1)
                        continue;
                    //이미 방문한 정점은 스킵 
                    if (visited[next])
                        continue;

                    //새로 조사된 정점의 최단거리를 계산한다
                    int nextDist = distance[now] + adj[now, next];
                    //만약 기존에 발견한 최단거리가 새로 조사된 최단거리보다 크면 , 정보를 갱신 
                    if(nextDist< distance[next])
                    {
                        distance[next] = nextDist;
                        parent[next] = now;
                    }
                }
            }

            
        }
        #endregion
    }

    class TreeNode<T>
    {
        public T Data { get; set; }
        public List<TreeNode<T>> children { get; set; } = new List<TreeNode<T>>();
    }

    // 우선 순위 큐
    class PriorityQueue<T> where T : IComparable<T>
    {
        List<T> _heap = new List<T>();

        //O(logN)
        public void Push(T data)
        {
            // 힙의 맨 끝에 새로운 데이터를 삽입한다.
            _heap.Add(data);

            int now = _heap.Count - 1;

            //꼭대기,루트로 이동 시키기 , 올라가기
            while(now > 0)
            {
                int next = (now - 1) / 2; // 공식 , 부모 노드 찾아가기
                if (_heap[now].CompareTo(_heap[next]) < 0)
                    break;

                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }
        }

        //O(logN)
        public T Pop()
        {
            // 반환할 데이터 저장 
            T ret = _heap[0];

            // 마지막 데이터 루트로 이동
            int lastIndex = _heap.Count - 1;
            _heap[0] = _heap[lastIndex];
            _heap.RemoveAt(lastIndex);
            lastIndex--;

            //내려가기
            int now = 0;
            while(true)
            {
                int left = 2 * now + 1;
                int right = 2 * now + 2;

                int next = now;
                if (left <= lastIndex && _heap[next].CompareTo(_heap[left]) < 0)
                    next = left;
                if (right <= lastIndex && _heap[next].CompareTo(_heap[right]) < 0)
                    next = right;

                if (next == now)
                    break;

                T temp = _heap[now];
                _heap[now] = _heap[next];
                _heap[next] = temp;

                now = next;
            }

            return ret;
        }

        public int Count()
        {
            return _heap.Count;
        }
    }

    class Knight : IComparable<Knight>
    {
        public int id { get; set; }

        public int CompareTo(Knight other)
        {
            if (id == other.id)
                return 0;
            return id > other.id ? 1 : -1;
        }
    }

    class Program
    {
        //트리 생성 
        static TreeNode<string> MakeTree()
        {
            TreeNode<string> root = new TreeNode<string>() { Data = "R1 개발실" };
            {
                {
                    TreeNode<string> node = new TreeNode<string> { Data = "디자인팀" };
                    node.children.Add(new TreeNode<string>() { Data = "전투" });
                    node.children.Add(new TreeNode<string>() { Data = "경제" });
                    node.children.Add(new TreeNode<string>() { Data = "스토리" });
                    root.children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string> { Data = "프로그래밍팀" };
                    node.children.Add(new TreeNode<string>() { Data = "서버" });
                    node.children.Add(new TreeNode<string>() { Data = "클라" });
                    node.children.Add(new TreeNode<string>() { Data = "엔진" });
                    root.children.Add(node);
                }

                {
                    TreeNode<string> node = new TreeNode<string> { Data = "아트팀" };
                    node.children.Add(new TreeNode<string>() { Data = "배경" });
                    node.children.Add(new TreeNode<string>() { Data = "캐릭터" });
                    root.children.Add(node);
                }

                return root;
            }
        }

        //트리 순환 출력 >> 트리는 거의 재귀함수 사용
        static void PrintTree(TreeNode<string> root)
        {
            //접근
            Console.WriteLine(root.Data);

            foreach (TreeNode<string> child in root.children)
                PrintTree(child);
        }

        //트리 높이 구하기
        static int GetHeight(TreeNode<string> root)
        {
            int height = 0;

            foreach(TreeNode<string> child in root.children)
            {
                int newHeight = GetHeight(child)+1;

                //if (height < newHeight)
                //    height = newHeight;
                height = Math.Max(height, newHeight); //if문 말고 이렇게도 할 수 있다.
            }


            return height;
        }



        static void Main(string[] args)
        {
            //// 스택 
            //Stack<int> stack = new Stack<int>();
            //
            //stack.Push(101);
            //stack.Push(102);
            //stack.Push(103);
            //stack.Push(104);
            //stack.Push(105);
            //
            //int data = stack.Pop();
            //int data2 = stack.Peek();
            //
            //// 큐
            //Queue<int> queue = new Queue<int>();
            //queue.Enqueue(101);
            //queue.Enqueue(102);
            //queue.Enqueue(103);
            //queue.Enqueue(104);
            //queue.Enqueue(105);
            //
            //int data3 = queue.Dequeue();
            //int data4 = queue.Peek();
            //
            //List<int> list = new List<int>() { 1, 2, 3, 4 };
            //
            //for(int i=0; i<list.Count; i++)
            //{
            //    Console.WriteLine(list[i]);
            //}
            //
            //foreach(int val in list)
            //{
            //    Console.WriteLine(val);
            //}

            //DFS (Depth First Search) : 깊이 우선 탐색
            //BFS (Breadth first Search) : 너비 우선 탐색 

            //이진 검색 트리 특징 : 왼쪽을 타고 가면 현재 값보다 작고 오른쪽을 타고 가면 현재 값보다 크다.
            //힙 트리 : 부모 노드가 가진 값이 자식 노드보다 항상 크고 자식 노드의 좌우 값은 상관이 없다. / 마지막 노드를 제외한 나머지 노드들은 꽉 차 있다. / 
            //         마지막 노드가 있을 때는 항상 왼쪽부터 순서대로 채워야 한다. / 노드의 개수를 알게되면 트리 구조는 확실하게 알 수 있다.
            //         배열로 표시할 때 : i 번 노드의 왼쪽 자식은 [(2*i) +1] , 오른쪽 자식은 [(2*i)+2] , 부모는 [(i-1)/2] 로 확인 가능
            //         최대값을 구할 때 편리 , 루트가 최대값

            Graph graph = new Graph();
            graph.Dijikstra(0);

            TreeNode<string> root = MakeTree();
            PrintTree(root);
            Console.WriteLine(GetHeight(root));

            PriorityQueue<Knight> q = new PriorityQueue<Knight>();
            q.Push(new Knight() { id = 20 });
            q.Push(new Knight() { id = 30 });
            q.Push(new Knight() { id = 40 });
            q.Push(new Knight() { id = 10 });
            q.Push(new Knight() { id = 05 });

            while(q.Count() > 0)
            {
                Console.WriteLine(q.Pop().id);
            }

            //-를 붙여주면 가장 작은수가 루트가 된다.
          /*q.Push(20);
            q.Push(10);
            q.Push(30);
            q.Push(90);
            q.Push(40);
            Console.WriteLine(-q.Pop()); 
          */

        }
    }
}
