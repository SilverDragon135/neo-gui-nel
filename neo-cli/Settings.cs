﻿using Microsoft.Extensions.Configuration;
using Neo.Network;
using System;
using System.IO;

namespace Neo
{
    internal class Settings
    {
        public PathsSettings Paths { get; }
        public P2PSettings P2P { get; }
        public RPCSettings RPC { get; }

        public static Settings Default { get; }

        static Settings()
        {
            IConfigurationSection section = new ConfigurationBuilder().AddJsonFile("config.json").Build().GetSection("ApplicationConfiguration");
            Default = new Settings(section);
        }

        public Settings(IConfigurationSection section)
        {
            this.Paths = new PathsSettings(section.GetSection("Paths"));
            this.P2P = new P2PSettings(section.GetSection("P2P"));
            this.RPC = new RPCSettings(section.GetSection("RPC"));
        }
    }

    internal class PathsSettings
    {
        public string Chain { get; }
        public string ApplicationLogs { get; }
        public string Fulllogs { get; }
        public int fulllog_splitcount { get; }
        public int fulllog_splitindex { get; }
        public PathsSettings(IConfigurationSection section)
        {
            this.Chain = section.GetSection("Chain").Value;
            this.ApplicationLogs = Path.Combine(AppContext.BaseDirectory, $"ApplicationLogs_{Message.Magic:X8}");
            this.Fulllogs = section.GetSection("Fulllogs").Value;
            var cvalue = section.GetSection("Fulllog_splitCount").Value;
            var ivalue = section.GetSection("Fulllog_splitIndex").Value;
            if (cvalue == null || ivalue == null)
            {
                this.fulllog_splitcount = 1;
                this.fulllog_splitindex = 0;
            }
            else
            {
                this.fulllog_splitcount = int.Parse(cvalue);
                this.fulllog_splitindex = int.Parse(ivalue);
            }
        }
    }

    internal class P2PSettings
    {
        public ushort Port { get; }
        public ushort WsPort { get; }

        public P2PSettings(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.WsPort = ushort.Parse(section.GetSection("WsPort").Value);
        }
    }

    internal class RPCSettings
    {
        public ushort Port { get; }
        public string SslCert { get; }
        public string SslCertPassword { get; }

        public RPCSettings(IConfigurationSection section)
        {
            this.Port = ushort.Parse(section.GetSection("Port").Value);
            this.SslCert = section.GetSection("SslCert").Value;
            this.SslCertPassword = section.GetSection("SslCertPassword").Value;
        }
    }
}
