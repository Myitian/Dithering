using System;
using System.Collections.Generic;
using System.Text;

namespace Myitian.Dithering
{
	/// <summary>
	/// 仿色
	/// </summary>
	public static class Dithering
	{
		/// <summary>
		/// 仿色方式
		/// </summary>
		public enum DitheringMethod : byte
		{
			/// <summary>无</summary>
			None,
			/// <summary>Atkinson</summary>
			Atkinson,
			/// <summary>Burkes</summary>
			Burkes,
			/// <summary>Floyd-Steinberg</summary>
			FloydSteinberg,
			/// <summary>Jarvis-Judice-Ninke</summary>
			JarvisJudiceNinke,
			/// <summary>Sierra</summary>
			Sierra,
			/// <summary>SierraLite</summary>
			SierraLite,
			/// <summary>SierraTwoRow</summary>
			SierraTwoRow,
			/// <summary>Stucki</summary>
			Stucki,
		}

		/// <summary>
		/// 通过名称选择仿色方式
		/// </summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		/// <param name="ditheringMethod">仿色方式</param>
		public static TempByteImageFormat SelectDitheringByName(in DitheringBase<byte>.ColorFunction colorFunction, in byte[,,] colorBytes, DitheringMethod ditheringMethod = DitheringMethod.None)
		{
			switch (ditheringMethod)
			{
				case DitheringMethod.Atkinson:
					return AtkinsonDithering(colorFunction, colorBytes);
				case DitheringMethod.Burkes:
					return BurkesDithering(colorFunction, colorBytes);
				case DitheringMethod.FloydSteinberg:
					return FloydSteinbergDithering(colorFunction, colorBytes);
				case DitheringMethod.JarvisJudiceNinke:
					return JarvisJudiceNinkeDithering(colorFunction, colorBytes);
				case DitheringMethod.Sierra:
					return SierraDithering(colorFunction, colorBytes);
				case DitheringMethod.SierraLite:
					return SierraLiteDithering(colorFunction, colorBytes);
				case DitheringMethod.SierraTwoRow:
					return SierraTwoRowDithering(colorFunction, colorBytes);
				case DitheringMethod.Stucki:
					return StuckiDithering(colorFunction, colorBytes);
				default:
					return FakeDithering(colorFunction, colorBytes);
			}
		}

		/// <summary>Atkinson仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat AtkinsonDithering(DitheringBase<byte>.ColorFunction colorFunction,byte[,,] colorBytes)
		{
			AtkinsonDitheringRGBByte atkinson = new AtkinsonDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			atkinson.DoDithering(temp);
			return temp;
		}

		/// <summary>Burkes仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat BurkesDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			BurkesDitheringRGBByte burkes = new BurkesDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			burkes.DoDithering(temp);
			return temp;
		}

		/// <summary>Floyd-Steinberg仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat FloydSteinbergDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			FloydSteinbergDitheringRGBByte floydSteinberg = new FloydSteinbergDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			floydSteinberg.DoDithering(temp);
			return temp;
		}

		/// <summary>Jarvis-Judice-Ninke仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat JarvisJudiceNinkeDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			JarvisJudiceNinkeDitheringRGBByte jarvisJudiceNinke = new JarvisJudiceNinkeDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			jarvisJudiceNinke.DoDithering(temp);
			return temp;
		}

		/// <summary>Sierra仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat SierraDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			SierraDitheringRGBByte sierra = new SierraDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			sierra.DoDithering(temp);
			return temp;
		}

		/// <summary>SierraLite仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat SierraLiteDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			SierraLiteDitheringRGBByte sierraLite = new SierraLiteDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			sierraLite.DoDithering(temp);
			return temp;
		}

		/// <summary>SierraTwoRow仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat SierraTwoRowDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			SierraTwoRowDitheringRGBByte sierraTwoRow = new SierraTwoRowDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			sierraTwoRow.DoDithering(temp);
			return temp;
		}

		/// <summary>Stucki仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat StuckiDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			StuckiDitheringRGBByte stucki = new StuckiDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			stucki.DoDithering(temp);
			return temp;
		}

		/// <summary>无仿色</summary>
		/// <param name="colorFunction">查找近似颜色</param>
		/// <param name="colorBytes">颜色，字节数组（第1维：x，第2维：y，第3维：RGB）</param>
		public static TempByteImageFormat FakeDithering(DitheringBase<byte>.ColorFunction colorFunction, byte[,,] colorBytes)
		{
			FakeDitheringRGBByte fake = new FakeDitheringRGBByte(colorFunction);
			TempByteImageFormat temp = new TempByteImageFormat(colorBytes);
			fake.DoDithering(temp);
			return temp;
		}
	}
}
