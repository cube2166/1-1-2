using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Collections.Specialized;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace _2048Demo
{
    public enum KeyArrow
    {
        UP,
        DOWN,
        LEFT,
        RIGHT,
    }

    public class MyObject
    {
        #region Property
        public float p_X { get; set; }
        public float p_Y { get; set; }
        public int p_Width { get; set; }
        public int p_Height { get; set; }
        public object p_Texture { get; set; }
        public Color p_Color { get; set; }
        #endregion

        #region Event
        #endregion

        #region Create

        #endregion

        #region Method

        public virtual void f_Update(float elapsedTime)
        {

        }

        public virtual void f_Show(SpriteBatch spriteBatch)
        {

        }

        #endregion
    }

    public class Board : MyObject
    {
        #region Property
        #endregion

        #region Private
        private bool _p_PrepareAdd;
        private int _p_MaxCol, _p_MaxRow;
        private PieceList _c_PieceList;
        private int[,,] _p_PieceInfo;
        private int _p_PieceWidth, _p_PieceHeight, _p_PieceInterval;
        private List<Texture2D> _l_TextureList;
        private Rectangle _s_Rect1, _s_Rect2;
        private Random _c_Rand;
        private int _p_Count;
        #endregion

        #region Create
        public Board(List<Texture2D>List, int size)
        {
            _c_PieceList = new PieceList();
            _c_Rand = new Random(DateTime.Now.Second);
            _p_MaxCol = _p_MaxRow = size;
            _l_TextureList = List;
            p_Height = Game1.p_WindowHeight;
            p_Width = p_Height;
            p_X = (Game1.p_WindowWidth - Game1.p_WindowHeight) / 2;
            p_Y = 0;
            int tempVal = p_Width/40;
            _s_Rect1 = new Rectangle((int)p_X, (int)p_Y, p_Width, p_Height);
            _s_Rect2 = new Rectangle((int)p_X + tempVal, (int)p_Y + tempVal, p_Width - (tempVal * 2), p_Height - (tempVal * 2));
            _p_PrepareAdd = false;
            _p_PieceInfo = new int[_p_MaxCol,_p_MaxRow,3];

            _p_PieceInterval = (_s_Rect2.Width / _p_MaxCol) / 20;
            _p_PieceWidth = (_s_Rect2.Width - _p_PieceInterval * 2) / _p_MaxCol;
            _p_PieceHeight = (_s_Rect2.Height - _p_PieceInterval * 2) / _p_MaxRow;

            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = 0; row < _p_MaxRow; row++)
                {
                    _p_PieceInfo[col, row, 0] = (_s_Rect2.X + _p_PieceWidth * col) + _p_PieceInterval * 2;
                    _p_PieceInfo[col, row, 1] = (_s_Rect2.Y + _p_PieceHeight * row) + _p_PieceInterval * 2;
                    _p_PieceInfo[col, row, 2] = -1;
                    _c_PieceList.Add(new Piece(_l_TextureList, _p_PieceWidth, _p_PieceHeight, _p_PieceInterval));
                }
            }
            f_InitPiece();
        }
        #endregion

        #region Method
        public override void f_Update(float elapsedTime)
        {
            foreach (var item in _c_PieceList)
            {
                item.f_Update(elapsedTime);
            }
            if(_p_PrepareAdd == true)
            {
                _p_PrepareAdd = false;
                foreach (var item in _c_PieceList)
                {
                    if (item.p_CanMove == false)
                    {
                        _p_PrepareAdd = true;
                        break;
                    }
                }
                if(_p_PrepareAdd == false)
                {
                    _f_AddPiece();
                }
            }
        }

        private void _f_OnKeyArrowUp()
        {
            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = 0; row < _p_MaxRow; row++)
                {
                    if (_p_PieceInfo[col, row, 2] == -1)
                    {
                        int insRow = row + 1;
                        if (insRow == _p_MaxRow) break;

                        if (_p_PieceInfo[col, insRow, 2] != -1)
                        {
                            _p_PieceInfo[col, row, 2] = _p_PieceInfo[col, insRow, 2];
                            _p_PieceInfo[col, insRow, 2] = -1;
                            _c_PieceList[_p_PieceInfo[col, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                            _p_PrepareAdd = true;
                        }
                    }
                    else
                    {
                        int insRow = row + 1;
                        if (insRow == _p_MaxRow) break;
                        if (_p_PieceInfo[col, insRow, 2] != -1)
                        {
                            if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value + _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value == 3)
                            {
                                _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_CanHide = true;
                                _p_PieceInfo[col, insRow, 2] = -1;
                                _p_PrepareAdd = true;
                            }
                            else if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value == _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value)
                            {
                                if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 1 && _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 2)
                                {
                                    _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_CanHide = true;
                                    _p_PieceInfo[col, insRow, 2] = -1;
                                    _p_PrepareAdd = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void _f_OnKeyArrowDown()
        {
            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = _p_MaxRow - 1; row >= 0; row--)
                {
                    if (_p_PieceInfo[col, row, 2] == -1)
                    {
                        int insRow = row - 1;
                        if (insRow < 0) break;

                        if (_p_PieceInfo[col, insRow, 2] != -1)
                        {
                            _p_PieceInfo[col, row, 2] = _p_PieceInfo[col, insRow, 2];
                            _p_PieceInfo[col, insRow, 2] = -1;
                            _c_PieceList[_p_PieceInfo[col, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                            _p_PrepareAdd = true;
                        }
                    }
                    else
                    {
                        int insRow = row - 1;
                        if (insRow < 0) break;
                        if (_p_PieceInfo[col, insRow, 2] != -1)
                        {
                            if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value + _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value == 3)
                            {
                                _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_CanHide = true;
                                _p_PieceInfo[col, insRow, 2] = -1;
                                _p_PrepareAdd = true;
                            }
                            else if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value == _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value)
                            {
                                if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 1 && _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 2)
                                {
                                    _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                    _c_PieceList[_p_PieceInfo[col, insRow, 2]].p_CanHide = true;
                                    _p_PieceInfo[col, insRow, 2] = -1;
                                    _p_PrepareAdd = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void _f_OnKeyArrowLeft()
        {
            for (int row = 0; row < _p_MaxRow; row++)
            {
                for (int col = 0; col < _p_MaxCol; col++)
                {
                    if (_p_PieceInfo[col, row, 2] == -1)
                    {
                        int insCol = col + 1;
                        if (insCol == _p_MaxCol) break;

                        if (_p_PieceInfo[insCol, row, 2] != -1)
                        {
                            _p_PieceInfo[col, row, 2] = _p_PieceInfo[insCol, row, 2];
                            _p_PieceInfo[insCol, row, 2] = -1;
                            _c_PieceList[_p_PieceInfo[col, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                            _p_PrepareAdd = true;
                        }
                    }
                    else
                    {
                        int insCol = col + 1;
                        if (insCol == _p_MaxCol) break;
                        if (_p_PieceInfo[insCol, row, 2] != -1)
                        {
                            if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value + _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value == 3)
                            {
                                _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_CanHide = true;
                                _p_PieceInfo[insCol, row, 2] = -1;
                                _p_PrepareAdd = true;
                            }
                            else if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value == _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value)
                            {
                                if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 1 && _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 2)
                                {
                                    _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_CanHide = true;
                                    _p_PieceInfo[insCol, row, 2] = -1;
                                    _p_PrepareAdd = true;
                                }
                            }
                        }
                    }
                }
            }
        }
        private void _f_OnKeyArrowRight()
        {
            for (int row = 0; row < _p_MaxRow; row++)
            {
                for (int col = _p_MaxCol - 1; col >= 0; col--)
                {
                    if (_p_PieceInfo[col, row, 2] == -1)
                    {
                        int insCol = col - 1;
                        if (insCol < 0) break;

                        if (_p_PieceInfo[insCol, row, 2] != -1)
                        {
                            _p_PieceInfo[col, row, 2] = _p_PieceInfo[insCol, row, 2];
                            _p_PieceInfo[insCol, row, 2] = -1;
                            _c_PieceList[_p_PieceInfo[col, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                            _p_PrepareAdd = true;
                        }
                    }
                    else
                    {
                        int insCol = col - 1;
                        if (insCol < 0) break;
                        if (_p_PieceInfo[insCol, row, 2] != -1)
                        {
                            if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value + _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value == 3)
                            {
                                _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value = 3;
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_CanHide = true;
                                _p_PieceInfo[insCol, row, 2] = -1;
                                _p_PrepareAdd = true;
                            }
                            else if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value == _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value)
                            {
                                if (_c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 1 && _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value != 2)
                                {
                                    _c_PieceList[_p_PieceInfo[col, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_Value *= 2;
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].s_DestPosition = new Vector2(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1]);
                                    _c_PieceList[_p_PieceInfo[insCol, row, 2]].p_CanHide = true;
                                    _p_PieceInfo[insCol, row, 2] = -1;
                                    _p_PrepareAdd = true;
                                }
                            }
                        }
                    }

                }
            }
        }

        public void f_Move(KeyArrow ka)
        {
            foreach (var item in _c_PieceList)
            {
                if (item.p_CanMove == false) return;
            }
            switch (ka)
            {
                case KeyArrow.UP:
                    _f_OnKeyArrowUp();
                    break;
                case KeyArrow.DOWN:
                    _f_OnKeyArrowDown();
                    break;
                case KeyArrow.LEFT:
                    _f_OnKeyArrowLeft();
                    break;
                case KeyArrow.RIGHT:
                    _f_OnKeyArrowRight();
                    break;
                default:
                    break;
            }           
        }

        public override void f_Show(SpriteBatch spriteBatch)
        {
            _f_ShowBackground(spriteBatch);
            foreach (var item in _c_PieceList)
            {
                item.f_Show(spriteBatch);
            }
        }

        public void f_InitPiece()
        {
            foreach (var item in _c_PieceList)
            {
                item.p_Value = 0;
            }
            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = 0; row < _p_MaxRow; row++)
                {
                    _p_PieceInfo[col, row, 2] = -1;
                }
            }
            for (int ii = 0; ii < 2; ii++)
            {
                _f_AddPiece();
            }
        }

        private void _f_AddPiece()
        {
            int count = 0;
            List<Vector2> tempList = new List<Vector2>();
            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = 0; row < _p_MaxRow; row++)
                {
                    if (_p_PieceInfo[col, row, 2] == -1)
                    {
                        count++;
                        Vector2 temp = new Vector2(col, row);
                        tempList.Add(temp);
                    }
                }
            }
            if (count < 1) return;
            int randTemp = _c_Rand.Next(0, tempList.Count - 1);
            int randCol = (int)tempList[randTemp].X;
            int randRow = (int)tempList[randTemp].Y;

            foreach (var item in _c_PieceList)
            {
                if (item.p_Value == 0)
                {
                    _p_PieceInfo[randCol, randRow, 2] = _c_PieceList.IndexOf(item);
                    item.s_DestPosition = new Vector2(_p_PieceInfo[randCol, randRow, 0], _p_PieceInfo[randCol, randRow, 1]);
                    item.s_SrcPosition = new Vector2(_p_PieceInfo[randCol, randRow, 0], _p_PieceInfo[randCol, randRow, 1]);
                    item.p_X = item.s_SrcPosition.X;
                    item.p_Y = item.s_SrcPosition.Y;
                    item.p_Value = (_p_Count % 2) + 1;
                    _p_Count++;
                    break;
                }
            }
        }

        private void _f_ShowBackground(SpriteBatch spriteBatch)
        {
            Texture2D bg1 = _f_SearchTexture("bg1", _l_TextureList);
            Texture2D bg2 = _f_SearchTexture("bg2", _l_TextureList);
            Texture2D block_0 = _f_SearchTexture("block_0", _l_TextureList);
            if (bg1 == null || bg2 == null || block_0 == null) return;
            spriteBatch.Draw(bg1, _s_Rect1, Color.White);
            spriteBatch.Draw(bg2, _s_Rect2, Color.White);

            for (int col = 0; col < _p_MaxCol; col++)
            {
                for (int row = 0; row < _p_MaxRow; row++)
                {
                    spriteBatch.Draw(block_0, new Rectangle(_p_PieceInfo[col, row, 0], _p_PieceInfo[col, row, 1],
                                                            _p_PieceWidth -(_p_PieceInterval * 2) , _p_PieceHeight - (_p_PieceInterval * 2))
                                                            , Color.White);
                }
            }
        }

        private Texture2D _f_SearchTexture(string name, List<Texture2D> List)
        {
            foreach (var item in List)
            {
                if (String.Equals(item.Name, name)) return item;
            }
            return null;
        }

        #endregion

        #region Event

        #endregion

        #region Class
        private class Piece : MyObject
        {
            #region Property
            public bool p_CanMove
            {
                get { return _p_CanMove; }
                set
                {
                    if(_p_CanMove != value)
                    _p_CanMove = value;
                }
            }

            public bool p_CanShow
            {
                get { return _p_CanShow; }
                set
                {
                    if (_p_CanShow != value)
                        _p_CanShow = value;
                }
            }

            public int p_Value
            {
                get { return _p_Value; }
                set
                {
                    if (_p_Value != value)
                    {
                        _p_Value = value;
                        string temp = string.Format("block_{0}", _p_Value);
                        p_Texture = _f_SearchTexture(temp, _l_TextureList);
                        if (_p_Value == 0) p_CanShow = false;
                        else p_CanShow = true;
                    }
                }
            }

            public Vector2 s_DestPosition
            {
                get { return _s_DestPosition; }
                set
                {
                    if (_s_DestPosition != value && p_CanMove == true)
                    {
                        _s_DestPosition = value;
                        p_CanMove = false;
                    }
                }
            }

            public Vector2 s_SrcPosition
            {
                get { return _s_SrcPosition; }
                set
                {
                    if (_s_SrcPosition != value)
                    {
                        _s_SrcPosition = value;
                        if (_s_SrcPosition == _s_DestPosition)
                        {
                                p_CanMove = true;
                            if(p_CanHide == true)
                            {
                                p_CanHide = false;
                                p_Value = 0;
                            }
                        }

                    }
                }
            }

            public bool p_CanHide
            {
                get { return _p_CanHide; }
                set
                {
                    if(_p_CanHide != value)
                    {
                        _p_CanHide = value;
                    }
                }
            }
            #endregion

            #region Private
            private bool _p_CanHide, _p_CanMove, _p_CanShow;
            private int _p_Value;
            private List<Texture2D> _l_TextureList;
            private Vector2 _s_DestPosition, _s_SrcPosition;
            private int _p_PieceWidth, _p_PieceHeight, _p_PieceInterval;
            #endregion

            #region Method
            private Texture2D _f_SearchTexture(string name, List<Texture2D> List)
            {
                foreach (var item in List)
                {
                    if (String.Equals(item.Name, name)) return item;
                }
                return null;
            }

            public override void f_Update(float elapsedTime)
            {
                double len = Math.Sqrt(Math.Pow((s_DestPosition.X - s_SrcPosition.X),2) + Math.Pow((s_DestPosition.Y - s_SrcPosition.Y), 2));
                p_X += (s_DestPosition.X - s_SrcPosition.X)  * elapsedTime * 10;
                p_Y += (s_DestPosition.Y - s_SrcPosition.Y)  * elapsedTime * 10;
                s_SrcPosition = new Vector2(p_X, p_Y);
                if (len < 1) s_SrcPosition = s_DestPosition;
            }

            public override void f_Show(SpriteBatch spriteBatch)
            {
                if(p_CanShow == true)
                {
                    int tempX = (int)p_X;
                    int tempY = (int)p_Y;
                    Rectangle temp = new Rectangle(tempX, tempY, _p_PieceWidth - (_p_PieceInterval * 2), _p_PieceHeight - (_p_PieceInterval * 2));
                    spriteBatch.Draw((Texture2D)p_Texture, temp, Color.White);
                }
            }
            #endregion

            #region Create
            public Piece(List<Texture2D> List, int pw, int ph, int pi)
            {
                _l_TextureList = List;
                p_CanMove = true;
                p_CanHide = false;

                _p_PieceWidth = pw;
                _p_PieceHeight = ph;
                _p_PieceInterval = pi;
            }
            #endregion

            #region Event

            #endregion

        }

        private class PieceList : ObservableCollection<Piece>
        {

        }
        #endregion
    }
}
