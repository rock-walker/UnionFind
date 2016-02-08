namespace monteCarloSimulatorPercolator
{
    public class UFFactory
    {
        public UfStrategy Create(UnionFindType type, SquareModel[,] matrix)
        {
            switch (type)
            {
                case UnionFindType.QuickFind:
                    return new QuickFindStrategy(matrix);
                case UnionFindType.QuickUnion:
                    return new QuickUnionStrategy(matrix);
                case UnionFindType.WeightenedUnion:
                    return new WeightenedUnionStrategy(matrix);
                default:
                    return new QuickFindStrategy(matrix);
            }
        }
    }
}
