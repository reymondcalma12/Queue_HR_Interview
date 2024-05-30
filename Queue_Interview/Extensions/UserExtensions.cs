using Newtonsoft.Json;
using Queue.Models;

namespace Queue_Interview.Extensions
{
	public static class UserExtensions
	{
        public static string GetUserId(this string user)
        {
            if (string.IsNullOrEmpty(user))
                return string.Empty;

            var userTable = JsonConvert.DeserializeObject<Table>(user);

            return userTable.StageId + "-" + userTable.TableId;
        }

        //public static string GetStageId(this string user)
        //{
        //    if (string.IsNullOrEmpty(user))
        //        return string.Empty;

        //    var userTable = JsonConvert.DeserializeObject<Table>(user);

        //    return userTable.StageId.ToString(); ;
        //}

        //public static string GetTableId(this string user)
        //{
        //    if (string.IsNullOrEmpty(user))
        //        return string.Empty;

        //    var userTable = JsonConvert.DeserializeObject<Table>(user);

        //    return userTable.TableId.ToString();
        //}


    }
}
