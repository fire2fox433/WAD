   M S B A M L   `   `   `  ���� >  :WAD, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:WADWAD  T PCefSharp.Wpf, Version=57.0.0.0, Culture=neutral, PublicKeyToken=40c4b6fc221f4138A0clr-namespace:CefSharp.Wpf;assembly=CefSharp.WpfCefSharp.Wpf     WAD.MainWindow   R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     @d2http://schemas.microsoft.com/expression/blend/2008     Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     #localclr-namespace:WAD     EcefSharp0clr-namespace:CefSharp.Wpf;assembly=CefSharp.Wpf     K GXamlAnimatedGif, Version=1.1.10.0, Culture=neutral, PublicKeyToken=null<gif2https://github.com/XamlAnimatedGif/XamlAnimatedGif  -     �Name$  	Ticketter�� � Title$ 	Ticketter��$	��640��$
��1024��
������� � Icon$ images/icon.png��.��� -    �Name$ baseGrid��$	��640��$��0q�$
��1024��$��Center=�$��Center=�.+��� -    ��Name$ startCanvas��$	��640��$��-2,-28,-6,-3q�$
��1024��$��images/start.png��$��Center=�$��Center=��� -    ��Name$ btnHomeSignIn��$��Sign In��$
��Left=�$��
843,35,0,0q�$	��Top=�$��82��
���  ��
�������$��37��$
��Bold"�$��14(��� -   $ btnHomeRegister��$��Register��$
��Left=�$��
930,42,0,0q�$	��Top=�$��75��$��14(�$
��Bold"���Z� ��Z� �� -    ��Name$ lblHomeHello��$��Hello,��$
��Left=�$��
787,39,0,0q�$	��Top=�$��14(�$y�Hidden=��� -   $ lblHomeName��$�� ��$
��Left=�$��
837,39,0,0q�$	��Top=�$��14(�$y�Hidden=�� -   $ homeGrid��$	��640��$��0q�$
��1024��$��Right=�$��Bottom=� � ColumnDefinitions �� $
��529* ��� $
��495* �.+��� $��Trending��$
��Left=�$��	34,35,0,0q�$	��Top=�$��Calibri)�$��22(��� -	   $ imgHome1��$
��Left=�$	��284��$��3,87,0,0q�$	��Top=�$
��1021��$��images/temp1.jpg��$��UniformToFill=�$��2���� -
    ��Name$ rctHomeOpacity1��
���   �$
��Left=�$	��284��$��3,87,0,0q�$	��Top=�$
��1021��	 J� Opacity$	 0Q�$��2���� -   $ lblHomeMovie1��$��Spiderman Homecoming��$
��Left=�$��385,318,0,0q�$	��Top=�$��24(�$
��Bold"�
���   �$��2����Z� �� -   $ imgHomeList��$
��Left=�$	��230��$��
18,381,0,0q�$	��Top=�$	��230��$��images/list.png���� -   $ rctHomeOpacity2��
���   �$
��Left=�$	��230��$��
18,381,0,0q�$	��Top=�$	��230��$	 0Q�� -   $ 	loginGrid��$
��Left=�$	��640��$	��Top=�$
��1024�����	#FF4A5459{�.  $y�Hidden=�.+��� $��Welcome back.��$
��Left=�$��429,119,0,0q�$	��Top=�
�������$
��Bold"�$��22(��� $��Email:��$
��Left=�$��326,193,0,0q�$	��Top=�
�������$��16(��� -   
 ��Name$
 txtLoginEmail��$
��Left=�$��23��$��381,197,0,0q�$�� ��$	��Top=�$	��263��
�������" �� VerticalScrollBarVisibility$ Disabled=� �� TextWrapping$ NoWrap=� �� MaxLines$ 1��$��14(��� $��	Password:��$
��Left=�$��298,233,0,0q�$	��Top=�
�������$��16(�G� -    G�Name$ txtLoginPassword��$
��Left=�$��23��$��381,239,0,0q�$	��Top=�$	��263��
�������$��14(��� -   $ btnLogin��$��Login��$
��Left=�$��381,278,0,0q�$	��Top=�$	��263��$��45��
���  ��
�������$
��Bold"�$��16(��� -   $ lblLoginIncorrect��$@��:Your email address and password
combination is incorrect!��$
��Left=�$��381,361,0,0q�$	��Top=����	#FFB90909$��18(�$y�Hidden=��� -   $ btnLoginRegister��$��Register��$
��Left=�$��468,323,0,0q�$	��Top=�$��90��$��45��
�������$
��Bold"�$��16(���Z� ��Z� � -   $ registerGrid��$	��640��$��
0,-12,0,12q�$
��1024��{�.  $��Right=�$��Bottom=�$y�Hidden=����	#FF4A5459 � RowDefinitions �� $
��517* ��� $
��123* �.+��� $��Join us.��$
��Left=�$��
468,68,0,0q�$	��Top=�
�������$
��Bold"�$��22(��� $��Email:��$
��Left=�$��326,145,0,0q�$	��Top=�
�������$��16(��� -   $
 txtRegisterEmail��$
��Left=�$��23��$��381,149,0,0q�$�� ��$	��Top=�$	��263��
�������$ Disabled=�$ NoWrap=�$ 1��$��14(��� $��	Password:��$
��Left=�$��298,199,0,0q�$	��Top=�
�������$��16(�G� -   $ txtRegisterPassword��$
��Left=�$��23��$��381,203,0,0q�$	��Top=�$	��263��
�������$��14(��� $��Confirm Password:��$
��Left=�$��237,253,0,0q�$	��Top=�
�������$��16(�G� -   $  txtRegisterConfirmPassword��$
��Left=�$��23��$��381,256,0,0q�$	��Top=�$	��263��
�������$��14(��� $��First Name:��$
��Left=�$��288,303,0,0q�$	��Top=�
�������$��16(��� -   $
 txtRegisterFirstName��$
��Left=�$��23��$��381,307,0,0q�$�� ��$	��Top=�$	��263��
�������$ Disabled=�$ NoWrap=�$ 1��$��14(��� $��Middle Name:��$
��Left=�$��268,354,0,0q�$	��Top=�
�������$��16(��� -   $
 txtRegisterMiddleName��$
��Left=�$��23��$��381,358,0,0q�$�� ��$	��Top=�$	��263��
�������$ Disabled=�$ NoWrap=�$ 1��$��14(��� $��
Last Name:��$
��Left=�$��290,404,0,0q�$	��Top=�
�������$��16(��� -   $
 txtRegisterLastName��$
��Left=�$��23��$��381,408,0,0q�$�� ��$	��Top=�$	��263��
�������$ Disabled=�$ NoWrap=�$ 1��$��14(��� -   $ btnRegisterRegister��$��Register��$
��Left=�$��
381,10,0,0q�$	��Top=�$	��263��$��45��
���  ��
�������$
��Bold"�$��16(�$��1���� $
��DOB:��$
��Left=�$��332,455,0,0q�$	��Top=�
�������$��16(��� -   $
 txtRegisterDOB��$
��Left=�$��23��$��381,459,0,0q�$�� ��$	��Top=�$	��263��
�������$ Disabled=�$ NoWrap=�$ 1��$��14(�� -   $ listGrid��$	��640��$��
0,-12,0,12q�$
��1024��$��Right=�$��Bottom=�$y�Hidden=�
�������{�.  .+��� -   $ rctList1��$
��Left=�$	��238��$��	67,89,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -   $ rctList2��$
��Left=�$	��238��$��
255,89,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -    $ rctList3��$
��Left=�$	��238��$��
449,89,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -!   $ rctList4��$
��Left=�$	��238��$��
639,89,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -"   $ rctList5��$
��Left=�$	��238��$��
829,89,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -#   $ rctList6��$
��Left=�$	��238��$��
67,342,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -$   $ rctList7��$
��Left=�$	��238��$��255,342,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -%   $ rctList8��$
��Left=�$	��238��$��449,342,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -&   $ rctList9��$
��Left=�$	��238��$��639,342,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� -'   $ 	rctList10��$
��Left=�$	��238��$��829,344,0,0q�$	��Top=�$	��121��
���   �$	 0Q��� $��Showing Movies.��$
��Left=�$��	26,37,0,0q�$	��Top=�$��18(�$
��Bold"��� -(   $ imgListImage1��$
��Left=�$	��174��$��	67,89,0,0q�$	��Top=�$	��121��$
��Fill=��� -)   $ lblListLabel1��$�� ��$
��Left=�$��
67,268,0,0q�$	��Top=�$��14(�$	��121��$��59��$��Center=��� -*   $ imgListImage2��$
��Left=�$	��174��$��
255,89,0,0q�$	��Top=�$	��122��$
��Fill=��� -+   $ lblListLabel2��$�� ��$
��Left=�$��255,268,0,0q�$	��Top=�$��14(�$	��122��$��59��$��Center=��� -,   $ imgListImage3��$
��Left=�$	��174��$��
449,89,0,0q�$	��Top=�$	��122��$
��Fill=��� --   $ lblListLabel3��$�� ��$
��Left=�$��449,268,0,0q�$	��Top=�$��14(�$	��122��$��59��$��Center=��� -.   $ imgListImage4��$
��Left=�$	��174��$��
639,89,0,0q�$	��Top=�$	��122��$
��Fill=��� -/   $ lblListLabel4��$�� ��$
��Left=�$��639,268,0,0q�$	��Top=�$��14(�$	��122��$��59��$��Center=��� -0   $ imgListImage5��$
��Left=�$	��174��$��
829,89,0,0q�$	��Top=�$	��122��$
��Fill=��� -1   $ lblListLabel5��$�� ��$
��Left=�$��831,280,0,0q�$	��Top=�$��14(�$	��122��$��59��$��Center=��� -2   $ imgListImage6��$
��Left=�$	��174��$��
67,342,0,0q�$	��Top=�$	��121��$
��Fill=��� -3   $ lblListLabel6��$�� ��$
��Left=�$��
67,520,0,0q�$	��Top=�$��14(�$	��121��$��60��$��Center=��� -4   $ imgListImage7��$
��Left=�$	��174��$��255,342,0,0q�$	��Top=�$	��121��$
��Fill=��� -5   $ lblListLabel7��$�� ��$
��Left=�$��255,520,0,0q�$	��Top=�$��14(�$	��121��$��60��$��Center=��� -6   $ imgListImage8��$
��Left=�$	��174��$��449,342,0,0q�$	��Top=�$	��121��$
��Fill=��� -7   $ lblListLabel8��$�� ��$
��Left=�$��449,520,0,0q�$	��Top=�$��14(�$	��121��$��60��$��Center=��� -8   $ imgListImage9��$
��Left=�$	��174��$��639,342,0,0q�$	��Top=�$	��121��$
��Fill=��� -9   $ lblListLabel9��$�� ��$
��Left=�$��639,520,0,0q�$	��Top=�$��14(�$	��121��$��60��$��Center=��� -:   $ imgListImage10��$
��Left=�$	��174��$��829,342,0,0q�$	��Top=�$	��121��$
��Fill=��� -;   $ lblListLabel10��$�� ��$
��Left=�$��829,520,0,0q�$	��Top=�$��14(�$	��121��$��60��$��Center=��� -<   $ btnListNext��$
��Next��$
��Left=�$��916,585,0,0q�$	��Top=�$��82��
���  ��
�������$��37��$
��Bold"�$��14(��� -=   $ btnListPrev��$��Previous��$
��Left=�$��
26,585,0,0q�$	��Top=�$��82��
���  ��
�������$��37��$
��Bold"�$��14(��� ->   $ btnListBookings��$��My bookings��$
��Left=�$��
878,37,0,0q�$	��Top=�$	��120��$��18(���Z� ��Z� � -?   $ 	movieGrid��$	��640��$��
0,-12,0,12q�$
��1024��$��Right=�$��Bottom=�$y�Hidden=�
�������{�.  .+��� -@   $ imgMovie��$
��Left=�$	��510��$��	44,68,0,0q�$	��Top=�$	��328���� -A   $ lblMovieTitle��$�� ��$
��Left=�$��
480,68,0,0q�$	��Top=�$��36(�$
��Bold"�$	��114��$	��444��$��Center=��� -B   $ lblMovieType��$�� ��$
��Left=�$��480,182,0,0q�$	��Top=����	#FF575757$��16(��� -C   $ btnMovieBook��$
��Book��$
��Left=�$��480,494,0,0q�$	��Top=�$	��444��
���  ��
�������$��84��$
��Bold"�$��24(��� -D   $ lblMoviePrice��$�� ��$
��Left=�$��824,547,0,0q�$	��Top=�$	��100��$��16(�$��Right=�
��������� -E   $ btnMovieBack��$
��Back��$
��Left=�$��
22,594,0,0q�$	��Top=�$��45��$��16(���Z� ��Z� �� -F   $ 
imgLoading��'  !XamlAnimatedGif.AnimationBehavior   	SourceUri$ images/loading.gif?�$��669,321,293,256q�%  CefSharp.Wpf.ChromiumWebBrowser  -G     Name$ wbMovie��$
��Left=�$	��235��$��474,230,0,0q�$	��Top=�$	��460��� -H   $ bookingGrid��$	��640��$
��1024��$��Right=�$��Bottom=�$y�Hidden=�
�������{�.  .+��� $
��Left=�$	��404��$��
51,184,0,0q�$	��Top=�$	��667��
���   ��� -I   $ lblBookingTitle��$�� ��$
��Left=�$��	51,60,0,0q�$	��Top=�$��18(�$
��Bold"�
���   ��� -J    ��Name$ ddlBookingDate��$
��Left=�$��142,126,0,0q�$	��Top=�$	��576��$��34���� $
��Left=�$	��376��$��754,126,0,0q�$	��Top=�$	��236�����	#FFE2E2E2�� -K   $ btnBookingConfirm��$��Confirm��$
��Left=�$��754,520,0,0q�$	��Top=�$	��236��
���  ��
�������$��71��$
��Bold"�$��24(����	#FF707070�� $��Seats booked.��$
��Left=�$��799,142,0,0q�$	��Top=�
���   �$��20(�$
��Bold"���Z� �� -L    ��Name$ lblBookingSeats��$�� ��$��WrapWithOverflow=�$
��Left=�$��776,218,0,0q�$	��Top=�$��18(�
���   �$	��164��$	��192���� $��SCREEN��$
��Left=�$��
70,535,0,0q�$	��Top=�$	��634��$��34��$��18(�{�.  �� -M   $ btnBookingC1��$��C1��$
��Left=�$��116,218,0,0q�$	��Top=�$��61��$��61��$��18(����	#FFA2A2A2
���������Z� �� -N   $ btnBookingC2��$��C2��$
��Left=�$��234,218,0,0q�$	��Top=�$��62��$��61��$��18(����	#FFA2A2A2
���������Z� �� -O   $ btnBookingC3��$��C3��$
��Left=�$��357,218,0,0q�$	��Top=�$��62��$��61��$��18(����	#FFA2A2A2
���������Z� �� -P   $ btnBookingC4��$��C4��$
��Left=�$��474,218,0,0q�$	��Top=�$��62��$��61��$��18(����	#FFA2A2A2
���������Z� �� -Q   $ btnBookingC5��$��C5��$
��Left=�$��585,218,0,0q�$	��Top=�$��62��$��61��$��18(����	#FFA2A2A2
���������Z� �� -R   $ btnBookingB1��$��B1��$
��Left=�$��116,322,0,0q�$	��Top=�$��61��$��60��$��18(����	#FFA2A2A2
���������Z� �� -S   $ btnBookingB2��$��B2��$
��Left=�$��234,322,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -T   $ btnBookingB3��$��B3��$
��Left=�$��357,322,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -U   $ btnBookingB4��$��B4��$
��Left=�$��474,322,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -V   $ btnBookingB5��$��B5��$
��Left=�$��585,322,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -W   $ btnBookingA1��$��A1��$
��Left=�$��116,424,0,0q�$	��Top=�$��61��$��60��$��18(����	#FFA2A2A2
���������Z� �� -X   $ btnBookingA2��$��A2��$
��Left=�$��234,424,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -Y   $ btnBookingA3��$��A3��$
��Left=�$��357,424,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -Z   $ btnBookingA4��$��A4��$
��Left=�$��474,424,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� -[   $ btnBookingA5��$��A5��$
��Left=�$��585,424,0,0q�$	��Top=�$��62��$��60��$��18(����	#FFA2A2A2
���������Z� �� $��
Timeslot: ��$
��Left=�$��
51,126,0,0q�$	��Top=�
���   �$��18(��� -\   $ btnBookingBack��$
��Back��$
��Left=�$��
10,589,0,0q�$	��Top=�$��51��$��18(���Z� ��Z� �� $��Total Seats:��$
��Left=�$��791,412,0,0q�$	��Top=�
���   �$��14(��� $��Price Per Ticket:��$
��Left=�$��764,432,0,0q�$	��Top=�
���   �$��14(��� $��Total:��$
��Left=�$��828,455,0,0q�$	��Top=�$��14(�
���   ��� -]   $ lblBookingTotalSeats��$��0��$
��Left=�$��950,412,0,0q�$	��Top=�$��14(�
���   ��� -^   $ lblBookingTicketPrice��$��$ 0.00��$
��Left=�$��921,433,0,0q�$	��Top=�$��14(�
���   ��� -_   $ lblBookingTotal��$��$ 0.00��$
��Left=�$��921,455,0,0q�$	��Top=�$��14(�
���   �� -`   $ confirmationGrid��$	��640��$
��1024��$��Right=�$��Bottom=�$y�Hidden=�
�������{�.  .+��� $��You are all set!��$
��Left=�$��
330,54,0,0q�$	��Top=�$��48(�$
��Bold"���  �� TextAlignment$ Center=�$��WrapWithOverflow=�$u��oThank you for using Ticketter, your tickets have been booked for the registered timings, here are your details:��$��Center=�$��260,258,292,0q�$	��Top=�$��20(�$	��166��$	��472���� $��images/icondark.png��$
��Left=�$	��100��$��444,137,0,0q�$	��Top=�$	��100���� -a   $ txtConfirmSeats��$ Center=�$��WrapWithOverflow=�$�� ��$
��Left=�$��246,354,0,0q�$	��Top=�$	��502��$��86��$��16(��� -b   $ lblConfirmTime��$ Center=�$��WrapWithOverflow=�$�� ��$
��Left=�$��246,445,0,0q�$	��Top=�$	��502��$��18(�$��39���� -c   $ btnConfirmRecept��$��Save Details��$
��Left=�$��380,507,0,0q�$	��Top=�$	��236��
���  ��
�������$��71��$
��Bold"�$��24(����	#FF707070�� -d   $ btnConfirmBack��$
��Back��$
��Left=�$��
26,572,0,0q�$	��Top=�$��60��$��18(�$��34����Z� ��Z� � -e   $ 	checkGrid��$	��640��$
��1024��$��Right=�$��Bottom=�$y�Hidden=�
�������{�. .+�&   System.Windows.Controls.DataGrid  -f     Name$ dgCheckBookings��   
IsReadOnly . $��Stretch=�$	��368��$��
34,182,0,0q�$	��Top=�$	��948��$��18(�   AutoGenerateColumns .     CanUserReorderColumns .     CanUserResizeColumns .     CanUserSortColumns .     MinColumnWidth$	 150Q� � 	Resources )  #System.Windows.Controls.DataGridRow �� 
TargetType��%   '        �� "  .�-g      Columns 0  *System.Windows.Controls.DataGridTextColumn  ,  &System.Windows.Controls.DataGridColumn   CanUserResize .      FontSize$  18Q�!   Header$! ID��1  +System.Windows.Controls.DataGridBoundColumn"   Binding" �� # �� Path$# ID�$   ClipboardContentBinding$ Z�    .  $  18Q�$! Movie Title��" �� $# Title�$ Z�    .  $  18Q�$! Seats Booked��" �� $# Seats�$ Z�    .  $  18Q�$
! Date��" �� $
# Date�$ Z�    .  $  18Q�$! Timeslot booked��" �� $
# Time�$ Z� �� -h   $ btnCheckBack��$
��Back��$
��Left=�$��
22,580,0,0q�$	��Top=�$��56��$��18(���Z� ��Z� �� -i   $
 txtCheckDelete��$
��Left=�$��36��$��187,134,0,0q�$
 Wrap=�$�� ��$	��Top=�$	��690��$��18(��� $��Enter Booking ID:��$
��Left=�$��
34,134,0,0q�$	��Top=�$��18(��� -j   $ btnCheckDelete��$��Delete��$
��Left=�$��897,136,0,0q�$	��Top=�$��85��$��34��$��16(��� $��Manage your bookings.��$
��Left=�$��	34,38,0,0q�$	��Top=�$
��Bold"�$��36(