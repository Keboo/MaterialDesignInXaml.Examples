using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using Microsoft.DwayneNeed.Numerics;
using Microsoft.DwayneNeed.Geometry;

namespace HierarchicalDataDemo
{
    class HeiankyoView
    {
        public Rect Bounds
        {
            get
            {
                if (_subspaces != null)
                {
                    return new Rect(new Point(_verticalPartitions[0], _horizontalPartitions[0]),
                                    new Point(_verticalPartitions[_verticalPartitions.Count - 1], _horizontalPartitions[_horizontalPartitions.Count - 1]));
                }
                else
                {
                    return Rect.Empty;
                }
            }
        }

        public Rect Add(Size requiredSize)
        {
            Rect rect;

            // The first rectangle is special.
            if (_subspaces == null)
            {
                rect = new Rect(new Point(0.0, 0.0), requiredSize);

                _verticalPartitions.Add(rect.Left);
                _verticalPartitions.Add(rect.Right);
                _horizontalPartitions.Add(rect.Top);
                _horizontalPartitions.Add(rect.Bottom);

                _subspaces = new SubspaceSet();
                _subspaces[0, 0] = true;
            }
            else
            {
                Rect? candidate = SearchCandidates(requiredSize);
                if (candidate == null)
                {
                    // This rect doesn't fit anywhere.  Decide where to put it.
                    // For now, add it to the bottom.  Consider better algorithms:
                    // 1) minimize for expansion
                    // 2) maintain aspect ratio
                    // 3) don't block open sites
                    // 4) etc.
                    rect = new Rect(new Point(_verticalPartitions.Interval.Min, _horizontalPartitions.Interval.Max), requiredSize);
                }
                else
                {
                    rect = candidate.Value;
                }

                AddRect(rect);
            }
            
            return rect;
        }


        private void AddRect(Rect rect)
        {
            InsertVerticalPartition(rect.Left);
            InsertHorizontalPartition(rect.Top);
            InsertVerticalPartition(rect.Right);
            InsertHorizontalPartition(rect.Bottom);

            Int32Rect subspaceBounds = GetSubspaceRect(rect);

            for (int iY = 0; iY < subspaceBounds.Height; iY++)
            {
                int y = iY + subspaceBounds.Y;
                for (int iX = 0; iX < subspaceBounds.Width; iX++)
                {
                    int x = iX + subspaceBounds.X;

                    _subspaces[x, y] = true;
                }
            }            
        }

        private void InsertVerticalPartition(Double42 partition)
        {
            if (_verticalPartitions.Add(partition))
            {
                int index;
                _verticalPartitions.TryFind(partition, out index);
                _subspaces.InsertColumn(index - 1);
            }
        }

        private void InsertHorizontalPartition(Double42 partition)
        {
            if (_horizontalPartitions.Add(partition))
            {
                int index;
                _horizontalPartitions.TryFind(partition, out index);
                _subspaces.InsertRow(index - 1);
            }
        }

        private Rect? SearchCandidates(Size requiredSize)
        {
            Rect? winningCandidate = null;
            Double42? minExpansion = null;

            foreach (Rect candidate in CandidateTraversal(requiredSize))
            {
                Double42 expansion;
                if (EvaluateCandidate(candidate, out expansion))
                {
                    if (expansion == 0)
                    {
                        // Take the first viable candidate that does not
                        // require expanding the bounds.
                        winningCandidate = candidate;
                        break;
                    }
                    else
                    {
                        if (minExpansion == null || expansion < minExpansion.Value)
                        {
                            minExpansion = expansion;
                            winningCandidate = candidate;
                        }
                    }
                }
            }

            return winningCandidate;
        }

        // Returns true if the candidate is available, and expansion
        // will be set to a non-null value if the candidate causes
        // the subspace bounds to expand.  Returns false if the candidate
        // overlaps other rectangles.
        public bool EvaluateCandidate(Rect candidateRect, out Double42 expansion)
        {
            expansion = 0.0;
            Int32Rect subspaceBounds = GetSubspaceRect(candidateRect);

            for (int iY = 0; iY < subspaceBounds.Height; iY++)
            {
                int y = iY + subspaceBounds.Y;
                for (int iX = 0; iX < subspaceBounds.Width; iX++)
                {
                    int x = iX + subspaceBounds.X;

                    if (_subspaces[x, y])
                    {
                        // There is already a rect in this subspace location.
                        // We can fail immediately.
                        return false;
                    }
                }
            }

            Rect oldBounds = Bounds;
            if (!oldBounds.Contains(candidateRect))
            {
                Rect newBounds = oldBounds;
                newBounds.Union(candidateRect);

                expansion = (newBounds.Width * newBounds.Height) - (oldBounds.Width * oldBounds.Height);
            }

            return true;
        }

