/*
   This file implements error pushing of dithering via (Daniel) Burkes kernel.

   This is free and unencumbered software released into the public domain.
*/

/// <summary>
/// Burkes dithering for RGB bytes
/// </summary>
public sealed class BurkesDitheringRGBByte : DitheringBase<byte>
{
	/// <summary>
	/// Constructor for Burkes dithering
	/// </summary>
	/// <param name="colorfunc">Color function</param>
	public BurkesDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "Burkes")
	{

	}

	/// <summary>
	/// Push error method for Burkes dithering
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	override protected void PushError(int x, int y, double[] quantError)
	{
		// Push error
		//                X     8/32   4/32 
		// 2/32   4/32   8/32   4/32   2/32

		int xMinusOne = x - 1;
		int xMinusTwo = x - 2;
		int xPlusOne = x + 1;
		int xPlusTwo = x + 2;
		int yPlusOne = y + 1;

		// Current row
		int currentRow = y;
		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 8.0f / 32.0f);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, 4.0f / 32.0f);
		}

		// Next row
		currentRow = yPlusOne;
		if (IsValidCoordinate(xMinusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusTwo, currentRow, quantError, 2.0f / 32.0f);
		}

		if (IsValidCoordinate(xMinusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, 4.0f / 32.0f);
		}

		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, 8.0f / 32.0f);
		}

		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 4.0f / 32.0f);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, 2.0f / 32.0f);
		}
	}
}

