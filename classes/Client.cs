// Decompiled with JetBrains decompiler
// Type: OnyxServer.CLASSES.Client
// Assembly: OnyxServer, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: 55A849F0-90C5-4313-B057-B0361B81D824
// Assembly location: C:\Users\Дмитрий\Downloads\New Compressed (zipped) Folder\servers\tls\OnyxServer.dll

using MySql.Data.MySqlClient;
using WARTLS.xmpp;
using WARTLS.xmpp.query;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Net;
using System.Net.Security;
using System.Net.Sockets;
using System.Text;
using System.Xml;
using System.Xml.Linq;

namespace WARTLS.CLASSES
{
	// Token: 0x0200006A RID: 106
	public class Client : IDisposable
	{
		// Token: 0x17000008 RID: 8
		// (get) Token: 0x060000F4 RID: 244 RVA: 0x00017159 File Offset: 0x00015359
		public string IPAddress
		{
			get
			{
				return ((IPEndPoint)this.Socket.RemoteEndPoint).Address.ToString();
			}
		}

		// Token: 0x060000F5 RID: 245 RVA: 0x00017178 File Offset: 0x00015378
		public static string ResolveNickname(long ID)
		{
			string result2;
			using (MySqlConnection result = SQL.GetConnection().GetAwaiter().GetResult())
			{
				try
				{
					result2 = new MySqlCommand(string.Format("SELECT nickname FROM users WHERE profileid='{0}';", ID), result).ExecuteScalar().ToString();
				}
				catch (Exception)
				{
					result2 = null;
				}
				finally
				{
					result.Close();
				}
			}
			return result2;
		}

		// Token: 0x060000F6 RID: 246 RVA: 0x000171FC File Offset: 0x000153FC
		public XElement ToElement(bool isWarface = true)
		{
			XElement xelement = new XElement("player");
			xelement.Add(new XAttribute("profile_id", this.Player.UserID));
			xelement.Add(new XAttribute("team_id", (byte)this.Player.RoomPlayer.TeamId));
			xelement.Add(new XAttribute("status", (byte)this.Player.RoomPlayer.Status));
			xelement.Add(new XAttribute("observer", this.Player.Observer));
			xelement.Add(new XAttribute("skill", "0.000"));
			xelement.Add(new XAttribute("nickname", this.Player.Nickname));
			xelement.Add(new XAttribute("clanName", (this.Player.Clan.ID != 0L) ? this.Player.Clan.Name : ""));
			xelement.Add(new XAttribute("class_id", this.Player.CurrentClass));
			xelement.Add(new XAttribute("online_id", this.JID));
			xelement.Add(new XAttribute("group_id", this.Player.RoomPlayer.GroupId));
			xelement.Add(new XAttribute("presence", this.Status));
			xelement.Add(new XAttribute("experience", this.Player.Experience));
			xelement.Add(new XAttribute("banner_badge", this.Player.BannerBadge));
			xelement.Add(new XAttribute("banner_mark", this.Player.BannerMark));
			xelement.Add(new XAttribute("banner_stripe", this.Player.BannerStripe));
			return xelement;
		}

		// Token: 0x060000F7 RID: 247 RVA: 0x0001743C File Offset: 0x0001563C
		internal void CheckExperience()
		{
			int num = Player.Rank - Player.OldRank;
			for (byte b = 0; b < num; b = (byte)(b + 1))
			{
				Player.AddRankNotifierNotification(Player.OldRank, Player.Rank);
				Player.OldRank++;
				Player.AddRandomBoxNotification("random_box_bonus_13");
			}
			if (num > 0)
			{
				new SyncNotification(this).Process();
			}
		}


		// Token: 0x060000F8 RID: 248 RVA: 0x000174AC File Offset: 0x000156AC
		public void ShowMessage(string Text, bool Green = false)
		{
			XElement xelement = new XElement("notif");
			xelement.Add(new XAttribute("id", 301));
			xelement.Add(new XAttribute("type", Green ? 512 : 8));
			xelement.Add(new XAttribute("confirmation", 0));
			xelement.Add(new XAttribute("from_jid", "onyx@server"));
			xelement.Add(new XAttribute("message", ""));
			if (Green)
			{
				XElement content = new XElement("announcement", new object[]
				{
					new XAttribute("id", this.Player.Random.Next(99, 9999999)),
					new XAttribute("is_system", 1),
					new XAttribute("frequency", int.MaxValue),
					new XAttribute("repeat_time", 0),
					new XAttribute("message", Text),
					new XAttribute("server", "onyx"),
					new XAttribute("channel", "onyx"),
					new XAttribute("place", 1)
				});
				xelement.Add(content);
			}
			else
			{
				XElement content2 = new XElement("message", new XAttribute("data", Text));
				xelement.Add(content2);
			}
			new XElement("message", new XAttribute("data", Text));
			new SyncNotification(this, xelement).Process();
		}

