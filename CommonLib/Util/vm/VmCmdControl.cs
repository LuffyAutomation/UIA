﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace CommonLib.Util.vm
{
    public class VmCmdControl : Vm, IVmControl
    {
        public string vmxFullPath;
        public VmCmdControl()
        {

        }
        public VmCmdControl(string vmxFullPath)
        {
            this.vmxFullPath = vmxFullPath;
        }
        public void SetVM(string vmxFullPath)
        {
            try
            {
                this.vmxFullPath = vmxFullPath;
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to set VM [{0}].", vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
            }   
        }

        public bool IsSnapshotExisting(string snapshotName)
        {
            try
            {
                string[] snapshots = UtilProcess.StartProcessGetStrings(VmCmdControl.vmrunInstallFullPath, string.Format("listSnapshots \"{0}\"", vmxFullPath));
                foreach (string snapshot in snapshots)
                {
                    if (snapshot.Equals(snapshotName))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to find Snapshot [{0}] from VM [{1}].", vmxFullPath, snapshotName), new StackFrame(0).GetMethod().Name, ex.Message);
                return false;
            }
        }
        public bool IsVmRunning(string vmxFullPath)
        {
            try
            {
                string[] runningVMs = UtilProcess.StartProcessGetStrings(VmCmdControl.vmrunInstallFullPath, "list");
                foreach (string vm in runningVMs)
                {
                    if (vm.Equals(vmxFullPath))
                    {
                        return true;
                    }
                }
                return false;
            }
            catch (Exception ex)
            {
                Logger.LogError(string.Format("Failed to get if {0} was running.", vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
                return true;
            }
        }
        public void PowerOff(int waitSecond = 10)
        {
            try
            {
                if (IsVmRunning(vmxFullPath))
                {
                    string strlist = UtilProcess.StartProcessGetString(VmCmdControl.vmrunInstallFullPath, string.Format("-T ws stop \"{0}\"", this.vmxFullPath));
                    UtilTime.WaitTime(waitSecond);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to power off VM [{0}]", this.vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
            }
        }

        public void PowerOn(int waitSecond = 10)
        {
            try
            {
                string result = UtilProcess.StartProcessGetString(VmCmdControl.vmrunInstallFullPath, string.Format("-T ws start \"{0}\"", this.vmxFullPath));
                if (!result.Equals(""))
                {
                    throw new Exception(result);       
                }
                UtilTime.WaitTime(waitSecond);
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to power on VM [{0}].", this.vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
            }     
        }

        public void RevertToSnapshot(string revertSnapshotName)
        {
            try
            {
                string result = UtilProcess.StartProcessGetString(VmCmdControl.vmrunInstallFullPath, string.Format("revertToSnapshot \"{0}\" \"{1}\"", this.vmxFullPath, revertSnapshotName));
                if (!result.Equals(""))
                {
                    throw new Exception(result);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to revert Snapshot [{0}] of VM [{1}].", revertSnapshotName, this.vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
            }
        }

        public void TakeSnapshot(string takeSnapshotName)
        {
            try
            {
                if (takeSnapshotName.Trim().Equals(""))
                {
                    throw new Exception("Snapshot was invalid.");
                }
                string result = UtilProcess.StartProcessGetString(VmCmdControl.vmrunInstallFullPath, string.Format("snapshot \"{0}\" \"{1}\"", this.vmxFullPath, takeSnapshotName));
                if (!result.Equals(""))
                {
                    throw new Exception(result);
                }
            }
            catch (Exception ex)
            {
                Logger.LogThrowMessage(string.Format("Failed to take Snapshot [{0}] of VM [{1}].", takeSnapshotName, this.vmxFullPath), new StackFrame(0).GetMethod().Name, ex.Message);
            }
        }
    }
}
