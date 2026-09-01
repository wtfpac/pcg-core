using System;

namespace PCG.Generation
{
    // gera arvore BSP a partir de uma area retangular inicial
    public class BspGenerator
    {
        private readonly int minRoomSize;
        private readonly int maxDepth;
        private readonly Random random; // sortador proprio dessa instancia

        // construtor roda quando alguem faz new BspGenerator(....)
        public BspGenerator(int minRoomSize, int maxDepth, int seed)
        {
            this.minRoomSize = minRoomSize; // evita comodo minusculo
            this.maxDepth = maxDepth; //limita qtde de comodos
            this.random = new Random(seed);
        }

        public BspNode Generate(Area root) //recebe area total, devolve bspnode (raiz da arvore)
        {
            BspNode node = new BspNode(root); // raiz sem cortes
            Split(node, 0); // 0 = tamano inicial, corta recursivamente a partir daq
            return node;
        }

        //corta area do nó em 2 e repete nos filhos
        // chama a si mesmo (recursao) ate bater na condiçao d parada
        private void Split(BspNode node, int depth)
        {
            // stop 1 - verificar se ja cortou demais
            if (depth >= maxDepth)
                return; // sai do metodo na hora e vira 1 comodo

            // verify se da p cortar +, espaço pra 2 comodos tamanho minimo?
            bool cabeVertical = node.Area.Width >= minRoomSize * 2;
            bool cabeHorizontal = node.Area.Height >= minRoomSize * 2;

            // stopp 2 - area pequena, nao da pra cortar mais
            if (!cabeVertical && !cabeHorizontal)
                return;
        }
    }
}