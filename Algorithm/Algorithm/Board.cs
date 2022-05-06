using System;
using System.Collections.Generic;
using System.Text;

namespace Algorithm
{
    #region List구현
    class MyList<T>
    {
        const int DEFAULTSize = 1;
        T[] _data = new T[DEFAULTSize];

        public int Count = 0; //기존 생성된 데이터 개수
        public int Capacity { get { return _data.Length; } } //미리 생성된 데이터 개수

        //Add 함수는 시간복잡도로 봤을때 상수는 다제외하므로 반복문 개수 확인해보면 O(n) 인데 if문안에서 조건계산하므로 없다고 쳐서 O(1) 로 상수복잡도가 된다. >> 예외 케이스 , 이사비용 무시
        public void Add(T item)
        {
            // 1. 공간 남아있는지 확인하기 
            if(Count >= Capacity)
            {
                // 공간 늘리기 
                T[] newArray = new T[Count * 2];
                for(int i=0; i<Count; i++)
                {
                    newArray[i] = _data[i];
                }

                _data = newArray;
            }

            // 2. 공간에 데이터 넣기 
            _data[Count] = item;
            Count++;
        }

        // 시간복잡도 : O(1)
        public T this[int index]
        {
            get { return _data[index]; }
            set { _data[index] = value; }
        }

        // 시간복잡도 : O(N)
        public void RemoveAt(int index)
        {
            for(int i= index; i<Count-1; i++)
            {
                _data[i] = _data[i + 1];
            }

            _data[Count - 1] = default(T); //T라는 형식의 default 값으로 밀어달라 >> int 면 0 , string 이면 null 등등...

            Count--;
        }
    }

    #endregion

    #region LinkedList구현
    class MyLinkedListNode<T>
    {
        public T Data;
        public MyLinkedListNode<T> Next;
        public MyLinkedListNode<T> Prev;
    }


    class MyLinkedList<T>
    {
        public MyLinkedListNode<T> Head = null;
        public MyLinkedListNode<T> Tail = null;
        public int Count = 0;

        //시간 복잡도 : O(1)
        public MyLinkedListNode<T> AddLast(T data)
        {
            MyLinkedListNode<T>  newRoom = new MyLinkedListNode<T>();
            newRoom.Data = data;

            //아직 방이 없다면 첫번째 방이 Head가 된다.
            if (Head == null)
                Head = newRoom;

            //기존의 방과 새로추가되는 방을 연결
            if(Tail!=null)
            {
                Tail.Next = newRoom;
                newRoom.Prev = Tail;
            }

            //새로 추가되는 방을 마지막 방으로 설정
            Tail = newRoom;
            Count++;
            return newRoom;
        }

        //시간 복잡도 : O(1)
        public void Remove(MyLinkedListNode<T> room)
        {
            //삭제하려는 방이 첫번쨰 방이면 두번째 방을 첫번째 방으로 설정한다.
            if (Head == room)
                Head = Head.Next;

            //삭제하려는 방이 마지막 방이면 마지막 이전방을 마지막 방으로 설정한다.
            if (Tail == room)
                Tail = Tail.Prev;

            if (room.Prev != null)
                room.Prev.Next = room.Next;

            if (room.Next != null)
                room.Next.Prev = room.Prev;

            Count--;
        }
    }

    #endregion

    class Board
    {
        public int[] _data = new int[25];
        public List<int> _data2 = new List<int>();
        public LinkedList<int> _data3 = new LinkedList<int>();

        const char CIRCLE = '\u25cf';
        public TileType[,] Tile { get; private set; }
        public int Size { get; private set; }

        public int DestY { get; private set; }
        public int DestX { get; private set; }

        Player _player;

        public enum TileType
        {
            Empty,
            Wall,
        }

        public void Initialize(int size,Player player)
        {
            #region List실행
            _data2.Add(101);
            _data2.Add(102);
            _data2.Add(103);
            _data2.Add(104);
            _data2.Add(105);

            int temp = _data2[2];

            _data2.Remove(2);
            #endregion

            #region LinkedList실행
            _data3.AddLast(101);
            _data3.AddLast(102);
            LinkedListNode<int> node = _data3.AddLast(103);
            _data3.AddLast(104);
            _data3.AddLast(105);

            _data3.Remove(node);
            #endregion

            if (size % 2 == 0)
                return;

            _player = player;

            Tile = new TileType[size, size];
            Size = size;

            DestY = Size - 2;
            DestX = Size - 2;

            //GenerateByBinaryTree();
            GenerateBySideWinder();
        }

        //Binary Tree Algorithm
        void GenerateByBinaryTree()
        {
            //길을 막는작업 
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            //랜덤으로 길을 뚫는 작업
            
            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                    }
                    else
                    {
                        Tile[y + 1, x] = TileType.Empty;
                    }

                }
            }
        }

        void GenerateBySideWinder()
        {
            //길을 막는작업 
            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        Tile[y, x] = TileType.Wall;
                    else
                        Tile[y, x] = TileType.Empty;
                }
            }

            //랜덤으로 길을 뚫는 작업

            Random rand = new Random();
            for (int y = 0; y < Size; y++)
            {
                int count = 1;
                for (int x = 0; x < Size; x++)
                {
                    if (x % 2 == 0 || y % 2 == 0)
                        continue;

                    if (y == Size - 2 && x == Size - 2)
                        continue;

                    if (y == Size - 2)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        continue;
                    }

                    if (x == Size - 2)
                    {
                        Tile[y + 1, x] = TileType.Empty;
                        continue;
                    }

                    if (rand.Next(0, 2) == 0)
                    {
                        Tile[y, x + 1] = TileType.Empty;
                        count++;
                    }
                    else
                    {
                        int randomIndex = rand.Next(0, count);
                        Tile[y + 1, x-randomIndex*2] = TileType.Empty;
                        count = 1;
                    }

                }
            }
        }

        public void Render()
        {
            ConsoleColor prevColor = Console.ForegroundColor;

            for (int y = 0; y < Size; y++)
            {
                for (int x = 0; x < Size; x++)
                {
                    if (y == _player.PosY && x == _player.PosX)
                        Console.ForegroundColor = ConsoleColor.Blue;
                    else if (y == DestY && x == DestX)
                        Console.ForegroundColor = ConsoleColor.Yellow;
                    else
                        Console.ForegroundColor = GetTileColor(Tile[y, x]);

                    Console.Write(CIRCLE);
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = prevColor;
        }

        ConsoleColor GetTileColor(TileType type)
        {
            switch(type)
            {
                case TileType.Empty:
                    return ConsoleColor.Green;
                case TileType.Wall:
                    return ConsoleColor.Red;
                default:
                    return ConsoleColor.Blue;
            }
        }
    }
}
