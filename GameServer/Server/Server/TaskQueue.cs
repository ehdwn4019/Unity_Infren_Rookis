using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Server
{
    /// <summary>
    /// JobQeueu의 옛날 방식?? 
    /// 지금도 사용은 하는데 람다 생기기 이전에 만들던 방식인것같다
    /// JobQueue 방식이 좀더 편리함 
    /// </summary>

    interface ITask
    {
        void Execute();
    }

    class BroadcastTask : ITask
    {
        GameRoom _room;
        ClientSession _session;
        string _chat;

        BroadcastTask(GameRoom room, ClientSession session, string chat)
        {
            _room = room;
            _session = session;
            _chat = chat;
        }
        public void Execute()
        {
            //_room.Broadcast(_session, _chat);
        }
    }

    class TaskQueue
    {
        Queue<ITask> _queue = new Queue<ITask>();
    }
}
