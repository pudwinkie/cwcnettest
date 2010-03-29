using System;
using System.Threading;
using System.Collections;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.Drawing.Drawing2D;
using System.Data;
using System.Windows.Forms;

namespace WhiteBoard
{
	/// <summary>
	/// Summary description for DrawAreaCtrl.
	/// </summary>
	public class DrawAreaCtrl : System.Windows.Forms.Control
	{
		public enum WHITEBOARD_DRAW_MODE { enModeLine, enModeRect, enModeEllipse, enModeFill, enWBNone};

		//private System.Windows.Forms.Label label1;
		//Holds the last pair of lines that make a line
		private Point					m_ptLineStart, m_ptLineEnd, m_ptLinePrev;
		//Flag to check on the mouse down status
		private bool					m_bMouseDown;
		

		//Holds the path 
		//private GraphicsPath			m_gPath;
		//The shadow bitmap that holds the entire drawing
		private Bitmap					m_bmpSaved;
		public	Bitmap					BitmapCanvas 
		{
			get 
			{
				return m_bmpSaved;
			}
		}

		private Graphics				m_grfxBm;
		private Color					m_ColorBack;
		//Stores the current Draw Mode
		public WHITEBOARD_DRAW_MODE		m_enDrawMode;

		public NetworkManager			m_NetMgr;
	
		public int						m_iNoOfUsrs;

		public DrawAreaCtrl()
		{
			m_enDrawMode = WHITEBOARD_DRAW_MODE.enWBNone;
			
			m_ColorBack	= Color.White  ; 
			this.BackColor =  m_ColorBack;
		
			m_ptLineStart = m_ptLineEnd = m_ptLinePrev = new Point(0,0);
			m_bMouseDown = false;

			//m_gPath = new GraphicsPath();
			
			Size sz = SystemInformation.PrimaryMonitorMaximizedWindowSize;
			m_bmpSaved = new Bitmap (sz.Width, sz.Height);
			m_grfxBm = Graphics.FromImage(m_bmpSaved);
			m_grfxBm.Clear(BackColor);

			m_iNoOfUsrs = 0;

			UnlockWhiteBoardMouseEvents();

		}

		private void InitializeComponent()
		{
			// 
			// DrawAreaCtrl
			// 
			this.BackColor = System.Drawing.SystemColors.Info;

		}

		protected override void OnPaint(PaintEventArgs e)
		{
			Graphics g = e.Graphics;
			g.DrawImage(m_bmpSaved, 0,0,m_bmpSaved.Width, m_bmpSaved.Height);

		}

		//All these functions execute on the UI thread and some of them would be called
		//by NetworkManager which would be executing from a thread from the ThreadPool
		//(Asynchronous IO ?)
		/*protected override void OnMouseDown(MouseEventArgs mea)
		{
			base.OnMouseDown(mea);
			if(m_iNoOfUsrs == 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseDown(pt);
				WhenMouseDownSentToPeers(pt);
			}
		}*/
		
