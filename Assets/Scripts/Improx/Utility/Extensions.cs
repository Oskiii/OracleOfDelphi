using System;
using System.Collections.Generic;
using System.Threading;
using UnityEngine;
using Random = System.Random;

namespace Improx.Utility
{
	public static class Extensions
	{
		public static void Shuffle<T>(this IList<T> list)
		{
			int n = list.Count;
			while (n > 1)
			{
				n--;
				int k = ThreadSafeRandom.ThisThreadsRandom.Next(n + 1);
				T value = list[k];
				list[k] = list[n];
				list[n] = value;
			}
		}

		public static double GetLuminance(this Color color)
		{
			return (0.2126 * color.r + 0.7152 * color.b + 0.0722 * color.b);
		}

		public static float Scale(this float value, float min, float max, float minScale, float maxScale)
		{
			return (float)(minScale + (double)(value - min) / (max - min) * (maxScale - minScale));
		}

		public static int Scale(this int value, int min, int max, int minScale, int maxScale)
		{
			return Mathf.RoundToInt((float)(minScale + (double)(value - min) / (max - min) * (maxScale - minScale)));
		}

		public static Color ToColor(this int hexVal)
		{
			byte r = (byte)((hexVal >> 16) & 0xFF);
			byte g = (byte)((hexVal >> 8) & 0xFF);
			byte b = (byte)((hexVal) & 0xFF);
			return new Color(r, g, b, 255);
		}

		public static List<T> Splice<T>(this List<T> list, int index, int count)
		{
			List<T> range = list.GetRange(index, count);
			list.RemoveRange(index, count);
			return range;
		}

		public static string[] SplitAndKeepPunctuations(this string phrase)
		{
			string[] punctuations = { "...", ".", ",", "!", "?" };

			phrase = phrase.Replace(" ", "|");
			foreach (string c in punctuations)
			{
				phrase = phrase.Replace(c, "|" + c + "|");
			}

			return phrase.Split(new[] { "|" }, StringSplitOptions.RemoveEmptyEntries);
		}
	}

	public static class ThreadSafeRandom
	{
		[ThreadStatic] private static Random _local;

		public static Random ThisThreadsRandom
		{
			get { return _local ?? (_local = new Random(unchecked(Environment.TickCount * 31 + Thread.CurrentThread.ManagedThreadId))); }
		}
	}
}
