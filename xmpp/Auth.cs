// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.Auth
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.NetworkInformation;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp
{
    /*internal*/
    public class Auth
    {
        public static WebClient WClient = new WebClient();
        private readonly XNamespace NameSpace = (XNamespace)"urn:ietf:params:xml:ns:xmpp-sasl";

        /// <summary>
        ///  
        /// </summary>
        /// <param >Запомнить!</param>
        /// <returns></returns>
        private string CheckSpecials(string s)
        {
            MySqlCommand cmd = new MySqlCommand("SELECT * FROM users WHERE ID=@id");
            cmd.Prepare(); // подгатавливает запрос
            cmd.Parameters.AddWithValue("@id", "Seagate\'; DROP TABLE users; -- ");
            cmd.ExecuteReader();
            return s.Replace("'", "\\'");
        }
        internal Auth(Client User, XmlDocument Packet)
        {
            using (MySqlConnection MySqlConnect = SQL.GetConnection().GetAwaiter().GetResult())
            {
                try
                {
                    string X = Encoding.UTF8.GetString(Convert.FromBase64String(Packet["auth"].InnerText));
                    string[] strArray = Encoding.UTF8.GetString(Convert.FromBase64String(Packet["auth"].InnerText)).Split(new char[1], StringSplitOptions.RemoveEmptyEntries);

                    if (strArray[0] != "dedicated" && strArray[1] != "dedicated")
                    {
                        Player player = new Player();

                        using (MySqlCommand cmd = new MySqlCommand("SELECT id,token,nickname,profileid FROM users WHERE Token=@1 AND Nickname=@2", MySqlConnect))
                        {
                            cmd.Prepare();
                            cmd.Parameters.AddWithValue("@1", strArray[0]);
                            cmd.Parameters.AddWithValue("@2", strArray[1]);
                            using (MySqlDataReader mySqlDataReader = cmd.ExecuteReader())
                            {
                                try
                                {
                                    if (mySqlDataReader.HasRows)
                                    {
                                        mySqlDataReader.Read();
                                        string str = mySqlDataReader.GetString(1);
                                        player.TicketId = mySqlDataReader.GetInt64(0);
                                        player.Nickname = mySqlDataReader.GetString(2);
                                        player.UserID = mySqlDataReader.GetInt64(3);
                                        mySqlDataReader.Close();
                                        if (player.Nickname == "")
                                            player.Nickname = string.Format("user{0}", (object)player.UserID);
                                        Program.WriteLine("Игрок " + strArray[1] + " присоединился к серверу!", ConsoleColor.Green);
                                        if (strArray[0].Replace("{:B:}row_emul", "").ToLower() == str.ToLower())
                                        {
                                            if (player.UserID != 0L && !player.Load(true).Result)
                                            {
                                                Console.WriteLine("Игрок " + strArray[1] + " не может авторизироваться!");
                                                User.Send(new XDocument(new object[1]
                                                {
                                        (object) new XElement(this.NameSpace + "failure")
                                                }).ToString(SaveOptions.DisableFormatting));
                                            }
                                            else
                                            {
                                                User.Authorized = true;
                                                if (player.BanType == BanType.ALL_PERMANENT)
                                                {
                                                    Program.WriteLine("Игрок " + strArray[1] + " забанен навсегда!", ConsoleColor.Red); Console.WriteLine("Игрок " + strArray[1] + " забанен навсегда!", ConsoleColor.Red);
                                                    User.Send(new XDocument(new object[1]
                                                    {
                                            (object) new XElement(this.NameSpace + "failure")
                                                    }).ToString(SaveOptions.DisableFormatting));
                                                }
                                                else
                                                {
                                                    if (player.BanType == BanType.ALL)
                                                    {
                                                        TimeSpan timeSpan = DateTimeOffset.FromUnixTimeSeconds(player.UnbanTime).Subtract(DateTimeOffset.UtcNow);
                                                        if (timeSpan.TotalSeconds <= 0.0)
                                                        {
                                                            player.BanType = BanType.NO;
                                                        }
                                                        else
                                                        {
                                                            Console.WriteLine("Player " + strArray[1] + " banned on " + timeSpan.ToString() + "!", ConsoleColor.Red);

                                                            User.Send(new XDocument(new object[1]
                                                            {
                            (object) new XElement(this.NameSpace + "failure")
                                                            }).ToString(SaveOptions.DisableFormatting));
                                                            return;
                                                        }
                                                    }
                                                    if (player != null)
                                                    {
                                                        if (User.Player == null)
                                                            User.Player = player;
                                                        ArrayList.OnlineUsers.Add(User);
                                                    }

                                                    Program.WriteLine("Игрок " + strArray[1] + " присоединился к серверу!", ConsoleColor.Green);

                                                    User.Send(new XDocument(new object[1]
                                                    {
                        (object) new XElement(this.NameSpace + "success")
                                                    }).ToString(SaveOptions.DisableFormatting));
                                                    Console.Title = string.Format("WarTLS 4.0 Fun Version (Online: {0})", (object)ArrayList.OnlineUsers.FindAll((Predicate<Client>)(Attribute => !Attribute.Dedicated)).Count);
                                                    MySqlCommand mySqlCommand = new MySqlCommand("UPDATE `system` SET value=@value WHERE system_cvar=@value2", MySqlConnect);
                                                    mySqlCommand.Parameters.AddWithValue("value2", (object)"online");
                                                    mySqlCommand.Parameters.AddWithValue("value", (object)ArrayList.OnlineUsers.FindAll((Predicate<Client>)(Attribute => !Attribute.Dedicated)).Count);
                                                    mySqlCommand.ExecuteNonQuery();
                                                    User.Player = player;
                                                }
                                            }
                                        }
                                        else
                                        {
                                            Console.WriteLine("Player " + strArray[1] + " can't log in. Invalid password!", ConsoleColor.Red);
                                            User.Send(new XDocument(new object[1]
                                            {
                    (object) new XElement(this.NameSpace + "failure")
                                            }).ToString(SaveOptions.DisableFormatting));
                                        }
                                    }
                                    else
                                        User.Send(new XDocument(new object[1]
                                        {
                  (object) new XElement(this.NameSpace + "failure")
                                        }).ToString(SaveOptions.DisableFormatting));
                                }
                                catch (Exception ex)
                                {
                                    Console.WriteLine((object)ex);
                                }
                            }
                        }
                    }
                    else if (!((IEnumerable<string>)System.IO.File.ReadAllLines("AuthorizedIPs.txt")).Contains<string>(User.IPAddress))
                    {
                        Console.ForegroundColor = ConsoleColor.Red;
                        Console.WriteLine("[" + User.IPAddress + "] You can't start a dedicated server from this IP address!");
                        Console.ResetColor();
                        User.Dispose();
                    }
                    else
                    {
                        Player player = new Player();
                        User.Dedicated = true;
                        User.Player = player;
                        Console.ForegroundColor = ConsoleColor.Green;
                        Console.WriteLine("[" + User.IPAddress + "] The dedicated server is connected!");
                        Console.ResetColor();
                        XDocument xdocument = new XDocument(new object[1]
                        {
            (object) new XElement(this.NameSpace + "success")
                        });
                        User.Authorized = true;
                        ArrayList.OnlineUsers.Add(User);
                        User.Send(xdocument.ToString(SaveOptions.DisableFormatting));
                    }
                }
                catch (MySqlException ex)
                {
                    if (!ex.Message.Contains("Closed"))
                        return;
                    MySqlConnect.Close();
                }

                catch (Exception ex)
                {
                    
                    Console.WriteLine(ex);
                }
            }
        }
    }
}
