// Decompiled with JetBrains decompiler
// Type: DPAPI
// Assembly: Heleks_IL, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null
// MVID: FBB44E79-1326-4A0E-9F49-E3687B1B8C49
// Assembly location: C:\Users\Пользователь\Desktop\Dumps\Heleks.exe

using System;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;

public class DPAPI
{
  private static IntPtr NullPtr = (IntPtr) 0;
  private static DPAPI.KeyType defaultKeyType = DPAPI.KeyType.UserKey;
  private const int CRYPTPROTECT_UI_FORBIDDEN = 1;
  private const int CRYPTPROTECT_LOCAL_MACHINE = 4;

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool CryptProtectData(ref DPAPI.DATA_BLOB pPlainText, string szDescription, ref DPAPI.DATA_BLOB pEntropy, IntPtr pReserved, ref DPAPI.CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DPAPI.DATA_BLOB pCipherText);

  [DllImport("crypt32.dll", CharSet = CharSet.Auto, SetLastError = true)]
  private static extern bool CryptUnprotectData(ref DPAPI.DATA_BLOB pCipherText, ref string pszDescription, ref DPAPI.DATA_BLOB pEntropy, IntPtr pReserved, ref DPAPI.CRYPTPROTECT_PROMPTSTRUCT pPrompt, int dwFlags, ref DPAPI.DATA_BLOB pPlainText);

  private static void InitPrompt(ref DPAPI.CRYPTPROTECT_PROMPTSTRUCT ps)
  {
    ps.cbSize = Marshal.SizeOf(typeof (DPAPI.CRYPTPROTECT_PROMPTSTRUCT));
    ps.dwPromptFlags = 0;
    ps.hwndApp = DPAPI.NullPtr;
    ps.szPrompt = (string) null;
  }

  private static void InitBLOB(byte[] data, ref DPAPI.DATA_BLOB blob)
  {
    if (data == null)
      data = new byte[0];
    blob.pbData = Marshal.AllocHGlobal(data.Length);
    if (blob.pbData == IntPtr.Zero)
      throw new Exception("Unable to allocate data buffer for BLOB structure.");
    blob.cbData = data.Length;
    Marshal.Copy(data, 0, blob.pbData, data.Length);
  }

  public static string Encrypt(string plainText)
  {
    return DPAPI.Encrypt(DPAPI.defaultKeyType, plainText, string.Empty, string.Empty);
  }

  public static string Encrypt(DPAPI.KeyType keyType, string plainText)
  {
    return DPAPI.Encrypt(keyType, plainText, string.Empty, string.Empty);
  }

  public static string Encrypt(DPAPI.KeyType keyType, string plainText, string entropy)
  {
    return DPAPI.Encrypt(keyType, plainText, entropy, string.Empty);
  }

  public static string Encrypt(DPAPI.KeyType keyType, string plainText, string entropy, string description)
  {
    if (plainText == null)
      plainText = string.Empty;
    if (entropy == null)
      entropy = string.Empty;
    return Convert.ToBase64String(DPAPI.Encrypt(keyType, Encoding.UTF8.GetBytes(plainText), Encoding.UTF8.GetBytes(entropy), description));
  }

