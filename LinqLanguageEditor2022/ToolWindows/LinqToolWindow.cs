using Community.VisualStudio.Toolkit;

using LinqLanguageEditor2022.Extensions;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;

using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

using static System.Net.Mime.MediaTypeNames;

namespace LinqLanguageEditor2022.ToolWindows
{
    public class LinqToolWindow : BaseToolWindow<LinqToolWindow>
    {
        public override string GetTitle(int toolWindowId) => Constants.LinqEditorToolWindowTitle;

        public override Type PaneType => typeof(Pane);

        public override async Task<FrameworkElement> CreateAsync(int toolWindowId, CancellationToken cancellationToken)
        {
            Project project = await VS.Solutions.GetActiveProjectAsync();
            LinqToolWindowMessenger toolWindowMessenger = await Package.GetServiceAsync<LinqToolWindowMessenger, LinqToolWindowMessenger>();
            return new LinqToolWindowControl(project, toolWindowMessenger);
        }

        [Guid("A938BB26-03F8-4861-B920-6792A7D4F07C")]
        internal class Pane : ToolWindowPane, IVsRunningDocTableEvents
        {

            private uint rdtCookie;
            private EnvDTE.Window win;
            protected override void Initialize()
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)
                this.GetService(typeof(SVsRunningDocumentTable));
                rdt.AdviseRunningDocTableEvents(this, out rdtCookie);
            }

            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
                ToolBar = new CommandID(PackageGuids.LinqLanguageEditor2022, PackageIds.LinqTWindowToolbar);
            }
            public int OnAfterFirstDocumentLock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterFirstDocumentLock: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                }).FireAndForget();
                return VSConstants.S_OK;
            }

            public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeLastDocumentUnlock: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                }).FireAndForget();
                return VSConstants.S_OK;
            }

            public int OnAfterSave(uint docCookie)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterSave: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                }).FireAndForget();

                return VSConstants.S_OK;
            }

            public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterAttributeChange: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                }).FireAndForget();

                return VSConstants.S_OK;
            }

            public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeDocumentWindowShow: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        win = VsShellUtilities.GetWindowObject(pFrame);
                        if (win != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeDocumentWindowShow: {win.Caption}");
                        }
                    }
                    catch (Exception)
                    { }
                    win = VsShellUtilities.GetWindowObject(pFrame);
                    string currentFilePath = win.Document.Path;
                    string currentFileTitle = win.Document.Name;
                    string currentFileFullPath = System.IO.Path.Combine(currentFilePath, currentFileTitle);
                    if (pFrame != null && currentFileTitle.EndsWith(".linq"))
                    {
                        ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                        {
                            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                            //string content = String.Empty;
                            //using (var reader = new StreamReader(win.Document.FullName))
                            //{
                            //    content = await reader.ReadToEndAsync();
                            //    if (content.StartsWith("<Query Kind="))
                            //    {
                            //        content = $"//{content}";
                            //    }
                            //}
                            //using (var writer = new StreamWriter(win.Document.FullName))
                            //{
                            //    await writer.WriteAsync(content);
                            //}
                            Project project = await VS.Solutions.GetActiveProjectAsync();
                            if (project != null)
                            {
                                XDocument xdoc = XDocument.Load(project.FullPath);
                                try
                                {
                                    xdoc = RemoveEmptyItemGroupNode(xdoc);
                                    xdoc.Save(project.FullPath);
                                    await project.SaveAsync();
                                    xdoc = XDocument.Load(project.FullPath);
                                }
                                catch (Exception)
                                { }
                                if (ItemGroupExists(xdoc, "ItemGroup", "Compile"))
                                {
                                    try
                                    {
                                        if (CompileItemExists(xdoc, currentFileTitle))
                                        {
                                            xdoc = UpdateItemGroupItem(xdoc, currentFileTitle, currentFileFullPath);
                                        }
                                        else if (ItemGroupExists(xdoc, "ItemGroup", "None"))
                                        {
                                            try
                                            {
                                                if (NoneCompileItemExists(xdoc, currentFileTitle))
                                                {
                                                    xdoc = UpdateItemGroupItem(xdoc, currentFileTitle, currentFileFullPath);
                                                }
                                                else
                                                {
                                                    xdoc = CreateNewCompileItem(xdoc, currentFileFullPath);
                                                }
                                                xdoc.Save(project.FullPath);
                                                await project.SaveAsync();
                                                xdoc = XDocument.Load(project.FullPath);
                                            }
                                            catch (Exception)
                                            { }
                                        }
                                        else
                                        {
                                            xdoc = CreateNewCompileItem(xdoc, currentFileFullPath);
                                        }
                                        xdoc.Save(project.FullPath);
                                        await project.SaveAsync();
                                        xdoc = XDocument.Load(project.FullPath);
                                    }
                                    catch (Exception)
                                    { }
                                }
                                else if (ItemGroupExists(xdoc, "ItemGroup", "None"))
                                {
                                    try
                                    {
                                        if (NoneCompileItemExists(xdoc, currentFileTitle))
                                        {
                                            xdoc = UpdateItemGroupItem(xdoc, currentFileTitle, currentFileFullPath);
                                        }
                                        else
                                        {
                                            xdoc = CreateNewCompileItem(xdoc, currentFileFullPath);
                                        }
                                        xdoc.Save(project.FullPath);
                                        await project.SaveAsync();
                                        xdoc = XDocument.Load(project.FullPath);
                                    }
                                    catch (Exception)
                                    { }
                                }
                                else
                                {
                                    xdoc = CreateNewItemGroup(xdoc, currentFileFullPath);
                                    xdoc.Save(project.FullPath);
                                    await project.SaveAsync();
                                }
                            }
                        }).FireAndForget();
                    }
                }).FireAndForget();
                return VSConstants.S_OK;
            }
            public XDocument RemoveEmptyItemGroupNode(XDocument xdoc)
            {
                try
                {
                    xdoc.Descendants("ItemGroup").Where(rec => rec.Nodes().IsNullOrEmpty()).Remove();
                    return xdoc;
                }
                catch (Exception)
                { }
                return xdoc;
            }

            public bool ItemGroupExists(XDocument xdoc, string groupName, string compile)
            {
                try
                {
                    if (!xdoc.Descendants(groupName).Descendants(compile).IsNullOrEmpty())
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public bool NoneCompileItemExists(XDocument xdoc, string currentFileTitle)
            {
                try
                {
                    if (!xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value.EndsWith(currentFileTitle)).IsNullOrEmpty())
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }
            public bool CompileItemExists(XDocument xdoc, string currentFileTitle)
            {
                try
                {
                    if (!xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec => rec.Attribute("Include").Value.EndsWith(currentFileTitle)).IsNullOrEmpty())
                    {
                        return true;
                    }
                    return false;
                }
                catch (Exception)
                {
                    return false;
                }
            }

            public XDocument CreateNewCompileItem(XDocument xdoc, string currentFileFullPath)
            {
                var newCompileItem = xdoc.Descendants("ItemGroup").Descendants("Compile").First(x => x.HasAttributes);
                newCompileItem.AddAfterSelf(new XElement("Compile", new XAttribute("Include", currentFileFullPath)));
                return xdoc;
            }
            public XDocument CreateNewItemGroup(XDocument xdoc, string currentFileFullPath)
            {
                XElement itemGroup = new("ItemGroup");
                XElement compile = new("Compile", new XAttribute("Include", currentFileFullPath));
                itemGroup.Add(compile);
                xdoc.Element("Project").Add(itemGroup);
                return xdoc;
            }
            public XDocument UpdateItemGroupItem(XDocument xdoc, string currentFileTitle, string currentFileFullPath)
            {
                if (!NoneCompileItemExists(xdoc, currentFileFullPath))
                {
                    currentFileFullPath = currentFileTitle;
                }
                try
                {
                    IEnumerable<XElement> xObj = xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath);

                    foreach (var element in xObj)
                    {
                        element.Name = "Compile";
                    }
                    return xdoc;
                }
                catch (Exception)
                { }
                return xdoc;
            }
            public XDocument RemoveCompileItem(XDocument xdoc, string caption)
            {
                try
                {
                    xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec =>
                    {
                        ThreadHelper.ThrowIfNotOnUIThread();
                        return rec.Attribute("Include").Value.EndsWith(caption);
                    }).Remove();
                    return xdoc;
                }
                catch (Exception)
                { }
                return xdoc;
            }
            public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        var win = VsShellUtilities.GetWindowObject(pFrame);
                        if (win != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {win.Caption}");
                        }
                    }
                    catch (Exception)
                    { }
                    win = VsShellUtilities.GetWindowObject(pFrame);
                    if (pFrame != null && win.Caption.EndsWith(".linq"))
                    {
                        ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                        {
                            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                            Project project = await VS.Solutions.GetActiveProjectAsync();
                            if (project != null)
                            {
                                XDocument xdoc = XDocument.Load(project.FullPath);
                                try
                                {
                                    xdoc = RemoveCompileItem(xdoc, win.Caption);
                                    xdoc.Save(project.FullPath);
                                }
                                catch (Exception)
                                { }
                                try
                                {
                                    xdoc = RemoveEmptyItemGroupNode(xdoc);
                                    xdoc.Save(project.FullPath);
                                }
                                catch (Exception)
                                {
                                }
                                xdoc.Save(project.FullPath);
                                await project.SaveAsync();
                            }
                        }).FireAndForget();
                    }
                }).FireAndForget();
                return VSConstants.S_OK;
            }
            protected override void Dispose(bool disposing)
            {
                // Release the RDT cookie.
                ThreadHelper.ThrowIfNotOnUIThread();
                IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)
                this.GetService(typeof(SVsRunningDocumentTable));
                rdt.UnadviseRunningDocTableEvents(rdtCookie);

                base.Dispose(disposing);
            }
        }

    }
}