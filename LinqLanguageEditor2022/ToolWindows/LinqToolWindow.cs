
using System.Collections.Generic;
using System.ComponentModel.Design;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

using LinqLanguageEditor2022.Extensions;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell.Interop;

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

        [Guid(Constants.PaneGuid)]
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
                            //((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterFirstDocumentLock: {activeItem.Name}");
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
                            //((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeLastDocumentUnlock: {activeItem.Name}");
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
                            //((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterSave: {activeItem.Name}");
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
                            //((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterAttributeChange: {activeItem.Name}");
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
                    var activeItem = await VS.Solutions.GetActiveItemAsync();
                    win = VsShellUtilities.GetWindowObject(pFrame);
                    string currentFilePath = win.Document.Path;
                    string currentFileTitle = win.Document.Name;
                    string currentFileFullPath = System.IO.Path.Combine(currentFilePath, currentFileTitle);
                    if (pFrame != null && currentFileTitle.EndsWith(Constants.LinqExt))
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
                                    xdoc = RemoveEmptyItemGroupNode(xdoc);
                                    xdoc.Save(project.FullPath);
                                    await project.SaveAsync();
                                    xdoc = XDocument.Load(project.FullPath);
                                }
                                catch (Exception)
                                { }
                                if (ItemGroupExists(xdoc, Constants.ProjectItemGroup, Constants.ProjectCompile))
                                {
                                    try
                                    {
                                        if (CompileItemExists(xdoc, currentFileTitle))
                                        {
                                            xdoc = UpdateItemGroupItem(xdoc, currentFileTitle, currentFileFullPath);
                                        }
                                        else if (ItemGroupExists(xdoc, Constants.ProjectItemGroup, Constants.ProjectNone))
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
                                else if (ItemGroupExists(xdoc, Constants.ProjectItemGroup, Constants.ProjectNone))
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
                    xdoc.Descendants(Constants.ProjectItemGroup).Where(rec => rec.Nodes().IsNullOrEmpty()).Remove();
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
                    if (!xdoc.Descendants(Constants.ProjectItemGroup).Descendants(Constants.ProjectNone).Where(rec => rec.Attribute(Constants.ProjectInclude).Value.EndsWith(currentFileTitle)).IsNullOrEmpty())
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
                    if (!xdoc.Descendants(Constants.ProjectItemGroup).Descendants(Constants.ProjectCompile).Where(rec => rec.Attribute(Constants.ProjectInclude).Value.EndsWith(currentFileTitle)).IsNullOrEmpty())
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
                var newCompileItem = xdoc.Descendants(Constants.ProjectItemGroup).Descendants(Constants.ProjectCompile).First(x => x.HasAttributes);
                newCompileItem.AddAfterSelf(new XElement(Constants.ProjectCompile, new XAttribute(Constants.ProjectInclude, currentFileFullPath)));
                return xdoc;
            }
            public XDocument CreateNewItemGroup(XDocument xdoc, string currentFileFullPath)
            {
                XElement itemGroup = new(Constants.ProjectItemGroup);
                XElement compile = new(Constants.ProjectCompile, new XAttribute(Constants.ProjectInclude, currentFileFullPath));
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
                    IEnumerable<XElement> xObj = xdoc.Descendants(Constants.ProjectItemGroup).Descendants(Constants.ProjectNone).Where(rec => rec.Attribute(Constants.ProjectInclude).Value == currentFileFullPath);

                    foreach (var element in xObj)
                    {
                        element.Name = Constants.ProjectCompile;
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
                    xdoc.Descendants(Constants.ProjectItemGroup).Descendants(Constants.ProjectCompile).Where(rec =>
                    {
                        ThreadHelper.ThrowIfNotOnUIThread();
                        return rec.Attribute(Constants.ProjectInclude).Value.EndsWith(caption);
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
                            //(LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    { }
                    try
                    {
                        var win = VsShellUtilities.GetWindowObject(pFrame);
                        if (win != null)
                        {
                            //((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {win.Caption}");
                        }
                    }
                    catch (Exception)
                    { }
                    win = VsShellUtilities.GetWindowObject(pFrame);
                    if (pFrame != null && win.Caption.EndsWith(Constants.LinqExt))
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
