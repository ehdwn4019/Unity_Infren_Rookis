using System;
using System.Collections.Generic;
using System.Reflection;

namespace CSharp
{
    //class Player
    //{
    //    public int hp;
    //    public int attack;
    //
    //    public Player()
    //    {
    //
    //    }
    //
    //    public Player(int hp) : this() // 본인객체 참조
    //    {
    //
    //    }
    //
    //    public void Move()
    //    {
    //
    //    }
    //
    //    public virtual void Run()
    //    {
    //
    //    }
    //}

    //class Knight : Player
    //{
    //    public Knight() : base() // 부모 객체 참조
    //    {
    //
    //    }
    //
    //    public new void Move() // new 키워드로  부모 Move 숨길수 있음
    //    {
    //
    //    }
    //
    //    //public sealed override void Run() // 쉴드로 상속에 상속된 클래스 오버라이드 방지 가능
    //    //{
    //    //    base.Run();
    //    //}
    //}

    //class Mage : Player
    //{
    //    public Mage() : base()
    //    {
    //
    //    }
    //
    //    public new void Move()
    //    {
    //
    //    }
    //}

    // 타일찍기
    class Map
    {
        //매우 중요한 정보 
        [Important("Very Important")]
        int[,] tiles = { 
            { 1, 1, 1, 1, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 0, 0, 0, 1 },
            { 1, 1, 1, 1, 1 }
        };

        public void Render()
        {
            ConsoleColor defaultColor = Console.ForegroundColor;

            for(int y=0; y<tiles.GetLength(1); y++)
            {
                for(int x=0; x<tiles.GetLength(0); x++)
                {
                    if (tiles[y, x] == 1)
                        Console.ForegroundColor = ConsoleColor.Red;
                    else
                        Console.ForegroundColor = ConsoleColor.Green;
                    Console.Write('\u25cf');
                }

                Console.WriteLine();
            }

            Console.ForegroundColor = defaultColor;
        }
    }

    //람다 
    enum ItemType
    {
        Weapon,
        Armor,
        Amulet,
        Ring
    }

    enum Rarity
    {
        Normal,
        Uncommon,
        Rare,
    }

    class Item
    {
        public ItemType ItemType;
        public Rarity Rarity;
    }

    //리플렉션 , 어트리뷰트
    class Important : System.Attribute //컴퓨터가 런타임에 주석을 체크할수 있는것 
    {
        string message;

        public Important(string message) { this.message = message; }
    }

    class Program 
    {
        enum Choice
        {
            ROCK = 1,
            PAPER = 2 ,
            SCISSORS = 0
        }

        //팩토리얼 함수 
        static int Factorial(int n)
        {
            int ret = 1;

            for (int num = 1; num <= n; num++)
            {
                ret *= num;
            }

            return ret;
        }

        //재귀 함수
        static int Factorial2(int n)
        {
            if (n <= 1) return 1;
            return n * Factorial2(n - 1);
        }

        #region TextRPG
        enum ClassType
        {
            None = 0,
            Knight = 1,
            Archer = 2,
            Mage = 3
        }

        enum MonsterType
        {
            None = 0,
            Slime = 1,
            Orc = 2,
            Skeleton = 3
        }

        struct Player
        {
            public int hp;
            public int attack;

            public static implicit operator Player(Knight v)
            {
                throw new NotImplementedException();
            }
        }

        struct Monster
        {
            public int hp;
            public int attack;

            public static implicit operator Monster(Orc v)
            {
                throw new NotImplementedException();
            }
        }

        static ClassType ChooseClass()
        {
            Console.WriteLine("직업을 선택하세요! ");
            Console.WriteLine("[1] 기사");
            Console.WriteLine("[2] 궁수");
            Console.WriteLine("[3] 법사");

            ClassType choice = ClassType.None;
            string input = Console.ReadLine();

            switch (input)
            {
                case "1":
                    choice = ClassType.Knight;
                    break;
                case "2":
                    choice = ClassType.Archer;
                    break;
                case "3":
                    choice = ClassType.Mage;
                    break;
            }

            return choice;
        }

        static void CreatePlayer(ClassType choice , out Player player)
        {
            //기사(100/100) 궁수(75/12) 법사(50/15)
            switch (choice)
            {
                case ClassType.Knight:
                    player.hp = 100;
                    player.attack = 10;
                    break;
                case ClassType.Archer:
                    player.hp = 75;
                    player.attack = 12;
                    break;
                case ClassType.Mage:
                    player.hp = 50;
                    player.attack = 15;
                    break;
                default:
                    player.hp = 0;
                    player.attack = 0;
                    break;
                    
            }

        }

