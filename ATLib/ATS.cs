﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ATLib
{
    public class ATS : ATElement
    {
        private AT[] ats = null;
        public ATS(AT[] ats)
        {
            this.ats = ats;
        }
        public AT[] GetATCollection()
        {
            return this.ats;
        }
        public int Length()
        {
            try
            {
                return GetATCollection().Length;
            }
            catch (Exception)
            {
                return 0;
            }
        }
        public ATS GetMatchedElements(string TreeScope = null, string Name = null, string AutomationId = null, string ClassName = null, string FrameworkId = null, string ControlType = null, string Index = null, string SelectNum = AT.SelectNum.Single)
        {
            List<AT> eleList = new List<AT>();
            foreach (AT item in GetATCollection())
            {
                try
                {
                    if (ATElement.IsElementsMatch(atObj: item, Name: Name, ClassName: ClassName, AutomationId: AutomationId))
                    {
                        item.GetElement(TreeScope: TreeScope, Name: Name, AutomationId: AutomationId, ClassName: ClassName, FrameworkId: FrameworkId, ControlType: ControlType);
                        eleList.Add(item);
                        if (SelectNum.Equals(AT.SelectNum.Single))
                            break;
                    }
                }
                catch (Exception) { }
            }
            if (eleList.Count == 0)
            {
                throw new Exception("There is no any item matching.");
            }
            AT[] arrAutomationElement = eleList.ToArray();
            return new ATS(arrAutomationElement);
        }
        public string SelectItemFromCollection(string strIndex = null, string name = null, string strDoMode = ATElement.SelectMode.Point)
        {
            try
            {
                AT ele = null;
                if (!String.IsNullOrEmpty(name))
                {
                    try
                    {
                        ele = this.GetMatchedElements(Name: name, TreeScope: AT.TreeScope.Element).GetATCollection()[0];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("The item {0} does not exist.", name, ex.Message));
                    }
                }
                else
                {
                    try
                    {
                        ele = this.GetATCollection()[Convert.ToInt16(strIndex)];
                    }
                    catch (Exception ex)
                    {
                        throw new Exception(string.Format("The item index {0} does not exist", strIndex, ex.Message));
                    }
                }
                ele.DoByMode(strDoMode);
                string t_name = "Can not get name";
                try { t_name = ele.GetElementInfo().Name(); }
                catch (Exception) { }
                return t_name;
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
