   M S B A M L   `   `   `  ����D  @MainTree2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MainTree2.ViewsMainTree2.Views  5   	   3clr-namespace:MainTree2.ModelsMainTree2.Models  65   ;"clr-namespace:MainTree2.ViewModelsMainTree2.ViewModels  6b       MainTree2.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   c _System.Windows.Interactivity, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bi:http://schemas.microsoft.com/expression/2010/interactivity  5   	   h dMicrosoft.Expression.Interactions, Version=4.5.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35Bie9http://schemas.microsoft.com/expression/2010/interactions  5   	   )localclr-namespace:MainTree2.Views  5   	   *modelclr-namespace:MainTree2.Models  65   +vm"clr-namespace:MainTree2.ViewModels  6b     � Title$  
MainWindow��5
   	   $	��720��6   $
��1080��6)   
�������.  (System.Windows.Interactivity.Interaction   Triggers 5      /  )System.Windows.Interactivity.EventTrigger  5   
      	EventName$ Closing��6   .  (System.Windows.Interactivity.TriggerBase   Actions. 5      6  0System.Windows.Interactivity.InvokeCommandAction     Command 6$   �� *CloseWindowCommand+5       � 	Resources 5      #   MainTree2.Models.TreeNodeItem   Directories �� DataType �� ItemsSource �� Path	 �� Orientation#   MainTree2.Models.LeafNodeItem!   MainTree2.Views.ContentView  ContentViewControl��%  &	        (��      5   #   *, +)(�� �     5      *, +)&	 �      5   
   6   �� 5   
   "  6#    5   #   �� $ LeafMembers�.R�5      �� $	 
Horizontal=�6   .+�5      �� $��15��6   $��\Assets\group1.png��6#   �� 5      ��6   �� $
 Name�5      5      �� 5   
   "  6   .R�5      �� $	 
Horizontal=�6   .+�5      �� $��15��6   $#��\Assets\device-dom-stream.png��6#   �� 5      ��6   �� $
 Name�5      5        5   
   6   5       .��5"      � .+�5#   
   �� -   5        
 ��Name$
 OverlayCanvas��5#       K� ZIndex$ 5��6(   � 5$   
   $��20q�6   
������� � ColumnDefinitions 5%      �� 5&      �� 5'      $��4* �6#    � RowDefinitions 5)      �� 5*      $
��auto �6    �� 5+      $��3* �6    �� 5,      .+�5.      �� -   5         ��Name$ 	xComboBox��5.      $	��Top=�6*    �� DisplayMemberPath$	 Key��6d   ��6B   �� $
 Root� 5/        50      $ SelectionChanged��6%   . 51         60   �� $ SelectionChangedCommand�   CommandParameter 6c   ��  �� ElementName$ 	xComboBox��$ SelectedItem�52      54      R� 56      -   5         R�Name$ 	xTreeList��56      $��1��6*   $��0, 10q�6]    J� 	AllowDrop . ��67   �� *DirectoryList+ 57        58      $ SelectedItemChanged��6%   . 59         60   �� *DisplayDataInfo+ 6T   �� $ 	xTreeList��$ SelectedValue�5:      ��5<      �� 5=       �� 
TargetType" Q�6   .�5>      -   5        5?      5A      �� 5C      $��2��6   $��1q�6"   
�������._�5D      �  5E      �� 5F      �� 5G      �� 5H      �� 5I      .+�5L      �� $	 
Horizontal=�6!   .+�5M      �� $��	Selected:��6    �� 5N      -   5         ��Name$ NickTextBox��5N       ��65   �� *Nick+5O      �� 5P      $	 
Horizontal=�6!   $��1��6:   .+�5Q      �� $��DeviceSrial:��6    �� 5R      -   5        $ 
DevTextBox��5R       ��64   �� *Dev+5S      �� 5T      $	 
Horizontal=�6!   $��2��6:   .+�5U      �� $��Channel Serial:��6    �� 5V      -   5        $ 
DchTextBox��5V       ��64   �� *Dch+5W      �� 5X      $	 
Horizontal=�6!   $��3��6:   .+�5Y      �� $��Media Serial:��6    �� 5Z      -   5        $ DchmTextBox��5Z       ��65   �� *Dchm+5[      5\      5]      � 5_      -	   5         �Name$ xGrid��5_      $��1��6"   $��3��62    . $	��5,5q�6T    5`      �� 5a      �� 5b       5d      �� 5e      �� 5f      .+�5h          5i      $��1��6$     5j      $��1��6$     5k      $��1��6$   $��1��64   5l      5m      5n      5o      