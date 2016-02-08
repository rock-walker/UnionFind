namespace monteCarloSimulatorPercolator
{
    public interface IUnionFindAlgorithm
    {
        bool Connected(int a, int b);
        void Union(int a, int b);
    }
}
