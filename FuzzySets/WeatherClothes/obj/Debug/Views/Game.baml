   M S B A M L   `   `   `  ����J  FTwentyQuestions, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null/clr-namespace:WeatherClothesWeatherClothes  5   	       WeatherClothes.Views.Game   5   	   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   *lclr-namespace:WeatherClothes     5   	   X TXceed.Wpf.Toolkit, Version=2.4.0.0, Culture=neutral, PublicKeyToken=3e4669d2f30244f44xctk)http://schemas.xceed.com/wpf/xaml/toolkit  5   	   -   5          � Title$  Weather Advisor��5   	   $	��500��6!   $	��400��6.   
�������$	��400��6Q   $	��500��6a    � 	Resources 5         ButtonStyle �� 
TargetType �� Property �� Value �� 
TargetType �� Stretch  	TextStyle��%�  &	        &	     �� 5	   
   " ��6$   .�5
      ��  � �� 5      �� 5      " ��6*   .R�5      � .+�5   "   A� 
������$
 Fill=�6=   
�������$��Stretch=�6c   �� 5   "   $��Center=�63   $��Center=�5   +   5      5      5      5      �� 5   
   " ��6"   .�5      ��  � ��$ 	ExtraBold"�6+   �� 5       � ��$ Century Gothic)�6+   �� 5       � ��$ Center=�62   �� 5       �@��TextAlignment$ Center=�6.   5      5      .��5   
   �  � RowDefinitions 5   
   �� 5       $��* �6   �� 5!      $��2* �6   �� 5"      $��2* �6   �� 5#      $
��0.5* �6    � ColumnDefinitions 5%   
   �� 5&      $��* �6   �� 5'      $��* �6   �� 5(      $��* �6   �� 5)      $��* �6   �� 5*      $��* �6   �� 5+      $��* �6   .+�5-   
   �� -   5        	 ��Name$	 Border1��5-      $��0��6#   $��1��60   $��4��6@   ._�5.      �� $��
/logo5.png��6   ��6(   �� 
 �� ElementName$
 Border1�� �� Path$ ActualWidth���6_   �� $
 Border1��$ ActualHeight�5/      �� 50   
   -   5         ��Name$ Canvas1��50      $��1��6"   $��0��6/   $��3��6?   $��Stretch=�6S   .+�51      �� 
�������
���   � �� RadiusX$ 50��6<    �� RadiusY$ 50��6I   ��6V   �� $
 Canvas1��$ ActualHeight���6�   �� $
 Canvas1��$ ActualWidth��� 52       �� Orientation$ Vertical=�6   .+�53      �� ��6   Z� ._�54      �� -   5         ��Name$ 
TextBlock1��54      $��18(�60   #��[ 6>   ��6m   �� $ ActualWidth�$
 Canvas1��55      &    WeatherClothes.TemperatureUpDown  56      -   5          Name$ Iupdown1��56   &   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$
 -273X�68   �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Int32, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   FormatString$ D0��6t      Maximum$	 100X�6�   $	��100��6�      Value 6G   �� *TemperatureDp+ �� Mode$ TwoWay=�6�   57      58      �� 59   
   -   5        $ Canvas2��59      $��2��6"   $��0��6/   $��3��6?   $��Stretch=�6S   .+�5:      �� 
���/���
���   �$ 50��6:   $ 50��6G   ��6T   �� $
 Canvas1��$ ActualHeight���6�   �� $
 Canvas2��$ ActualWidth��� 5;      $ Vertical=�6   .+�5<      �� ��6   Z� ._�5=      �� -   5        $ 
TextBlock2��5=       $��18(�64   #��[ 6B   ��6q   �� $ ActualWidth�$
 Canvas2��5>          WeatherClothes.SpeedUpDown  5?      -   5          Name$ Iupdown2��5?       $ D0��62   $ 0X�6D   $ 40X�6z   $	��100��6�    6P   �� *WindSpeedDp+$ TwoWay=�6�   5@      5A      �� 5C      -	   5        $ Canvas3��5C      $��1��6   $��3��6+   $��3��6;   $��Stretch=�6O   .+�5D   
   �� 
������
���   �$ 50��65   $ 50��6B   ��6O   �� $
 Canvas3��$ ActualHeight���6�   �� $
 Canvas3��$ ActualWidth��� 5E   
   $ Vertical=�6   .+�5F      �� ��6   Z� ._�5G      �� -
   5        $ 
TextBlock3��5G       $��18(�64   #��[ 6B   ��6q   �� $ ActualWidth�$
 Canvas3��5H      $  Xceed.Wpf.Toolkit.DoubleUpDown  5I      -   5          Name$ Iupdown3��5I       �  �Xceed.Wpf.Toolkit.NumericUpDown`1[[System.Nullable`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   FormatString$ P0��62   �  �Xceed.Wpf.Toolkit.Primitives.UpDownBase`1[[System.Nullable`1[[System.Double, mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]], mscorlib, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089]]   Minimum$ 0X�6m      Maximum$ 1X�6y   $	��100��6�      	Increment$	 .01X�6�      Value 6D   �� *
MoistureDp+$ TwoWay=�6�   5J      5K      �� 5L   
   -   5        $�� 3��5L      $��Century Gothic)�6   $��	ExtraBold"�6;   #��[  6R   $��2��6�   $��2��6�   $��	Analyse
��6�   5M      5N      