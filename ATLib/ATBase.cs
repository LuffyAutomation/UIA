﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Automation;
using static ATLib.Input.HWSimulator;

namespace ATLib
{
    public class ATBase : ATElement
    {
        protected AutomationElement me = null;
        protected void SetMe(AutomationElement me)
        {
            this.me = me;
        }
        protected AutomationElement GetMe(string timeout = null)
        {
            return me;
        }
        protected ATBase()
        {
        }
        protected ATBase(AutomationElement elePara)
        {
            me = elePara;
        }

        public void DoByMode(string DoMode, double waitTime = 0.1)
        {
            try
            {
                switch (DoMode)
                {
                    case ATBase.DoMode.Invoke:
                        DoClick(waitTime);
                        break;
                    case ATBase.DoMode.Select:
                        DoSelect(waitTime);
                        break;
                    case ATBase.DoMode.Point:
                        DoClickPoint(0, 0, waitTime);
                        break;
                    default:
                        throw new Exception("DoMode " + DoMode + " does not exist.");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public void DoExpand(double waitTime = 0.2)
        {
            try
            {
                ExpandCollapsePattern t = me.GetCurrentPattern(ExpandCollapsePattern.Pattern) as ExpandCollapsePattern;
                t.Expand();
                Thread.Sleep((int)(waitTime * 1000));
            }
            catch (Exception ex)
            {
                throw new Exception("Expand error. " + ex);
            }
        }
        public void DoSelect(double waitTime = 0.1)
        {
            try
            {
                SelectionItemPattern t = (SelectionItemPattern)me.GetCurrentPattern(SelectionItemPattern.Pattern);
                t.Select();
                Thread.Sleep((int)(waitTime * 1000));
            }
            catch (Exception ex)
            {
                throw new Exception("Select error. " + ex);
            }
            Thread.Sleep((int)(waitTime * 1000));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="x"></param>
        /// <param name="y"></param>
        /// <param name="waitTime"></param>
        public void DoClickPoint(double x = 0, double y = 0, double waitTime = 0.1)
        {
            Point ptClick = new Point();
            if (x == 0 && y == 0)
            {
                try
                {
                    ptClick = me.GetClickablePoint();
                }
                catch
                {
                    try
                    {
                        ptClick = new Point((me.Current.BoundingRectangle.Left + 2), (me.Current.BoundingRectangle.Top + 2));
                    }
                    catch (Exception ex)
                    {
                        throw new Exception("Failed to GetClickablePoint. " + ex);
                    }
                }
            }
            else
            {
                try
                {
                    ptClick = new Point((me.Current.BoundingRectangle.Left), (me.Current.BoundingRectangle.Top));
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to GetBoundingRectangle. " + ex);
                }
            }
            try
            {
                HWSend.MoveMouseAndClickLeft((int)ptClick.X + (int)x, (int)ptClick.Y + (int)y);
            }
            catch (Exception ex)
            {
                throw new Exception("Click point error. " + ex);
            }
            Thread.Sleep((int)(waitTime * 1000));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public ScrollEvents DoScrollEvents()
        {
            try
            {
                return new ScrollEvents(new AT(me));
            }
            catch (Exception ex)
            {
                throw new Exception("DoScroll error. " + ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class ScrollEvents
        {
            /// <summary>
            /// 
            /// </summary>
            AT elePara;
            /// <summary>
            /// 
            /// </summary>
            ScrollPattern scrollPattern = null;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="elePara"></param>
            public ScrollEvents(AT elePara)
            {
                this.elePara = elePara;
                try
                {
                    scrollPattern = (ScrollPattern)elePara.GetMe().GetCurrentPattern(ScrollPattern.Pattern);
                }
                catch (Exception)
                {
                    //throw new Exception("Failed to get scrollable item.");
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="waitTime"></param>
            public void ScrollVerticalTop(double waitTime = 0.1)
            {
                try
                {
                    if (scrollPattern != null)
                    {
                        scrollPattern.ScrollVertical(ScrollAmount.LargeDecrement);
                    }
                }
                catch (Exception ex)
                {
                    throw new Exception("ScrollVerticalTop error. " + ex);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitTime"></param>
        public void DoClick(double waitTime = 0.1)
        {
            //try { DoSetFocus(ele); }catch (Exception) { }
            try
            {
                InvokePattern t = (InvokePattern)me.GetCurrentPattern(InvokePattern.Pattern);
                Thread invokeThread = new Thread(t.Invoke);
                invokeThread.Start();
                //t.Invoke();
            }
            catch (Exception ex)
            {
                throw new Exception("Click error. " + ex);
            }
            Thread.Sleep((int)(waitTime * 1000));
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="waitTime"></param>
        public void DoSetFocus(double waitTime = 0.1)
        {
            try
            {
                me.SetFocus();
                Thread.Sleep((int)(waitTime * 1000));
            }
            catch (Exception ex)
            {
                throw new Exception("SetFocus error. " + ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public WindowEvents DoWindowEvents()
        {
            try
            {
                return new WindowEvents(new AT(me));
            }
            catch (Exception ex)
            {
                throw new Exception("DoWindowEvents error." + ex);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class WindowEvents
        {
            /// <summary>
            /// 
            /// </summary>
            AT elePara;
            /// <summary>
            /// 
            /// </summary>
            WindowPattern WindowPattern;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="elePara"></param>
            public WindowEvents(AT elePara)
            {
                this.elePara = elePara;
                try
                {
                    WindowPattern = (WindowPattern)elePara.GetMe().GetCurrentPattern(WindowPattern.Pattern);
                }
                catch (Exception)
                {
                    throw new Exception("Failed to get WindowEvents.");
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="waitTime"></param>
            public void Normal(double waitTime = 0.1)
            {
                try
                {
                    WindowPattern.SetWindowVisualState(WindowVisualState.Normal);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to set WindowVisualState to Normal. " + ex);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="waitTime"></param>
            public void Maximized(double waitTime = 0.1)
            {
                try
                {
                    WindowPattern.SetWindowVisualState(WindowVisualState.Maximized);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to set WindowVisualState to Maximized. " + ex);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="waitTime"></param>
            public void Minimized(double waitTime = 0.1)
            {
                try
                {
                    WindowPattern.SetWindowVisualState(WindowVisualState.Minimized);
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to set WindowVisualState to Minimized. " + ex);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
            /// <summary>
            /// 
            /// </summary>
            /// <param name="waitTime"></param>
            public void Close(double waitTime = 0.1)
            {
                try
                {
                    WindowPattern.Close();
                }
                catch (Exception ex)
                {
                    throw new Exception("Failed to close the window. " + ex);
                }
                Thread.Sleep((int)(waitTime * 1000));
            }
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="strValue"></param>
        /// <param name="waitTime"></param>
        public void DoSetValue(string strValue, double waitTime = 0.1)
        {
            try { DoSetFocus(); }
            catch (Exception) { }
            try
            {
                ValuePattern tbTestBox = me.GetCurrentPattern(ValuePattern.Pattern) as ValuePattern;
                tbTestBox.SetValue("");
                Thread.Sleep((int)(0.3 * 1000));
                //PublicClass.Sendkeys(strValue);
                Thread.Sleep((int)(waitTime * 1000));
            }
            catch (Exception ex)
            {
                throw new Exception("Set value error. " + ex.Message);
            }
        }
        /// <summary>
        /// 
        /// </summary>
        public class CurrentElement
        {
            AutomationElement.AutomationElementInformation current;
            /// <summary>
            /// 
            /// </summary>
            AT elePara;
            /// <summary>
            /// 
            /// </summary>
            /// <param name="elePara"></param>
            public CurrentElement(AT elePara)
            {
                this.elePara = elePara;
                this.current = elePara.GetMe().Current;
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string ProcessId()
            {
                try
                {
                    return current.ProcessId.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get ProcessId. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool Exists()
            {
                try
                {
                    string t = this.ProcessId();
                    return true;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsEnabled()
            {
                try
                {
                    return current.IsEnabled;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get IsEnabled status. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public bool IsSelected()
            {
                try
                {
                    SelectionItemPattern t = (SelectionItemPattern)elePara.GetMe().GetCurrentPattern(SelectionItemPattern.Pattern);
                    return t.Current.IsSelected;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get isSelected status. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string GetSelectionName()
            {
                try
                {
                    SelectionPattern t = (SelectionPattern)elePara.GetMe().GetCurrentPattern(SelectionPattern.Pattern);
                    return t.Current.GetSelection()[0].Current.Name;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to GetSelection. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string Value()
            {
                try
                {
                    ValuePattern t = (ValuePattern)elePara.GetMe().GetCurrentPattern(ValuePattern.Pattern);
                    return t.Current.Value;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get Value. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string IsChecked()
            {
                try
                {
                    TogglePattern t = (TogglePattern)elePara.GetMe().GetCurrentPattern(TogglePattern.Pattern);
                    return t.Cached.ToggleState.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get isSelected status. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public IntPtr GetHwnd()
            {
                try
                {
                    return new IntPtr(elePara.GetMe().Current.NativeWindowHandle);
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get isSelected status. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string ToggleState()
            {
                try
                {
                    TogglePattern t = (TogglePattern)elePara.GetMe().GetCurrentPattern(TogglePattern.Pattern);
                    return t.Current.ToggleState.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get isSelected status. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string Name()
            {
                try
                {
                    return current.Name;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get Name. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string ClassName()
            {
                try
                {
                    return current.ClassName;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get ClassName. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string AutomationId()
            {
                try
                {
                    return current.AutomationId;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get AutomationId. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string ControlType()
            {
                try
                {
                    return current.ControlType.ToString();
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get ControlType. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public string FrameworkId()
            {
                try
                {
                    return current.FrameworkId;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get FrameworkId. {0}", ex));
                }
            }
            /// <summary>
            /// 
            /// </summary>
            /// <returns></returns>
            public Rect BoundingRectangle()
            {
                try
                {
                    return current.BoundingRectangle;
                }
                catch (Exception ex)
                {
                    throw new Exception(string.Format("Failed to get BoundingRectangle. {0}", ex));
                }
            }
        }
    }
}
