using System;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Linq;
using MySql.Data.MySqlClient;
using WARTLS.xmpp.query;

namespace WARTLS.CLASSES
{
	// Token: 0x02000068 RID: 104
	public class Clan
	{
		// Token: 0x060000E7 RID: 231 RVA: 0x000167C5 File Offset: 0x000149C5
		public Clan()
		{
		}

		// Token: 0x060000E8 RID: 232 RVA: 0x000167E4 File Offset: 0x000149E4
		public Clan(string Name, string Description, Client Master)
		{
			using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
			{
				this.Name = Name;
				this.Description = Description;
				this.CreationTime = DateTimeOffset.UtcNow.ToUnixTimeSeconds();
				Master.Player.Clan = this;
				Master.Player.ClanPlayer.InvitationDate = this.CreationTime;
				Master.Player.ClanPlayer.ClanRole = Clan.CLAN_ROLE.LEADER;
				this.LeaderName = Master.Player.Nickname;
				this.AddPlayer(Master.Player);
				new MySqlCommand(string.Format("INSERT INTO clans (Name, Points, Description, Players, CreationTime, LeaderInfo) VALUES ('{0}', '{1}', '{2}','{3}','{4}', '{5}');", new object[]
				{
					this.Name,
					0,
					Description,
					this.ClanMembers.ToString(SaveOptions.DisableFormatting),
					this.CreationTime,
					this.LeaderName
				}), result).ExecuteScalar();
				this.ID = long.Parse(new MySqlCommand("SELECT id FROM clans WHERE name='" + this.Name + "'", result).ExecuteScalar().ToString());
				Master.Player.Save();
				ArrayList.ClanList.Add(this);
				result.Close();
			}
		}