		// Token: 0x060000F9 RID: 249 RVA: 0x000176A8 File Offset: 0x000158A8
		public async void Send(string Message)
		{
			byte[] bytes = Encoding.UTF8.GetBytes(Message);
			MemoryStream memoryStream = new MemoryStream();
			BinaryWriter binaryWriter = new BinaryWriter(memoryStream);
			binaryWriter.Write(Gateway.MagicBytes);
			binaryWriter.Write((long)bytes.Length);
			binaryWriter.Write(bytes);
			try
			{
				if (this.SslStream == null || !this.SslStream.IsAuthenticated)
				{
					await this.Socket.SendAsync(new ArraySegment<byte>(memoryStream.ToArray()), SocketFlags.None);
				}
				else
				{
					this.EnqueueDataForWrite(this.SslStream, memoryStream.ToArray());
				}
			}
			catch (Exception ex)
			{
				
				Console.WriteLine(ex.Message);
			}
		}

		// Token: 0x060000FA RID: 250 RVA: 0x000176EC File Offset: 0x000158EC
		public void SendAPI(string Message)
		{
		//	Console.Out.WriteLineAsync(Message);
			byte[] bytes = Encoding.UTF8.GetBytes(Message);
			try
			{
				this.Socket.SendAsync(new ArraySegment<byte>(bytes), SocketFlags.None);
			}
			catch (Exception ex)
			{
				Console.WriteLine(ex.Message);
			}
		}

		// Token: 0x060000FB RID: 251 RVA: 0x0001774C File Offset: 0x0001594C
		private void EnqueueDataForWrite(SslStream sslStream, byte[] buffer)
		{
			if (buffer != null)
			{
				this.writePendingData.Enqueue(buffer);
				ConcurrentQueue<byte[]> obj = this.writePendingData;
				lock (obj)
				{
					if (this.sendingData)
					{
						return;
					}
					this.sendingData = true;
				}
				this.Write(sslStream);
			}
		}

		// Token: 0x060000FC RID: 252 RVA: 0x000177B0 File Offset: 0x000159B0
		private void Write(SslStream sslStream)
		{
			byte[] array = null;
			try
			{
				if (this.writePendingData.Count > 0 && this.writePendingData.TryDequeue(out array))
				{
					sslStream.BeginWrite(array, 0, array.Length, new AsyncCallback(this.WriteCallback), sslStream);
				}
				else
				{
					ConcurrentQueue<byte[]> obj = this.writePendingData;
					lock (obj)
					{
						this.sendingData = false;
					}
				}
			}
			catch (Exception)
			{
				ConcurrentQueue<byte[]> obj = this.writePendingData;
				lock (obj)
				{
					this.sendingData = false;
				}
			}
		}

		// Token: 0x060000FD RID: 253 RVA: 0x0001786C File Offset: 0x00015A6C
		private void WriteCallback(IAsyncResult ar)
		{
			SslStream sslStream = (SslStream)ar.AsyncState;
			try
			{
				sslStream.EndWrite(ar);
			}
			catch (Exception)
			{
				return;
			}
			this.Write(sslStream);
		}

		// Token: 0x060000FE RID: 254 RVA: 0x000178A8 File Offset: 0x00015AA8
		private void SocketWrite(IAsyncResult Result)
		{
			this.Socket.EndSend(Result);
		}

		// Token: 0x060000FF RID: 255 RVA: 0x000178B7 File Offset: 0x00015AB7
		public void Dispose()
		{
			new StreamError(this, "resource-constraint");
			if (this.Socket != null)
			{
				this.Socket.Close();
			}
			if (this.SslStream != null)
			{
				this.SslStream.Close();
			}
		}

		// Token: 0x040000AC RID: 172
		public bool UnhandledAllowed = true;

		// Token: 0x040000AD RID: 173
		public Dictionary<string, XElement> DedicatedTelemetryes;
		internal Dictionary<string, Stopwatch> ChatWatcher = new Dictionary<string, Stopwatch>();
		
		// Token: 0x040000AE RID: 174а
		public Socket Socket;

		// Token: 0x040000AF RID: 175
		public Player Player;

		// Token: 0x040000B0 RID: 176
		public SslStream SslStream;

		// Token: 0x040000B1 RID: 177
		public Channel Channel;

		// Token: 0x040000B2 RID: 178
		public List<InvitationTicket> InvitationTicket = new List<InvitationTicket>();

		// Token: 0x040000B3 RID: 179
		public byte[] Buffer;

		// Token: 0x040000B4 RID: 180
		public int Status;

		// Token: 0x040000B5 RID: 181
		public bool Dedicated;

		// Token: 0x040000B6 RID: 182
		public bool Authorized;

		// Token: 0x040000B7 RID: 183
		public string JID;

		// Token: 0x040000B8 RID: 184
		public string Location = "";

		// Token: 0x040000B9 RID: 185
		public int Received;

		// Token: 0x040000BA RID: 186
		public ushort DedicatedPort;

		// Token: 0x040000BB RID: 187
		private ConcurrentQueue<byte[]> writePendingData = new ConcurrentQueue<byte[]>();

		// Token: 0x040000BC RID: 188
		private bool sendingData;

		// Token: 0x040000BD RID: 189
		public long ID;

		// Token: 0x040000BE RID: 190
		public string Password;

		// Token: 0x040000BF RID: 191
		public string Token;

		// Token: 0x040000C0 RID: 192
		public bool isAuthed;

		// Token: 0x040000C1 RID: 193
		public int adminStatus;
	}
}
