// Decompiled with JetBrains decompiler
// Type: OnyxServer.xmpp.query.Messages
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.CLASSES;
using System;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.xmpp.query
{
	internal class Messages : Stanza
	{
		private enum Currency
		{
			crown_money,
			game_money,
			cry_money
		}

		private enum ItemType
		{
			Expiration,
			Consumable,
			Permanent
		}

		private string Channel;

		public XElement MessageElement;

		public Messages(Client User, XmlDocument Packet)
			: base(User, Packet)
		{
			Console.ForegroundColor = ConsoleColor.Yellow;
			Messages messages = this;
			if (base.Packet["message"] != null)
			{
				Console.WriteLine("[" + User.JID + "] -> [" + To + "] (GLOBAL-CHAT) " + base.Packet["message"].InnerText);
				
			}
			Console.ResetColor();
			if (!User.ChatWatcher.ContainsKey(To))
			{
				User.ChatWatcher.Add(To, new Stopwatch());
			}
			Stopwatch stopwatch = User.ChatWatcher[To];
			if (stopwatch.Elapsed.TotalMilliseconds != 0.0 && stopwatch.Elapsed.TotalSeconds < 1.0)
			{
				new StreamError(User, "policy-violation");
				return;
			}
		if (stopwatch.Elapsed.TotalMilliseconds != 0.0 && stopwatch.Elapsed.TotalSeconds < 3.0)
			{
				MessageElement = new XElement("message", new XAttribute("from", To), new XAttribute("to", User.JID), new XAttribute("type", "error"), new XElement("body", ""), new XElement("error", new XAttribute("code", "503"), new XAttribute("type", "cancel"), new XElement((XNamespace)"urn:ietf:params:xml:ns:xmpp-stanzas" + "service-unavailable")));
				User.Send(MessageElement.ToString(SaveOptions.DisableFormatting));
				return;
			}
			
			if (User.Player.BanType == BanType.CHAT)
			{
				TimeSpan timeSpan = DateTimeOffset.FromUnixTimeSeconds(User.Player.UnbanTime).Subtract(DateTimeOffset.UtcNow);
				if (timeSpan.TotalSeconds > 0.0)
				{
					MessageElement = new XElement("message", new XAttribute("from", To + "/Система"), new XAttribute("to", User.JID), new XAttribute("type", "groupchat"), new XElement("body", $" Вам был выдан мут чата. Вы сможете отправлять сообщения через: {timeSpan.Hours} часов {timeSpan.Minutes} минут {timeSpan.Seconds} секунд"));
					User.Send(MessageElement.ToString(SaveOptions.DisableFormatting));
					return;
				}
			} 
			string text = (base.Packet["message"] != null) ? base.Packet["message"].InnerText : "";
			if (User.Player.Privilegie > PrivilegieId.PLAYER && text.StartsWith("/"))
			{
				string[] array = text.Split(new char[1]
				{
					' '
				}, StringSplitOptions.RemoveEmptyEntries);
				string text2 = "";
				Client client = null;
				string text3 = array[0];
				if (text3 != null)
				{
					string Nickname;
					switch (text3)
					{
						case "/help":
							{
								text2 = ("Команды:<br>" +
									   "/bsay(А) (Оповещение зеленым цветом наверху)<br>" +
									   "/say (А) (Оповещение белым цветом посередине)<br>" +
									   "/gc (А) (Информация о сервере)<br>" +
									   "/item (А) (Выдача предметов)<br>" +
									   "/ban (М) (Банит на время)<br>" +
									  "/permban(М) (Банит навсегда)<br>" +
									  "/kick(М) (Кикает с сервера)<br>" +
									  "/money(М) (Выдача валюты)<br>" +
									  "/spectatoron(М) (Выдача валюты)<br>" +
									  "/spectatoroff (М) (Выдача валюты)<br>+" +
									  "/upshop(A) (Обновить магазин)<br>");

								break;
							}
						case "/kick":
							Nickname = null;
							try
							{
								Nickname = array[1];
							}
							catch
							{
								text2 = "Argument exception!<br>Example: /kick Nickname <br> Команда для того, чтобы кикнуть игрока с сервера.'";
								break;
							}
							client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
							if (client == null)
							{
								Player player3 = new Player
								{
									Nickname = Nickname
								};
								if (!player3.Load(true).Result)
								{
									text2 = "Пользователь с ником : " + Nickname + " не найден на сервере!";
									break;
								}
								client = new Client
								{
									Player = player3
								};
							}
							//client.Player.BanType = BanType.ALL_PERMANENT;
							//client.Player.UnbanTime = 0L;
							//client.Player.Save();
							if (client.JID != null)
							{
								client.Dispose();
							}
							text2 = "Пользователь с ником : " + Nickname + " успешно кикнут с сервера!";
							Console.WriteLine($"Администратор {User.Player.Nickname} : успешно кикнул с сервера игрока {Nickname}!");
							break;

						case "/bsay":
							{
								string text5 = "";
								string[] array2 = array;
								foreach (string text6 in array2)
								{
									if (!(text6 == "/bsay"))
									{
										text5 = text5 + text6 + " ";
									}
								}
								foreach (Client onlineUser in ArrayList.OnlineUsers)
								{
									onlineUser.ShowMessage(text5, Green: true);
								}
								break;
							}
						case "/say":
							{
								string text5 = "";
								string[] array2 = array;
								foreach (string text6 in array2)
								{
									if (!(text6 == "/say"))
									{
										text5 = text5 + text6 + " ";
									}
								}
								foreach (Client onlineUser in ArrayList.OnlineUsers)
								{
									onlineUser.ShowMessage(text5, Green: false);
								}
								break;
							}
						case "/broadcast":
							text2 += "Вы превысили лимит.";
							break;
						case "/psave":
							//	User.Player.ClanPlayer.Clan.Save();
							break;
						case "/gc":
							{
								text2 += $"Онлайн: {ArrayList.OnlineUsers.Count((Client Attribute) => !Attribute.Dedicated)}<br>";
								text2 += $"Выделенных серверов: {ArrayList.OnlineUsers.Count((Client Attribute) => Attribute.Dedicated)}<br>";
								text2 += $"Активных боев: {ArrayList.OnlineUsers.Count(delegate (Client Attribute) { if (Attribute.Dedicated) { return Attribute.Player.RoomPlayer.Room != null; } return false; })}<br><br>";
								text2 += $"ОС: {Environment.OSVersion.ToString()}<br>";
								//	 text2 += $"База данных: {new SqlCommand("select @@VERSION;").ExecuteScalar().ToString()}";
								break;
							}
						case "/ban":
							{
								Nickname = null;
								long num3 = -1L;
								try
								{
									Nickname = array[1];
									num3 = Tools.GetTotalSeconds(array[2]);
								}
								catch
								{
									text2 = "Ошибка! Пример: / ban Nickname 1d <br> Эта команда блокирует игрока на 1 день.<br>Доступные временные интервалы:<br> <br> h - час - m - минута < br> d - день - s - секунда";
									break;
								}
								if (Nickname == base.User.Player.Nickname)
								{
									text2 = "Вы хотите заблокировать себя? Что?";
								}
								else if (num3 > 0)
								{
									client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
									if (client == null && !new Player
									{
										Nickname = Nickname
									}.Load(true).Result)
									{
										text2 = "Данный пользователь: " + Nickname + " не найден на сервере!";
										break;
									}
									client.Player.BanType = BanType.ALL;
									client.Player.UnbanTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + num3;
									client.Player.Save();
									if (client.JID != null)
									{
										client.Dispose();
									}
									text2 = "Данный пользователь: " + Nickname + " успешно заблокирован на сервере!";
								}
								else
								{
									text2 = "Вы неправильно ввели время блокировки!";
								}
								break;
							}
						case "/permban":
							Nickname = null;
							try
							{
								Nickname = array[1];
							}
							catch
							{
								text2 = "Ошибка!<br> Пример: / permban Nickname <br> Эта команда блокирует игрока навсегда.'";
								break;
							}
							client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
							if (client == null)
							{
								Player player3 = new Player
								{
									Nickname = Nickname
								};
								if (!player3.Load(true).Result)
								{
									text2 = "Данный пользователь: " + Nickname + " не был найден на сервере!";
									break;
								}
								client = new Client
								{
									Player = player3
								};
							}
							client.Player.BanType = BanType.ALL_PERMANENT;
							client.Player.UnbanTime = 0L;
							client.Player.Save();
							if (client.JID != null)
							{
								client.Dispose();
							}
							text2 = "Данный пользователь: " + Nickname + " был успешно заблокирован!";
							break;

						case "/mute":
							{
								Nickname = null;
								long num3 = 0L;
								try
								{
									Nickname = array[1];
									num3 = Tools.GetTotalSeconds(array[2]);
								}
								catch
								{
									text2 = "Ошибка!<br>Пример: /mute Nickname 1h <br>Данная команда заблокирует чат игроку (Nickname, 1 hour) на всех каналах. Включая личные сообщения.<br>Доступные интервалы сообщений:<br><br>h - час<br>m - минута<br>d - день<br>s - секунда";
									break;
								}
								client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
								if (client == null && !new Player
								{
									Nickname = Nickname
								}.Load(false).Result)
								{
									text2 = "Данный пользователь: " + Nickname + " не найден на сервере!";
									break;
								}
								client.Player.BanType = BanType.CHAT;
								client.Player.UnbanTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds() + num3;
								client.Player.Save();
								text2 = "Данному пользователю: " + Nickname + " был успешно выдан мут!";
								break;
							}
						case "/money":
							{
								if (User.Player.Privilegie != PrivilegieId.ADMINISTRATOR)
								{
									text2 += "Данная команда Вам не доступна";
									break;
								}
								Currency currency = Currency.crown_money;
								int num = 0;
								try
								{
									Nickname = array[1];
									currency = (Currency)byte.Parse(array[2]);
									num = int.Parse(array[3]);
									if (!Enum.IsDefined(typeof(Currency), currency))
									{
										text2 = $"Эта валюта ({currency}) не найдена!<br> Доступные аргументы :<br><br>0 - CROWN<br>1 - WARBAX (GameMoney)<br>2 - CREDIT";
										break;
									}
								}
								catch
								{
									text2 = "Исключение аргументов!<br>пример: /money Nickname 2 5000<br> Эта команда отправит игроку игровые ресурсы (Nickname, 5000 CREDITS). Доступные валюты :<br><br>0 - Короны<br>1 - Варбаксы (GameMoney)<br>2 - Кредиты";
									break;
								}
								client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
								Player player4 = null;
								if (client != null)
								{
									player4 = client.Player;
								}
								if (player4 == null)
								{
									Player player5 = new Player
									{
										Nickname = Nickname
									};
									if (!player5.Load(true).Result)
									{
										text2 = "Пользователь: " + Nickname + " не найден на сервере!";
										break;
									}
									player4 = player5;
								}
								switch (currency)
								{
									case Currency.crown_money:
										player4.CrownMoney += num;
										break;
									case Currency.game_money:
										player4.GameMoney += num;
										break;
									case Currency.cry_money:
										player4.CryMoney += num;
										break;
								}
								player4.AddMoneyNotification(currency.ToString(), num, User.Player.Nickname + " начислил Вам:");
								player4.Save();
								if (client != null)
								{
									new SyncNotification(client).Process();
									new ResyncProfile(client);
								}
                               
								text2 = $"Пользователю с именем: {Nickname} успешно отправлено {num} {currency.ToString()}!";
								break;
							}
						case "/item":
							{
								if (User.Player.Privilegie != PrivilegieId.ADMINISTRATOR)
								{
									text2 += "Данная команда Вам не доступна";
									break;
								}
								ItemType itemType = ItemType.Expiration;
								int num = 0;
								long num2 = 0L;
								string text4 = "";
								try
								{
									Nickname = array[1];
									text4 = array[3];
									itemType = (ItemType)byte.Parse(array[2]);
									if (itemType == ItemType.Expiration)
									{
										num2 = Tools.GetTotalSeconds(array[4]);
									}
									if (itemType == ItemType.Consumable)
									{
										num2 = ushort.Parse(array[4]);
									}
									if (!Enum.IsDefined(typeof(ItemType), num))
									{
										text2 = $"Этот тип элемента ({itemType}) не найден!<br> Доступные аргументы :<br><br>0 - Expiration<br>1 - Consumable<br>2 - Permanent";
										break;
									}
								}
								catch
								{
									text2 = "Ошибка!<br>пример : /item Nickname 2 ar12_shop 1h <br> Эта команда будет отправлять товар на игрока (Nickname, 1 hour, M16A3). Доступные валюты :<br><br>0 - CROWN<br>1 - Варбаксы[GameMoney]<br>2 - Кредиты<br><br>В интервале:<br>h - Час<br>d - День<br><br>0 - Сроком действия<br>1 - Расходный материал<br>2 - Постоянный";
									break;
								}
								client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
								if (client == null)
								{
									Player player = new Player
									{
										Nickname = Nickname
									};
									if (!player.Load(true).Result)
									{
										text2 = "Пользователь с именем: " + Nickname + " не найден на сервере!";
										break;
									}
									client = new Client
									{
										Player = player
									};
								}
								int itemType2;
								switch (itemType)
								{
									default:
										itemType2 = 2;
										break;
									case ItemType.Consumable:
										itemType2 = 1;
										break;
									case ItemType.Expiration:
										itemType2 = 5;
										break;
								}
								Item item = new Item((WARTLS.CLASSES.ItemType)itemType2, client.Player.ItemSeed, text4, (int)TimeSpan.FromSeconds(num2).TotalHours, (int)num2, (itemType == ItemType.Permanent) ? 36000 : 0);
								client.Player.Items.Add(item);
								Player player2 = client.Player;
								string offerType = itemType.ToString();
								string name = text4;
								int amount;
								switch (itemType)
								{
									default:
										amount = 0;
										break;
									case ItemType.Consumable:
										amount = (int)num2;
										break;
									case ItemType.Expiration:
										amount = (int)TimeSpan.FromSeconds(num2).TotalHours;
										break;
								}
								player2.AddItemNotification(offerType, name, amount, "Зачислил: " + User.Player.Nickname);
								client.Player.Save();
								if (client.Socket != null)
								{
									new SyncNotification(client).Process();
									new ResyncProfile(client);
								}
								text2 = "Имя пользователя: " + Nickname + " успешно отправлено " + text4 + " " + itemType.ToString() + "!";
								break;
							}

						case "/upshop":
							{
								if (User.Player.Privilegie != PrivilegieId.ADMINISTRATOR)
								{
									text2 += "Данная команда Вам не доступна";
									break;
								}
								else
								{
									text2 += "Магазин успешно обновлен";
									Core.GameResources = new GameResources();
								}
								break;
							}
						case "/spectatoron":
							{
								if (User.Player.Privilegie != PrivilegieId.PLAYER)
								{
									User.Player.Observer = 1;
									text2 += "Спектатор включен!";
								}
								else
								{
									text2 += "Данная команда вам недоступна!";

								}
								break;
							}
						case "/spectatoroff":
							{
								if (User.Player.Privilegie != PrivilegieId.PLAYER)
								{
									User.Player.Observer = 0;
									text2 += "Спектатор отключен!";
								}
								else
								{
									text2 += "Данная команда вам недоступна!";

								}
								break;
							}
						case "/achiev":
							{
								using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
								{
									if (User.Player.Privilegie < PrivilegieId.ADMINISTRATOR)
									{
										text2 += "Данная команда Вам не доступна";
										break;
									}
									Nickname = null;
									int ID = 0;
									{
										try
										{
											Nickname = array[1];
											ID = Convert.ToInt32(array[2]);
										}
										catch
										{
											text2 = "Не правильные аргументы!<br>Пример: !achiev nick ID <br> Эта команда выдает достижения";
											break;
										}
									}
									client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == Nickname);
									if (client == null)
									{
										Player player3 = new Player
										{
											Nickname = Nickname
										};
										if (!player3.Load().Result)
										{
											text2 = "Игрок с ником: " + Nickname + " не найден на сервере!";
											break;
										}
										client = new Client
										{
											Player = player3
										};
									}

									var x = client.Player.Achievements;
									XDocument xDocument = XDocument.Parse(client.Player.Achievements.InnerXml);
									xDocument.Descendants("achievements").First().Add(new XElement("chunk", new XAttribute("achievement_id", array[2]),
									new XAttribute("progress", "9999999999999"),
									new XAttribute("completion_time", DateTimeOffset.Now.ToUnixTimeSeconds())));
									client.Player.Achievements.LoadXml(xDocument.ToString());
									client.Player.Save();
									XElement xPacket = XElement.Parse
									($@"


                                    <iq to='{client.JID}' id='{"uid" + User.Player.Random.Next(999999, int.MaxValue)}' type='get' from='masterserver@warface/wfserver' xmlns='jabber:client'>
                                    <query xmlns='urn:cryonline:k01'>
                                    <sync_notifications xmlns=''>
                                    <notif id='{User.Player.Random.Next(999999, int.MaxValue)}' type='4' confirmation='0' from_jid='wfserver@server' message=''>
                                    <achievement achievement_id='{array[2]}' progress='9999999999999' completion_time='{DateTimeOffset.Now.ToUnixTimeSeconds()}'/>
                                     </notif>
                                   </sync_notifications>
                                   </query>
                                     </iq>


                                                                                  ");

									client.Send(xPacket.ToString());
									text2 = "Игроку " + Nickname + " успешно выдано достижение с ID " + ID + "!";
									break;
								}

							}
					}
					User.ShowMessage($"WarTLS  {Assembly.GetExecutingAssembly().GetName().Version.Major}.{Assembly.GetExecutingAssembly().GetName().Version.Minor}<br><br>{text2}");
					return;
				}
			}
			else
			{
				if (this.To == User.Channel.JID + "@conference.warface")
				{
					foreach (Client Сlient in User.Channel.Users.ToArray())
					{
						this.MessageElement = new XElement((XName)"message", new object[4]
						{
				(object) new XAttribute((XName) "from", (object) ("global." + User.Channel.Resource + "@conference.warface/" + User.Player.Nickname)),
				(object) new XAttribute((XName) "to", (object) Сlient.JID),
				(object) new XAttribute((XName) "type", (object) "groupchat"),
				(object) new XElement((XName) "body", (object) this.Packet["message"].InnerText)
						});
						Сlient.Send(this.MessageElement.ToString(SaveOptions.DisableFormatting));
					}
				}
				else if (User.Player.RoomPlayer.Room != null && this.To == string.Format("room.{0}@conference.warface", (object)User.Player.RoomPlayer.Room.Core.RoomId))
				{
					foreach (Client Cl2 in User.Player.RoomPlayer.Room.Players.Users.ToArray())
					{
						this.MessageElement = new XElement((XName)"message", new object[4]
						{
				(object) new XAttribute((XName) "from", (object) string.Format("room.{0}@conference.warface/{1}", (object) User.Player.RoomPlayer.Room.Core.RoomId, (object) User.Player.Nickname)),
				(object) new XAttribute((XName) "to", (object) Cl2.JID),
				(object) new XAttribute((XName) "type", (object) "groupchat"),
				(object) new XElement((XName) "body", (object) this.Packet["message"].InnerText)
						});
						Cl2.Send(this.MessageElement.ToString(SaveOptions.DisableFormatting));
					}
				}
				else if (User.Player.RoomPlayer.Room != null && this.To == string.Format("team.room.{0}@conference.warface", (object)User.Player.RoomPlayer.Room.Core.RoomId))
				{
					foreach (Client Сlient in User.Player.RoomPlayer.Room.Players.Users.ToList<Client>().FindAll((Predicate<Client>)(Attribute => Attribute.Player.RoomPlayer.TeamId == User.Player.RoomPlayer.TeamId)).ToArray())
					{
						this.MessageElement = new XElement((XName)"message", new object[4]
						{
				(object) new XAttribute((XName) "from", (object) string.Format("team.room.{0}@conference.warface/{1}", (object) User.Player.RoomPlayer.Room.Core.RoomId, (object) User.Player.Nickname)),
				(object) new XAttribute((XName) "to", (object) Сlient.JID),
				(object) new XAttribute((XName) "type", (object) "groupchat"),
				(object) new XElement((XName) "body", (object) this.Packet["message"].InnerText)
						});
						Сlient.Send(this.MessageElement.ToString(SaveOptions.DisableFormatting));
					}
				}
				else if (User.Player.Clan.ID != 0L && To == $"clan.{User.Player.Clan.ID}@conference.warface")
				{
					foreach (Client item2 in ArrayList.OnlineUsers.FindAll(delegate (Client Attribute)
					{
						if (Attribute.Player.Clan != null)
						{
							return User.Player.Clan.ID == Attribute.Player.Clan.ID;
						}
						return false;
					}))
					{
						MessageElement = new XElement("message", new XAttribute("from", $"clan.{User.Player.Clan.ID}@conference.warface/{User.Player.Nickname}"), new XAttribute("to", item2.JID), new XAttribute("type", "groupchat"), new XElement("body", base.Packet["message"].InnerText));
						item2.Send(MessageElement.ToString(SaveOptions.DisableFormatting));
					}
				}
				else if (this.To == "wfc.warface" || this.To == "wfc.warface" || this.To == "wfc.warface")
				{
					Client Сlient = ArrayList.OnlineUsers.Find((Predicate<Client>)(Attribute => Attribute.Player.Nickname == messages.Query.Attributes["nick"].InnerText));
					if (Сlient == null)
					{
						ToOnlinePlayers toOnlinePlayers = new ToOnlinePlayers(User, Packet);
						return;
					}
					XElement xelement1 = new XElement(Gateway.JabberNS + "iq");
					xelement1.Add((object)new XAttribute((XName)"type", (object)"get"));
					xelement1.Add((object)new XAttribute((XName)"from", (object)Сlient.JID));
					xelement1.Add((object)new XAttribute((XName)"to", (object)Сlient.JID));
					xelement1.Add((object)new XAttribute((XName)"id", (object)this.Id));
					XElement xelement2 = new XElement(Stanza.NameSpace + "query");
					xelement2.Add((object)new XElement((XName)"message", new object[3]
					{
			   new XAttribute((XName) "from", (object) User.Player.Nickname),
			   new XAttribute((XName) "nick", (object) Сlient.Player.Nickname),
			   new XAttribute((XName) "message", (object) this.Query.Attributes["message"].InnerText)
					}));
					xelement1.Add((object)xelement2);
					Сlient.Send(xelement1.ToString(SaveOptions.None));
				}
				else if (this.Type == "result")
				{
					ToOnlinePlayers toOnlinePlayers1 = new ToOnlinePlayers(User, Packet);
				}
				else
				{
					try
					{
						XElement xelement = new XElement((XName)"message", new object[5]
						{
				(object) new XAttribute((XName) "from", (object) this.To),
				(object) new XAttribute((XName) "to", (object) User.JID),
				(object) new XAttribute((XName) "type", (object) "error"),
				(object) new XElement((XName) "body", (object) this.Packet["message"].InnerText),
				(object) new XElement((XName) "error", new object[4]
				{
				  (object) new XAttribute((XName) "code", (object) "406"),
				  (object) new XAttribute((XName) "type", (object) "modify"),
				  (object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "not-acceptable"),
				  (object) new XElement((XNamespace) "urn:ietf:params:xml:ns:xmpp-stanzas" + "text", (object) "Only occupants are allowed to send messages to the conference")
				})
						});
						User.Send(xelement.ToString(SaveOptions.DisableFormatting));
					}
					catch
					{
					}
				}
				stopwatch.Reset();
				stopwatch.Start();
			}
		}
	}
}
