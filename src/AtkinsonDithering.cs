/*
   This file implements error pushing of dithering via Atkinson kernel.

   This is free and unencumbered software released into the public domain.
*/

/// <summary>
/// Atkinson dithering for RGB bytes
/// </summary>
public sealed class AtkinsonDitheringRGBByte : DitheringBase<byte>
{
	/// <summary>
	/// Constructor for Atkinson dithering
	/// </summary>
	/// <param name="colorfunc">Color function</param>
	public AtkinsonDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "Atkinson")
	{

	}

	/// <summary>
	/// Push error method for Atkinson dithering
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	override protected void PushError(int x, int y, double[] quantError)
	{
		// Push error
		//        X    1/8   1/8 
		// 1/8   1/8   1/8
		//       1/8

		int xMinusOne = x - 1;
		int xPlusOne = x + 1;
		int xPlusTwo = x + 2;
		int yPlusOne = y + 1;
		int yPlusTwo = y + 2;

		double multiplier = 1.0 / 8.0; // Atkinson Dithering has same multiplier for every item

		// Current row
		int currentRow = y;
		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, multiplier);
		}

		// Next row
		currentRow = yPlusOne;
		if (IsValidCoordinate(xMinusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, multiplier);
		}

		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
		}

		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, multiplier);
		}

		// Next row
		currentRow = yPlusTwo;
		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, multiplier);
		}
	}
}

