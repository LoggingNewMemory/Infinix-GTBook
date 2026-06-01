using System;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace BYDWmi;

public class Win32Control
{
	public delegate int HookProc(int nCode, int wParam, IntPtr lParam);

	[StructLayout(LayoutKind.Sequential)]
	public class KeyBoardHookStruct
	{
		public int vkCode;

		public int scanCode;

		public int flags;

		public int time;

		public int dwExtraInfo;
	}

	private static int hHook;

	public const int WH_KEYBOARD_LL = 13;

	private static HookProc KeyBoardHookProcedure;

	[DllImport("user32.dll")]
	public static extern int SetWindowsHookEx(int idHook, HookProc lpfn, IntPtr hInstance, int threadId);

	[DllImport("user32.dll", CallingConvention = CallingConvention.StdCall, CharSet = CharSet.Auto)]
	public static extern bool UnhookWindowsHookEx(int idHook);

	[DllImport("user32.dll")]
	public static extern int CallNextHookEx(int idHook, int nCode, int wParam, IntPtr lParam);

	[DllImport("kernel32.dll")]
	public static extern IntPtr GetModuleHandle(string name);

	private int KeyBoardHookProc(int nCode, int wParam, IntPtr lParam)
	{
		if (nCode >= 0)
		{
			KeyBoardHookStruct keyBoardHookStruct = (KeyBoardHookStruct)Marshal.PtrToStructure(lParam, typeof(KeyBoardHookStruct));
			if (keyBoardHookStruct.vkCode == 91 || keyBoardHookStruct.vkCode == 92)
			{
				return 1;
			}
		}
		return CallNextHookEx(hHook, nCode, wParam, lParam);
	}

	public void Hook_Start()
	{
		if (hHook == 0)
		{
			KeyBoardHookProcedure = KeyBoardHookProc;
			hHook = SetWindowsHookEx(13, KeyBoardHookProcedure, GetModuleHandle(Process.GetCurrentProcess().MainModule.ModuleName), 0);
			if (hHook == 0)
			{
				Hook_Clear();
			}
		}
	}

	public void Hook_Clear()
	{
		if (hHook != 0)
		{
			UnhookWindowsHookEx(hHook);
			hHook = 0;
		}
	}
}
