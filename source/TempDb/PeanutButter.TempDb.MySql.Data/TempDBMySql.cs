﻿using System;
using System.Diagnostics;
using MySql.Data.MySqlClient;
using PeanutButter.TempDb.MySql.Base;

// ReSharper disable IdentifierTypo
// ReSharper disable StringLiteralTypo
// ReSharper disable CommentTypo
// ReSharper disable UnusedMember.Global

// ReSharper disable MemberCanBePrivate.Global
// ReSharper disable AutoPropertyCanBeMadeGetOnly.Global

namespace PeanutButter.TempDb.MySql.Data
{
    /// <summary>
    /// Provides the TempDB implementation for MySql, using
    /// MySql.Data as the connector library
    /// </summary>
    // ReSharper disable once InconsistentNaming
    public class TempDBMySql : TempDBMySqlBase<MySqlConnection>
    {
        /// <summary>
        /// Construct a TempDbMySql with zero or more creation scripts and default options
        /// </summary>
        /// <param name="creationScripts"></param>
        public TempDBMySql(params string[] creationScripts)
            : base(new TempDbMySqlServerSettings(), creationScripts)
        {
            Warn();
        }

        private const string WARNING =
@"The PeanutButter.TempDb.MySql package is deprecated in favor of two 
new packages:
 - PeanutButter.TempDb.Data
   which uses Oracle's MySql.Data package for connecting
 - PeanutButter.TempDb.Connector
   which uses the opensource MySqlConnector package for connecting
The current installation for PeanutButter.TempDb.MySql is a small wrapper
around PeanutButter.TempDb.Data and may disappear at any time. Please update
your references to PeanutButter.TempDb.Data if you plan on continuing to use
the MySql.Data connector";
        
        private void Warn()
        {
            Trace.WriteLine(WARNING);
            if (Debugger.IsAttached)
            {
                Debug.WriteLine(WARNING);
            }
            else
            {
                Console.WriteLine(WARNING);
            }
        }

        /// <summary>
        /// Create a TempDbMySql instance with provided options and zero or more creation scripts
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="creationScripts"></param>
        public TempDBMySql(
            TempDbMySqlServerSettings settings,
            params string[] creationScripts
        )
            : base(
                settings,
                o =>
                {
                },
                creationScripts)
        {
        }

        /// <summary>
        /// Create a TempDbMySql instance with provided options, an action to run before initializing and
        /// zero or more creation scripts
        /// </summary>
        /// <param name="settings"></param>
        /// <param name="beforeInit"></param>
        /// <param name="creationScripts"></param>
        public TempDBMySql(
            TempDbMySqlServerSettings settings,
            Action<object> beforeInit,
            params string[] creationScripts
        ) : base(
            settings,
            o => BeforeInit(o as TempDBMySqlBase<MySqlConnection>, beforeInit, settings),
            creationScripts
        )
        {
        }
        
        /// <summary>
        /// Generates the connection string to use for clients
        /// wishing to connect to this temp instance
        /// </summary>
        /// <returns></returns>
        protected override string GenerateConnectionString()
        {
            var builder = new MySqlConnectionStringBuilder
            {
                Port = (uint) _port,
                UserID = "root",
                Password = "",
                Server = "localhost",
                AllowUserVariables = true,
                SslMode = MySqlSslMode.None,
                Database = _schema,
                ConnectionTimeout = DefaultTimeout,
                DefaultCommandTimeout = DefaultTimeout
            };
            return builder.ToString();
        }
    }
}