        public IEnumerable<Rect> CandidateTraversal(Size requiredSize)
        {
            foreach (Point2D<int> coord in SpiralSubspaceTraversal)
            {
                // I don't think the order in which we check the 4 possible
                // candidates for each grid position is important.
                yield return CreateCandidateRect(coord, +1, +1, requiredSize);
                yield return CreateCandidateRect(coord, +1, -1, requiredSize);
                yield return CreateCandidateRect(coord, -1, +1, requiredSize);
                yield return CreateCandidateRect(coord, -1, -1, requiredSize);
            }
        }

        private Rect CreateCandidateRect(Point2D<int> start, int deltaX, int deltaY, Size requiredSize)
        {
            Double42 x = deltaX > 0 ? _verticalPartitions[start.X] : (Double42) (_verticalPartitions[start.X + 1] - requiredSize.Width);
            Double42 y = deltaY > 0 ? _horizontalPartitions[start.Y] : (Double42) (_horizontalPartitions[start.Y + 1] - requiredSize.Height);

            return new Rect(new Point(x, y), requiredSize);
        }

        // Return the range of subspace indexes that the candidate rectangle spans.
        private Int32Rect GetSubspaceRect(Rect candidateRect)
        {
            int x1 = FindFirstSubspaceIndex(_verticalPartitions, candidateRect.Left);
            int x2 = FindLastSubspaceIndex(_verticalPartitions, candidateRect.Right, x1);
            int y1 = FindFirstSubspaceIndex(_horizontalPartitions, candidateRect.Top);
            int y2 = FindLastSubspaceIndex(_horizontalPartitions, candidateRect.Bottom, y1);

            return new Int32Rect(x1, y1, x2 - x1 + 1, y2 - y1 + 1);
        }

        // Find the first subspace index that spans space that contains the value.
        // A key assumption is that this is only used for mapping candidate rects
        // onto the subspace grid, and candidate rects always intersect the 
        // subspace grid.  Thus, if the value is less than the partitions, we
        // assume that we should still start with index 0.
        private static int FindFirstSubspaceIndex(NumberSet<Double42> partitions, Double42 value)
        {
            int first = 0;
            for (int iPartition = 1; iPartition < partitions.Count; iPartition++)
            {
                if (partitions[iPartition] > value)
                {
                    first = iPartition - 1;
                    break;
                }
            }

            return first;
        }

        // Find the last subspace index that spans space that contains the value.
        // A key assumption is that this is only used for mapping candidate rects
        // onto the subspace grid, and candidate rects always intersect the 
        // subspace grid.  Thus, if the value is greater than the partitions, we
        // assume that we should still end with the last index.
        private static int FindLastSubspaceIndex(NumberSet<Double42> partitions, Double42 value, int iSubspaceStart)
        {
            int last = partitions.Count - 1;
            for (int iPartition = iSubspaceStart + 1; iPartition < partitions.Count; iPartition++)
            {
                if (partitions[iPartition]>=value)
                {
                    last = iPartition - 1;
                    break;
                }
            }

            return last;
        }

        private IEnumerable<Point2D<int>> SpiralSubspaceTraversal
        {
            get
            {
                int x = (_subspaces.Width-1)/2;
                int y = (_subspaces.Height-1)/2;
                int maxLength = _subspaces.Width > _subspaces.Height ? _subspaces.Width : _subspaces.Height;
                int direction = 1;

                // Return the starting coordinate.
                yield return new Point2D<int>(x, y);

                for (int legLength = 1; legLength <= maxLength; legLength++)
                { 
                    // First go up/down
                    if (x >= 0 && x < _subspaces.Width)
                    {
                        for (int i = 0; i < legLength; i++)
                        {
                            y+=direction;
                            if (y >= 0 && y < _subspaces.Height)
                            {
                                yield return new Point2D<int>(x, y);
                            }
                        }
                    }
                    else
                    {
                        y += legLength*direction;
                    }

                    // Then go right/left
                    if (y >= 0 && y < _subspaces.Height)
                    {
                        for (int i = 0; i < legLength; i++)
                        {
                            x+=direction;
                            if (x >= 0 && x < _subspaces.Width)
                            {
                                yield return new Point2D<int>(x, y);
                            }
                        }
                    }
                    else
                    {
                        x += legLength*direction;
                    }

                    direction *= -1;
                }
            }
        }

        NumberSet<Double42> _verticalPartitions = new NumberSet<Double42>();
        NumberSet<Double42> _horizontalPartitions = new NumberSet<Double42>();
        SubspaceSet _subspaces;
    }
}
