using System;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Windows;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace CustomControl;

public class ColorPicker
{
	public static System.Windows.Media.Color PointColor(System.Windows.Point point)
	{
		IntPtr dC = GetDC(IntPtr.Zero);
		uint pixel = GetPixel(dC, (int)point.X, (int)point.Y);
		ReleaseDC(IntPtr.Zero, dC);
		return System.Windows.Media.Color.FromRgb((byte)(0xFF & pixel), (byte)(0xFF & (pixel >> 8)), (byte)(0xFF & (pixel >> 16)));
	}

	public static BitmapSource ScreenSnapshot()
	{
		using Bitmap bitmap = new Bitmap((int)SystemParameters.PrimaryScreenWidth, (int)SystemParameters.PrimaryScreenHeight);
		using (Graphics graphics = Graphics.FromImage(bitmap))
		{
			graphics.CopyFromScreen(0, 0, 0, 0, bitmap.Size, CopyPixelOperation.SourceCopy);
		}
		IntPtr hbitmap = bitmap.GetHbitmap();
		try
		{
			return Imaging.CreateBitmapSourceFromHBitmap(hbitmap, IntPtr.Zero, Int32Rect.Empty, BitmapSizeOptions.FromEmptyOptions());
		}
		finally
		{
			DeleteObject(hbitmap);
		}
	}

	[DllImport("gdi32.dll")]
	public static extern bool DeleteObject(IntPtr hObject);

	[DllImport("user32.dll")]
	private static extern IntPtr GetDC(IntPtr hwnd);

	[DllImport("user32.dll")]
	private static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

	[DllImport("gdi32.dll")]
	private static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);
}
