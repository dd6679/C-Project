   M S B A M L   `   `   `  ����D  @MainTree2, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null1clr-namespace:MainTree2.ViewsMainTree2.Views  5   	   Q MLibrary.Mvvm.Bindables, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null_Dclr-namespace:Library.Mvvm.Bindables;assembly=Library.Mvvm.BindablesLibrary.Mvvm.Bindables 5   	       MainTree2.Views.LogIn   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   /localclr-namespace:MainTree2.Views     5   	   RwDclr-namespace:Library.Mvvm.Bindables;assembly=Library.Mvvm.Bindables     5   	     � Title$  	로그인��5	   	   $	��300��6   $	��300��6"   
������� � 
ResizeMode$ NoResize=�6G    � 	Resources 5
       �� 
TargetType �� Property �� Value��%�  '��       '�� p     'G�     '�� �    �� 5   
   " ��6   .�5      ��  � ��$ 12(�6)   �� 5       � ��$ Center=�62   5      �� 5   
   " ��6   .�5      ��  � ��$ 30��6'   �� 5       � ��$ Center=�62   �� 5       � ��$	 180��6&   5      �� 5   
   " G�6   .�5      ��  � ��$ 30��6'   �� 5       � ��$ Center=�62   �� 5       � ��$	 180��6&   5      �� 5   
   " ��6   .�5      ��  � ��
 �����6+   �� 5       � ��$ 70��6&   �� 5       � ��$ 30��6'   �� 5       � ��$ Right=�64   �� 5       � ��$ 0,0,20,0q�6'   5      5       .��5!      � $��10,10,10,10q�6    � ColumnDefinitions 5"   
   �� 5#      $
��Auto �6   �� 5$       � RowDefinitions 5&   
   �� 5'      �� 5(      �� 5)      �� 5*      �� 5+      .+�5.   
   �� $��서버주소��6   �� 5/   
   $��포트��6   $��1��6   �� 50   
   $��	사용자��6   $��2��6   �� 51   
   $��비밀번호��6   $��3��6   �� 53   
   -   5         ��Name$ ServerTextBox��53      $��1��6)   ��69   �� *Server+�� 54   
   -   5        $ PortTextBox��54      $��1��6'   $��1��64   ��6D   �� *Port+�� 55   
   -   5        $ UserTextBox��55      $��2��6'   $��1��64   ��6D   �� *User+G� 56   
   -   5        	 G�Name$	 PasswordTextBox��56      +  %Library.Mvvm.Bindables.PasswordHelper
   Attach
 . $��3��6�   $��1��6�      Password 6N   �� *Text+ �� Mode$ TwoWay=�6�   �� 58   
   $��접속��6   $��1��6   $��4��6.   ��6;   �� *
LogInBtn+59      5:      