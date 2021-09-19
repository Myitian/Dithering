/*
   This file implements base dithering class that can be used to implement error pushing dithering implementations.

   This is free and unencumbered software released into the public domain.
*/


/// <summary>
/// Abstract base class for dithering implementations
/// </summary>
public abstract class DitheringBase<T>
{
	/// <summary>
	/// Width of bitmap
	/// </summary>
	protected int width;

	/// <summary>
	/// Height of bitmap
	/// </summary>
	protected int height;

	/// <summary>
	/// Long name of the dither method
	/// </summary>
	private readonly string methodLongName = "";

	/// <summary>
	/// Color reduction function/method
	/// </summary>
	protected ColorFunction colorFunction = null;

	/// <summary>
	/// Current bitmap
	/// </summary>
	private IImageFormat<T> currentBitmap;

	/// <summary>
	/// Color function for color reduction
	/// </summary>
	/// <param name="x"></param>
	/// <param name="y"></param>
	/// <param name="inputColors">Input colors</param>
	/// <param name="outputColors">Output colors</param>
	public delegate void ColorFunction(in int x, in int y, in T[] inputColors, ref T[] outputColors);

	/// <summary>
	/// Base constructor
	/// </summary>
	/// <param name="colorfunc">Color reduction function/method</param>
	/// <param name="longName">Long name of dither method</param>
	public DitheringBase(ColorFunction colorfunc, string longName)
	{
		colorFunction = colorfunc;
		methodLongName = longName;
	}

	/// <summary>
	/// Do dithering for chosen image with chosen color reduction method. Work horse, call this when you want to dither something
	/// </summary>
	/// <param name="input">Input image</param>
	/// <returns>Dithered image</returns>
	public IImageFormat<T> DoDithering(IImageFormat<T> input)
	{
		width = input.GetWidth();
		height = input.GetHeight();
		int channelsPerPixel = input.GetChannelsPerPixel();
		currentBitmap = input;

		T[] originalPixel = new T[channelsPerPixel];
		T[] newPixel = new T[channelsPerPixel];
		tempBuffer = new T[channelsPerPixel];
		double[] quantError = new double[channelsPerPixel];

		for (int y = 0; y < height; y++)
		{
			for (int x = 0; x < width; x++)
			{
				input.GetPixelChannels(x, y, ref originalPixel);
				colorFunction(in x, in y,originalPixel, ref newPixel);

				input.SetPixelChannels(x, y, newPixel);

				input.GetQuantErrorsPerChannel(in originalPixel, in newPixel, ref quantError);

				PushError(x, y, quantError);
			}
		}

		return input;
	}

	/// <summary>
	/// Get dither method name
	/// </summary>
	/// <returns>String method name</returns>
	public string GetMethodName()
	{
		return methodLongName;
	}

	/// <summary>
	/// Check if image coordinate is valid
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <returns>True if valid; False otherwise</returns>
	protected bool IsValidCoordinate(int x, int y)
	{
		return (0 <= x && x < width && 0 <= y && y < height);
	}

	/// <summary>
	/// How error cumulation should be handled. Implement this for every dithering method
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	protected abstract void PushError(int x, int y, double[] quantError);

	private T[] tempBuffer = null;

	/// <summary>
	/// Modify image with error and multiplier
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	/// <param name="multiplier">Multiplier</param>
	public void ModifyImageWithErrorAndMultiplier(int x, int y, double[] quantError, double multiplier)
	{
		currentBitmap.GetPixelChannels(x, y, ref tempBuffer);

		// We limit the color here because we don't want the value go over min or max
		currentBitmap.ModifyPixelChannelsWithQuantError(ref tempBuffer, quantError, multiplier);

		currentBitmap.SetPixelChannels(x, y, tempBuffer);
	}
}
