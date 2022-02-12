# Researching the Custom Editor Walkthrough Solution

<!--Start-Of-TOC-->
   - [My questions are:](#My-questions-are:)
<!--End-Of-TOC-->


I am including [Leslie Richardson](https://github.com/leslierichardson95) and [Mads
Kristensen](@mkristensen) in this question since they are the best resource
for Extending Visual Studio 2022.

Hi Leslie, my name is [Danny McNaught](mailto:danny.mcnaught@dannymcnaught.com)
I have been a developer for over 25 years, but this is the first time I am
creating an Editor Extension for Visual Studio. So, when I offered to contribute
to [Mads Kristensen’s](@mkristensen) VSIX Cookbook; a complete
‘Walkthrough’ on how to include a Language Editor in a Visual Studio 2022
Extension. I found documentation that sends you everywhere but the right place.
I can create the Editor Extension using the LanguageFactory using Mads examples
and it works great. The issue that I am finding is that the current available
[MEF](https://docs.microsoft.com/en-us/dotnet/framework/mef/) documentation
dated 11/06/2021 does not provide any examples/walkthroughs on how to add syntax
coloring (CSharp file like code coloring) using MEF, there are examples for
legacy language services using IScanner, Colorizer… But they do not provide
information on using the IScanner or Colorizer in MEF. The Legacy Language
Services documentation also states:

![Text Description automatically
generated](media/c124b315274f77c03410bf4703606152.png)

Then on top of that when you follow the [Managed Extensibility Framework
(MEF)](https://docs.microsoft.com/en-us/dotnet/framework/mef/) and to the [Next
steps](https://docs.microsoft.com/en-us/dotnet/framework/mef/#next-steps)
section the included link for: For more information and code examples, see
[Managed Extensibility Framework](https://github.com/MicrosoftArchive/mef)

This link takes you to GitHub where the Readme.md states:

>   [MEF](https://github.com/MicrosoftArchive/mef#mef)

>   This project has been migrated from CodePlex and isn't actively maintained.
>   It's provided for archival purposes only.

>   You can find the original wiki home page
>   [here](https://github.com/microsoftarchive/mef/blob/master/Wiki/Home.md).

>   [Replaced by
>   CoreFx](https://github.com/MicrosoftArchive/mef#replaced-by-corefx)

>   If you're looking for the current MEF code base, you should go to
>   [CoreFx](https://github.com/dotnet/corefx).

This CoreFx link then takes you to another broken GitHub page that states:

>   Going forward, the .NET team is using
>   [https://github.com/dotnet/runtime](https://github.com/dotnet/runtime%20) to
>   develop the code and issues formerly in this repository.

>   Please see the following for more context:

>   [dotnet/announcements\#119 "Consolidating .NET GitHub
>   repos"](https://github.com/dotnet/announcements/issues/119)

Now that we are at a currently supported GitHub site, but remember I wanted
information on MEF and Visual Studio 2022. I was dropped at the root of the
dotnet/runtime site.

I am still using these documents no closer than I was to getting the CSharp
syntax highlighting I am wanting for the walkthrough in the VSIX Cookbook.

The walkthrough: VSIXLinqLanguageEditor2022 ([Sample code is located
here](https://github.com/SFC-Sarge/VSIXLinqLangugeEditor2022)) 
> Please forgive the spelling error in GitHub project name, not sure how to change 
the name once it is create in GitHub.

Once I get the code to work as expected then 
I will create the documentation on how to do it
for the VSIX Cookbook). I have tried many things so the sample has a lot of
extra stuff that would be removed once I get it working correctly. (i.e.
LinqEditorLightBulb.cs, etc.)

The Walkthrough I am creating for the VSIX Cookbook is as follows:

1.  Visual Studio 2022 Extension

2.  Language Editor using the LanguageFactory from theHu
    [Community.VisualStudio.Toolkit.LanguageBase](https://github.com/VsixCommunity/Community.VisualStudio.Toolkit).

3.  ToolWindow allows you to select a line of LINQ code from your open CSharp
    file, select a method that contains a LINQ query, or open a .linq file.

4.  The ToolWindow command then opens your selection in a new Editor window
    where you can modify/test it and get and display query results from the LINQ
    query inside of Visual Studio. *(Note: Currently works using LinqPad’s
    “LPRun7-x64.exe, and LINQPad.Runtime.dll”)*

5.  The new Editor window with the file extension .linq instead of .cs contains
    you code. This still works as expected and allows you to get the results of
    the query in the ToolWindow or a Dump Window based on Options settings for
    the linq Text Editor.

6.  What does not work is the code syntax colorization of the code.  
    Test.cs file look like this:  
    ![Text Description automatically
    generated](media/62f8ccded5d1d0d5b5279bd386ccac37.png)  
    So if I select the private static void method above and click the
    ![](media/31880e84106e0cc841b6e0422ca46131.png) button the new editor will
    display the private static void method and the ToolWindow will display the
    results.   
    ![A screenshot of a computer Description automatically generated with medium
    confidence](media/ddb0b8eff4f290fed90a759f3c72bd88.png)  
    But the new editor window does not have any syntax colorization.

    > NOTE: I have noticed that if I take a CSharp (.cs) file in the solution copy
    it back in the solution and rename the extension to (.linq) all the
    colorization remains. This does not work if I open a .linq file or create
    the .linq file from the selection.

    ## My questions are:

    How to you get CSharp style colorization to work in MEF and Visual Studio
    2022 Editor extension?

    Can you use IScanner and IVsColorizer in a MEF Extension? If so, how do you,
    [Import] [Export] IScanner and IVsColorizer in your MEF package?

    Do you currently have documentation on using MEF package to colorize your code?
