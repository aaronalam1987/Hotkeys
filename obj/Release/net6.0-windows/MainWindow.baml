   M S B A M L   `   `   `  ���� B  >Hotkeys, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null!clr-namespace:HotkeysHotkeys      Hotkeys.MainWindow   W SPresentationCore, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089R NWindowsBase, Version=6.0.2.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     R NSystem.Xaml, Version=6.0.2.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     'localclr-namespace:Hotkeys     5ui&http://schemas.lepo.co/wpfui/2022/xaml     -     � Title$  Hotkeys��$	��370��$	��700�� � 
ResizeMode$ CanMinimize=� � SizeToContent$ WidthAndHeight=� � WindowStartupLocation$ CenterScreen=� � Icon$ /192x192.png��.��� -    �Name$
 Main��$	��355��$	��700��$	��Top=�.+��� -    ��Name$ 	ButtonOne��$��
Button One��$��0,10,0,0q�$��Center=�$	��Top=�$	��680��$��65��.��� -   $ Group��$��35��$	��680��.+��� -    ��Name$ Button1��$��60��$��20��$��5,0,0,0q�$��Browse��$
��Left=�$��Center=�
 � Tag$ 1���� -   	 ��Name$	 B1_Location��$
��Left=�$��72,0,0,0q�
 �� TextWrapping$

 Wrap=�$��Center=�$	��490��$��20�� �� 
IsReadOnly$
 True���� -    ��Name$ typeLabelB1��$��Type: Not set��$
��Left=�$��	567,4,0,3q��� -   $ 	ButtonTwo��$��
Button Two��$��0,80,0,0q�$��Center=�$	��Top=�$	��680��$��65��.��� -	   $ Group1��$��35��$	��680��.+��� -
   $ Button2��$��60��$��20��$��5,0,0,0q�$��Browse��$
��Left=�$��Center=�$ 2���� -   $	 B2_Location��$
��Left=�$��72,0,0,0q�$

 Wrap=�$��Center=�$	��490��$��20��$
 True���� -   $ typeLabelB2��$��Type: Not set��$
��Left=�$��	567,4,0,3q��� -   $ ButtonThree��$��Button Three��$��	0,150,0,0q�$��Center=�$	��Top=�$	��680��$��65��.��� -   $ Group2��$��35��$	��680��.+��� -   $ Button3��$��60��$��20��$��5,0,0,0q�$��Browse��$
��Left=�$��Center=�$ 3���� -   $	 B3_Location��$
��Left=�$��72,0,0,0q�$

 Wrap=�$��Center=�$	��490��$��20��$
 True���� -   $ typeLabelB3��$��Type: Not set��$
��Left=�$��	567,4,0,3q��� -   $ 
ButtonFour��$��Button Four��$��	0,220,0,0q�$��Center=�$	��Top=�$	��680��$��65��.��� -   $ Group3��$��35��$	��680��.+��� -   $ Button4��$��60��$��20��$��5,0,0,0q�$��Browse��$
��Left=�$��Center=�$ 4���� -   $	 B4_Location��$
��Left=�$��72,0,0,0q�$

 Wrap=�$��Center=�$	��490��$��20��$
 True���� -   $ typeLabelB4��$��Type: Not set��$
��Left=�$��	567,4,0,3q��� -   $ CreateMacro��$��80��$��20��$
��Left=�$��10,290,0,20q�$��Create Macro���� -   $
 Save��$��60��$��20��$��Right=�$��0,290,77,20q�$
��Save���� -   $
 Exit��$��60��$��20��$��Right=�$��0,290,10,20q�$
��Exit���� -    ��Name$ 	statusBar��$��Bottom=�$��25��.@��� .���� -    ��Name$ conState��$
��Left=��� $�Waiting for connection...��