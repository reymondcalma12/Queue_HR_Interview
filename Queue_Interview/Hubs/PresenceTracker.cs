namespace API.SignalR
{
    public class PresenceTracker
    {
        private static readonly Dictionary<string, List<string>> TableUsed = new Dictionary<string, List<string>>();

        public Task<bool> UserConnected(string tablename, string connectionId)
        {
            bool isUsed = false;

            lock(TableUsed)
            {
                if(TableUsed.ContainsKey(tablename))
                {
                    TableUsed[tablename].Add(connectionId);
                }
                else 
                {
                    TableUsed.Add(tablename, new List<string>{connectionId});
                    isUsed = true;
                }
            }

            return Task.FromResult(isUsed);
        }

        public Task<bool> UserDisconnected(string tablename, string connectionId)
        {
            bool isNotUsed = false;

            lock(TableUsed)
            {
                if(!TableUsed.ContainsKey(tablename)) return Task.FromResult(isNotUsed);

                TableUsed[tablename].Remove(connectionId);

                if(TableUsed[tablename].Count == 0)
                {
                    TableUsed.Remove(tablename);
                    isNotUsed = true;
                }

                return Task.FromResult(isNotUsed);
            }
        }

        public Task<string[]> GetOnlineUsers()
        {
            string[] tableUse;
            lock(TableUsed)
            {
                tableUse = TableUsed.OrderBy(k => k.Key).Select(k => k.Key).ToArray();
            }

            return Task.FromResult(tableUse);
        }

        //public static Task<List<string>> GetConnectionsForUser(string tablename)
        //{
        //    List<string> connectionIds;

        //    lock(TableUsed)
        //    {
        //        connectionIds = TableUsed.GetValueOrDefault(tablename);
        //    }

        //    return Task.FromResult(connectionIds);
        //}

    }
}