        static void CreateRandomMonster(out Monster monster)
        {
            Random rand = new Random();
            int randMonster = rand.Next(1, 4);
            switch (randMonster)
            {
                case (int)MonsterType.Slime:
                    Console.WriteLine("슬라임이 스폰되었습니다!");
                    monster.hp = 20;
                    monster.attack = 2;
                    break;
                case (int)MonsterType.Orc:
                    Console.WriteLine("오크가 스폰되었습니다!");
                    monster.hp = 40;
                    monster.attack = 4;
                    break;
                case (int)MonsterType.Skeleton:
                    Console.WriteLine("스켈레톤이 스폰되었습니다!");
                    monster.hp = 30;
                    monster.attack = 3;
                    break;
                default:
                    monster.hp = 0;
                    monster.attack = 0;
                    break;
            }
        }

        static void Fight(ref Player player,ref Monster monster)
        {
            while(true)
            {
                //플레이어가 몬스터 공격
                monster.hp -= player.attack;
                if(monster.hp<=0)
                {
                    Console.WriteLine("승리했습니다!");
                    Console.WriteLine($"남은 체력 : {player.hp}");
                    break;
                }

                //몬스터 반격
                player.hp -= monster.attack;
                if(player.hp<=0)
                {
                    Console.WriteLine("패배했습니다!");
                    break;
                }
            }
        }

        static void EnterGame(ref Player player)
        {

            while(true)
            {
                Console.WriteLine("마을에 접속했습니다!");
                Console.WriteLine("[1] 필드로 간다");
                Console.WriteLine("[2] 로비로 돌아가기");

                string input = Console.ReadLine();
                switch (input)
                {
                    case "1":
                        EnterField(ref player);
                        break;
                    case "2":
                        return;
                }
            }
            
        }

        static void EnterField(ref Player player)
        {
            while(true)
            {
                Console.WriteLine("필드에 접속했습니다!");

                //몬스터 생성
                Monster monster;
                CreateRandomMonster(out monster);

                Console.WriteLine("[1] 전투 모드로 돌입");
                Console.WriteLine("[2] 일정 확률로 마을로 도망");

                string input = Console.ReadLine();
                if(input == "1")
                {
                    Fight(ref player,ref monster);
                }
                else if(input =="2")
                {
                    //33% 
                    Random rand = new Random();
                    int randValue = rand.Next(0, 101);
                    if(randValue <= 33)
                    {
                        Console.WriteLine("도망치는데 성공했습니다!");
                        break;
                    }
                    else
                    {
                        Fight(ref player, ref monster);
                    }
                }
            }

            

        }

        #endregion

        //is as TEST 
        static void IsAsTest(Player player)
        {
            //bool isPlayer = (player is Mage); // is 
            //Mage mage = (player as Mage); // as 

            //if (isPlayer)
            //{
            //
            //}

            //if(mage != null)
            //{
            //
            //}
        }

        //배열 
        static int GetHighestScore(int[] scores)
        {
            int maxValue = 0;

            foreach(int score in scores)
            {
                if (score>=maxValue)
                    maxValue = score;
            }

            return maxValue;
        }

        static int GetAverageScore(int[] scores)
        {
            if (scores.Length == 0)
                return 0;

            int sum = 0;
            foreach(int score in scores)
            {
                sum += score;
            }

            return sum/scores.Length;
        }

        static int GetIndexOf(int[] scores, int value)
        {
            for(int i=0; i<scores.Length; i++)
            {
                if (scores[i] == value)
                    return i;
            }

            return -1;
        }

        static void Sort(int[] scores)
        {
            for(int i=0; i<scores.Length; i++)
            {
                int minIndex = i;
                for(int j= i; j<scores.Length; j++)
                {
                    if (scores[j] < scores[minIndex])
                        minIndex = j;
                }

                int temp = scores[i];
                scores[i] = scores[minIndex];
                scores[minIndex] = temp;
            }
        }

        //델리게이트 , 이벤트 
        delegate int OnClicked();

        static void ButtonPressed(OnClicked clickedFunction)
        {
            clickedFunction();
        }

        static int TestDelegate()
        {
            Console.WriteLine("Hello Delegate");
            return 0;
        }

        static int TestDelegate2()
        {
            Console.WriteLine("Hello Delegate2");
            return 0;
        }


        static void OnInputTest()
        {
            Console.WriteLine("Input Received!");
        }



        //람다 
        static List<Item> _items = new List<Item>();

        delegate Return MyFunc<Return>();
        delegate Return MyFunc<T,Return>(T item);
        delegate Return MyFunc<T1, T2 , Return>(T1 t1 , T2 t2);

        static Item FindItem(MyFunc<Item,bool> selector)
        {
            foreach(Item item in _items)
            {
                if (selector(item))
                    return item;
            }

            return null;
        }



