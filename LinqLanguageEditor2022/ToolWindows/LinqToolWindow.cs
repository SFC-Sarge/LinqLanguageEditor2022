using LinqLanguageEditor2022.Extensions;

using Microsoft.VisualStudio;
using Microsoft.VisualStudio.Imaging;
using Microsoft.VisualStudio.Shell.Interop;

using System.ComponentModel.Design;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Xml.Linq;

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
                //((MyToolWindowControl)this.Content).listBox.Items.Add("Entering OnAfterFirstDocumentLock");
                return VSConstants.S_OK;
            }

            public int OnBeforeLastDocumentUnlock(uint docCookie, uint dwRDTLockType, uint dwReadLocksRemaining, uint dwEditLocksRemaining)
            {
                //((MyToolWindowControl)this.Content).listBox.Items.Add("Entering OnBeforeLastDocumentUnlock");
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
                ThreadHelper.ThrowIfNotOnUIThread();
                var win = VsShellUtilities.GetWindowObject(pFrame);
                string currentFilePath = win.Document.Path;
                string currentFileTitle = win.Document.Name;
                string currentFileFullPath = System.IO.Path.Combine(currentFilePath, currentFileTitle);
                if (pFrame != null && currentFileTitle.EndsWith(".linq"))
                {
                    //string CurrentLinqFile = pFrame.ToString().Trim('{', '}');

                    ThreadHelper.ThrowIfNotOnUIThread();
                    ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                    {
                        Project project = await VS.Solutions.GetActiveProjectAsync();
                        if (project != null)
                        {
                            XDocument xdoc = XDocument.Load(project.FullPath);

                            try
                            {
                                if (!xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).IsNullOrEmpty())
                                {
                                    return;
                                }
                                if (!xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).IsNullOrEmpty())
                                {
                                    xdoc.Descendants("ItemGroup").Descendants("None").Where(rec => rec.Attribute("Include").Value == currentFileFullPath).Remove();
                                    xdoc.Save(project.FullPath);

                                }
                                if (!xdoc.Descendants("ItemGroup").Descendants("Compile").IsNullOrEmpty())
                                {
                                    var newCompileItem = xdoc.Descendants("ItemGroup").Descendants("Compile").First(x => x.HasAttributes);
                                    newCompileItem.AddAfterSelf(new XElement("Compile", new XAttribute("Include", currentFileFullPath)));
                                    xdoc.Save(project.FullPath);
                                }
                                else
                                {
                                    XElement itemGroup = new XElement("ItemGroup");
                                    XElement compile = new XElement("Compile", new XAttribute("Include", currentFileFullPath));
                                    itemGroup.Add(compile);
                                    xdoc.Element("Project").Add(itemGroup);
                                    xdoc.Save(project.FullPath);

                                }
                                xdoc.Save(project.FullPath);
                            }
                            catch (Exception ex)
                            {

                            }
                            finally
                            {
                                xdoc.Save(project.FullPath);
                            }
                            await project.SaveAsync();
                        }
                    }).FireAndForget();
                }
                return VSConstants.S_OK;
            }

            public int OnAfterDocumentWindowHide(uint docCookie, IVsWindowFrame pFrame)
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                var win = VsShellUtilities.GetWindowObject(pFrame);
                if (pFrame != null && win.Caption.EndsWith(".linq"))
                {
                    //string CurrentLinqFile = pFrame.ToString().Trim('{', '}');
                    ThreadHelper.ThrowIfNotOnUIThread();
                    ThreadHelper.JoinableTaskFactory.RunAsync(async () =>
                    {
                        Project project = await VS.Solutions.GetActiveProjectAsync();
                        if (project != null)
                        {
                            XDocument xdoc = XDocument.Load(project.FullPath);
                            try
                            {
                                xdoc.Descendants("ItemGroup").Descendants("Compile").Where(rec =>
                                {
                                    ThreadHelper.ThrowIfNotOnUIThread();
                                    return rec.Attribute("Include").Value.EndsWith(win.Caption);
                                }).Remove();
                                xdoc.Save(project.FullPath);
                            }
                            catch (Exception ex)
                            {
                            }
                            finally
                            {
                                xdoc.Save(project.FullPath);
                            }
                            await project.SaveAsync();
                        }
                    }).FireAndForget();
                }
                return VSConstants.S_OK;
            }
            protected override void Dispose(bool disposing)
            {
                ThreadHelper.ThrowIfNotOnUIThread();
                // Release the RDT cookie.
                //IVsRunningDocumentTable rdt = (IVsRunningDocumentTable)
                //    Package..GetGlobalService(typeof(SVsRunningDocumentTable));
                //rdt.UnadviseRunningDocTableEvents(rdtCookie);

                base.Dispose(disposing);
            }

        }
    }
}