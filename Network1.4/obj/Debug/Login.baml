   M S B A M L   `   `   `  ����E  ANetwork1.4, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null)clr-namespace:Network1._4Network1._4  5          Network1._4.Page1   5      \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      +localclr-namespace:Network1._4     5        N� Title$  Page1��5	      $	��689��6   $
��1272��6"    � 	Resources 5
         loginBtn g� AutoReverse g� RepeatBehavior �� TargetProperty �� 
TargetName9  3System.Windows.Media.Animation.EasingDoubleKeyFrame O� KeyTime O� Value��%+  &	        �� 5   
    . $ Forever��69   .��5      U� $V P(UIElement.RenderTransform).(TransformGroup.Children)[2].(RotateTransform.Angle)�6,   $ buttonLogin��6�   .\�5        $ 0��6'   $ -4.217Q�63     5      $ 0:0:0.44��6'   $ 3.798Q�6:   5      5      5       � Triggers 5      :� 5   
   	 :� RoutedEvent$	 FrameworkElement.Loaded��6   .[�5      �� #��[  6   5      .��5      � .+�5   
   �� -   5        
 ��Name$
 textMainTitle��5      $
��Left=�6+   $��232,117.097,0,0q�6F   $
��Wrap=�6_   $	��Top=�6s   $��106.667(�6�   $	��773��6�   $��
JasmineUPC)�6�   $9��3เกมเป่ายิงฉุบมรณะ��6�   
�������$	��133��6�    J� RenderTransformOrigin$ 0.5,0.5&�6�    �� TextAlignment$ Center=�6  z�5      V� 5      .~�5      �� �� 5      �� 5      T� 5      5       5"      �� 5#   
   -   5         ��Name$ textBoxLogin��5#      $
��Left=�6(   $��45��6C   $��395,410,0,0q�6O    �� TextWrapping$
 Wrap=�6d   $9��3กรอกชื่อผู้ใช่งาน��6x   $	��Top=�6�   $	��465��6�   $��29.333(�6�   
���   �$	��1.5q�6�   $	��RSU)�6�   .��6_  ��5$      �� 5%       �� Color$ 	#FF00BAFF��6"    �� Opacity$	 0.5Q�64   .��5&   "   5'      �� 5(   
   -   5         ��Name$ buttonLogin��5(      $
��JOIN��6&   $
��Left=�65   $��485,485,0,0q�6P   $	��Top=�6e   $	��291��6}   $��53��6�   $��29.333(�6�   
����������	#FFF04765$ 0.5,0.5&�6�   ��6�   Z� z�5)      V� 5*      .~�5+      �� �� 5,      �� 5-      T� 5.      5/      51      52      53      