// Copyright (c) 2011, Yves Goergen, http://unclassified.software/source/maidenheadlocator
// Updated 2020 by Tom Fanning and released as a Nuget package
//
// Copying and distribution of this file, with or without modification, are permitted provided the
// copyright notice and this notice are preserved. This file is offered as-is, without any warranty.

// This class is based on a Perl module by Dirk Koopman, G1TLH, from 2002-11-07.
// Source: http://www.koders.com/perl/fidDAB6FD208AC4F5C0306CA344485FD0899BD2F328.aspx

using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace MaidenheadLib
{
	/// <summary>
	/// Class providing static methods for calculating with Maidenhead locators, especially
	/// distance and bearing.
	/// </summary>
	public static class MaidenheadLocator
	{
		/// <summary>
		/// Convert a locator to latitude and longitude in degrees
		/// </summary>
		/// <param name="locator">Locator string to convert</param>
		public static (double lat, double lon) LocatorToLatLng(string locator)
		{
			locator = locator.Trim().ToUpper();
			if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}$"))
			{
				var lon = (locator[0] - 'A') * 20 + (locator[2] - '0' + 0.5) * 2 - 180;
				var lat = (locator[1] - 'A') * 10 + (locator[3] - '0' + 0.5) - 90;
				return (lat, lon);
			}
			else if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}$"))
			{
				var lon = (locator[0] - 'A') * 20 + (locator[2] - '0') * 2 + (locator[4] - 'A' + 0.5) / 12 - 180;
				var lat = (locator[1] - 'A') * 10 + (locator[3] - '0') + (locator[5] - 'A' + 0.5) / 24 - 90;
				return (lat, lon);
			}
			else if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}$"))
			{
				var lon = (locator[0] - 'A') * 20 + (locator[2] - '0') * 2 + (locator[4] - 'A' + 0.0) / 12 + (locator[6] - '0' + 0.5) / 120 - 180;
				var lat = (locator[1] - 'A') * 10 + (locator[3] - '0') + (locator[5] - 'A' + 0.0) / 24 + (locator[7] - '0' + 0.5) / 240 - 90;
				return (lat, lon);
			}
			else if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}[A-X]{2}$"))
			{
				var lon = (locator[0] - 'A') * 20 + (locator[2] - '0') * 2 + (locator[4] - 'A' + 0.0) / 12 + (locator[6] - '0' + 0.0) / 120 + (locator[8] - 'A' + 0.5) / 120 / 24 - 180;
				var lat = (locator[1] - 'A') * 10 + (locator[3] - '0') + (locator[5] - 'A' + 0.0) / 24 + (locator[7] - '0' + 0.0) / 240 + (locator[9] - 'A' + 0.5) / 240 / 24 - 90;
				return (lat, lon);
			}
			else
			{
				throw new FormatException("Invalid locator format");
			}
		}

		/// <summary>
		/// Convert latitude and longitude in degrees to a locator
		/// </summary>
		/// <param name="Lat">Latitude to convert</param>
		/// <param name="Long">Longitude to convert</param>
		/// <returns>Locator string</returns>
		public static string LatLngToLocator(double Lat, double Long)
		{
			return LatLngToLocator(Lat, Long, 0);
		}

		/// <summary>
		/// Convert latitude and longitude in degrees to a locator
		/// </summary>
		/// <param name="Lat">Latitude to convert</param>
		/// <param name="Long">Longitude to convert</param>
		/// <param name="Ext">Extra precision (0, 1, 2)</param>
		/// <returns>Locator string</returns>
		public static string LatLngToLocator(double Lat, double Long, int Ext)
		{
			string locator = "";

			Lat += 90;
			Long += 180;

			locator += (char)('A' + Math.Floor(Long / 20));
			locator += (char)('A' + Math.Floor(Lat / 10));
			Long = Math.IEEERemainder(Long, 20);
			if (Long < 0) Long += 20;
			Lat = Math.IEEERemainder(Lat, 10);
			if (Lat < 0) Lat += 10;

			locator += (char)('0' + Math.Floor(Long / 2));
			locator += (char)('0' + Math.Floor(Lat / 1));
			Long = Math.IEEERemainder(Long, 2);
			if (Long < 0) Long += 2;
			Lat = Math.IEEERemainder(Lat, 1);
			if (Lat < 0) Lat += 1;

			locator += (char)('a' + Math.Floor(Long * 12));
			locator += (char)('a' + Math.Floor(Lat * 24));
			Long = Math.IEEERemainder(Long, (double)1 / 12);
			if (Long < 0) Long += (double)1 / 12;
			Lat = Math.IEEERemainder(Lat, (double)1 / 24);
			if (Lat < 0) Lat += (double)1 / 24;

			if (Ext >= 1)
			{
				locator += (char)('0' + Math.Floor(Long * 120));
				locator += (char)('0' + Math.Floor(Lat * 240));
				Long = Math.IEEERemainder(Long, (double)1 / 120);
				if (Long < 0) Long += (double)1 / 120;
				Lat = Math.IEEERemainder(Lat, (double)1 / 240);
				if (Lat < 0) Lat += (double)1 / 240;
			}

			if (Ext >= 2)
			{
				locator += (char)('a' + Math.Floor(Long * 120 * 24));
				locator += (char)('a' + Math.Floor(Lat * 240 * 24));
				Long = Math.IEEERemainder(Long, (double)1 / 120 / 24);
				if (Long < 0) Long += (double)1 / 120 / 24;
				Lat = Math.IEEERemainder(Lat, (double)1 / 240 / 24);
				if (Lat < 0) Lat += (double)1 / 240 / 24;
			}

			return locator;

			//Lat += 90;
			//Long += 180;
			//v = (int) (Long / 20);
			//Long -= v * 20;
			//locator += (char) ('A' + v);
			//v = (int) (Lat / 10);
			//Lat -= v * 10;
			//locator += (char) ('A' + v);
			//locator += ((int) (Long / 2)).ToString();
			//locator += ((int) Lat).ToString();
			//Long -= (int) (Long / 2) * 2;
			//Lat -= (int) Lat;
			//locator += (char) ('A' + Long * 12);
			//locator += (char) ('A' + Lat * 24);
			//return locator;
		}

		/// <summary>
		/// Convert radians to degrees
		/// </summary>
		/// <param name="rad"></param>
		/// <returns></returns>
		public static double RadToDeg(double rad)
		{
			return rad / Math.PI * 180;
		}

		/// <summary>
		/// Convert degrees to radians
		/// </summary>
		/// <param name="deg"></param>
		/// <returns></returns>
		public static double DegToRad(double deg)
		{
			return deg / 180 * Math.PI;
		}

		/// <summary>
		/// Calculate the distance in km between two locators
		/// </summary>
		/// <param name="A">Start locator string</param>
		/// <param name="B">End locator string</param>
		/// <returns>Distance in km</returns>
		public static double Distance(string A, string B)
		{
			return Distance(LocatorToLatLng(A), LocatorToLatLng(B));
		}

		/// <summary>
		/// Calculate the distance in km between two locators
		/// </summary>
		/// <param name="A">Start LatLng structure</param>
		/// <param name="B">End LatLng structure</param>
		/// <returns>Distance in km</returns>
		public static double Distance((double Lat, double Long) A, (double Lat, double Long) B)
		{
			if (A.CompareTo(B) == 0) return 0;

			double hn = DegToRad(A.Lat);
			double he = DegToRad(A.Long);
			double n = DegToRad(B.Lat);
			double e = DegToRad(B.Long);

			double co = Math.Cos(he - e) * Math.Cos(hn) * Math.Cos(n) + Math.Sin(hn) * Math.Sin(n);
			double ca = Math.Atan(Math.Abs(Math.Sqrt(1 - co * co) / co));
			if (co < 0) ca = Math.PI - ca;
			double dx = 6367 * ca;

			return dx;
		}

		/// <summary>
		/// Calculate the azimuth in degrees between two locators
		/// </summary>
		/// <param name="A">Start locator string</param>
		/// <param name="B">End locator string</param>
		/// <returns>Azimuth in degrees</returns>
		public static double Azimuth(string A, string B)
		{
			return Azimuth(LocatorToLatLng(A), LocatorToLatLng(B));
		}

		/// <summary>
		/// Calculate the azimuth in degrees between two locators
		/// </summary>
		/// <param name="A">Start LatLng structure</param>
		/// <param name="B">End LatLng structure</param>
		/// <returns>Azimuth in degrees</returns>
		public static double Azimuth((double Lat, double Long) A, (double Lat, double Long) B)
		{
			if (A.CompareTo(B) == 0) return 0;

			double hn = DegToRad(A.Lat);
			double he = DegToRad(A.Long);
			double n = DegToRad(B.Lat);
			double e = DegToRad(B.Long);

			double co = Math.Cos(he - e) * Math.Cos(hn) * Math.Cos(n) + Math.Sin(hn) * Math.Sin(n);
			double ca = Math.Atan(Math.Abs(Math.Sqrt(1 - co * co) / co));
			if (co < 0) ca = Math.PI - ca;

			double si = Math.Sin(e - he) * Math.Cos(n) * Math.Cos(hn);
			co = Math.Sin(n) - Math.Sin(hn) * Math.Cos(ca);
			double az = Math.Atan(Math.Abs(si / co));
			if (co < 0) az = Math.PI - az;
			if (si < 0) az = -az;
			if (az < 0) az = az + 2 * Math.PI;

			return RadToDeg(az);
		}
		
		private static readonly char[] Base18Ar = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R' };
		private static readonly char[] Base24Ax = { 'A', 'B', 'C', 'D', 'E', 'F', 'G', 'H', 'I', 'J', 'K', 'L', 'M', 'N', 'O', 'P', 'Q', 'R', 'S', 'T', 'U', 'V', 'W', 'X' };

		private static int GetLastLocatorPart(string locator)
		{
			int? lastPart = null;

			if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}$")) lastPart = 2;

			if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}$")) lastPart = 3;

			if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}$")) lastPart = 4;

			if (Regex.IsMatch(locator, "^[A-R]{2}[0-9]{2}[A-X]{2}[0-9]{2}[A-X]{2}$")) lastPart = 5;
			
			if (lastPart is null) throw new FormatException("Invalid locator format");

			return lastPart.Value;
		}
		
		public static string LocatorShiftUp(string locator)
		{
			locator = locator.Trim().ToUpper();

			int lastPart = GetLastLocatorPart(locator);
			
			char p1Long = locator[0]; //A-R
			char p1Lat = locator[1]; //A-R
			char p2Long = locator[2]; //0-9
			char p2Lat = locator[3]; //0-9
			char? p3Long = locator.Length >= 5 ? locator[4] : (char?)null; //A-X
			char? p3Lat = locator.Length >= 5 ? locator[5] : (char?)null; //A-X
			char? p4Long = locator.Length >= 7 ? locator[6] : (char?)null; //0-9
			char? p4Lat =  locator.Length >= 7 ? locator[7] : (char?)null; //0-9
			char? p5Long = locator.Length >= 9 ? locator[8] : (char?)null; //A-X
			char? p5Lat = locator.Length >= 9 ? locator[9] : (char?)null; //A-X
			
			//Check P5
			bool p5LatCarry = false;
			
			if (lastPart == 5)
			{
				//P5 increment
				int p5LatInt = Array.IndexOf(Base24Ax, p5Lat) + 1;
					
				try
				{
					p5Lat = Base24Ax[p5LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p5Lat = Base24Ax[0];
					p5LatCarry = true;
				}
			}
			
			//Check P4
			bool p4LatCarry = false;
			
			if (lastPart == 4 || (lastPart > 4 && p5LatCarry))
			{
				//P4 increment
				int p4LatInt = int.Parse(p4Lat.ToString()) + 1;

				if (p4LatInt > 9)
				{
					p4Lat = '0';
					p4LatCarry = true;
				}
				else
				{
					p4Lat = p4LatInt.ToString()[0];
				}
			}
			
			//Check P3
			bool p3LatCarry = false;

			if (lastPart == 3 || (lastPart > 3 && p4LatCarry))
			{
				//P3 increment
				int p3LatInt = Array.IndexOf(Base24Ax, p3Lat) + 1;
					
				try
				{
					p3Lat = Base24Ax[p3LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p3Lat = Base24Ax[0];
					p3LatCarry = true;
				}
			}
			
			//Check P2
			bool p2LatCarry = false;

			if (lastPart == 2 || (lastPart > 2 && p3LatCarry))
			{
				//P2 increment
				int p2LatInt = int.Parse(p2Lat.ToString()) + 1;

				if (p2LatInt > 9)
				{
					p2Lat = '0';
					p2LatCarry = true;
				}
				else
				{
					p2Lat = p2LatInt.ToString()[0];
				}
			}
				
			//P1 carry from P2
			if (p2LatCarry)
			{
				int p1LatInt = Array.IndexOf(Base18Ar, p1Lat) + 1;
					
				try
				{
					p1Lat = Base18Ar[p1LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p1Lat = Base18Ar[0];
				}
			}
			
			//Concatenate result
			return $"{p1Long}{p1Lat}{p2Long}{p2Lat}{p3Long}{p3Lat}{p4Long}{p4Lat}{p5Long}{p5Lat}";
		}

		public static string LocatorShiftDown(string locator)
		{
			locator = locator.Trim().ToUpper();

			int lastPart = GetLastLocatorPart(locator);
			
			char p1Long = locator[0]; //A-R
			char p1Lat = locator[1]; //A-R
			char p2Long = locator[2]; //0-9
			char p2Lat = locator[3]; //0-9
			char? p3Long = locator.Length >= 5 ? locator[4] : (char?)null; //A-X
			char? p3Lat = locator.Length >= 5 ? locator[5] : (char?)null; //A-X
			char? p4Long = locator.Length >= 7 ? locator[6] : (char?)null; //0-9
			char? p4Lat =  locator.Length >= 7 ? locator[7] : (char?)null; //0-9
			char? p5Long = locator.Length >= 9 ? locator[8] : (char?)null; //A-X
			char? p5Lat = locator.Length >= 9 ? locator[9] : (char?)null; //A-X
			
			//Check P5
			bool p5LatCarry = false;
			
			if (lastPart == 5)
			{
				//P5 decrement
				int p5LatInt = Array.IndexOf(Base24Ax, p5Lat) - 1;
					
				try
				{
					p5Lat = Base24Ax[p5LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p5Lat = Base24Ax[23];
					p5LatCarry = true;
				}
			}
			
			//Check P4
			bool p4LatCarry = false;
			
			if (lastPart == 4 || (lastPart > 4 && p5LatCarry))
			{
				//P4 decrement
				int p4LatInt = int.Parse(p4Lat.ToString()) - 1;

				if (p4LatInt < 0)
				{
					p4Lat = '9';
					p4LatCarry = true;
				}
				else
				{
					p4Lat = p4LatInt.ToString()[0];
				}
			}
			
			//Check P3
			bool p3LatCarry = false;

			if (lastPart == 3 || (lastPart > 3 && p4LatCarry))
			{
				//P3 decrement
				int p3LatInt = Array.IndexOf(Base24Ax, p3Lat) - 1;
					
				try
				{
					p3Lat = Base24Ax[p3LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p3Lat = Base24Ax[23];
					p3LatCarry = true;
				}
			}
			
			//Check P2
			bool p2LatCarry = false;

			if (lastPart == 2 || (lastPart > 2 && p3LatCarry))
			{
				//P2 increment
				int p2LatInt = int.Parse(p2Lat.ToString()) - 1;

				if (p2LatInt < 0)
				{
					p2Lat = '9';
					p2LatCarry = true;
				}
				else
				{
					p2Lat = p2LatInt.ToString()[0];
				}
			}
				
			//P1 carry from P2
			if (p2LatCarry)
			{
				int p1LatInt = Array.IndexOf(Base18Ar, p1Lat) - 1;
					
				try
				{
					p1Lat = Base18Ar[p1LatInt];
				}
				catch (IndexOutOfRangeException)
				{
					p1Lat = Base18Ar[17];
				}
			}
			
			//Concatenate result
			return $"{p1Long}{p1Lat}{p2Long}{p2Lat}{p3Long}{p3Lat}{p4Long}{p4Lat}{p5Long}{p5Lat}";
		}

		public static string LocatorShiftLeft(string locator)
		{
			locator = locator.Trim().ToUpper();

			int lastPart = GetLastLocatorPart(locator);
			
			char p1Long = locator[0]; //A-R
			char p1Lat = locator[1]; //A-R
			char p2Long = locator[2]; //0-9
			char p2Lat = locator[3]; //0-9
			char? p3Long = locator.Length >= 5 ? locator[4] : (char?)null; //A-X
			char? p3Lat = locator.Length >= 5 ? locator[5] : (char?)null; //A-X
			char? p4Long = locator.Length >= 7 ? locator[6] : (char?)null; //0-9
			char? p4Lat =  locator.Length >= 7 ? locator[7] : (char?)null; //0-9
			char? p5Long = locator.Length >= 9 ? locator[8] : (char?)null; //A-X
			char? p5Lat = locator.Length >= 9 ? locator[9] : (char?)null; //A-X
			
			//Check P5
			bool p5LongCarry = false;
			
			if (lastPart == 5)
			{
				//P5 increment
				int p5LongInt = Array.IndexOf(Base24Ax, p5Long) - 1;
					
				try
				{
					p5Long = Base24Ax[p5LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p5Long = Base24Ax[23];
					p5LongCarry = true;
				}
			}
			
			//Check P4
			bool p4LongCarry = false;
			
			if (lastPart == 4 || (lastPart > 4 && p5LongCarry))
			{
				//P4 increment
				int p4LongInt = int.Parse(p4Long.ToString()) - 1;

				if (p4LongInt < 0)
				{
					p4Long = '9';
					p4LongCarry = true;
				}
				else
				{
					p4Long = p4LongInt.ToString()[0];
				}
			}
			
			//Check P3
			bool p3LongCarry = false;

			if (lastPart == 3 || (lastPart > 3 && p4LongCarry))
			{
				//P3 increment
				int p3LongInt = Array.IndexOf(Base24Ax, p3Long) - 1;
					
				try
				{
					p3Long = Base24Ax[p3LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p3Long = Base24Ax[23];
					p3LongCarry = true;
				}
			}
			
			//Check P2
			bool p2LongCarry = false;

			if (lastPart == 2 || (lastPart > 2 && p3LongCarry))
			{
				//P2 increment
				int p2LongInt = int.Parse(p2Long.ToString()) - 1;

				if (p2LongInt < 0)
				{
					p2Long = '9';
					p2LongCarry = true;
				}
				else
				{
					p2Long = p2LongInt.ToString()[0];
				}
			}
				
			//P1 carry from P2
			if (p2LongCarry)
			{
				int p1LongInt = Array.IndexOf(Base18Ar, p1Long) - 1;
					
				try
				{
					p1Long = Base18Ar[p1LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p1Long = Base18Ar[17];
				}
			}
			
			//Concatenate result
			return $"{p1Long}{p1Lat}{p2Long}{p2Lat}{p3Long}{p3Lat}{p4Long}{p4Lat}{p5Long}{p5Lat}";
		}

		public static string LocatorShiftRight(string locator)
		{
			locator = locator.Trim().ToUpper();

			int lastPart = GetLastLocatorPart(locator);
			
			char p1Long = locator[0]; //A-R
			char p1Lat = locator[1]; //A-R
			char p2Long = locator[2]; //0-9
			char p2Lat = locator[3]; //0-9
			char? p3Long = locator.Length >= 5 ? locator[4] : (char?)null; //A-X
			char? p3Lat = locator.Length >= 5 ? locator[5] : (char?)null; //A-X
			char? p4Long = locator.Length >= 7 ? locator[6] : (char?)null; //0-9
			char? p4Lat =  locator.Length >= 7 ? locator[7] : (char?)null; //0-9
			char? p5Long = locator.Length >= 9 ? locator[8] : (char?)null; //A-X
			char? p5Lat = locator.Length >= 9 ? locator[9] : (char?)null; //A-X
			
			//Check P5
			bool p5LongCarry = false;
			
			if (lastPart == 5)
			{
				//P5 increment
				int p5LongInt = Array.IndexOf(Base24Ax, p5Long) + 1;
					
				try
				{
					p5Long = Base24Ax[p5LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p5Long = Base24Ax[0];
					p5LongCarry = true;
				}
			}
			
			//Check P4
			bool p4LongCarry = false;
			
			if (lastPart == 4 || (lastPart > 4 && p5LongCarry))
			{
				//P4 increment
				int p4LongInt = int.Parse(p4Long.ToString()) + 1;

				if (p4LongInt > 9)
				{
					p4Long = '0';
					p4LongCarry = true;
				}
				else
				{
					p4Long = p4LongInt.ToString()[0];
				}
			}
			
			//Check P3
			bool p3LongCarry = false;

			if (lastPart == 3 || (lastPart > 3 && p4LongCarry))
			{
				//P3 increment
				int p3LongInt = Array.IndexOf(Base24Ax, p3Long) + 1;
					
				try
				{
					p3Long = Base24Ax[p3LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p3Long = Base24Ax[0];
					p3LongCarry = true;
				}
			}
			
			//Check P2
			bool p2LongCarry = false;

			if (lastPart == 2 || (lastPart > 2 && p3LongCarry))
			{
				//P2 increment
				int p2LongInt = int.Parse(p2Long.ToString()) + 1;

				if (p2LongInt > 9)
				{
					p2Long = '0';
					p2LongCarry = true;
				}
				else
				{
					p2Long = p2LongInt.ToString()[0];
				}
			}
				
			//P1 carry from P2
			if (p2LongCarry)
			{
				int p1LongInt = Array.IndexOf(Base18Ar, p1Long) + 1;
					
				try
				{
					p1Long = Base18Ar[p1LongInt];
				}
				catch (IndexOutOfRangeException)
				{
					p1Long = Base18Ar[0];
				}
			}
			
			//Concatenate result
			return $"{p1Long}{p1Lat}{p2Long}{p2Lat}{p3Long}{p3Lat}{p4Long}{p4Lat}{p5Long}{p5Lat}";
		}

		public static double[][] LocatorToPolygonPoints(string locator)
		{
			List<double[]> points = new List<double[]>();
			
			//Bottom Left
			(double lat, double lon) currentPoint = LocatorToLatLng(locator);
			points.Add(new []{ currentPoint.lat, currentPoint.lon });

			//Top Left
			currentPoint = LocatorToLatLng(LocatorShiftUp(locator));
			points.Add(new []{ currentPoint.lat, currentPoint.lon });
			
			//Top Right
			currentPoint = LocatorToLatLng(LocatorShiftLeft(LocatorShiftUp(locator)));
			points.Add(new []{ currentPoint.lat, currentPoint.lon });

			//Bottom Right
			currentPoint = LocatorToLatLng(LocatorShiftLeft(locator));
			points.Add(new []{ currentPoint.lat, currentPoint.lon });

			return points.ToArray();
		}
	}
}