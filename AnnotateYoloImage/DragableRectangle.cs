using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace AnnotateYoloImage
{
    public partial class DragableRectangle : Label
    {
        public DragableRectangle()
        {
            InitializeComponent();
            BackColor = Color.Transparent;
            AutoSize = false;
            Text = string.Empty;
            DoubleBuffered = true;
        }

        public void SetParentCtrl(Control parent)
        {
            Parent = parent;
            Parent.MouseUp += Parent_MouseUp;
        }

        private void Parent_MouseUp(object sender, MouseEventArgs e)
        {
            this.OnMouseUp(e);
        }

        public DragableRectangle(IContainer container)
        {
            container.Add(this);

            InitializeComponent();
        }

        private bool _isMouseDown = false;
        private Point _mouseOldPos = new Point(-1, -1);
        /// <summary>
        /// 边界框内对象所属类别Id
        /// </summary>
        public int ClassId { get; internal set; }
        /// <summary>
        /// 是否选中
        /// </summary>
        public bool IsSelected { get; set; }

        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            _isMouseDown = true;
            _mouseOldPos = MouseUtils.GetCursorPos();
            if (Cursor != Cursors.SizeNWSE && Cursor != Cursors.SizeNESW)
                this.Cursor = Cursors.SizeAll;
            var ctrlPos = PointToClient(_mouseOldPos);
            _resizePos = CornerRegionList.FindIndex(c=>c.Contains(ctrlPos));
            Debug.WriteLine($"_resizePos = {_resizePos}");
        }

        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            _isMouseDown = false;
            this.Cursor = Cursors.Default;
        }

        private int _resizePos = -1;

        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            var pos = MouseUtils.GetCursorPos();
            if (_isMouseDown && Cursor == Cursors.SizeAll)
            {

                Left += (pos.X - _mouseOldPos.X);
                Top += (pos.Y - _mouseOldPos.Y);
                _mouseOldPos = MouseUtils.GetCursorPos();
                return;
            }
            else if (_isMouseDown && (Cursor == Cursors.SizeNWSE || Cursor == Cursors.SizeNESW))
            {
                ChangeCtrlSizeAndPos(_mouseOldPos, pos, _resizePos);
                _mouseOldPos = MouseUtils.GetCursorPos();
                return;
            }
            var localPos = PointToClient(pos);
            if (CornerRegionList[1].Contains(localPos) || CornerRegionList[2].Contains(localPos))
            {
                Cursor = Cursors.SizeNESW;
            }
            else if (CornerRegionList[0].Contains(localPos) || CornerRegionList[3].Contains(localPos))
            {
                Cursor = Cursors.SizeNWSE;
            }
            else
            {
                Cursor = Cursors.Default;
            }

        }

        private void ChangeCtrlSizeAndPos(Point mouseOldPos, Point pos, int resizePos)
        {
            switch (resizePos)
            {
                case 0:
                    Left += (pos.X - _mouseOldPos.X);
                    Top += (pos.Y - _mouseOldPos.Y);
                    Width -= (pos.X - _mouseOldPos.X);
                    Height -= (pos.Y - _mouseOldPos.Y);
                    break;
                case 1:
                    //Left -= (pos.X - _mouseOldPos.X);
                    Top += (pos.Y - _mouseOldPos.Y);
                    Width += (pos.X - _mouseOldPos.X);
                    Height -= (pos.Y - _mouseOldPos.Y);
                    break;
                case 2:
                    Left += (pos.X - _mouseOldPos.X);
                    Width -= (pos.X - _mouseOldPos.X);
                    Height += (pos.Y - _mouseOldPos.Y);
                    break;
                case 3:
                    Width += (pos.X - _mouseOldPos.X);
                    Height += (pos.Y - _mouseOldPos.Y);
                    break;
                default:
                    break;
            }
            /*
            else if(_isMouseDown && )
            {
                Width -= (pos.X - _mouseOldPos.X);
                Height -= (pos.Y - _mouseOldPos.Y);
                Left += (pos.X - _mouseOldPos.X);
                Top += (pos.Y - _mouseOldPos.Y);
                _mouseOldPos = MouseUtils.GetCursorPos();
                return;
            }
             */
        }

        private Brush _selectedStateBrush = new SolidBrush(Color.Lime);

        private List<Rectangle> CornerRegionList
        {
            get
            {
                return new List<Rectangle>
                {
                    new Rectangle(0, 0, 10, 10),
                    new Rectangle(Width - 10, 0, 10, 10),
                    new Rectangle(0, Height - 10, 10, 10),
                    new Rectangle(Width - 10, Height - 10, 10, 10)
                };
            }
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics, Color.Lime, Width, Height);
            if (IsSelected)
            {
                CornerRegionList.ForEach(c => e.Graphics.FillRectangle(_selectedStateBrush, c));
            }
        }

        private void DrawBorder(Graphics g, Color bordercolor, int x, int y)
        {
            Rectangle myRectangle = new Rectangle(0, 0, x - 1, y - 1);
            ControlPaint.DrawBorder(g, myRectangle, bordercolor, ButtonBorderStyle.Solid);//画个边框
        }

        public event Action<DragableRectangle> OnRectDoubleClick;

        protected override void OnDoubleClick(EventArgs e)
        {
            base.OnDoubleClick(e);
            OnRectDoubleClick?.Invoke(this);
        }

        protected override void OnClick(EventArgs e)
        {
            base.OnClick(e);
            BringToFront();
            var brotherList = Parent.Controls.Cast<Control>().Where(c => c is DragableRectangle).Select(c => (DragableRectangle)c).ToList();
            brotherList.ForEach(c => c.IsSelected = false);
            IsSelected = true;
            brotherList.ForEach(c => c.Invalidate());
        }

    }
}
