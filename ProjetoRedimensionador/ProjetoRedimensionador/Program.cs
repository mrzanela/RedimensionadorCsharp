using System.Drawing;

internal class Program
{
    private static void Main(string[] args)
    {
        Console.WriteLine("Iniciando redimensionador");

        Thread th = new Thread(Redimensionar);
        th.Start();


        Console.Read();

        static void Redimensionar()
        {
            #region "Diretorios"
            string diretorio_entrada = "Arquivos_Entrada";
            string diretorio_finalizado = "Arquivos_Finalizados";
            string diretorio_redimensionado = "Arquivos_Redimensionados";


            if (!Directory.Exists(diretorio_entrada))
            {
                Directory.CreateDirectory(diretorio_entrada);
            }

            if (!Directory.Exists(diretorio_finalizado))
            {
                Directory.CreateDirectory(diretorio_finalizado);
            }

            if (!Directory.Exists(diretorio_redimensionado))
            {
                Directory.CreateDirectory(diretorio_redimensionado);
            }
            else
            {
                Console.WriteLine("Diretórios já criados");
            }
            #endregion
            FileStream fileStream;
            FileInfo fileInfo;
            while (true)
            {
                //Meu programa vai olhar para a pasta de entrada
                //SE tiver arquivo, ele irá redimensionar
                var arquivosEntrada = Directory.EnumerateFiles(diretorio_entrada);

                //ler o tamanho que irá redimensionar
                int novaAltura = 200;

                foreach (var arquivo in arquivosEntrada)
                {

                    fileStream = new FileStream(arquivo, FileMode.Open, FileAccess.ReadWrite, FileShare.ReadWrite);
                    fileInfo = new FileInfo(arquivo);

                    string caminho = Environment.CurrentDirectory + @"\" + diretorio_redimensionado
                        + @"\" + DateTime.Now.Millisecond.ToString() + "_" + fileInfo.Name;

                    //redimensiona + //copia os arquivos redimensionados para a pasta de redimensionados
                    Redimensionador(Image.FromStream(fileStream), novaAltura, caminho);

                    //fecha o arquivo
                    fileStream.Close();

                    //move o arquivo de entrada para a pasta de finalizados
                    string caminhoFinalizado = Environment.CurrentDirectory + @"\" + diretorio_finalizado + @"\" + fileInfo.Name;
                    fileInfo.MoveTo(caminhoFinalizado);

                }

                Thread.Sleep(new TimeSpan(0, 0, 3));

            }

        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="imagem">Imagem a ser redimensionada</param>
        /// <param name="altura">Altura que desejamos redimensionar</param>
        /// <param name="caminho">caminho onde iremos gravar o arquivo redimensionado</param>
        static void Redimensionador(Image imagem, int altura, string caminho)
        {
            double ratio = (double)altura / imagem.Height;
            int novaLargura = (int)(imagem.Width * ratio);
            int novaAltura = (int)(imagem.Height * ratio);

            Bitmap novaImage = new Bitmap(novaLargura, novaAltura);

            using (Graphics g = Graphics.FromImage(novaImage))
            {
                g.DrawImage(imagem, 0, 0, novaLargura, novaAltura);
            }
            novaImage.Save(caminho);
            imagem.Dispose();
        }
    }
}