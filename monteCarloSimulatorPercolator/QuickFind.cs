using System;

namespace monteCarloSimulatorPercolator
{
    public class QuickFind: IUnionFindAlgorithm
    {
        private SquareModel[,] _matrix;
        private readonly int _size;
        public QuickFind(SquareModel[,] matrix)
        {
            _matrix = matrix;
            _size = (int)Math.Sqrt(_matrix.Length);
        }
        public bool Connected(int a, int b)
        {
            return false;
        }

        public void Union(int a, int b)
        {
            var isLeftEdge = a == 0;
            var isRightEdge = a == _size- 1;
            var isTopEdge = b == 0;
            var isBottomEdge = b == _size - 1;

            var newValue = _matrix[a, b].Value;

            if (!isLeftEdge && _matrix[a - 1, b].IsSquared)
            {
                var prevValue = _matrix[a - 1, b].Value;
                _matrix[a - 1, b].Value = newValue;
                SetupOldValues(prevValue, newValue);
            }

            if (!isRightEdge && _matrix[a + 1, b].IsSquared)
            {
                var prevValue = _matrix[a + 1, b].Value;
                _matrix[a + 1, b].Value = newValue;
                SetupOldValues(prevValue, newValue);
            }

            if (!isBottomEdge && _matrix[a, b + 1].IsSquared)
            {
                var prevValue = _matrix[a, b + 1].Value;
                _matrix[a, b + 1].Value = newValue;
                SetupOldValues(prevValue, newValue);
            }

            if (!isTopEdge && _matrix[a, b - 1].IsSquared)
            {
                var prevValue = _matrix[a, b - 1].Value;
                _matrix[a, b - 1].Value = newValue;
                SetupOldValues(prevValue, newValue);
            }
        }

        private void SetupOldValues (int prev, int @new)
        {
            for (var i = 0; i < _size; i++)
            {
                for (var j = 0; j < _size; j++)
                {
                    if (_matrix[i, j].Value == prev)
                    {
                        _matrix[i, j].Value = @new;
                    }
                }
            }
        }
    }
}
