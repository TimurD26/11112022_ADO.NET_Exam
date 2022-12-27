using _11112022_ADO.NET_Exam.Data;
using _11112022_ADO.NET_Exam.Models;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Security.Cryptography;
using System.Text;

namespace _11112022_ADO.NET_Exam
{
    public class Program
    {
        public static void AddNewUser(string usrnme)
        {
            const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
            try
            {
                string password = "";
                while (password.IsNullOrEmpty())
                {
                    Console.WriteLine("create password");
                    password = Console.ReadLine();
                }
                using var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                Console.WriteLine("connection is opened");
                //hash password
                string hashpas = GetHashString(password);
                Console.WriteLine(hashpas);
                //end

                using var dbContext = new ChatDbContext();
                var theuser = new User();
                theuser.Login = usrnme;
                theuser.Password = hashpas;

                if (!CheckUser(usrnme))
                {
                    dbContext.SaveChanges();
                    Console.WriteLine("successfully updated.");
                }
                else
                {
                    Console.WriteLine("error.login exist");
                }

                dbContext.Users.Add(theuser);
                dbContext.SaveChanges();
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static bool CheckUser(string testlogin)
        {
            try
            {
                using var dbContext = new ChatDbContext();
                var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    if (user.Login == testlogin)
                    {
                        Console.WriteLine($"{user.Login} - {user.Password}");
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
        public static void GetUsers()
        {
            try
            {
                Console.WriteLine();
                using var dbContext = new ChatDbContext();
                var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    Console.WriteLine($"{user.Id} - {user.Login} - {user.Password}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void GetUsersNoEF()
        {
            const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
            try
            {
                const string SqlQuery = "SELECT Id, Login, Password FROM dbo.Users ";

                using var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                Console.WriteLine("Connection is opened");
                using var sqlCommand = new SqlCommand(SqlQuery, sqlConnection);
                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var Id = reader["Id"].ToString();
                    var Login = reader["Login"].ToString();
                    var Password = reader["Password"].ToString();

                    Console.WriteLine($"Id- {Id}, Login - {Login}, Password - {Password}");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static byte[] GetHash(string inputString)
        {
            using (HashAlgorithm algorithm = SHA256.Create())
                return algorithm.ComputeHash(Encoding.UTF8.GetBytes(inputString));
        }
        public static string GetHashString(string inputString)
        {
            StringBuilder sb = new StringBuilder();
            foreach (byte b in GetHash(inputString))
                sb.Append(b.ToString("X2"));

            return sb.ToString();
        }
        public static bool VerifyPassword(string login, string wordpas)
        {
            try
            {
                using var dbContext = new ChatDbContext();
                var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    string buf1 = user.Login;
                    string buf2 = user.Password;

                    if (user.Login == login && GetHashString(wordpas) == user.Password)
                    {
                        //if (user.Login == login && wordpas == user.Password)
                        //{
                        //Console.WriteLine("");
                        //Console.WriteLine("you are authorized");
                        //InternalMenu();
                        return true;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return false;
        }
        public static int GetUserId(string login, string wordpas)
        {
            try
            {
                int userid = 0;
                using var dbContext = new ChatDbContext();
                var users = dbContext.Users.ToList();
                foreach (var user in users)
                {
                    string buf1 = user.Login;
                    string buf2 = user.Password;

                    if (user.Login == login && GetHashString(wordpas) == user.Password)
                    {
                        Console.WriteLine("");
                        Console.WriteLine("you are authorized");
                        InternalMenu();
                        return userid;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
        public static int ExternalMenu()
        {
            int ch = 99;
            while (ch != 0 && ch != 1 && ch != 2 && ch != 3 && ch != 4)
            {
                Console.WriteLine("Выберете:");
                Console.WriteLine("0 - Список пользователей ");
                Console.WriteLine("1 - Создать нового пользователя ");
                Console.WriteLine("2 - Авторизоваться ");
                Console.WriteLine("3 - Получить cписок пользователей(без EF Core) ");
                Console.WriteLine("4 - Получить список созданных групп, не используя EF Core ");
                Console.WriteLine("5 - Удалить группу");

                Console.WriteLine("");
                ch = Int32.Parse(Console.ReadLine());
            }
            return ch;
        }
        public static int InternalMenu()
        {
            Console.WriteLine("Выберете:");
            Console.WriteLine("");
            Console.WriteLine("1 - Отправить сообщение в директ");
            Console.WriteLine("2 - Редактировать отправленное сообщение ");
            Console.WriteLine("3 - Удалить сообщение ");
            Console.WriteLine("4 - Добавить пользователя в черный список ");
            Console.WriteLine("5 - Записать аудиосообщение и отправить адресату ");
            Console.WriteLine("6 - Отправить файл пользователю ");
            Console.WriteLine("7 - Создать группу ");
            Console.WriteLine("8 - пригласить пользователя в группу ");
            Console.WriteLine("9 - удалить пользователя из группы ");
            Console.WriteLine("10 - Показать историю переписки с пользователем. Выбор пользователя по логину ");
            Console.WriteLine("11 - Получение новых(непрочитанных) сообщений ");
            Console.WriteLine("12 - Показать список созданных групп ");
            Console.WriteLine("13 - Показать историю переписки в группе ");
            Console.WriteLine("");

            int ch = Int32.Parse(Console.ReadLine());
            return ch;

            BackToInternalMenu();
        }
        public static void InternalSwitchMenu(int switchval, int fromuserid)
        {
            int touserid = 0;
            int userid = 0;

            switch (switchval)
            {
                case 1:
                    {
                        GetUsers();
                        SendMessage(fromuserid);
                        Console.WriteLine(fromuserid);
                        BackToInternalMenu();
                    }
                    break;
                case 2:
                    {
                        GetUsers();
                        GetPrivateMessages(fromuserid);
                        EditPrivateMessage();
                        BackToInternalMenu();
                    }
                    break;
                case 3:
                    {
                        //GetUsers();
                        GetPrivateMessages(fromuserid);

                        DeletePrivateMessage();
                        BackToInternalMenu();
                    }
                    break;
                case 4:
                    {
                        GetUsers();
                        BlackListUserMessages();
                        BackToInternalMenu();
                    }
                    break;
                case 7:
                    {
                        GetAllGroups();
                        CreateGroup(fromuserid);
                        BackToInternalMenu();
                    }
                    break;
                case 8:
                    {
                        GetUsers();
                        GetAllGroups();
                        AddUserToGroup();
                        BackToInternalMenu();
                    }
                    break;
                case 9:
                    {
                        GetUsers();
                        GetAllGroups();
                        DeleteUserFromGroup();
                        BackToInternalMenu();
                    }
                    break;
                case 10:
                    {
                        GetUsers();
                        GetMessageHistoryByUsername(fromuserid.ToString());
                        BackToInternalMenu();
                    }
                    break;
                case 11:
                    {
                        while (true)
                        {
                            var curmessages = GetMessages(fromuserid);
                            //GetNewMessages(fromuserid, curmessages);
                            GetNewMessagesNoEF(fromuserid, curmessages);
                            Console.Clear();
                            PrintPrivateMessage(fromuserid, curmessages);
                            Thread.Sleep(3000);

                            BackToInternalMenu();
                        }
                    }
                    break;
                case 12:
                    {

                        GetAllGroups();
                        BackToInternalMenu();
                    }
                    break;
                case 13:
                    {
                   
                        GetAllGroups();
                        GetGroupMessages();
                        BackToInternalMenu();
                    }
                    break;
            }
        }
        public static void SwitchMenu(int caseval)
        {
            switch (caseval)
            {
                case 0:
                    {
                        GetUsers();
                        BackToExternalMenu();
                    }
                    break;
                case 1:
                    {
                        string inputUsrName = "";
                        while (inputUsrName.IsNullOrEmpty())
                        {
                            Console.Write("create userlogin ");
                            inputUsrName = Console.ReadLine();
                            AddNewUser(inputUsrName);
                            //Реализовать проверку на уникальность пользователя
                        }
                        BackToExternalMenu();
                    }
                    break;
                case 2:
                    {
                        string login = "";
                        string password = "";
                        int userid = 0;
                        while (login.IsNullOrEmpty() || password.IsNullOrEmpty())
                        {
                            Console.Write("input userlogin ");
                            login = Console.ReadLine();
                            Console.Write("input password ");
                            password = Console.ReadLine();
                        }
                        if (VerifyPassword(login, password))
                        {
                            Console.WriteLine("you are authorized");
                            Console.WriteLine("");
                            InternalSwitchMenu(InternalMenu(), getUserId(login));
                            Console.WriteLine("");
                            BackToInternalMenu();
                        }
                        else
                        {
                            Console.WriteLine("wrong login or password");
                        }
                    }
                    break;
                case 3:
                    {
                        GetUsersNoEF();
                        BackToExternalMenu();
                    }
                    break;
                case 4:
                    {
                        GetAllGroupsNoEF();
                        //GetMessageHistoryByUsername();
                        BackToExternalMenu();
                    }
                    break;
                case 5:
                    {
                        GetAllGroupsNoEF();
                        DeleteGroup();
                        //GetNewMessages();
                        BackToExternalMenu();
                    }
                    break;
            }
        }
        public static void GetMessageHistoryByUsername(string seconduser)
        {
            try
            {
                string firstuser = "";
                //string seconduser = "";
                while (firstuser.IsNullOrEmpty() || seconduser.IsNullOrEmpty())
                {
                    firstuser = Console.ReadLine();
                    Console.WriteLine("enter user name");
                    //seconduser = Console.ReadLine();
                    //Console.WriteLine("enter user name");
                }
                int one = Int32.Parse(firstuser);
                int two = Int32.Parse(seconduser);
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();

                List<PrivateMessage> messages = new List<PrivateMessage>();

                foreach (var privatemessage in privatemessages)
                {
                    if (privatemessage.FromUserId == one && privatemessage.ToUserId == two)
                    {
                        messages.Add(privatemessage);
                        //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                        //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                        //Console.WriteLine($"Message:\n{privatemessage.Message}");
                    }
                    if (privatemessage.ToUserId == one && privatemessage.FromUserId == two)
                    {
                        messages.Add(privatemessage);
                        //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                        //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                    }
                }
                //messages.Sort();
                //List<PrivateMessage> SortedList = messages.OrderBy(o => o.CreateDate);

                List<PrivateMessage> SortedList = messages.AsEnumerable().OrderBy(o => o.CreateDate).ToList();

                foreach (var item in SortedList)
                {
                    if (item.FromUserId == two)
                    {
                        Console.WriteLine($"id - {item.Id}\t CreateDate - {item.CreateDate}");
                        Console.WriteLine($"FromUserId - {item.FromUserId}\t ToUserId- {item.ToUserId}");
                        Console.WriteLine($"{item.Message}");
                    }
                    if (item.FromUserId == one)
                    {
                        Console.WriteLine($"\t\t\t\tid - {item.Id}\t CreateDate - {item.CreateDate}");
                        Console.WriteLine($"\t\t\t\tFromUserId - {item.FromUserId}\t ToUserId- {item.ToUserId}");
                        Console.WriteLine($"\t\t\t\t{item.Message}");
                    }

                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void ShowConversationHistory(List<PrivateMessage> conversation)
        {
            foreach (var item in conversation)
            {
                /*PrintPrivateMessage(item);*/
            }
        }
        public static List<PrivateMessage> GetMessages(int usrd)
        {
            List<PrivateMessage> messagesold = new List<PrivateMessage>();
            try
            {
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();

                //List<PrivateMessage> messagesnew = new List<PrivateMessage>();

                foreach (var privatemessage in privatemessages)
                {
                    if (privatemessage.ToUserId == usrd)
                    {
                        messagesold.Add(privatemessage);
                        //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                        //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                    }
                    if (privatemessage.FromUserId == usrd)
                    {
                        messagesold.Add(privatemessage);
                    }
                }
                return messagesold;
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
                return messagesold;
            }
        }
        public static void GetNewMessages(int usrd, List<PrivateMessage> currentmessages)
        {
            //List<PrivateMessage> messagesnew = new List<PrivateMessage>();
            //List<PrivateMessage> SortedList = GetMessages(usrd).AsEnumerable().OrderBy(o => o.CreateDate).ToList();

            try
            {
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();

                if (privatemessages.Last().CreateDate != currentmessages.Last().CreateDate)
                {
                    foreach (var privatemessage in privatemessages)
                    {
                        Console.WriteLine("New Messeges recieved");
                        Console.WriteLine("");

                        if (privatemessage.ToUserId == usrd)
                        {
                            currentmessages.Add(privatemessage);
                            //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                            //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                            //Console.WriteLine($"{privatemessage.Message}");
                        }
                        if (privatemessage.FromUserId == usrd)
                        {
                            currentmessages.Add(privatemessage);
                            //Console.WriteLine($"\t\t\t\tid - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                            //Console.WriteLine($"\t\t\t\tFromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                            //Console.WriteLine($"\t\t\t\t{privatemessage.Message}");
                        }
                    }
                }


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        } 
        public static void GetNewMessagesNoEF(int usrd, List<PrivateMessage> currentmessages)
        {
            try
            {
                const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
                try
                {
                    const string SqlQuery = "SELECT Id, CreateDate, FromUserId, ToUserId, Message FROM dbo.PrivateMessages ";

                    using var sqlConnection = new SqlConnection(connectionString);
                    sqlConnection.Open();
                    Console.WriteLine("Connection is opened");
                    using var sqlCommand = new SqlCommand(SqlQuery, sqlConnection);
                    using var reader = sqlCommand.ExecuteReader();
                    while (reader.Read())
                    {
                        var id = reader["Id"].ToString();
                        var createDate = reader["CreateDate"].ToString();
                        var fromUserId = reader["FromUserId"].ToString();
                        var toUserId = reader["ToUserId"].ToString();
                        var message = reader["Message"].ToString();

                        PrivateMessage tmpmessage = new PrivateMessage();
                        tmpmessage.Id = Int32.Parse(id);
                        tmpmessage.FromUserId = Int32.Parse(fromUserId);
                        tmpmessage.ToUserId = Int32.Parse(toUserId);
                        tmpmessage.Message = message;
                        tmpmessage.CreateDate = DateTime.Parse(createDate);

                        if (tmpmessage.ToUserId == usrd)
                        {
                            currentmessages.Add(tmpmessage);
                        }

                        if (tmpmessage.FromUserId == usrd)
                        {
                            currentmessages.Add(tmpmessage);
                        }
                    }
                }
                catch (Exception e)
                {
                    Console.WriteLine(e.Message);
                }


                //using var dbContext = new ChatDbContext();
                //var privatemessages = dbContext.PrivateMessages.ToList();

                //if (privatemessages.Last().CreateDate != currentmessages.Last().CreateDate)
                //{
                //    foreach (var privatemessage in privatemessages)
                //    {
                //        Console.WriteLine("New Messeges recieved");
                //        Console.WriteLine("");

                //        if (privatemessage.ToUserId == usrd)
                //        {
                //            currentmessages.Add(privatemessage);
                //            //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                //            //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                //            //Console.WriteLine($"{privatemessage.Message}");
                //        }
                //        if (privatemessage.FromUserId == usrd)
                //        {
                //            currentmessages.Add(privatemessage);
                //            //Console.WriteLine($"\t\t\t\tid - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                //            //Console.WriteLine($"\t\t\t\tFromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                //            //Console.WriteLine($"\t\t\t\t{privatemessage.Message}");
                //        }
                //    }
                //}


            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void PrintPrivateMessage(int usrd, List<PrivateMessage> curmessages)
        {
            foreach (var curmassage in curmessages)
            {
                if (curmassage.ToUserId == usrd)
                {
                    Console.WriteLine($"id - {curmassage.Id}\t CreateDate - {curmassage.CreateDate}");
                    Console.WriteLine($"FromUserId - {curmassage.FromUserId}\t ToUserId- {curmassage.ToUserId}");
                    Console.WriteLine($"{curmassage.Message}");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
                if (curmassage.FromUserId == usrd)
                {
                    Console.WriteLine($"\t\t\t\t\t\t\t\tid - {curmassage.Id}\t CreateDate - {curmassage.CreateDate}");
                    Console.WriteLine($"\t\t\t\t\t\t\t\tFromUserId - {curmassage.FromUserId}\t ToUserId- {curmassage.ToUserId}");
                    Console.WriteLine($"\t\t\t\t\t\t\t\t{curmassage.Message}");
                    Console.WriteLine("");
                    Console.WriteLine("");
                }
            }
        }
        public static int getUserId(string username)
        {
            try
            {
                using var dbContext = new ChatDbContext();
                var users = dbContext.Users.ToList();
                foreach (User user in users)
                {
                    if (user.Login == username)
                    {
                        return user.Id;
                    }
                    else
                    {
                        Console.WriteLine("no user");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
        public static int getGroupId(string usergroup)
        {
            try
            {
                using var dbContext = new ChatDbContext();
                var groups = dbContext.Groups.ToList();
                foreach (var group in groups)
                {
                    if (group.Name == usergroup)
                    {
                        return group.Id;
                    }
                    else
                    {
                        Console.WriteLine("no group");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
            return 0;
        }
        public static void AddUserToGroup()
        {
            string tmpuser = "";
            string tmpgroup = "";
            int tmpuserid = 0;
            int tmpgroupid = 0;
            while (tmpuser.IsNullOrEmpty() || tmpgroup.IsNullOrEmpty())
            {
                Console.WriteLine("enter user id");
                tmpuser = Console.ReadLine();
                Console.WriteLine("enter group id");
                tmpgroup = Console.ReadLine();
            }
            tmpuserid = Int32.Parse(tmpuser);
            tmpgroupid = Int32.Parse(tmpgroup);
            try
            {
                using var dbContext = new ChatDbContext();
                UserGroup theusergoup = new UserGroup();
                theusergoup.GroupId = tmpgroupid;
                theusergoup.UserId = tmpuserid;
                dbContext.UserGroups.Add(theusergoup);
                dbContext.SaveChanges();
                Console.WriteLine("user added to group");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void DeleteUserFromGroup()
        {
            string tmpuser = "";
            string tmpgroup = "";
            int tmpuserid = 0;
            int tmpgroupid = 0;
            while (tmpuser.IsNullOrEmpty() || tmpgroup.IsNullOrEmpty())
            {
                Console.WriteLine("enter user id");
                tmpuser = Console.ReadLine();
                Console.WriteLine("enter group id");
                tmpgroup = Console.ReadLine();
            }
            tmpuserid = Int32.Parse(tmpuser);
            tmpgroupid = Int32.Parse(tmpgroup);
            try
            {
                using var dbContext = new ChatDbContext();
                UserGroup theusergoup = new UserGroup();
                //theusergoup.GroupId = tmpgroupid;
                //theusergoup.UserId = tmpuserid;
                var usergroups = dbContext.UserGroups.ToList();
                foreach (var usergroup in usergroups)
                {
                    if (usergroup.UserId == tmpuserid && usergroup.GroupId == tmpgroupid)
                    {
                        dbContext.UserGroups.Remove(theusergoup);
                        dbContext.SaveChanges();
                        Console.WriteLine("user deleted from group");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void GetAllGroups()
        {
            using var dbContext = new ChatDbContext();
            var groups = dbContext.Groups.ToList();
            foreach (var group in groups)
            {
                Console.WriteLine($"Id - {group.Id}\tName - {group.Name}\tOwner_Id - {group.Owner_Id}");
            }
        }
        public static void DeleteGroup(string groupname)
        {
            using var dbContext = new ChatDbContext();
            var groups = dbContext.Groups.ToList();
            foreach (var group in groups)
            {
                if (group.Name == groupname)
                {
                    dbContext.Groups.Remove(group);
                    dbContext.SaveChanges();
                }
            }
        }
        public static void GetAllGroupsNoEF()
        {
            const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
            try
            {
                const string SqlQuery = "SELECT id, name, owner_id FROM dbo.Groups ";

                using var sqlConnection = new SqlConnection(connectionString);
                sqlConnection.Open();
                Console.WriteLine("Connection is opened");
                using var sqlCommand = new SqlCommand(SqlQuery, sqlConnection);
                using var reader = sqlCommand.ExecuteReader();
                while (reader.Read())
                {
                    var Id = reader["id"].ToString();
                    var Name = reader["name"].ToString();
                    var Owner_id = reader["owner_id"].ToString();

                    Console.WriteLine($"Id- {Id}, Name - {Name}, Owner_id - {Owner_id}");
                }
                Console.WriteLine("");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void CreateGroup(int frmuserid)
        {
            string groupname = "";
            while (groupname.IsNullOrEmpty())
            {
                groupname = Console.ReadLine();
                Console.WriteLine("enter group name");
            }
            Group thegroup = new Group();
            thegroup.Name = groupname;
            thegroup.Owner_Id = frmuserid;

            using var dbContext = new ChatDbContext();
            //var groups = dbContext.Groups.ToList();
            dbContext.Groups.Add(thegroup);
            dbContext.SaveChanges();

            Console.WriteLine($"group {groupname} created");
        }
        public static void DeleteGroup()
        {
            string groupname = "";
            while (groupname.IsNullOrEmpty())
            {
                groupname = Console.ReadLine();
                Console.WriteLine("enter group name");
            }
            using var dbContext = new ChatDbContext();
            var groups = dbContext.Groups.ToList();
            foreach (var group in groups)
            {
                if (group.Name == groupname)
                {
                    dbContext.Groups.Remove(group);
                    Console.WriteLine("group deleted");
                }
                else
                {
                    Console.WriteLine("unable to delete group");
                }
            }
        }
        public static void BlackListUserMessages()
        {
            string usertoblacklist = "";
            while (usertoblacklist.IsNullOrEmpty())
            {
                Console.WriteLine("enter username you want to blacklist");
                usertoblacklist = Console.ReadLine();
            }
            try
            {
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();
                var users = dbContext.Users.ToList();
                int flag = 0;
                foreach (var user in users)
                {
                    if (user.Login == usertoblacklist)
                    {
                        foreach (var privatemessage in privatemessages)
                        {
                            if (privatemessage.FromUserId == user.Id)
                            {
                                privatemessage.IsUserInBlackList = true;
                                Console.WriteLine($"message with id - {privatemessage.Id}\t is blacklisted " +
                                    $"\t CreateDate - {privatemessage.CreateDate}");
                                Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                                Console.WriteLine("");
                                dbContext.PrivateMessages.Update(privatemessage);
                                dbContext.SaveChanges();
                                //Console.WriteLine($"Message:\n{privatemessage.Message}");
                                flag++;
                            }
                        }
                        if (flag > 0)
                            Console.WriteLine($"all messages from user {user.Login} is blacklisted");
                        else
                        {
                            Console.WriteLine($"no messages from user {user.Login}");
                        }
                    }

                }
                if (flag == 0)
                {
                    Console.WriteLine("no such username");
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void SendMessage(int fromuserid)
        {
            //outmessage.FromUserId = ;
            const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
            try
            {
                using var dbContext = new ChatDbContext();
                var touserid = "";
                string inputmessage = "";
                int iter = 0;
                int bufid = 0;
                while (touserid.IsNullOrEmpty() || inputmessage.IsNullOrEmpty())
                {
                    if (iter > 0)
                    {
                        Console.WriteLine("fill all fields");
                    }
                    Console.WriteLine("");
                    Console.WriteLine("enter destination userid");
                    touserid = Console.ReadLine();
                    bufid = Int32.Parse(touserid);
                    Console.WriteLine("enter message");
                    inputmessage = Console.ReadLine();

                    iter++;
                }
                PrivateMessage outmessage = new PrivateMessage();
                outmessage.FromUserId = fromuserid;
                outmessage.ToUserId = bufid;
                outmessage.Message = inputmessage;
                outmessage.CreateDate = DateTime.Now;

                dbContext.PrivateMessages.Add(outmessage);
                dbContext.SaveChanges();
                Console.WriteLine("successfully updated.");

            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void DeletePrivateMessage()
        {
            //outmessage.FromUserId = ;

            const string connectionString = "Server=KCELL50787\\MSSQLSERVER2;Database=ChatDb;User Id=sa;Password=Qwerty123!;Encrypt=false;";
            try
            {
                var inputmessageid = "";
                int bufid = 0;
                if (inputmessageid.IsNullOrEmpty())
                {
                    Console.WriteLine("enter message id");
                    inputmessageid = Console.ReadLine();
                    bufid = Int32.Parse(inputmessageid);
                }
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();
                foreach (var privatemessage in privatemessages)
                {
                    if (privatemessage.Id == bufid)
                    {
                        privatemessage.Is_deleted = true;

                        //Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                        //Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                        //Console.WriteLine($"Message:\n{privatemessage.Message}");

                        //dbContext.PrivateMessages.Remove(privatemessage);

                        dbContext.PrivateMessages.Update(privatemessage);
                        dbContext.SaveChanges();
                        Console.WriteLine("message deleted");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void GetPrivateMessages(int fromuserid)
        {
            try
            {
                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();
                foreach (var privatemessage in privatemessages)
                {
                    if (privatemessage.FromUserId == fromuserid)
                    {
                        Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}");
                        Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}");
                        Console.WriteLine($"{privatemessage.Message}");
                        //Console.WriteLine($"Message:\n{privatemessage.Message}");
                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void GetGroupMessages()
        {
            try
            {
                int tempid = 0;
                string group = "";
                while (group.IsNullOrEmpty())
                {
                    Console.WriteLine("Enter group id");
                    group = Console.ReadLine();
                    tempid = Int32.Parse(group);
                }
                using var dbContext = new ChatDbContext();
                var groupmessages = dbContext.GroupMessages.ToList();
                foreach (var groupmessage in groupmessages)
                {
                    if (groupmessage.GroupId == tempid)
                    {
                        Console.WriteLine("");
                        Console.WriteLine($"Id - {groupmessage.Id}\tGroup_id - {groupmessage.GroupId}\t UserId- {groupmessage.UserId}\n CreateDate - {groupmessage.CreateDate}");
                        Console.WriteLine($"Message\n{groupmessage.Message}");
                        //Console.WriteLine($"Message:\n{privatemessage.Message}");
                        Console.WriteLine("");
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void EditPrivateMessage()
        {
            try
            {
                string msgid = "";
                int intmsgid = 0;

                using var dbContext = new ChatDbContext();
                var privatemessages = dbContext.PrivateMessages.ToList();

                //foreach (var privatemessage in privatemessages)
                //{
                //    Console.WriteLine($"id - {privatemessage.Id}\t CreateDate - {privatemessage.CreateDate}\n");
                //    Console.WriteLine($"FromUserId - {privatemessage.FromUserId}\t ToUserId- {privatemessage.ToUserId}\n");
                //    Console.WriteLine($"Message:\n{privatemessage.Message}");
                //}
                int k = 0;
                while (msgid.IsNullOrEmpty())
                {
                    Console.WriteLine("enter message id");
                    msgid = Console.ReadLine();
                    intmsgid = Int32.Parse(msgid);
                }

                foreach (var privatemessage in privatemessages)
                {

                    if (privatemessage.Id == intmsgid)
                    {
                        string newmessage = "";
                        while (newmessage.IsNullOrEmpty())
                        {
                            Console.WriteLine("enter new message text");
                            newmessage = Console.ReadLine();
                            privatemessage.Message = newmessage;
                        }
                        dbContext.PrivateMessages.Update(privatemessage);
                        dbContext.SaveChanges();
                        Console.WriteLine("successfully updated");
                        k++;
                    }

                }
                if (k == 0)
                    Console.WriteLine("no messages");
            }
            catch (Exception e)
            {
                Console.WriteLine(e.Message);
            }
        }
        public static void BackToInternalMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("100. Вернуться в меню.");
            Console.WriteLine("111. Выйти");
            var inputString = Console.ReadLine();
            if (string.IsNullOrEmpty(inputString))
            {
                Console.WriteLine("Введите цифру");
            }
            //var outputString = Console.ReadLine();

            Console.WriteLine();
            int number = int.Parse(inputString);

            if (number == 100)
            {
                SwitchMenu(InternalMenu());
            }
            else if (number == 111)
            {
                return;
            }
        }
        public static void BackToExternalMenu()
        {
            Console.WriteLine("");
            Console.WriteLine("");
            Console.WriteLine("100. Вернуться в меню.");
            Console.WriteLine("111. Выйти");
            Console.WriteLine("");
            var inputString = Console.ReadLine();
            if (string.IsNullOrEmpty(inputString))
            {
                Console.WriteLine("Введите цифру");
            }
            //var outputString = Console.ReadLine();

            Console.WriteLine();
            int number = int.Parse(inputString);

            if (number == 100)
            {
                SwitchMenu(ExternalMenu());
            }
            else if (number == 111)
            {
                return;
            }
        }
        static void Main(string[] args)
        {
            try
            {
                SwitchMenu(ExternalMenu());
            }
            catch (Exception e)
            {

                Console.WriteLine(e.Message); ;
            }
        }
    }
}