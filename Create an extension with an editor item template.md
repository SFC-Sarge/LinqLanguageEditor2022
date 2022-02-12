# Create an extension with an editor item template

Article

08/05/2021

You can use item templates that are included in the Visual Studio SDK to create
basic editor extensions that add classifiers, adornments, and margins to the
editor. The editor item templates are available for Visual C\# or Visual Basic
VSIX projects.

## Prerequisites

Starting in Visual Studio 2015, you do not install the Visual Studio SDK from
the download center. It is included as an optional feature in Visual Studio
setup. You can also install the VS SDK later. For more information, see [Install
the Visual Studio
SDK](https://docs.microsoft.com/en-us/visualstudio/extensibility/installing-the-visual-studio-sdk?view=vs-2022).

## Create a classifier extension

The Editor Classifier item template creates an editor classifier that colors the
appropriate text (in this case, everything) in any text file.

In the New Project dialog box, expand Visual C\# or Visual Basic and then click
Extensibility. In the Templates pane, select VSIX Project. In the Name box, type
TestClassifier. Click OK.

In the Solution Explorer, right-click the project node and select Add \> New
Item. Go to the Visual C\# Extensibility node and select Editor Classifier.
Leave the default file name (*EditorClassifier1.cs*).

There are four code files, as follows:

1.  *EditorClassifier1.cs* contains the EditorClassifier1 class.

2.  *EditorClassifier1ClassificationDefinition.cs* contains the
    EditorClassifier1ClassificationDefinition class.

3.  *EditorClassifier1Format.cs* contains the EditorClassifier1Format class.

4.  *EditorClassifier1Provider.cs* contains the EditorClassifier1Provider class.

Build the project and start debugging. The experimental instance of Visual
Studio appears.

If you open a text file, all the text is underlined against a violet background.

## Create a text-relative adornment extension

The Editor Text Adornment template creates a text-relative adornment that
decorates all instances of the text character 'a' by using a box that has a red
outline and a blue background. It is text-relative because the box always
overlays the 'a' characters, even when they are moved or reformatted.

In the New Project dialog box, expand Visual C\# or Visual Basic and then click
Extensibility. In the Templates pane, select VSIX Project. In the Name box, type
TestAdornment. Click OK.

In the Solution Explorer, right-click the project node and select Add \> New
Item. Go to the Visual C\# Extensibility node and select Editor Text Adornment.
Leave the default file name (*TextAdornment1.cs/vb*).

There are two code files, as follows:

1.  *TextAdornment1.cs* contains the TextAdornment1 class.

2.  *TextAdornment1TextViewCreationListener.cs* contains the
    TextAdornment1TextViewCreationListener class.

Build the project and start debugging. The experimental instance appears. If you
open a text file, all the 'a' characters in the text are outlined in red against
a blue background.

## Create a viewport-relative adornment extension

The Editor Viewport Adornment template creates a viewport-relative adornment
that adds a violet box that has a red outline to the top-right corner of the
viewport.

### Note

The viewport is the area of the text view that is currently displayed.

### To create a viewport adornment extension by using the Editor Viewport Adornment template

In the New Project dialog box, expand Visual C\# or Visual Basic and then click
Extensibility. In the Templates pane, select VSIX Project. In the Name box, type
ViewportAdornment. Click OK.

In the Solution Explorer, right-click the project node and select Add \> New
Item. Go to the Visual C\# Extensibility node and select Editor Viewport
Adornment. Leave the default file name (*ViewportAdornment1.cs/vb*).

There are two code files, as follows:

*ViewportAdornment1.cs* contains the ViewportAdornment1 class.

*ViewportAdornment1TextViewCreationListener.cs* contains the
ViewportAdornment1TextViewCreationListener class

Build the project and start debugging. The experimental instance appears. If you
create a new text file, a violet box that has a red outline is displayed in the
top-right corner of the viewport.

## Create a margin extension

The Editor Margin template creates a green margin that appears together with the
words \**Hello world!* below the horizontal scroll bar.

### To create a margin extension by using the Editor Margin template

In the New Project dialog box, expand Visual C\# or Visual Basic and then click
Extensibility. In the Templates pane, select VSIX Project. In the Name box, type
MarginExtension. Click OK.

In the Solution Explorer, right-click the project node and select Add \> New
Item. Go to the Visual C\# Extensibility node and select Editor Margin. Leave
the default file name (EditorMargin1.cs/vb).

There are two code files, as follows:

1.  *EditorMargin1.cs* contains the EditorMargin1 class.

2.  *EditorMargin1Factory.cs* contains the EditorMargin1Factory class.

Build this project and start debugging. The experimental instance appears. If
you open a text file, a green margin that has the words Hello EditorMargin1 is
displayed below the horizontal scroll bar.