  public static byte[] Encrypt(DPAPI.KeyType keyType, byte[] plainTextBytes, byte[] entropyBytes, string description)
  {
    if (plainTextBytes == null)
      plainTextBytes = new byte[0];
    if (entropyBytes == null)
      entropyBytes = new byte[0];
    if (description == null)
      description = string.Empty;
    DPAPI.DATA_BLOB dataBlob1 = new DPAPI.DATA_BLOB();
    DPAPI.DATA_BLOB pCipherText = new DPAPI.DATA_BLOB();
    DPAPI.DATA_BLOB dataBlob2 = new DPAPI.DATA_BLOB();
    DPAPI.CRYPTPROTECT_PROMPTSTRUCT cryptprotectPromptstruct = new DPAPI.CRYPTPROTECT_PROMPTSTRUCT();
    DPAPI.InitPrompt(ref cryptprotectPromptstruct);
    try
    {
      try
      {
        DPAPI.InitBLOB(plainTextBytes, ref dataBlob1);
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot initialize plaintext BLOB.", ex);
      }
      try
      {
        DPAPI.InitBLOB(entropyBytes, ref dataBlob2);
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot initialize entropy BLOB.", ex);
      }
      int dwFlags = 1;
      if (keyType == DPAPI.KeyType.MachineKey)
        dwFlags |= 4;
      if (!DPAPI.CryptProtectData(ref dataBlob1, description, ref dataBlob2, IntPtr.Zero, ref cryptprotectPromptstruct, dwFlags, ref pCipherText))
        throw new Exception("CryptProtectData failed.", (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      byte[] destination = new byte[pCipherText.cbData];
      Marshal.Copy(pCipherText.pbData, destination, 0, pCipherText.cbData);
      return destination;
    }
    catch (Exception ex)
    {
      throw new Exception("DPAPI was unable to encrypt data.", ex);
    }
    finally
    {
      if (dataBlob1.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(dataBlob1.pbData);
      if (pCipherText.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(pCipherText.pbData);
      if (dataBlob2.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(dataBlob2.pbData);
    }
  }

  public static string Decrypt(string cipherText)
  {
    string description;
    return DPAPI.Decrypt(cipherText, string.Empty, out description);
  }

  public static string Decrypt(string cipherText, out string description)
  {
    return DPAPI.Decrypt(cipherText, string.Empty, out description);
  }

  public static string Decrypt(string cipherText, string entropy, out string description)
  {
    if (entropy == null)
      entropy = string.Empty;
    return Encoding.UTF8.GetString(DPAPI.Decrypt(Convert.FromBase64String(cipherText), Encoding.UTF8.GetBytes(entropy), out description));
  }

  public static byte[] Decrypt(byte[] cipherTextBytes, byte[] entropyBytes, out string description)
  {
    DPAPI.DATA_BLOB pPlainText = new DPAPI.DATA_BLOB();
    DPAPI.DATA_BLOB dataBlob1 = new DPAPI.DATA_BLOB();
    DPAPI.DATA_BLOB dataBlob2 = new DPAPI.DATA_BLOB();
    DPAPI.CRYPTPROTECT_PROMPTSTRUCT cryptprotectPromptstruct = new DPAPI.CRYPTPROTECT_PROMPTSTRUCT();
    DPAPI.InitPrompt(ref cryptprotectPromptstruct);
    description = string.Empty;
    try
    {
      try
      {
        DPAPI.InitBLOB(cipherTextBytes, ref dataBlob1);
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot initialize ciphertext BLOB.", ex);
      }
      try
      {
        DPAPI.InitBLOB(entropyBytes, ref dataBlob2);
      }
      catch (Exception ex)
      {
        throw new Exception("Cannot initialize entropy BLOB.", ex);
      }
      int dwFlags = 1;
      if (!DPAPI.CryptUnprotectData(ref dataBlob1, ref description, ref dataBlob2, IntPtr.Zero, ref cryptprotectPromptstruct, dwFlags, ref pPlainText))
        throw new Exception("CryptUnprotectData failed.", (Exception) new Win32Exception(Marshal.GetLastWin32Error()));
      byte[] destination = new byte[pPlainText.cbData];
      Marshal.Copy(pPlainText.pbData, destination, 0, pPlainText.cbData);
      return destination;
    }
    catch (Exception ex)
    {
      throw new Exception("DPAPI was unable to decrypt data.", ex);
    }
    finally
    {
      if (pPlainText.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(pPlainText.pbData);
      if (dataBlob1.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(dataBlob1.pbData);
      if (dataBlob2.pbData != IntPtr.Zero)
        Marshal.FreeHGlobal(dataBlob2.pbData);
    }
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct DATA_BLOB
  {
    public int cbData;
    public IntPtr pbData;
  }

  [StructLayout(LayoutKind.Sequential, CharSet = CharSet.Unicode)]
  internal struct CRYPTPROTECT_PROMPTSTRUCT
  {
    public int cbSize;
    public int dwPromptFlags;
    public IntPtr hwndApp;
    public string szPrompt;
  }

  public enum KeyType
  {
    UserKey = 1,
    MachineKey = 2,
  }
}
