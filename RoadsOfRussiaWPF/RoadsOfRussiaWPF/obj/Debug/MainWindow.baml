   M S B A M L   `   `   `  ����K  GRoadsOfRussiaWPF, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null3clr-namespace:RoadsOfRussiaWPFRoadsOfRussiaWPF  5   	   !    RoadsOfRussiaWPF.MainWindow   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   0localclr-namespace:RoadsOfRussiaWPF     5   	     � Title$  Главное окно��5   	   $	��450��6   $	��800��6+   .��5	      �  � ColumnDefinitions 5
   
   �� 5      $��* �6   �� 5      $��* �6    � RowDefinitions 5   
   �� 5      $��30 �6   �� 5      $��* �6   .+�5   
   � $��0��6   $��2��6   ���#78B24B.+�5      �� $��	/Logo.png��6   $��5,2,2,2q�6'   $
��Left=�68   �� 5      
�������$��
35,5,200,5q�6(    �� CornerRadius$ 5��6<   �� 5      $7��1Организационная структура��6   $��Center=�69   $
��Left=�6T   $��50,0,0,0q�6o   5      � 5   
   $��1��6   $��0��6   .+�5      R� -   5         R�Name$ DivisionTreeView��5      61   5      � 5   
   $��1��6   $��1��6   .+�5      �� -   5         ��Name$ EmployeeListView��5      ��5      �� 5       �� 
TargetType" ��6    �� Triggers 5       �� 5!       �� Binding 6*   �� *Fired+	 �� Value	 true6D   .a�5"   "   �� 
 �� Property
 � �� �� Value
 �����6?   5#      �� 5$       6*   �� *Fired+		 false6D   .a�5%   "   �� 
 � �� �#78B24B6?   5&      5(      ��5*      �� 5+      .R�5,      � $	��350��6   $��Stretch=�6+   .+�5-      �� $��5q�6)   .+�5.   "   � .+�5/   &   �� ��60   �� *
Division+�� 50   &   $	�� - ��60   �� 51   &   ��60   �� *Post+52   #   � 53   "   .+�54   &   �� $��16(�6I   ��60   �� *	Surname+6I   �� 55   &   $�� ��60   $��16(�69   �� 56   &   $��16(�6F   ��60   �� *Name+6F   �� 57   &   $�� ��60   $��16(�69   �� 58   &   $��16(�6L   ��60   �� *
Patronymic+6L   59   #   � 5:   "   .+�5;   &   �� $��14(�6K   ��60   �� *	CorpPhone+6K   �� 5<   &   $�� ��60   $��14(�69   �� 5=   &   $��14(�6G   ��60   �� *Email+6G   5>   #   �� 5?   "   ��6,   �� *	Cabinet+5@      5A      5B      5D      �� 5E      -   5         ��Name$ AddEmployee��5E      $��Right=�6*   $��Bottom=�6F   $��0,0,20,0q�6a   $��35��6s   $��35��6~   $	��➕��6�   6�   5F      5G      5H      