		protected void OnMouseDownHdlr(object sender, MouseEventArgs mea)
		{
			if(m_iNoOfUsrs <= 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseDown(pt);
				WhenMouseDownSentToPeers(pt);
			}
		}
		public void WhenMouseDown(Point pt)
		{
			//If its not already mousedown (Other user is using the whiteboard)
			switch (m_enDrawMode)
			{
				case WHITEBOARD_DRAW_MODE.enModeLine:
					m_ptLineStart.X  = m_ptLinePrev.X = pt.X;
					m_ptLineStart.Y  = m_ptLinePrev.Y = pt.Y;
					m_bMouseDown = true;
					//Check if connected to another peer
					break;
				case WHITEBOARD_DRAW_MODE.enModeRect:
					m_ptLineStart.X = pt.X;
					m_ptLineStart.Y = pt.Y;
					m_bMouseDown = true;
					break;
				case WHITEBOARD_DRAW_MODE.enModeEllipse:
					m_ptLineStart.X = pt.X;
					m_ptLineStart.Y = pt.Y;
					m_bMouseDown = true;
					break;
				default:
					break;
			}
		}
		public void WhenMouseDownSentToPeers(Point pt)
		{
			if(m_NetMgr != null)
			{
				switch (m_enDrawMode)
				{
					case WHITEBOARD_DRAW_MODE.enModeLine:
						WBMsgDrawBegin MsgLine = new WBMsgDrawBegin();
						MsgLine.m_bMouseDown = true;
						MsgLine.m_PtBegin = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgLine);
						break;
					case WHITEBOARD_DRAW_MODE.enModeRect:
						WBMsgDrawBegin MsgRect = new WBMsgDrawBegin();
						MsgRect.m_bMouseDown = true;
						MsgRect.m_PtBegin = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgRect);
						break;
					case WHITEBOARD_DRAW_MODE.enModeEllipse:
						WBMsgDrawBegin MsgEllipse = new WBMsgDrawBegin();
						MsgEllipse.m_bMouseDown = true;
						MsgEllipse.m_PtBegin = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgEllipse);
						break;
					default:
						break;
				}
			}
		}

		/*protected override void OnMouseMove(MouseEventArgs mea)
		{
			base.OnMouseMove(mea);
			if(m_iNoOfUsrs == 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseMove(pt);
				WhenMouseMoveSentToPeers(pt);
			}
		}*/
		
		protected void OnMouseMoveHdlr(object sender, MouseEventArgs mea)
		{
			if(m_iNoOfUsrs <= 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseMove(pt);
				WhenMouseMoveSentToPeers(pt);
			}
		}

        					
		public void WhenMouseMove(Point pt)
		{
			switch (m_enDrawMode)
			{
				case WHITEBOARD_DRAW_MODE.enModeLine:
					m_ptLineEnd.X = pt.X;
					m_ptLineEnd.Y = pt.Y;
					if	(m_bMouseDown)
					{
						Pen pn = new Pen(Color.Black, 10);
						Graphics grfx = CreateGraphics();
						grfx.DrawLine(pn, m_ptLinePrev, m_ptLineEnd);
						grfx.Dispose();
						//Draw on the bitmap
						m_grfxBm.DrawLine(pn, m_ptLinePrev, m_ptLineEnd);
					}
					m_ptLinePrev = m_ptLineEnd;
					break;
				case WHITEBOARD_DRAW_MODE.enModeRect:
					m_ptLineEnd.X = pt.X;
					m_ptLineEnd.Y = pt.Y;
					if	(m_bMouseDown)
					{ 
						Pen pn = new Pen(Color.Black, 1);
						Pen pnFore = new Pen(m_ColorBack , 1); 
						Graphics grfx = CreateGraphics();
					
						//Erases the previous rectangle 
						grfx.DrawRectangle(pnFore, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLinePrev.X - (float)m_ptLineStart.X, 
							(float)m_ptLinePrev.Y - (float)m_ptLineStart.Y);

						//Draws the new rectangle
						grfx.DrawRectangle(pn, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLineEnd.X - (float)m_ptLineStart.X, 
							(float)m_ptLineEnd.Y - (float)m_ptLineStart.Y);
						//Math.Abs
						grfx.Dispose();
						//Erase the previous rectangle on the bitmap
						m_grfxBm.DrawRectangle(pnFore, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLinePrev.X - (float)m_ptLineStart.X, 
							(float)m_ptLinePrev.Y - (float)m_ptLineStart.Y);
						//Draw the new rectangle on the bitmap
						m_grfxBm.DrawRectangle(pn, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLineEnd.X - (float)m_ptLineStart.X, 
							(float)m_ptLineEnd.Y - (float)m_ptLineStart.Y);
					}
					m_ptLinePrev = m_ptLineEnd;
					break;
				case WHITEBOARD_DRAW_MODE.enModeEllipse:
					m_ptLineEnd.X = pt.X;
					m_ptLineEnd.Y = pt.Y;
					if	(m_bMouseDown)
					{
						Pen pn = new Pen(Color.Black, 1);
						Pen pnFore = new Pen(m_ColorBack , 1); 
						Graphics grfx = CreateGraphics();
					
						//Erases the previous Ellipse 
						grfx.DrawEllipse(pnFore, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLinePrev.X - (float)m_ptLineStart.X, 
							(float)m_ptLinePrev.Y - (float)m_ptLineStart.Y);
						//Draw the new Ellipse
						grfx.DrawEllipse(pn, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLineEnd.X - (float)m_ptLineStart.X, 
							(float)m_ptLineEnd.Y - (float)m_ptLineStart.Y);
						//Math.Abs
						grfx.Dispose();
						//Erase the previous Ellipse on the bitmap
						m_grfxBm.DrawEllipse(pnFore, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLinePrev.X - (float)m_ptLineStart.X, 
							(float)m_ptLinePrev.Y - (float)m_ptLineStart.Y);
						//Draw the new Ellipse on the bitmap
						m_grfxBm.DrawEllipse(pn, 
							(float)m_ptLineStart.X,
							(float)m_ptLineStart.Y, 
							(float)m_ptLineEnd.X - (float)m_ptLineStart.X, 
							(float)m_ptLineEnd.Y - (float)m_ptLineStart.Y);
					}
					m_ptLinePrev = m_ptLineEnd;
					break;

				default:
					break;
			}

		}
		public void WhenMouseMoveSentToPeers(Point pt)
		{
			if(m_NetMgr != null)
			{
				switch (m_enDrawMode)
				{
					case WHITEBOARD_DRAW_MODE.enModeLine:
						if	(m_bMouseDown)
						{
							WBMsgDrawLine MsgLine = new WBMsgDrawLine();
							MsgLine.m_Pt = pt;
							m_NetMgr.SendWBMsgConnectedHdlr(MsgLine);
						}
						break;
					case WHITEBOARD_DRAW_MODE.enModeRect:
						if	(m_bMouseDown)
						{
							WBMsgDrawRectangle MsgRect = new WBMsgDrawRectangle();
							MsgRect.m_Pt = pt;
							m_NetMgr.SendWBMsgConnectedHdlr(MsgRect);
						}
						break;
					case WHITEBOARD_DRAW_MODE.enModeEllipse:
						if	(m_bMouseDown)
						{
							WBMsgDrawEllipse MsgEllipse = new WBMsgDrawEllipse();
							MsgEllipse.m_Pt = pt;
							m_NetMgr.SendWBMsgConnectedHdlr(MsgEllipse);
						}
						break;
					default:
						break;
				}
			}
		}

		/*protected override void OnMouseUp(MouseEventArgs mea)
		{
			this.MouseUp -= new EventHandler(OnMouseUp);
			base.OnMouseUp(mea);
			if(m_iNoOfUsrs == 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseUp(pt);
				WhenMouseUpSentToPeers(pt);
			}
		}*/

		protected void OnMouseUpHdlr(object sender, MouseEventArgs mea)
		{
			//base.OnMouseUp(mea);
			if(m_iNoOfUsrs <= 0)
			{
				Point pt = new Point(mea.X, mea.Y);
				WhenMouseUp(pt);
				WhenMouseUpSentToPeers(pt);
			}
		}
		public void WhenMouseUp(Point pt)
		{
			switch (m_enDrawMode)
			{
				case WHITEBOARD_DRAW_MODE.enModeLine:
				case WHITEBOARD_DRAW_MODE.enModeRect:
				case WHITEBOARD_DRAW_MODE.enModeEllipse:
					m_bMouseDown = false;
					break;
					/*case WHITEBOARD_DRAW_MODE.enModeRect:
						m_bMouseDown = false;
						break;
					case WHITEBOARD_DRAW_MODE.enModeEllipse:
						m_bMouseDown = false;
						break;*/
				default:
					break;
			}
		}

		public void WhenMouseUpSentToPeers(Point pt)
		{
			if(m_NetMgr != null)
			{
				switch (m_enDrawMode)
				{
					case WHITEBOARD_DRAW_MODE.enModeLine:
						WBMsgDrawEnd MsgLine = new WBMsgDrawEnd();
						MsgLine.m_PtEnd = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgLine);
						break;
					case WHITEBOARD_DRAW_MODE.enModeRect:
						WBMsgDrawEnd MsgRect = new WBMsgDrawEnd();
						MsgRect.m_PtEnd = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgRect);
						break;
					case WHITEBOARD_DRAW_MODE.enModeEllipse:
						WBMsgDrawEnd MsgEllipse = new WBMsgDrawEnd();
						MsgEllipse.m_PtEnd = pt;
						m_NetMgr.SendWBMsgConnectedHdlr(MsgEllipse);
						break;
					default:
						break;
				}
			}
		}

		public void OnClearScreen()
		{
			ClearScreen();
			ClearScreenSentToPeers();
		}

		public void ClearScreen()
		{
			// Create solid brush.
			SolidBrush BkgBrush = new SolidBrush(m_ColorBack);
			// Create location and size of rectangle.
			float x = 0.0F;
			float y = 0.0F;
			float width = this.Width;
			float height = this.Height;
			// Fill rectangle to screen.
			Graphics grfx = CreateGraphics();
			grfx.FillRectangle(BkgBrush, x, y, width, height);
			grfx.Dispose();
			//Draw on the bitmap
			m_grfxBm.FillRectangle(BkgBrush, x, y, width, height);

		}
		protected void ClearScreenSentToPeers()
		{
			if(m_NetMgr != null)
			{
				WBMsgClearScreen MsgClrScreen = new WBMsgClearScreen();
				m_NetMgr.SendWBMsgConnectedHdlr(MsgClrScreen);
			}
		}
		public void LockWhiteBoardMouseEvents()
		{
			lock(this)
			{
				this.MouseDown	-= new MouseEventHandler(this.OnMouseDownHdlr);
				this.MouseMove  -= new MouseEventHandler(this.OnMouseMoveHdlr);
				this.MouseUp	-= new MouseEventHandler(this.OnMouseUpHdlr);
			}
		}

		public void UnlockWhiteBoardMouseEvents()
		{
			lock(this)
			{
				this.MouseDown	+= new MouseEventHandler(this.OnMouseDownHdlr);
				this.MouseMove  += new MouseEventHandler(this.OnMouseMoveHdlr);
				this.MouseUp	+= new MouseEventHandler(this.OnMouseUpHdlr);
			}
		}

	}
}
