   M S B A M L   `   `   `  ����i  eMicrosoft.VisualStudio.Shell.15.0, Version=17.0.0.0, Culture=neutral, PublicKeyToken=b03f5f7f11d50a3a�Zclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0!Microsoft.VisualStudio.PlatformUI  5      h dCommunity.VisualStudio.Toolkit, Version=17.0.394.0, Culture=neutral, PublicKeyToken=79441d341a79572cwTclr-namespace:Community.VisualStudio.Toolkit;assembly=Community.VisualStudio.ToolkitCommunity.VisualStudio.Toolkit 5      S OLinqLanguageEditor2022, Version=1.200.0.0, Culture=neutral, PublicKeyToken=nullW0clr-namespace:LinqLanguageEditor2022.ToolWindows"LinqLanguageEditor2022.ToolWindows 5      ;   5LinqLanguageEditor2022.ToolWindows.ResultDialogWindow   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      oplatformZclr-namespace:Microsoft.VisualStudio.PlatformUI;assembly=Microsoft.VisualStudio.Shell.15.0     5      htoolkitTclr-namespace:Community.VisualStudio.Toolkit;assembly=Community.VisualStudio.Toolkit     5      @rlb0clr-namespace:LinqLanguageEditor2022.ToolWindows     5      -   5        +  %Community.VisualStudio.Toolkit.Themes    
UseVsTheme  . $	��600��5      $	��400��5      $	��200��5      $	��300��5       � SizeToContent$ WidthAndHeight=�5       � 
ResizeMode$ NoResize=�5       � ShowInTaskbar .   � WindowStartupLocation$ CenterOwner=�5      .��5      � $��10q�6    � 	Resources 5   
    �� 
TargetType �� Property �� Value��%�  '��       '�� �     '�� �     �� 5      " ��6   .�5      ��  � ��$ 0,3,5,10q�6+   �� 5       �@��Padding$ 0,0,0,10q�6,   5      �� 5      " ��6   .�5      ��  � ��$ 	0,10,0,10q�6+   5      �� 5       " ��6   .�5!      ��  � ��$ 70��6*   �� 5"       � ��$ 25��6+   �� 5#       � ��$ 5,0,0,0q�6+   5$      5%      	 � ColumnDefinitions	 5'   
   �� 5(      $
��Auto �6   �� 5)      
 � RowDefinitions
 5,   
   �� 5-      $
��Auto �6   �� 5.      $
��Auto �6   �� 5/      $
��Auto �6   �� 50      $
��Auto �6   �� 51      .+�54   
   �� -   5         ��Name$ tbResultDialgoTitle��54      $��0��61   $��0��6A   $4��.Selected Result LINQ Query Variable Not Found!��6N   
���  ��$
��Bold"�6�   $��Center=�6�   $��16(�6�   6�   �� 55   
   -   5        $ tbResultChange��55      $��0��6,   $��1��6<   $4��.Selected Result LINQ Query Variable Not Found!��6I   6�   �� 56   
   -   5        $ CurrentSelection��56      $��0��6.   $��2��6>   $1��+Select a LINQ Query Result Variable to use:��6K   6�   5  /LinqLanguageEditor2022.ToolWindows.RadioListBox  57   
   -   5          Name$ RadioListBox1��58      5  /LinqLanguageEditor2022.ToolWindows.RadioListBox   SelectionChanged$# RadioListBox_SelectionChanged��57   8   $��0��6   $��3��6+   $	��Top=�58   $   $
��Left=�6<   59      �� 5<   
   $��0��6   $��2��6%   $��4��69    �� Orientation$ 
Horizontal=�6F   $��Right=�6_   .+�5=      �� -   5        $��okButton��5=       �� 	IsDefault . .��6M   OK6Q   �� 5>      -   5        $��cancelButton��5>       �� IsCancel . .��6T   Cancel6\   5?      5@      5A      