using System;

namespace monteCarloSimulatorPercolator
{
    public abstract class UfStrategy
    {
        protected abstract IUnionFindAlgorithm Algorithm { get; set; }
        public abstract void Execute(int x, int y);
    }

    public class QuickFindStrategy: UfStrategy
    {
        private SquareModel[,] _matrix;
        protected sealed override IUnionFindAlgorithm Algorithm { get; set; }
        public QuickFindStrategy(SquareModel[,] matrix)
        {
            _matrix = matrix;
            Algorithm = new QuickFind(matrix);
        }
        public override void Execute(int x, int y)
        {
            if (!Algorithm.Connected(x, y))
                Algorithm.Union(x, y);
        }
    }

    public class QuickUnionStrategy : UfStrategy
    {
        private SquareModel[,] _matrix;
        protected override IUnionFindAlgorithm Algorithm { get; set; }
        public QuickUnionStrategy(SquareModel[,] matrix)
        {
            _matrix = matrix;
        }
        public override void Execute(int x, int y)
        {
            throw new NotImplementedException();
        }
    }

    public class WeightenedUnionStrategy : UfStrategy
    {
        private SquareModel[,] _matrix;
        protected override IUnionFindAlgorithm Algorithm { get; set; }
        public WeightenedUnionStrategy(SquareModel[,] matrix)
        {
            _matrix = matrix;
        }
        public override void Execute(int x, int y)
        {
            throw new NotImplementedException();
        }
    }

    public class Context
    {
        private int[,] _matrix;
        private UfStrategy _ufStrategy;
        public void SetStrategy(UfStrategy stragegy)
        {
            _ufStrategy = stragegy;
        }

        public void Execute(int x, int y)
        {
            _ufStrategy.Execute(x, y);
        }
    }
}