		// Token: 0x060000E9 RID: 233 RVA: 0x0001695C File Offset: 0x00014B5C
		public XElement Serialize(Client User)
		{
			XElement result2;
			using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
			{
				int num = 0;
				using (MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT ID FROM clans ORDER BY Points DESC;", result).ExecuteReader())
				{
					if (mySqlDataReader.HasRows)
					{
						mySqlDataReader.Read();
						try
						{
							while (mySqlDataReader.Read())
							{
								num++;
								if (mySqlDataReader.GetInt64(0) == this.ID)
								{
									mySqlDataReader.Close();
									break;
								}
							}
							mySqlDataReader.Close();
						}
						catch
						{
						}
					}
				}
				this.Points = 0;
				XElement xelement = new XElement("clan");
				try
				{
					xelement.Add(new XAttribute("name", this.Name));
					xelement.Add(new XAttribute("description", Convert.ToBase64String(Encoding.UTF8.GetBytes(this.Description))));
					xelement.Add(new XAttribute("clan_id", this.ID));
					xelement.Add(new XAttribute("creation_date", this.CreationTime));
					xelement.Add(new XAttribute("leaderboard_position", num));
					Client client = ArrayList.OnlineUsers.Find((Client Attribute) => Attribute.Player.Nickname == this.LeaderName);
					if (client != null)
					{
						xelement.Add(new XAttribute("master_badge", client.Player.BannerBadge));
						xelement.Add(new XAttribute("master_stripe", client.Player.BannerStripe));
						xelement.Add(new XAttribute("master_mark", client.Player.BannerMark));
					}
					else
					{
						Player player = new Player
						{
							Nickname = this.LeaderName
						};
						player.Clan.ID = this.ID;
						if (player.Load(false).Result)
						{
							xelement.Add(new XAttribute("master_badge", player.BannerBadge));
							xelement.Add(new XAttribute("master_stripe", player.BannerStripe));
							xelement.Add(new XAttribute("master_mark", player.BannerMark));
						}
					}
					foreach (XElement content in this.ClanMembers.Elements("clan_member_info"))
					{
						xelement.Add(content);
					}
					xelement.Add(new XAttribute("clan_points", User.Player.Clan.Points));
					User.Player.Clan.ID = this.ID;
					User.Player.Save();
					result2 = xelement;
				}
				catch (Exception)
				{
					result2 = xelement;
				}
			}
			return result2;
		}

		// Token: 0x060000EA RID: 234 RVA: 0x00016CCC File Offset: 0x00014ECC
		public async Task<bool> Find(Client User)
		{
			bool result;
			if (User.Player.Clan.ID == 0L)
			{
				result = false;
			}
			else
			{
				this.ID = User.Player.Clan.ID;
				Clan clan = ArrayList.ClanList.Find((Clan x) => x.ID == this.ID);
				if (clan != null)
				{
					User.Player.Clan = clan;
					if (User.JID != null)
					{
						new ClanInfo(User, null);
					}
				}
				else
				{
					if (!this.Load().Result)
					{
						User.Player.Clan = new Clan();
						User.Player.Save();
						return false;
					}
					User.Player.Clan = this;
					if (User.JID != null)
					{
						new ClanInfo(User, null);
					}
					ArrayList.ClanList.Add(this);
				}
				foreach (XElement xelement in User.Player.Clan.ClanMembers.Elements("clan_member_info").ToList<XElement>())
				{
					if (xelement.Attribute("profile_id").Value == User.Player.UserID.ToString())
					{
						User.Player.ClanPlayer.ClanRole = (Clan.CLAN_ROLE)int.Parse(xelement.Attribute("clan_role").Value);
						User.Player.ClanPlayer.InvitationDate = long.Parse(xelement.Attribute("invite_date").Value);
						User.Player.ClanPlayer.Points = int.Parse(xelement.Attribute("clan_points").Value);
						return true;
					}
				}
				result = false;
			}
			return result;
		}

		// Token: 0x060000EB RID: 235 RVA: 0x00016D1C File Offset: 0x00014F1C
		public void AddPlayer(Player player)
		{
			XElement xelement = new XElement("clan_member_info");
			xelement.Add(new XAttribute("nickname", player.Nickname));
			xelement.Add(new XAttribute("profile_id", player.UserID));
			xelement.Add(new XAttribute("experience", player.Experience));
			xelement.Add(new XAttribute("jid", player.UserID.ToString() + "@warface/GameClient"));
			xelement.Add(new XAttribute("clan_points", player.ClanPlayer.Points));
			xelement.Add(new XAttribute("invite_date", player.ClanPlayer.InvitationDate));
			xelement.Add(new XAttribute("clan_role", Convert.ToInt32(player.ClanPlayer.ClanRole)));
			xelement.Add(new XAttribute("status", "9"));
			this.ClanMembers.Add(xelement);
			if (player.RoomPlayer.Room != null)
			{
				player.RoomPlayer.Room.Sync(null);
			}
			this.Save();
		}

		// Token: 0x060000EC RID: 236 RVA: 0x00016E84 File Offset: 0x00015084
		public void RemovePlayer(Player player)
		{
			foreach (XElement xelement in this.ClanMembers.Elements("clan_member_info").ToList<XElement>())
			{
				if (xelement.Attribute("profile_id").Value == player.UserID.ToString())
				{
					xelement.Remove();
				}
			}
			this.Save();
		}

		// Token: 0x060000ED RID: 237 RVA: 0x00016F18 File Offset: 0x00015118
		public void UpdatePlayer(Player player)
		{
			this.RemovePlayer(player);
			this.AddPlayer(player);
			this.Save();
		}

		// Token: 0x060000EE RID: 238 RVA: 0x00016F30 File Offset: 0x00015130
		public async void DeleteClan()
		{
			using (MySqlConnection Connect = SQL.GetConnection().GetAwaiter().GetResult())
			{
				await new MySqlCommand(string.Format("DELETE FROM clans WHERE ID='{0}';", this.ID), Connect).ExecuteNonQueryAsync();
				Connect.Close();
			}
			MySqlConnection MySqlConnect = null;
		}

		// Token: 0x060000EF RID: 239 RVA: 0x00016F6C File Offset: 0x0001516C
		public void Save()
		{
			using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
			{
				this.Points = 0;
				foreach (XElement xelement in this.ClanMembers.Elements("clan_member_info").ToList<XElement>())
				{
					this.Points += Convert.ToInt32(xelement.Attribute("clan_points").Value);
					if (xelement.Attribute("clan_role").Value == "1")
					{
						this.LeaderName = xelement.Attribute("nickname").Value;
					}
				}
				new MySqlCommand(string.Format("UPDATE clans SET Name='{0}', Description='{1}', Players='{2}',Points='{3}',LeaderInfo='{4}' WHERE ID='{5}';", new object[]
				{
					this.Name,
					this.Description,
					this.ClanMembers.ToString(SaveOptions.DisableFormatting),
					this.Points,
					this.LeaderName,
					this.ID
				}), result).ExecuteNonQuery();
				result.Close();
			}
		}

		// Token: 0x060000F0 RID: 240 RVA: 0x000170E4 File Offset: 0x000152E4
		public async Task<bool> Load()
		{
			bool result2;
			using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
			{
				using (MySqlDataReader mySqlDataReader = new MySqlCommand("SELECT * FROM clans WHERE ID = '" + this.ID.ToString() + "'", result).ExecuteReader())
				{
					if (!mySqlDataReader.HasRows)
					{
						mySqlDataReader.Close();
						result2 = false;
					}
					else
					{
						mySqlDataReader.Read();
						this.ID = mySqlDataReader.GetInt64(0);
						this.Name = mySqlDataReader.GetString(1);
						this.Description = mySqlDataReader.GetString(2);
						this.CreationTime = mySqlDataReader.GetInt64(3);
						this.Points = mySqlDataReader.GetInt32(4);
						this.ClanMembers = XElement.Parse(mySqlDataReader.GetString(5));
						this.LeaderName = mySqlDataReader.GetString(6);
						mySqlDataReader.Close();
						result2 = true;
					}
				}
			}
			return result2;
		}

		// Token: 0x040000A2 RID: 162
		public long ID;

		// Token: 0x040000A3 RID: 163
		public string Name;

		// Token: 0x040000A4 RID: 164
		public string Description;

		// Token: 0x040000A5 RID: 165
		public long CreationTime;

		// Token: 0x040000A6 RID: 166
		public int Points;

		// Token: 0x040000A7 RID: 167
		public string LeaderName;

		// Token: 0x040000A8 RID: 168
		public XElement ClanMembers = new XElement("clan_members_info");

		// Token: 0x020000B9 RID: 185
		public enum CLAN_ROLE
		{
			// Token: 0x04000207 RID: 519
			NOT_IN_CLAN,
			// Token: 0x04000208 RID: 520
			LEADER,
			// Token: 0x04000209 RID: 521
			CO_LEADER,
			// Token: 0x0400020A RID: 522
			DEFAULT
		}
	}
}
