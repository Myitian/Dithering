using System;

/// <summary>
/// Temp double based image format. 0.0 is zero color, 1.0 is max color
/// </summary>
public sealed class TempDoubleImageFormat : IImageFormat<double>
{
	/// <summary>
	/// Width of bitmap
	/// </summary>
	public readonly int width;

	/// <summary>
	/// Height of bitmap
	/// </summary>
	public readonly int height;

	private readonly double[,,] content3d;

	private readonly double[] content1d;

	/// <summary>
	/// How many color channels per pixel
	/// </summary>
	public readonly int channelsPerPixel;

	/// <summary>
	/// Constructor for temp double image format
	/// </summary>
	/// <param name="input">Input bitmap as three dimensional (widht, height, channels per pixel) double array</param>
	/// <param name="createCopy">True if you want to create copy of data</param>
	public TempDoubleImageFormat(double[,,] input, bool createCopy = false)
	{
		if (createCopy)
		{
			content3d = (double[,,])input.Clone();
		}
		else
		{
			content3d = input;
		}
		
		content1d = null;
		width = input.GetLength(0);
		height = input.GetLength(1);
		channelsPerPixel = input.GetLength(2);
	}

	/// <summary>
	/// Constructor for temp double image format
	/// </summary>
	/// <param name="input">Input double array</param>
	/// <param name="imageWidth">Width</param>
	/// <param name="imageHeight">Height</param>
	/// <param name="imageChannelsPerPixel">Image channels per pixel</param>
	/// <param name="createCopy">True if you want to create copy of data</param>
	public TempDoubleImageFormat(double[] input, int imageWidth, int imageHeight, int imageChannelsPerPixel, bool createCopy = false)
	{
		content3d = null;
		if (createCopy)
		{
			content1d = new double[input.Length];
			Buffer.BlockCopy(input, 0, content1d, 0, input.Length * sizeof(double));
		}
		else
		{
			content1d = input;
		}
		width = imageWidth;
		height = imageHeight;
		channelsPerPixel = imageChannelsPerPixel;
	}

	/// <summary>
	/// Constructor for temp double image format
	/// </summary>
	/// <param name="input">Existing TempDoubleImageFormat</param>
	public TempDoubleImageFormat(TempDoubleImageFormat input)
	{
		if (input.content1d != null)
		{
			content1d = input.content1d;
			content3d = null;
		}
		else
		{
			content3d = input.content3d;
			content1d = null;
		}

		width = input.width;
		height = input.height;
		channelsPerPixel = input.channelsPerPixel;
	}

	/// <summary>
	/// Get width of bitmap
	/// </summary>
	/// <returns>Width in pixels</returns>
	public int GetWidth()
	{
		return width;
	}    
	
	/// <summary>
	/// Get height of bitmap
	/// </summary>
	/// <returns>Height in pixels</returns>
	public int GetHeight()
	{
		return height;
	}

	/// <summary>
	/// Get channels per pixel
	/// </summary>
	/// <returns>Channels per pixel</returns>
	public int GetChannelsPerPixel()
	{
		return channelsPerPixel;
	}

	/// <summary>
	/// Get raw content as double array
	/// </summary>
	/// <returns>Double array</returns>
	public double[] GetRawContent()
	{
		if (content1d != null)
		{
			return content1d;
		}
		else
		{
			double[] returnArray = new double[width * height * channelsPerPixel];
			int currentIndex = 0;
			for (int y = 0; y < height; y++)
			{
				for (int x = 0; x < width; x++)
				{
					for (int i = 0; i < channelsPerPixel; i++)
					{
						returnArray[currentIndex] = content3d[x, y, i];
						currentIndex++;
					}
				}
			}

			return returnArray;
		}
	}

	/// <summary>
	/// Set pixel channels of certain coordinate
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="newValues">New values as double array</param>
	public void SetPixelChannels(int x, int y, double[] newValues)
	{
		if (content1d != null)
		{
			int indexBase = y * width * channelsPerPixel + x * channelsPerPixel;
			for (int i = 0; i < channelsPerPixel; i++)
			{
				content1d[indexBase + i] = newValues[i];
			}
		}
		else
		{
			for (int i = 0; i < channelsPerPixel; i++)
			{
				content3d[x, y, i] = newValues[i];
			}
		}
	}

	/// <summary>
	/// Get pixel channels of certain coordinate
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <returns>Values as double array</returns>
	public double[] GetPixelChannels(int x, int y)
	{
		double[] returnArray = new double[channelsPerPixel];

		if (content1d != null)
		{
			int indexBase = y * width * channelsPerPixel + x * channelsPerPixel;
			for (int i = 0; i < channelsPerPixel; i++)
			{
				returnArray[i] = content1d[indexBase + i];
			}
		}
		else
		{
			for (int i = 0; i < channelsPerPixel; i++)
			{
				returnArray[i] = content3d[x, y, i];
			}
		}

		return returnArray;
	}

	/// <summary>
	/// Get pixel channels of certain coordinate
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="pixelStorage">Array where pixel channels values will be written</param>
	public void GetPixelChannels(int x, int y, ref double[] pixelStorage)
	{
		if (content1d != null)
		{
			int indexBase = y * width * channelsPerPixel + x * channelsPerPixel;
			for (int i = 0; i < channelsPerPixel; i++)
			{
				pixelStorage[i] = content1d[indexBase + i];
			}
		}
		else
		{
			for (int i = 0; i < channelsPerPixel; i++)
			{
				pixelStorage[i] = content3d[x, y, i];
			}
		}
	}

	/// <summary>
	/// Get quantization errors per channel
	/// </summary>
	/// <param name="originalPixel">Original pixels</param>
	/// <param name="newPixel">New pixels</param>
	/// <returns>Error values as double array</returns>
	public double[] GetQuantErrorsPerChannel(double[] originalPixel, double[] newPixel)
	{
		double[] returnValue = new double[channelsPerPixel];

		for (int i = 0; i < channelsPerPixel; i++)
		{
			returnValue[i] = (double)originalPixel[i] - (double)newPixel[i];
		}

		return returnValue;
	}

	/// <summary>
	/// Get quantization errors per channel
	/// </summary>
	/// <param name="originalPixel">Original pixels</param>
	/// <param name="newPixel">New pixels</param>
	/// <param name="errorValues">Error values as double array</param>
	public void GetQuantErrorsPerChannel(in double[] originalPixel, in double[] newPixel, ref double[] errorValues)
	{
		for (int i = 0; i < channelsPerPixel; i++)
		{
			errorValues[i] = originalPixel[i] - newPixel[i];
		}
	}

	/// <summary>
	/// Modify existing values with quantization errors
	/// </summary>
	/// <param name="modifyValues">Values to modify</param>
	/// <param name="quantErrors">Quantization errors</param>
	/// <param name="multiplier">Multiplier</param>
	public void ModifyPixelChannelsWithQuantError(ref double[] modifyValues, double[] quantErrors, double multiplier)
	{
		for (int i = 0; i < channelsPerPixel; i++)
		{
			modifyValues[i] = GetLimitedValue((byte)modifyValues[i], quantErrors[i] * multiplier);
		}
	}

	private static double GetLimitedValue(byte original, double error)
	{
		double newValue = original + error;
		return Clamp(newValue, 0.0, 1.0);
	}

	// C# doesn't have a Clamp method so we need this
	private static double Clamp(double value, double min, double max)
	{
		return (value < min) ? 0.0 : (value > max) ? 1.0 : value;
	}
}