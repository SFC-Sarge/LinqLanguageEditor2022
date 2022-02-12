# LinqLanguge

<!--Start-Of-TOC-->
   - [My Linq Query Tool Window](#My-Linq-Query-Tool-Window)
   - [LinqPad Dump](#LinqPad-Dump)
   - [Sample results from the following document view method selection](#Sample-results-from-the-following-document-view-method-selection)
   - [Run Selected Query Method](#Run-Selected-Query-Method)
   - [Query Method Results in Toolwindow example](#Query-Method-Results-in-Toolwindow-example)
   - [Query Method Results in LinqPad Dump window example](#Query-Method-Results-in-LinqPad-Dump-window-example)
   - [Sample results from the following document view statement selection](#Sample-results-from-the-following-document-view-statement-selection)
   - [Run Selected Query Statement](#Run-Selected-Query-Statement)
   - [Query Statement Results in Toolwindow example](#Query-Statement-Results-in-Toolwindow-example)
   - [Query Statement Results in LinqPad Dump window example](#Query-Statement-Results-in-LinqPad-Dump-window-example)
   - [LPRun7 Info](#LPRun7-Info)
<!--End-Of-TOC-->



Linq Editor for Visual Studio is a Visual Studio 2022 Extension, that allows developer to run a selected linq query or linq method in the active document, and display those results in a Visual Studio ToolWindow.

>Note: This is not a replacement for [LinqPad](https://www.linqpad.net/), which in my opinon is the best Linq Query builder/tester on the market. I recommend you try LinqPad and then purchase a license for it.

This works by using "LinqPad Launchers for Command-Line Support"! LPRun7-x64.exe runs the query and then returns the script and results of the script to the Visual Studio 2022 Toolwindow called:

## My Linq Query Tool Window 

![My Linq Query Tool Window](https://user-images.githubusercontent.com/67446778/148121369-fac645c0-009b-46a6-9db3-516b87e11d1e.png)


It also currently creates a Output window called:

## LinqPad Dump

![LinqPad Dump](https://user-images.githubusercontent.com/67446778/148121472-8676afc8-faaf-4313-ac5e-1b00da586d46.png)



I had to use LPRun7-x64.exe instead of LinqPad.Util.Run found in NuGet packages LinqPad (.net 4.8) and LinqPad.Runtime (.net core 3.0) because even thou I could get the code to run in an .net 4.8 or .net core 6.0 application and returned correct query results, but when using the same code in a Viusal Studio extension, the results returned are always an empty string result for the query.

I do not see this issue when using LPRun7-x64.exe to replace the Nuget package. 

>Note: I will revisit this issue once the [LinqPad](https://www.linqpad.net/) author releases a NuGet package that supports Visual Studio 2022 x64.

![LinqPad NuGet Packages](https://user-images.githubusercontent.com/67446778/148120404-9e35b180-89d9-4bf3-9bf4-b80765460768.png)


The Toolwindow Toolbar button calls the following using process:

`LPRun7-x64.exe -fx=6.0 (selected query to run)   // Run query under .NET 6.0`


 ```csharp
{
   using Process process = new();
    process.StartInfo = new ProcessStartInfo()
    {
        UseShellExecute = false,
        CreateNoWindow = true,
        WindowStyle = ProcessWindowStyle.Hidden,
        FileName = fileLPRun7,
        Arguments = $"{fileLPRun7Args} {tempQueryPath}",
        RedirectStandardError = true,
        RedirectStandardOutput = true
    };
    process.Start();
    queryResult = await process.StandardOutput.ReadToEndAsync();
    process.WaitForExit();
  }
  ```


and returns the query results to the **My Linq Query Tool Window** and **LinqPad Dump** windows.

>Important Note: When running a selected Linq Query Statement you must have the value to be returned assigned to "result". This is becuase, I have not been able to determine from a multi-line statement which result you want returned so which ever statement run will only return the "result". 
>i.e. The following code snippet has a result variable: var result. This is the result and get returned, if this is not named result then you will get an empty return value from the Linq Query Statement.

This example works and returns a result.

```csharp
    List<string> vegetables = new List<string> { "Cucumber", "Tomato", "Broccoli" };

    var result = vegetables.Cast<string>();
```

The above query works and returns:

![Working Statement](https://user-images.githubusercontent.com/67446778/148125528-55657e42-7621-4d28-86b9-55a7be497dd0.png)

This example does not work since the `var value` has the value, not a variable called result.

```csharp
    List<string> vegetables = new List<string> { "Cucumber", "Tomato", "Broccoli" };

    var value = vegetables.Cast<string>();
```

The above query does not work returns nothing but the script and an empty result.

![Not Working Statement](https://user-images.githubusercontent.com/67446778/148125718-ac97ef7f-343c-4304-84e3-1816ecebd929.png)

>Note: The above statement issue is not a problem for the Method selection since it returns all the `Debug.WriteLine` statements listed in the selected method.


## Sample results from the following document view method selection


![Linq Method Selection](https://user-images.githubusercontent.com/67446778/148121805-81419e3a-054d-4d7b-8f61-e3f3ce5557cb.png)


## Run Selected Query Method


![Run Selected Query Method](https://user-images.githubusercontent.com/67446778/148122715-f677bdd8-7895-498a-9d00-4f86a97dea2b.png)


## Query Method Results in Toolwindow example


![Method Results](https://user-images.githubusercontent.com/67446778/148122003-2c67de36-ee76-4f19-9ab8-3583f96f24ac.png)


## Query Method Results in LinqPad Dump window example


![Query Method Results in LinqPad Dump](https://user-images.githubusercontent.com/67446778/148122240-a5f74919-2001-4bcb-8776-cb07836d0d5c.png)


## Sample results from the following document view statement selection


![Query Statement Selection](https://user-images.githubusercontent.com/67446778/148122952-1893a4d7-cf2f-452c-8e49-0dce8ad7aee3.png)


## Run Selected Query Statement


![Run Selected Query Statement](https://user-images.githubusercontent.com/67446778/148123089-5b0ee5b0-8099-4f84-bf34-e518869c3384.png)


## Query Statement Results in Toolwindow example


![Query Statement Results in Toolwindow](https://user-images.githubusercontent.com/67446778/148123280-6c683763-ab63-441b-9239-edb729e2e9a2.png)



## Query Statement Results in LinqPad Dump window example


![Query Statement Results in LinqPad Dump window](https://user-images.githubusercontent.com/67446778/148123386-17154680-8a19-4171-a382-df701d6139f8.png)



## LPRun7 Info  

[LPRun7](https://www.linqpad.net/)


![LPRun7](https://user-images.githubusercontent.com/67446778/148120780-69d97423-63e8-4a08-8638-a9ceb6dd0f39.png)


> Note: You must have [Community.VisualStudio.Toolkit](https://github.com/VsixCommunity/Community.VisualStudio.Toolkit) installed for your extension project.
