﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CommonLib.Util.zip
{
    public class Zip7z : IZip
    {
        private string tool_7z;

        public Zip7z(string tool_7z)
        {
            this.tool_7z = tool_7z;
        }

        public string Tool_7z
        {
            get
            {
                return this.tool_7z;
            }

            set
            {
                this.tool_7z = value;
            }
        }

        public void ExtractZip(string source)
        {
            throw new NotImplementedException();
        }

        public void ExtractZip(string source, string destination)
        {
            //Shell(constPath7zEXE & " x """ & pathBuildZIP & "\" & fullBuildName & ".zip"" - y - o""" & pathATScript & "\" & strWhichVM & "\" & """")
            try
            {
                UtilProcess.StartProcessGetInt(Tool_7z, string.Format("x \"{0}\" -y -o\"{1}\"", source, destination));
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to extract zip [{0}] to [{2}].", source, destination), new StackFrame(0).GetMethod().Name, ex.Message);
            }
        }
    }
}