using System;

namespace PCG.Generation
{
    // gera arvore BSP a partir de uma area retangular inicial
    public class BspGenerator
    {
        private readonly int minRoomSize;
        private readonly int maxDepth;
        private readonly Random random; // sortador proprio dessa instancia

        private const float AspectThreshold = 1.25f; //acima a area é considerada esticada e o corte é forçado
        private const float MinSplitRatio = 0.35f;  // faixa q pode cortar - 35 a 65% comprimento
        private const float MaxSplitRatio = 0.65f;

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

            // se só um eixo cabe, usa ele / se ambos cabem, o formato decide
            bool cortarVertical;

            if (cabeVertical && !cabeHorizontal)
                cortarVertical = true;
            else if (!cabeVertical && cabeHorizontal)
                cortarVertical = false;
            else
                cortarVertical = ChooseVerticalAxis(node.Area);

            Area area = node.Area; // copia local, p encurtar as linhas

            if (cortarVertical)
            {
                int cut = RandomCut(area.Width);

                // esquerda fica com 'cut' de largura, direita com o resto
                node.Left = new BspNode(new Area(area.X, area.Y, cut, area.Height));
                node.Right = new BspNode(new Area(area.X + cut, area.Y, area.Width - cut, area.Height));
            }
            else
            {
                int cut = RandomCut(area.Height);

                //topo fica com 'cut de largura, base com o resto
                node.Left = new BspNode(new Area(area.X, area.Y, area.Width, cut));
                node.Right = new BspNode(new Area(area.X, area.Y + cut, area.Width, area.Height - cut));
            }

            //recursao, repetir o processo nos dois pedaços
            Split(node.Left, depth + 1);
            Split(node.Right, depth + 1);
        }

        //decide o eixo do corte, true = corta na vertical
        private bool ChooseVerticalAxis(Area area)
        {
            //float obriga divisao decimal, sem ele 7/0 daria 0
            float aspect = (float)area.Width / area.Height;

            if (aspect > AspectThreshold) // larga demais
                return true;              // corta vertical p encurtar

            if (aspect < 1f / AspectThreshold)  //alta demais
                return false;                   //corta horizontal

            return random.Next(2) == 0; // quase quadrada = sorteia
        }

        private int RandomCut(int length) // sorteia onde cortar, dentro da faixa e sem violar tamanho min
        {
            int min = (int)(length * MinSplitRatio);
            int max = (int)(length * MaxSplitRatio);

            //trava os limites, nenhum lado pode ficar menor q o min
            if (min < minRoomSize)
                min = minRoomSize;

            if (max > length - minRoomSize)
                max = length - minRoomSize

            return random.Next(min, max + 1); // +1 pq o topo é exclusivo
        }
    }
}