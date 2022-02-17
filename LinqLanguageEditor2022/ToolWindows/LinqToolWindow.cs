using LinqLanguageEditor2022.Extensions;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell.Interop;

using System.Collections.Generic;
using System.ComponentModel.Design;
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
        internal class Pane : ToolWindowPane, IVsWindowFrameNotify3, IVsRunningDocTableEvents
        {
            private uint rdtCookie;
            protected override void Initialize()
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    // Create the RDT cookie.
                    IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)
                    this.GetService(typeof(SVsRunningDocumentTable));
                    rdt.AdviseRunningDocTableEvents(this, out rdtCookie);
                }).FireAndForget();
            }
            public Pane()
            {
                BitmapImageMoniker = KnownMonikers.ToolWindow;
                ToolBar = new CommandID(PackageGuids.LinqLanguageEditor2022, PackageIds.LinqTWindowToolbar);
            }

            public int OnShow(int fShow)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnShow GetActiveItemAsync: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    {

                    }
                }).FireAndForget();

                return VSConstants.S_OK;
            }

            public int OnMove(int x, int y, int w, int h)
            {
                return VSConstants.S_OK;
            }

            public int OnSize(int x, int y, int w, int h)
            {
                return VSConstants.S_OK;
            }

            public int OnDockableChange(int fDockable, int x, int y, int w, int h)
            {
                return VSConstants.S_OK;
            }

            public int OnClose(ref uint pgrfSaveOptions)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnClose GetActiveItemAsync: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    {
                    }
                }).FireAndForget();
                return VSConstants.S_OK;
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
                    {

                    }
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
                    {

                    }
                }).FireAndForget();
                return VSConstants.S_OK;
            }

            public int OnAfterSave(uint docCookie)
            {
                return VSConstants.S_OK;
            }

            public int OnAfterAttributeChange(uint docCookie, uint grfAttribs)
            {
                return VSConstants.S_OK;
            }

            public int OnBeforeDocumentWindowShow(uint docCookie, int fFirstShow, IVsWindowFrame pFrame)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeDocumentWindowShow: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        var win = VsShellUtilities.GetWindowObject(pFrame);
                        if (win != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnBeforeDocumentWindowShow: {win.Caption}");
                        }

                    }
                    catch (Exception)
                    {

                    }
                    //ThreadHelper.ThrowIfNotOnUIThread();
                    //var win = VsShellUtilities.GetWindowObject(pFrame);
                    //string currentFilePath = win.Document.Path;
                    //string currentFileTitle = win.Document.Name;
                    //string currentFileFullPath = System.IO.Path.Combine(currentFilePath, currentFileTitle);
                    //if (pFrame != null && currentFileTitle.EndsWith(".linq"))
                    //{
                    //    ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                    //    {
                    //        Project project = await VS.Solutions.GetActiveProjectAsync();
                    //        if (project != null)
                    //        {
                    //            XDocument xdoc = XDocument.Load(project.FullPath);
                    //            try
                    //            {
                    //                await project.AddExistingFilesAsync(currentFileFullPath);
                    //                if (xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).IsNullOrEmpty())
                    //                {
                    //                    if (xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).IsNullOrEmpty())
                    //                    {
                    //                        //await project.AddExistingFilesAsync(currentFileFullPath);
                    //                        //await project.SaveAsync();
                    //                    }
                    //                }
                    //                else if (!xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).IsNullOrEmpty())
                    //                {
                    //                    //xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).Remove();
                    //                    IEnumerable<XElement> xObj = xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath);

                    //                    foreach (var element in xObj)
                    //                    {
                    //                        element.Name = "Compile";
                    //                    }

                    //                    xdoc.Save(project.FullPath);

                    //                }
                    //                else if (!xdoc.Descendants("ItemGroup").Descendants("Compile").IsNullOrEmpty())
                    //                {
                    //                    var newCompileItem = xdoc.Descendants("ItemGroup").Descendants("Compile").First(x => x.HasAttributes);
                    //                    newCompileItem.AddAfterSelf(new XElement("Compile", new XAttribute("Include", currentFileFullPath)));
                    //                    xdoc.Save(project.FullPath);
                    //                }
                    //                else
                    //                {
                    //                    XElement itemGroup = new XElement("ItemGroup");
                    //                    XElement compile = new XElement("Compile", new XAttribute("Include", currentFileFullPath));
                    //                    itemGroup.Add(compile);
                    //                    xdoc.Element("Project").Add(itemGroup);
                    //                    xdoc.Save(project.FullPath);
                    //                }
                    //                xdoc.Save(project.FullPath);

                    //            }
                    //            catch (Exception ex)
                    //            {

                    //            }
                    //            finally
                    //            {
                    //                xdoc.Save(project.FullPath);
                    //            }
                    //            try
                    //            {
                    //                //xdoc.Descendants("ItemGroup").Where(rec => rec.Nodes().IsNullOrEmpty()).Remove();
                    //            }
                    //            catch (Exception ex)
                    //            {
                    //            }
                    //            finally
                    //            {
                    //                xdoc.Save(project.FullPath);
                    //            }
                    //            await project.SaveAsync();
                    //        }
                    //    }).FireAndForget();
                    //}
                }).FireAndForget();
                return VSConstants.S_OK;
            }

            public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await Microsoft.VisualStudio.Shell.ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    try
                    {
                        var activeItem = await VS.Solutions.GetActiveItemAsync();
                        if (activeItem != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {activeItem.Name}");
                        }
                    }
                    catch (Exception)
                    {

                    }
                    try
                    {
                        var win = VsShellUtilities.GetWindowObject(pFrame);
                        if (win != null)
                        {
                            ((LinqToolWindowControl)this.Content).LinqlistBox.Items.Add($"OnAfterDocumentWindowHide: {win.Caption}");
                        }
                    }
                    catch (Exception)
                    {

                    }
                    //ThreadHelper.ThrowIfNotOnUIThread();
                    //var win = VsShellUtilities.GetWindowObject(pFrame);
                    //if (pFrame != null && win.Caption.EndsWith(".linq"))
                    //{
                    //ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                    //{
                    //    Project project = await VS.Solutions.GetActiveProjectAsync();
                    //    if (project != null)
                    //    {
                    //        XDocument xdoc = XDocument.Load(project.FullPath);
                    //        try
                    //        {
                    //            xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec => rec.Attribute("Include").Value.EndsWith(win.Caption)).Remove();
                    //            xdoc.Save(project.FullPath);
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //        }
                    //        finally
                    //        {
                    //            xdoc.Save(project.FullPath);
                    //        }
                    //        try
                    //        {
                    //            xdoc.Descendants("ItemGroup").Where(rec => rec.Nodes().IsNullOrEmpty()).Remove();
                    //        }
                    //        catch (Exception ex)
                    //        {
                    //        }
                    //        finally
                    //        {
                    //            xdoc.Save(project.FullPath);
                    //        }
                    //        await project.SaveAsync();
                    //    }
                    //}).FireAndForget();
                    //}
                }).FireAndForget();

                return VSConstants.S_OK;
            }
            protected override void Dispose(bool disposing)
            {
                ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                {
                    await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync();
                    // Release the RDT cookie.
                    IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)
                    this.GetService(typeof(SVsRunningDocumentTable));
                    rdt.UnadviseRunningDocTableEvents(rdtCookie);
                }).FireAndForget();
                base.Dispose(disposing);
            }

        }

    }
}