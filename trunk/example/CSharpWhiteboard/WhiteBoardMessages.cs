// WhiteBoardMessages.cs
//
// Copyright (C) Ranjeet Chakraborty, July 2002, ranjeetc@hotmail.com
//
// Contains the WhiteBoard Message classes, WhiteBoard message 
// encoder/decoder classes which is the heart of the entire messaging
// infrastructure of the application. 

using System;
using System.Drawing;
using System.IO;
using System.Runtime.Serialization.Formatters;
using System.Runtime.Serialization.Formatters.Soap;
using System.Runtime.Serialization.Formatters.Binary;

namespace WhiteBoard
{
	/// <summary>
	/// Summary description for WhiteBoardMessages.
	/// </summary>
	
	[Serializable]
	public abstract class WBMessage
	{
		public enum WHITEBOARD_MESSAGE_TYPE 
		{ 
			enWBBegin,
			enWBLine, 
			enWBRectangle, 
			enWBEllipse,
			enWBClearScreen,
			enWBFill, 
			enWBEnd
		};

		public abstract WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get;
		}
		
		public WBMessage()
		{
		}
	}

	[Serializable]
	public class WBMsgDrawBegin :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBBegin;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
		public Point m_PtBegin;
		public bool	 m_bMouseDown;	
	}
	
	[Serializable]
	public class WBMsgDrawEnd :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBEnd;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
		public Point m_PtEnd;
	}

	[Serializable]
	public class WBMsgDrawLine :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBLine;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
		public Point m_Pt;
	}

	[Serializable]
	public class WBMsgDrawRectangle :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBRectangle;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
		public Point m_Pt;
	}

	[Serializable]
	public class WBMsgDrawEllipse :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBEllipse;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
		public Point m_Pt;
	}

	[Serializable]
	public class WBMsgClearScreen :  WBMessage 
	{
		//Implement the abstract property 
		private const WHITEBOARD_MESSAGE_TYPE m_enMsgType = WHITEBOARD_MESSAGE_TYPE.enWBClearScreen;
		public override WHITEBOARD_MESSAGE_TYPE MessageType
		{
			get
			{
				return m_enMsgType;
			}
		}
	}


	public class WBMessageHelper
	{
		public static MemoryStream Serialize(WBMessage msg)
		{
			MemoryStream ms				= new MemoryStream(); 
			BinaryFormatter formatter	= new BinaryFormatter();
			formatter.AssemblyFormat	= FormatterAssemblyStyle.Simple;
			formatter.TypeFormat		= FormatterTypeStyle.TypesWhenNeeded;
			switch(msg.MessageType)
			{
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBBegin:
					formatter.Serialize(ms, (WBMsgDrawBegin)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEnd:
					formatter.Serialize(ms, (WBMsgDrawEnd)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBLine:
					formatter.Serialize(ms, (WBMsgDrawLine)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBRectangle:
					formatter.Serialize (ms, (WBMsgDrawRectangle)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEllipse:
					formatter.Serialize (ms, (WBMsgDrawEllipse)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBClearScreen:
					formatter.Serialize(ms, (WBMsgClearScreen)msg);
					break;
			}
			return ms;
		}

		public static MemoryStream Serialize(long lObject)
		{
			MemoryStream ms			= new MemoryStream(); 
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;
			formatter.Serialize(ms, lObject);
			return ms;
		}

		public static WBMessage Deserialize(byte [] bytes, int offset, int count)
		{
			MemoryStream ms = new MemoryStream(bytes, offset, count);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;
			WBMessage msg;
			msg = (WBMessage)formatter.Deserialize(ms);
			return msg;
		}
		public static long DeserializeL(byte [] bytes, int offset, int count)
		{
			long lOriginal;
			MemoryStream ms = new MemoryStream(bytes, offset, count);
			BinaryFormatter formatter = new BinaryFormatter();
			formatter.AssemblyFormat = FormatterAssemblyStyle.Simple;
			formatter.TypeFormat = FormatterTypeStyle.TypesWhenNeeded;
			lOriginal = (long)formatter.Deserialize(ms);
			ms.Close();
			return lOriginal;
		}

		public static string SoapSerialize(WBMessage msg)
		{
			MemoryStream ms			= new MemoryStream(); 
			SoapFormatter formatter = new SoapFormatter();
			switch(msg.MessageType)
			{
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBBegin:
					formatter.Serialize(ms, (WBMsgDrawBegin)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEnd:
					formatter.Serialize(ms, (WBMsgDrawEnd)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBLine:
					formatter.Serialize(ms, (WBMsgDrawLine)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBRectangle:
					formatter.Serialize(ms, (WBMsgDrawRectangle)msg);
					break;
				case WBMessage.WHITEBOARD_MESSAGE_TYPE.enWBEllipse:
					formatter.Serialize(ms, (WBMsgDrawEllipse)msg);
					break;

			}
			string StrXML = System.Text.Encoding.ASCII.GetString(ms.GetBuffer(),0, (int)ms.Length);
			ms.Close();
			return StrXML;
		}
	}

}
