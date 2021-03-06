/*
   This file implements error pushing of dithering via (Frankie) Sierra Lite kernel.

   This is free and unencumbered software released into the public domain.
*/

/// <summary>
/// Sierra lite dithering for RGB bytes
/// </summary>
public sealed class SierraLiteDitheringRGBByte : DitheringBase<byte>
{
	/// <summary>
	/// Constructor for Sierra lite dithering
	/// </summary>
	/// <param name="colorfunc">Color function</param>
	public SierraLiteDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "SierraLite")
	{

	}

	/// <summary>
	/// Push error method for Sierra lite dithering
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	override protected void PushError(int x, int y, double[] quantError)
	{
		// Push error
		//        X    2/4
		// 1/4   1/4

		int xMinusOne = x - 1;
		int xPlusOne = x + 1;
		int yPlusOne = y + 1;

		// Current row
		int currentRow = y;
		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 2.0 / 4.0);
		}

		// Next row
		currentRow = yPlusOne;
		if (IsValidCoordinate(xMinusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, 1.0 / 4.0);
		}

		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, 1.0 / 4.0);
		}
	}
}
