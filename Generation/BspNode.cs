namespace PCG.Generation
{
    // nó da arvore bsp. guarda uma area e se foi cortada, os dois pedaços
    public class BspNode
    {
        public Area Area;
        public BspNode Left; //nll enquanto nao houver corte
        public BspNode Right;

        public BspNode(Area area)
        {
            Area = area;
        }

        // folha = nunca foi cortado = vira 1 comodo
        public bool IsLeaf => Left == null && Right == null;
    }
}