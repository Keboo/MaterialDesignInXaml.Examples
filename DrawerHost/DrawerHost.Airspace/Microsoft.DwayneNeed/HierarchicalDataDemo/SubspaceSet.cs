using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace HierarchicalDataDemo
{
    class SubspaceSet
    {
        public SubspaceSet()
        {
            _width = 1;
            _height = 1;
            _subspaces = new bool[1, 1];
        }

        public int Width
        {
            get
            {
                return _width;
            }
        }

        public int Height
        {
            get
            {
                return _height;
            }
        }

        public bool this[int x, int y]
        {
            get
            {
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                {
                    throw new IndexOutOfRangeException();
                }

                return _subspaces[x, y];
            }

            set
            {
                if (x < 0 || x >= _width || y < 0 || y >= _height)
                {
                    throw new IndexOutOfRangeException();
                }

                _subspaces[x, y] = value;
            }
        }

        public void InsertColumn(int splitIndex)
        {
            if (splitIndex < -1 || splitIndex > _width)
            {
                throw new IndexOutOfRangeException();
            }

            int newWidth = _width + 1;
            int newHeight = _height == 0 ? 1 : _height;

            bool[,] newSubspaces = new bool[newWidth, newHeight];

            // Copy the current subspaces data to our new subspaces.
            if (_width > 0 && _height > 0)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (x <= splitIndex)
                        {
                            newSubspaces[x, y] = _subspaces[x, y];
                        }
                        if (x >= splitIndex)
                        {
                            newSubspaces[x + 1, y] = _subspaces[x, y];
                        }
                    }
                }
            }

            _subspaces = newSubspaces;
            _width = newWidth;
            _height = newHeight;
        }

        public void InsertRow(int splitIndex)
        {
            if (splitIndex < -1 || splitIndex > _height)
            {
                throw new IndexOutOfRangeException();
            }

            int newWidth = _width == 0 ? 1 : _width;
            int newHeight = _height + 1;

            bool[,] newSubspaces = new bool[newWidth, newHeight];

            // Copy the current subspaces data to our new subspaces.
            if (_width > 0 && _height > 0)
            {
                for (int y = 0; y < _height; y++)
                {
                    for (int x = 0; x < _width; x++)
                    {
                        if (y <= splitIndex)
                        {
                            newSubspaces[x, y] = _subspaces[x, y];
                        }
                        if (y >= splitIndex)
                        {
                            newSubspaces[x, y + 1] = _subspaces[x, y];
                        }
                    }
                }
            }

            _subspaces = newSubspaces;
            _width = newWidth;
            _height = newHeight;
        }

        private bool[,] _subspaces;
        private int _width = 0;
        private int _height = 0;
    }
}
