   M S B A M L   `   `   `  ����S  OLinqLanguageEditor2022, Version=1.200.0.0, Culture=neutral, PublicKeyToken=nullO,clr-namespace:LinqLanguageEditor2022.OptionsLinqLanguageEditor2022.Options  5      4    .LinqLanguageEditor2022.Options.AdvancedOptions   5      W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      >local,clr-namespace:LinqLanguageEditor2022.Options     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      .��5      �   � ColumnDefinitions  5      �� 5      $��* �6   .+�5      �� $��0��6   $
��Auto=�6!   .��5      �� $��20, 20q�6    �� Orientation$ Vertical=�6    .+�5      �� -   5         ��Name$ LinqResultsText��5      $	��0,5q�5      $,��&Select LINQ Results Variable to Return��5      $
��Left=�5      $	��Top=�5      $
��Wrap=�5      �� 5      -   5         ��Name$ linqResultVariableText��5      $	��0,5q�5      $	��250��6   $��result��5      $
��Left=�5      $��Right=�6!   $	��Top=�5       �� TextWrapping$
 Wrap=�5      6U   �� 5      -   5        $ advanceOptionText��5      $	��0,5q�5      $
��Left=�5      $	��Top=�5      $
��Wrap=�5       �� 5!      -   5         ��Name$ cbOpenInVSPreviewTab��5"      $	��0,5q�5!      $
��Left=�5#      $	��Top=�5$      $=��7Open Linq Query and result in Visual Studio Preview Tab��5&       d� IsThreeState .  5(      �� 5)      -   5        $ cbEnableToolWindowResults��5*      $	��0,5q�5)      $
��Left=�5+      $	��Top=�5,      $3��-Enable Tool Window for Linq Query and results��5.       .  50      �� 51      -   5        $ tbResultCodeColor��52      $	��0,5q�51      $
��Left=�53      $	��Top=�54      $
��Wrap=�55      �� 56      -   5        $��cmbResultCodeColor��56      $	��0,5q�6)   $��30��58   	   $	��250��6   $
��Left=�6!   ��59      �� 5:      .R�5;   	   �� $ 
Horizontal=�6   .+�5<   
   �� $��16��6*   $��16��65   $��0,2,5,2q�6A   ��6   �� *Name+6A   �� 5=   
   ��6   �� *Name+5>   
   5?   	   5A      �� 5B      -   5        $ tbResultColor��5C      $	��0,5q�5B      $
��Left=�5D      $	��Top=�5E      $
��Wrap=�5F      �� 5G      -	   5        $��cmbResultColor��5G      $	��0,5q�6%   $��30��5I   	   $	��250��6   $
��Left=�6!   ��5J      �� 5K      .R�5L   	   �� $ 
Horizontal=�6   .+�5M   
   �� $��16��6*   $��16��65   $��0,2,5,2q�6A   ��6   �� *Name+6A   �� 5N   
   ��6   �� *Name+5O   
   5P   	   5R      �� 5S      -
   5        $ tbRunningQueryMsgColor��5T      $	��0,5q�5S      $
��Left=�5U      $	��Top=�5V      $
��Wrap=�5W      �� 5X      -   5        $��cmbRunningQueryMsgColor��5X      $	��0,5q�6.   $��30��5Z   	   $	��250��6   $
��Left=�6!   ��5[      �� 5\      .R�5]   	   �� $ 
Horizontal=�6   .+�5^   
   �� $��16��6*   $��16��65   $��0,2,5,2q�6A   ��6   �� *Name+6A   �� 5_   
   ��6   �� *Name+5`   
   5a   	   5c      �� 5d      -   5        $ tbResultsEqualMsgColor��5e      $	��0,5q�5d      $
��Left=�5f      $	��Top=�5g      $
��Wrap=�5h      �� 5i      -   5        $��cmbResultsEqualMsgColor��5i      $	��0,5q�6.   $��30��5k   	   $	��250��6   $
��Left=�6!   ��5l      �� 5m      .R�5n   	   �� $ 
Horizontal=�6   .+�5o   
   �� $��16��6*   $��16��65   $��0,2,5,2q�6A   ��6   �� *Name+6A   �� 5p   
   ��6   �� *Name+5q   
   5r   	   5t      �� 5u      -   5        $! tbExceptionAdditionMsgColor��5v      $	��0,5q�5u      $
��Left=�5w      $	��Top=�5x      $
��Wrap=�5y      �� 5z      -   5        $"��cmbExceptionAdditionMsgColor��5z      $	��0,5q�63   $��30��5|   	   $	��250��6   $
��Left=�6!   ��5}      �� 5~      .R�5   	   �� $ 
Horizontal=�6   .+�5�   
   �� $��16��6*   $��16��65   $��0,2,5,2q�6A   ��6   �� *Name+6A   �� 5�   
   ��6   �� *Name+5�   
   5�   	   5�      5�      5�      5�      5�      