        static void Main(string[] args)
        {
            #region 기초 내용들
            //int hp;
            //short level = 100;
            //long id;
            //hp = 100;
            //
            //byte attack = 0;
            //attack--;
            //
            //bool b;
            //b = true;
            //b = false;
            //
            //float f = 3.14f;
            //double d = 3.14;
            //
            //char c = 'a';
            //string str = "Hello World";
            //
            //Console.WriteLine(str);
            //
            //int a = 1000;
            //short e = (short)a;
            //int g = a;
            //
            //byte h = 255;
            //sbyte sb = (sbyte)h;
            //
            //int i = 0x0FFFFFFF;
            //short sb2 = (short)i;
            //
            //float f2 = 3.1414f;
            //double d2 = f;
            //
            //Console.WriteLine("Hello Number ! {0}", attack);
            //
            //int hp2 = 100;
            //hp2 = 100 + 1;
            //hp2 = 100 - 1;
            //hp2 = 100 * 2;
            //hp2 = 100 / 2;
            //hp2 = hp2 % 3;
            //
            //hp2++;
            //hp2 = hp2 + 1;
            //
            //hp2--;
            //hp2 = hp2 - 1;
            //
            //++hp2;
            //--hp2;

            //int hp = 100;
            //int level = 50;
            //
            //bool isAlive = (hp > 0);
            //bool isHighLevel = (level >= 40);
            //
            //bool a = isAlive && isHighLevel;
            //
            //bool b = isAlive || isHighLevel;

            //int num = 1;
            //num = num << 1;
            //
            //Console.WriteLine(num);

            //int hp = 100;
            //bool isDead = (hp <= 0);
            //
            //if (isDead)
            //    Console.WriteLine("You are dead!");

            //int choice = 0;
            //
            //switch (choice)
            //{
            //    case 0:
            //        Console.WriteLine("가위입니다.");
            //        break;
            //    case 1:
            //        Console.WriteLine("바위입니다.");
            //        break;
            //    case 2:
            //        Console.WriteLine("보입니다.");
            //        break;
            //    case 3:
            //        Console.WriteLine("치트키입니다.");
            //        break;
            //    default:
            //        Console.WriteLine("다 실패했습니다.");
            //        break;
            //}
            //
            //if (choice == 0)
            //{
            //    Console.WriteLine("가위입니다.");
            //}
            //else if (choice == 1)
            //{
            //    Console.WriteLine("바위입니다.");
            //}
            //else if(choice ==2 )
            //{
            //    Console.WriteLine("보입니다.");
            //}
            //else
            //{
            //    Console.WriteLine("치트키입니다.");
            //}
            //
            //int number = 25;
            //
            //bool isPair;
            //
            //if ((number % 2) == 0)
            //    isPair = true;
            //else
            //    isPair = false;


            //===가위바위보===
            //Random rand = new Random();
            //int aiChoice = rand.Next(0, 3);
            //
            //int choice = Convert.ToInt32(Console.ReadLine());
            //
            //switch (choice)
            //{
            //    case 0:
            //        Console.WriteLine("당신의 선택은 가위입니다.");
            //        break;
            //    case 1:
            //        Console.WriteLine("당신의 선택은 바위입니다.");
            //        break;
            //    case 2:
            //        Console.WriteLine("당신의 선택은 보입니다.");
            //        break;
            //}
            //
            //switch (aiChoice)
            //{
            //    case 0:
            //        Console.WriteLine("컴퓨터의 선택은 가위입니다.");
            //        break;
            //    case 1:
            //        Console.WriteLine("컴퓨터의 선택은 바위입니다.");
            //        break;
            //    case 2:
            //        Console.WriteLine("컴퓨터의 선택은 보입니다.");
            //        break;
            //}
            //
            //if(choice ==0)
            //{
            //    if(aiChoice ==0)
            //    {
            //        //비김
            //    }
            //    else if(aiChoice == 1)
            //    {
            //        
            //    }
            //    else
            //    {
            //
            //    }
            //}
            //else if (choice == 1)
            //{
            //    if (aiChoice == 0)
            //    {
            //
            //    }
            //    else if (aiChoice == 1)
            //    {
            //        //비김
            //    }
            //    else
            //    {
            //
            //    }
            //}
            //else
            //{
            //    if (aiChoice == 0)
            //    {
            //
            //    }
            //    else if (aiChoice == 1)
            //    {
            //
            //    }
            //    else
            //    {
            //        //비김
            //    }
            //}


            //===가위바위보 끝===

            #endregion  //  // 기초 내용들

            #region 별찍기
            ////
            //for (int i=0; i<5; i++)
            //{
            //    for(int j=0; j<=i; j++)
            //    {
            //        Console.Write("*");
            //    }
            //    Console.WriteLine();
            //}
            //
            ////팩토리얼
            //
            //int ret = Factorial(5);
            //ret = Factorial2(5);
            //Console.WriteLine(ret);
            #endregion

            #region TEXTRPG
            //TextRPG 
            //while(true)
            //{
            //    ClassType choice = ChooseClass();
            //
            //    if (choice != ClassType.None)
            //    {
            //        //캐릭터 생성 
            //        Player player;
            //        //int hp;
            //        //int attack;
            //        CreatePlayer(choice , out player);
            //
            //        //필드로 가서 몬스터랑 싸운다
            //        EnterGame(ref player);
            //    }
            //       
            //}
            #endregion

            #region IsAsTest
            //Knight knight = new Knight();
            //Mage mage = new Mage();
            //IsAsTest(mage);
            #endregion

            #region TextRPG2
            //Game game = new Game();
            //while(true)
            //{
            //    game.Process();
            //}
            #endregion

            #region 배열
            //int[] scores = new int[5] { 10, 30, 40, 20, 50 };
            //
            //int highestScore = GetHighestScore(scores);
            //Console.WriteLine(highestScore);
            //
            //int averageScore = GetAverageScore(scores);
            //Console.WriteLine(averageScore);
            //
            //int index = GetIndexOf(scores, 20);
            //Console.WriteLine(index);
            //
            //Sort(scores);
            #endregion

            #region 타일찍기
            //Map map = new Map();
            //map.Render();
            #endregion

            #region 델리게이트,이벤트

            OnClicked clicked = new OnClicked(TestDelegate);
            clicked += TestDelegate2;

            ButtonPressed(clicked);

            //InputManager inputManager = new InputManager();
            //
            //inputManager.InputKey += OnInputTest; // 구독신청 느낌 
            //inputManager.InputKey -= OnInputTest; // 구독취소 느낌  
            //
            //while(true)
            //{
            //    inputManager.Update();
            //}
            #endregion

            #region 람다
            //_items.Add(new Item() { ItemType = ItemType.Weapon, Rarity = Rarity.Normal });
            //_items.Add(new Item() { ItemType = ItemType.Armor, Rarity = Rarity.Uncommon });
            //_items.Add(new Item() { ItemType = ItemType.Ring, Rarity = Rarity.Rare });
            //
            //MyFunc<Item, bool> selector = new MyFunc<Item, bool>((Item Item) => { return Item.ItemType == ItemType.Weapon; });
            //
            //Item item = FindItem(delegate (Item Item){ return Item.ItemType == ItemType.Weapon; }); //델리게이트 버전 : 초창기 버전
            //Item item2 = FindItem((Item Item) => { return Item.ItemType == ItemType.Weapon; }); //람다
            //
            ////delegate 를 직접 선언하지 않아도, 이미 만들어진 애들이 존재한다. 
            ////-> 반환 타입이 있을 경우 Func<> 사용
            ////-> 반환 타입이 없으면 Action<> 사용
            #endregion

            #region 리플렉션

            // 객체의 타입들을 전부다 알수 있는 방법 
            // unity에서 툴만들때 사용 가능 >> public으로 인스펙터에서 직접 수치 입력할수있게 가능 

            //Map map2 = new Map();
            //Type type = map2.GetType();
            //
            //var fields = type.GetFields(System.Reflection.BindingFlags.Public | //or 로 여러 타입 확인 가능
            //    System.Reflection.BindingFlags.NonPublic | 
            //    System.Reflection.BindingFlags.Static | 
            //    System.Reflection.BindingFlags.Instance);
            //
            ////using System.Reflection; 선언해야지 FieldInfo 사용 가능
            //
            //foreach (FieldInfo field in fields)
            //{
            //    string access = "protected";
            //    if (field.IsPublic)
            //        access = "public";
            //    else if (field.IsPrivate)
            //        access = "private";
            //
            //    var attributes = field.GetCustomAttributes();
            //
            //    Console.WriteLine($"{access} {field.FieldType.Name} {field.Name}");
            //}

            #endregion

            #region Nullable

            //int? number = 5; // << int 형식에 null값 대입가능 
            //
            //int b = number ?? 0 ; // << number가 null이 아니라면 number 안에 들어 있는 값을 넣어주고 null 이면 0을 넣어준다. / null 체크 최신버전 , 삼항연산자로도 가능
            //
            //if(number != null) //null 체크 구버전
            //{
            //    int a = number.Value;
            //    Console.WriteLine(a);
            //}
            //
            //if(number.HasValue)//null 체크 신버전
            //{
            //    int a = number.Value;
            //    Console.WriteLine(a);
            //}
            //
            //Map map3 = null;
            //
            //if(map3 !=null)
            //{
            //    string temp = map3.ToString();
            //}
            //
            //string? type = map3?.ToString(); //이말을 풀어 쓰면 밑에와 같다.
            //
            //if(map3==null)
            //{
            //    type = null;
            //}
            //else
            //{
            //    type = map3.ToString();
            //}

            #endregion
        }
    }
}
