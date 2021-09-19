/*
   This file implements error pushing of dithering via (Peter) Stucki kernel.

   This is free and unencumbered software released into the public domain.
*/


/// <summary>
/// Stucki dithering for RGB bytes
/// </summary>
public sealed class StuckiDitheringRGBByte : DitheringBase<byte>
{
	/// <summary>
	/// Constructor for Stucki dithering
	/// </summary>
	/// <param name="colorfunc">Color function</param>
	public StuckiDitheringRGBByte(ColorFunction colorfunc) : base(colorfunc, "Stucki")
	{

	}

	/// <summary>
	/// Push error method for Stucki dithering
	/// </summary>
	/// <param name="x">X coordinate</param>
	/// <param name="y">Y coordinate</param>
	/// <param name="quantError">Quantization error</param>
	override protected void PushError(int x, int y, double[] quantError)
	{
		// Push error
		//                X     8/42   4/42 
		// 2/42   4/42   8/42   4/42   2/42
		// 1/42   2/42   4/42   2/42   1/42

		int xMinusOne = x - 1;
		int xMinusTwo = x - 2;
		int xPlusOne = x + 1;
		int xPlusTwo = x + 2;
		int yPlusOne = y + 1;
		int yPlusTwo = y + 2;

		// Current row
		int currentRow = y;
		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 8.0 / 42.0);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, 4.0 / 42.0);
		}

		// Next row
		currentRow = yPlusOne;
		if (IsValidCoordinate(xMinusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusTwo, currentRow, quantError, 2.0 / 42.0);
		}

		if (IsValidCoordinate(xMinusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, 4.0 / 42.0);
		}

		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, 8.0 / 42.0);
		}

		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 4.0 / 42.0);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, 2.0 / 42.0);
		}

		// Next row
		currentRow = yPlusTwo;
		if (IsValidCoordinate(xMinusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusTwo, currentRow, quantError, 1.0 / 42.0);
		}

		if (IsValidCoordinate(xMinusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xMinusOne, currentRow, quantError, 2.0 / 42.0);
		}

		if (IsValidCoordinate(x, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(x, currentRow, quantError, 4.0 / 42.0);
		}

		if (IsValidCoordinate(xPlusOne, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusOne, currentRow, quantError, 2.0 / 42.0);
		}

		if (IsValidCoordinate(xPlusTwo, currentRow))
		{
			ModifyImageWithErrorAndMultiplier(xPlusTwo, currentRow, quantError, 1.0 / 42.0);
		}
	}
}
