//Maya ASCII 2019 scene
//Name: test.ma
//Last modified: Sun, Feb 11, 2024 04:53:46 PM
//Codeset: 1253
requires maya "2019";
requires -dataType "HIKCharacter" -dataType "HIKCharacterState" -dataType "HIKEffectorState"
		 -dataType "HIKPropertySetState" "mayaHIK" "1.0_HIK_2016.5";
currentUnit -l centimeter -a degree -t film;
fileInfo "application" "maya";
fileInfo "product" "Maya 2019";
fileInfo "version" "2019";
fileInfo "cutIdentifier" "201905131615-158f5352ad";
fileInfo "osv" "Microsoft Windows 10 Technical Preview  (Build 22631)\n";
createNode clipLibrary -n "clipLibrary1";
	rename -uid "7A2B87F0-46AB-8AB6-0FDA-769041890154";
	setAttr -s 144 ".cel[0].cev";
	setAttr ".cd[0].cm" -type "characterMapping" 144 "Character1_Ctrl_RightHandThumbEffector.translateX" 
		1 1 "Character1_Ctrl_RightHandThumbEffector.translateY" 1 2 "Character1_Ctrl_RightHandThumbEffector.translateZ" 
		1 3 "Character1_Ctrl_RightHandThumbEffector.rotateX" 2 1 "Character1_Ctrl_RightHandThumbEffector.rotateY" 
		2 2 "Character1_Ctrl_RightHandThumbEffector.rotateZ" 2 3 "Character1_Ctrl_LeftHandRingEffector.translateX" 
		1 4 "Character1_Ctrl_LeftHandRingEffector.translateY" 1 5 "Character1_Ctrl_LeftHandRingEffector.translateZ" 
		1 6 "Character1_Ctrl_LeftHandRingEffector.rotateX" 2 4 "Character1_Ctrl_LeftHandRingEffector.rotateY" 
		2 5 "Character1_Ctrl_LeftHandRingEffector.rotateZ" 2 6 "Character1_Ctrl_LeftHandIndexEffector.translateX" 
		1 7 "Character1_Ctrl_LeftHandIndexEffector.translateY" 1 8 "Character1_Ctrl_LeftHandIndexEffector.translateZ" 
		1 9 "Character1_Ctrl_LeftHandIndexEffector.rotateX" 2 7 "Character1_Ctrl_LeftHandIndexEffector.rotateY" 
		2 8 "Character1_Ctrl_LeftHandIndexEffector.rotateZ" 2 9 "Character1_Ctrl_RightHipEffector.translateX" 
		1 10 "Character1_Ctrl_RightHipEffector.translateY" 1 11 "Character1_Ctrl_RightHipEffector.translateZ" 
		1 12 "Character1_Ctrl_RightHipEffector.rotateX" 2 10 "Character1_Ctrl_RightHipEffector.rotateY" 
		2 11 "Character1_Ctrl_RightHipEffector.rotateZ" 2 12 "Character1_Ctrl_LeftHipEffector.translateX" 
		1 13 "Character1_Ctrl_LeftHipEffector.translateY" 1 14 "Character1_Ctrl_LeftHipEffector.translateZ" 
		1 15 "Character1_Ctrl_LeftHipEffector.rotateX" 2 13 "Character1_Ctrl_LeftHipEffector.rotateY" 
		2 14 "Character1_Ctrl_LeftHipEffector.rotateZ" 2 15 "Character1_Ctrl_HeadEffector.translateX" 
		1 16 "Character1_Ctrl_HeadEffector.translateY" 1 17 "Character1_Ctrl_HeadEffector.translateZ" 
		1 18 "Character1_Ctrl_HeadEffector.rotateX" 2 16 "Character1_Ctrl_HeadEffector.rotateY" 
		2 17 "Character1_Ctrl_HeadEffector.rotateZ" 2 18 "Character1_Ctrl_LeftShoulderEffector.translateX" 
		1 19 "Character1_Ctrl_LeftShoulderEffector.translateY" 1 20 "Character1_Ctrl_LeftShoulderEffector.translateZ" 
		1 21 "Character1_Ctrl_LeftShoulderEffector.rotateX" 2 19 "Character1_Ctrl_LeftShoulderEffector.rotateY" 
		2 20 "Character1_Ctrl_LeftShoulderEffector.rotateZ" 2 21 "Character1_Ctrl_ChestOriginEffector.translateX" 
		1 22 "Character1_Ctrl_ChestOriginEffector.translateY" 1 23 "Character1_Ctrl_ChestOriginEffector.translateZ" 
		1 24 "Character1_Ctrl_ChestOriginEffector.rotateX" 2 22 "Character1_Ctrl_ChestOriginEffector.rotateY" 
		2 23 "Character1_Ctrl_ChestOriginEffector.rotateZ" 2 24 "Character1_Ctrl_RightElbowEffector.translateX" 
		1 25 "Character1_Ctrl_RightElbowEffector.translateY" 1 26 "Character1_Ctrl_RightElbowEffector.translateZ" 
		1 27 "Character1_Ctrl_RightElbowEffector.rotateX" 2 25 "Character1_Ctrl_RightElbowEffector.rotateY" 
		2 26 "Character1_Ctrl_RightElbowEffector.rotateZ" 2 27 "Character1_Ctrl_LeftElbowEffector.translateX" 
		1 28 "Character1_Ctrl_LeftElbowEffector.translateY" 1 29 "Character1_Ctrl_LeftElbowEffector.translateZ" 
		1 30 "Character1_Ctrl_LeftElbowEffector.rotateX" 2 28 "Character1_Ctrl_LeftElbowEffector.rotateY" 
		2 29 "Character1_Ctrl_LeftElbowEffector.rotateZ" 2 30 "Character1_Ctrl_RightWristEffector.translateX" 
		1 31 "Character1_Ctrl_RightWristEffector.translateY" 1 32 "Character1_Ctrl_RightWristEffector.translateZ" 
		1 33 "Character1_Ctrl_RightWristEffector.rotateX" 2 31 "Character1_Ctrl_RightWristEffector.rotateY" 
		2 32 "Character1_Ctrl_RightWristEffector.rotateZ" 2 33 "Character1_Ctrl_LeftWristEffector.translateX" 
		1 34 "Character1_Ctrl_LeftWristEffector.translateY" 1 35 "Character1_Ctrl_LeftWristEffector.translateZ" 
		1 36 "Character1_Ctrl_LeftWristEffector.rotateX" 2 34 "Character1_Ctrl_LeftWristEffector.rotateY" 
		2 35 "Character1_Ctrl_LeftWristEffector.rotateZ" 2 36 "Character1_Ctrl_RightAnkleEffector.translateX" 
		1 37 "Character1_Ctrl_RightAnkleEffector.translateY" 1 38 "Character1_Ctrl_RightAnkleEffector.translateZ" 
		1 39 "Character1_Ctrl_RightAnkleEffector.rotateX" 2 37 "Character1_Ctrl_RightAnkleEffector.rotateY" 
		2 38 "Character1_Ctrl_RightAnkleEffector.rotateZ" 2 39 "Character1_Ctrl_HipsEffector.translateX" 
		1 40 "Character1_Ctrl_HipsEffector.translateY" 1 41 "Character1_Ctrl_HipsEffector.translateZ" 
		1 42 "Character1_Ctrl_HipsEffector.rotateX" 2 40 "Character1_Ctrl_HipsEffector.rotateY" 
		2 41 "Character1_Ctrl_HipsEffector.rotateZ" 2 42 "Character1_Ctrl_LeftHandThumbEffector.translateX" 
		1 43 "Character1_Ctrl_LeftHandThumbEffector.translateY" 1 44 "Character1_Ctrl_LeftHandThumbEffector.translateZ" 
		1 45 "Character1_Ctrl_LeftHandThumbEffector.rotateX" 2 43 "Character1_Ctrl_LeftHandThumbEffector.rotateY" 
		2 44 "Character1_Ctrl_LeftHandThumbEffector.rotateZ" 2 45 "Character1_Ctrl_RightShoulderEffector.translateX" 
		1 46 "Character1_Ctrl_RightShoulderEffector.translateY" 1 47 "Character1_Ctrl_RightShoulderEffector.translateZ" 
		1 48 "Character1_Ctrl_RightShoulderEffector.rotateX" 2 46 "Character1_Ctrl_RightShoulderEffector.rotateY" 
		2 47 "Character1_Ctrl_RightShoulderEffector.rotateZ" 2 48 "Character1_Ctrl_ChestEndEffector.translateX" 
		1 49 "Character1_Ctrl_ChestEndEffector.translateY" 1 50 "Character1_Ctrl_ChestEndEffector.translateZ" 
		1 51 "Character1_Ctrl_ChestEndEffector.rotateX" 2 49 "Character1_Ctrl_ChestEndEffector.rotateY" 
		2 50 "Character1_Ctrl_ChestEndEffector.rotateZ" 2 51 "Character1_Ctrl_LeftKneeEffector.translateX" 
		1 52 "Character1_Ctrl_LeftKneeEffector.translateY" 1 53 "Character1_Ctrl_LeftKneeEffector.translateZ" 
		1 54 "Character1_Ctrl_LeftKneeEffector.rotateX" 2 52 "Character1_Ctrl_LeftKneeEffector.rotateY" 
		2 53 "Character1_Ctrl_LeftKneeEffector.rotateZ" 2 54 "Character1_Ctrl_LeftAnkleEffector.translateX" 
		1 55 "Character1_Ctrl_LeftAnkleEffector.translateY" 1 56 "Character1_Ctrl_LeftAnkleEffector.translateZ" 
		1 57 "Character1_Ctrl_LeftAnkleEffector.rotateX" 2 55 "Character1_Ctrl_LeftAnkleEffector.rotateY" 
		2 56 "Character1_Ctrl_LeftAnkleEffector.rotateZ" 2 57 "Character1_Ctrl_RightFootEffector.translateX" 
		1 58 "Character1_Ctrl_RightFootEffector.translateY" 1 59 "Character1_Ctrl_RightFootEffector.translateZ" 
		1 60 "Character1_Ctrl_RightFootEffector.rotateX" 2 58 "Character1_Ctrl_RightFootEffector.rotateY" 
		2 59 "Character1_Ctrl_RightFootEffector.rotateZ" 2 60 "Character1_Ctrl_LeftFootEffector.translateX" 
		1 61 "Character1_Ctrl_LeftFootEffector.translateY" 1 62 "Character1_Ctrl_LeftFootEffector.translateZ" 
		1 63 "Character1_Ctrl_LeftFootEffector.rotateX" 2 61 "Character1_Ctrl_LeftFootEffector.rotateY" 
		2 62 "Character1_Ctrl_LeftFootEffector.rotateZ" 2 63 "Character1_Ctrl_RightKneeEffector.translateX" 
		1 64 "Character1_Ctrl_RightKneeEffector.translateY" 1 65 "Character1_Ctrl_RightKneeEffector.translateZ" 
		1 66 "Character1_Ctrl_RightKneeEffector.rotateX" 2 64 "Character1_Ctrl_RightKneeEffector.rotateY" 
		2 65 "Character1_Ctrl_RightKneeEffector.rotateZ" 2 66 "Character1_Ctrl_RightHandIndexEffector.translateX" 
		1 67 "Character1_Ctrl_RightHandIndexEffector.translateY" 1 68 "Character1_Ctrl_RightHandIndexEffector.translateZ" 
		1 69 "Character1_Ctrl_RightHandIndexEffector.rotateX" 2 67 "Character1_Ctrl_RightHandIndexEffector.rotateY" 
		2 68 "Character1_Ctrl_RightHandIndexEffector.rotateZ" 2 69 "Character1_Ctrl_RightHandRingEffector.translateX" 
		1 70 "Character1_Ctrl_RightHandRingEffector.translateY" 1 71 "Character1_Ctrl_RightHandRingEffector.translateZ" 
		1 72 "Character1_Ctrl_RightHandRingEffector.rotateX" 2 70 "Character1_Ctrl_RightHandRingEffector.rotateY" 
		2 71 "Character1_Ctrl_RightHandRingEffector.rotateZ" 2 72  ;
	setAttr ".cd[0].cim" -type "Int32Array" 144 0 1 2 3 4
		 5 6 7 8 9 10 11 12 13 14 15 16
		 17 18 19 20 21 22 23 24 25 26 27 28
		 29 30 31 32 33 34 35 36 37 38 39 40
		 41 42 43 44 45 46 47 48 49 50 51 52
		 53 54 55 56 57 58 59 60 61 62 63 64
		 65 66 67 68 69 70 71 72 73 74 75 76
		 77 78 79 80 81 82 83 84 85 86 87 88
		 89 90 91 92 93 94 95 96 97 98 99 100
		 101 102 103 104 105 106 107 108 109 110 111 112
		 113 114 115 116 117 118 119 120 121 122 123 124
		 125 126 127 128 129 130 131 132 133 134 135 136
		 137 138 139 140 141 142 143 ;
createNode animClip -n "clip1Source";
	rename -uid "BB7502FA-44F4-C663-9480-51A95D7763F3";
	setAttr ".ihi" 0;
	setAttr -s 144 ".ac[0:143]" yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes yes 
		yes yes yes yes yes yes yes;
	setAttr ".ss" 1;
	setAttr ".se" 179;
	setAttr ".ci" no;
createNode animCurveTL -n "Character1_Ctrl_RightHandThumbEffector_translateX";
	rename -uid "57EBECC3-4614-65FE-CB50-8DA5C5C473B3";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.94849050045013428 100 -0.90731024742126465
		 179 -0.94849050045013428;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandThumbEffector_translateY";
	rename -uid "A02D6BEA-4456-FDE9-BA19-73B0A46A8435";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.8840012550354004 100 1.8952127695083618
		 179 1.8840012550354004;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandThumbEffector_translateZ";
	rename -uid "155D34AD-405B-793B-F81C-3685761BAD16";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.60995006561279297 100 0.60403800010681152
		 179 0.60995006561279297;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightHandThumbEffector_rotateX";
	rename -uid "41164448-4287-35BA-5267-27B235C6DA1B";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.7662635687700505 100 4.4240561297614089
		 179 3.7662635687700505;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandThumbEffector_rotateY";
	rename -uid "058D6CA4-438A-7E2C-B675-FE93926052D5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 60.29236343228547 100 58.831844254085397
		 179 60.29236343228547;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandThumbEffector_rotateZ";
	rename -uid "4987B09B-4C8C-9C7F-1A2C-2082F573CA0E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -12.860220147339021 100 -19.757948697976847
		 179 -12.860220147339021;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftHandRingEffector_translateX";
	rename -uid "36503C57-4079-97AD-53D5-7BA68C278275";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.97510695457458496 100 0.94112467765808105
		 179 0.97510695457458496;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandRingEffector_translateY";
	rename -uid "71D32C85-4436-5220-0D19-8590A6DC6A29";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.5845328569412231 100 1.5229731798171997
		 179 1.5845328569412231;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandRingEffector_translateZ";
	rename -uid "CA38A58C-4138-9DFF-3710-51B5509A360B";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.025145649909973145 100 -0.012968629598617554
		 179 -0.025145649909973145;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftHandRingEffector_rotateX";
	rename -uid "13047787-472F-AB4B-E7EE-E49C8B11FC19";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 24.415093880853064 100 20.171160636013699
		 179 24.415093880853064;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandRingEffector_rotateY";
	rename -uid "A32F6E1A-47D5-81AC-8E12-8E96F8B554A4";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -18.20235653868848 100 -16.198373588418892
		 179 -18.20235653868848;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandRingEffector_rotateZ";
	rename -uid "C926EFD0-4987-B251-67D3-CCA3268F9454";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 61.716143647114478 100 63.544725587553955
		 179 61.716143647114478;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftHandIndexEffector_translateX";
	rename -uid "13098E23-4E3F-C510-64A0-208CAD1F2039";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.1027461290359497 100 1.0478872060775757
		 179 1.1027461290359497;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandIndexEffector_translateY";
	rename -uid "447FAC74-4E45-23E5-788F-3CBE87FED0B9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.5241109132766724 100 1.4759786128997803
		 179 1.5241109132766724;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandIndexEffector_translateZ";
	rename -uid "6B6F605A-4D97-4030-215B-739F229F32EE";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.28201684355735779 100 0.30433973670005798
		 179 0.28201684355735779;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftHandIndexEffector_rotateX";
	rename -uid "AB0EDB4E-4FA3-69DA-363A-ACBD24F067C6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 20.321907235572322 100 15.949172722763254
		 179 20.321907235572322;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandIndexEffector_rotateY";
	rename -uid "2C935190-43B4-43DE-B434-FDBCDBAA8EC7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -11.928692293824724 100 -10.396794642492292
		 179 -11.928692293824724;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandIndexEffector_rotateZ";
	rename -uid "D80D3E09-478B-3531-18F0-05BF44065BC6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 65.948469601442326 100 66.962254817861705
		 179 65.948469601442326;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightHipEffector_translateX";
	rename -uid "1DC554BB-4FA3-660F-99E1-259CBA21EFFD";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.44550004601478577 100 -0.44550004601478577
		 179 -0.44550004601478577;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHipEffector_translateY";
	rename -uid "4FD93C8D-4483-2EA2-19EE-BCA9FB21A9A7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.204547643661499 100 2.1076645851135254
		 179 2.204547643661499;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHipEffector_translateZ";
	rename -uid "96D8437E-4DB0-76D7-F5A3-93B78A039CDB";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightHipEffector_rotateX";
	rename -uid "1FD4122F-4353-FC69-E977-94AE656C8C02";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 8.8850838650827888e-12 100 0.96165036011482374
		 179 8.8850782873359248e-12;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHipEffector_rotateY";
	rename -uid "FD18CF1B-4948-84FD-7045-849B65DF72A3";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.7928421320462043e-05 100 1.1487016674555091
		 179 2.7928421320494759e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHipEffector_rotateZ";
	rename -uid "78E15AE2-4C23-674F-D0A3-76B78E5E1770";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.6455895608823427e-05 100 -6.4337835312871166
		 179 3.645589560883099e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftHipEffector_translateX";
	rename -uid "AA21CFC0-43E2-7789-A9EA-3C9B00B51F3E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.44550004601478577 100 0.44550004601478577
		 179 0.44550004601478577;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHipEffector_translateY";
	rename -uid "5B305480-4114-6B69-1F55-63AFD12FE084";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.204547643661499 100 2.1076645851135254
		 179 2.204547643661499;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHipEffector_translateZ";
	rename -uid "1C81C9C7-4C6F-BB31-B8E7-4881E0954FA5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftHipEffector_rotateX";
	rename -uid "B5FC4332-447C-BA4C-B55F-9AAD2480CFF1";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -2.9517878254992523e-06 100 -0.96166704057729535
		 179 -2.9517878255832191e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHipEffector_rotateY";
	rename -uid "1DA91B3C-4EA9-0B3D-BB31-F79FB38F58B0";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -2.802196744355851e-05 100 -1.1487011132525202
		 179 -2.8021967443533455e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHipEffector_rotateZ";
	rename -uid "C4F737C3-4800-0BF0-CF48-998EC40EC66D";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 5.1910620779353563e-05 100 -6.4337740769284117
		 179 5.1910620779560653e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_HeadEffector_translateX";
	rename -uid "36C3C2E7-4DC2-CBB5-0966-62BE59757DD5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.9715904494121843e-17 100 0 179 -1.9715904494121843e-17;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_HeadEffector_translateY";
	rename -uid "BE181E14-41CB-3C05-9223-86953E043E64";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 4.8207073211669922 100 4.7238245010375977
		 179 4.8207073211669922;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_HeadEffector_translateZ";
	rename -uid "9BE6CA33-443D-695B-893C-0087B5455B15";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.47399803996086121 100 0.47399803996086121
		 179 0.47399803996086121;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_HeadEffector_rotateX";
	rename -uid "1C1AD2BB-438B-26A9-0912-42A2E4A52A0E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_HeadEffector_rotateY";
	rename -uid "7B1B9CFE-4EBE-9176-0766-06BDF4E30630";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_HeadEffector_rotateZ";
	rename -uid "C77339B3-47DE-10D4-BE48-528D5558C801";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftShoulderEffector_translateX";
	rename -uid "1CC5127B-4DFD-31C5-41ED-8CB3D5B9F1DD";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.81514298915863037 100 0.81514298915863037
		 179 0.81514298915863037;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftShoulderEffector_translateY";
	rename -uid "B28E7C7C-4878-A0E3-151A-ECA8EE2C6B82";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.9668822288513184 100 3.8699991703033447
		 179 3.9668822288513184;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftShoulderEffector_translateZ";
	rename -uid "EE945E6E-4F47-3F19-AB45-C6A1C362A818";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.01630820706486702 100 -0.01630820706486702
		 179 -0.01630820706486702;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftShoulderEffector_rotateX";
	rename -uid "8A5F4FEE-41AD-C407-D848-15B9DC5F42F0";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 29.380438278563393 100 31.669345353047301
		 179 29.380438278563393;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftShoulderEffector_rotateY";
	rename -uid "AC9236F2-4C8D-ED9D-6549-358715ABBA4F";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 43.624910947092999 100 41.5842746383899
		 179 43.624910947092999;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftShoulderEffector_rotateZ";
	rename -uid "9DDF606C-47D0-F95B-8157-049E3A2C3260";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 17.669808807297343 100 21.32625145744144
		 179 17.669808807297343;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_ChestOriginEffector_translateX";
	rename -uid "286C6FB8-417B-38BF-91A5-F08DEA87A94E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_ChestOriginEffector_translateY";
	rename -uid "250888C9-4FF0-DB33-1BEF-E5BEFC0DFF76";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.6598136425018311 100 2.5629305839538574
		 179 2.6598136425018311;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_ChestOriginEffector_translateZ";
	rename -uid "3B189AC1-475F-C6EB-B132-478BA7A3AAE2";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_ChestOriginEffector_rotateX";
	rename -uid "0645DB08-4DDE-96C9-D3B2-24ACF56B8A3E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_ChestOriginEffector_rotateY";
	rename -uid "BBDD2BC7-4F75-4BD6-A74A-40A7DDD2106E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_ChestOriginEffector_rotateZ";
	rename -uid "0845BEE0-410D-1557-154A-FEB33BE7B14C";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightElbowEffector_translateX";
	rename -uid "82799C55-4FA1-2EA1-503D-69A6D141FE75";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.96326547861099243 100 -0.97873139381408691
		 179 -0.96326547861099243;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightElbowEffector_translateY";
	rename -uid "AD1B1857-4559-BE56-1905-1E8FEE2B8F4B";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.1116981506347656 100 3.0558886528015137
		 179 3.1116981506347656;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightElbowEffector_translateZ";
	rename -uid "649DFAD9-416E-92B7-4443-87B695CB95B7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.35923713445663452 100 -0.44215115904808044
		 179 -0.35923713445663452;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightElbowEffector_rotateX";
	rename -uid "4D48BFBE-4E94-527C-A807-D7AC5319890C";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -44.267985857959793 100 -45.230864872923419
		 179 -44.267985857959793;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightElbowEffector_rotateY";
	rename -uid "F59AF702-42F2-4A79-A790-D3B3B15E0E62";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -54.897190201754171 100 -50.217771506345493
		 179 -54.897190201754171;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightElbowEffector_rotateZ";
	rename -uid "9DBD88C0-4F47-8217-7C05-059F905AE1A8";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 32.654990265460839 100 38.878474484902029
		 179 32.654990265460839;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftElbowEffector_translateX";
	rename -uid "B120FBF0-4832-D8EF-C300-249791473F1F";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.0163798332214355 100 1.0082931518554688
		 179 1.0163798332214355;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftElbowEffector_translateY";
	rename -uid "AF252902-48D9-B525-777A-DC8E01431D42";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.1186518669128418 100 3.0415282249450684
		 179 3.1186518669128418;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftElbowEffector_translateZ";
	rename -uid "3D6C2048-484A-595E-CC78-F9B386E58954";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.3493061363697052 100 -0.3999859094619751
		 179 -0.3493061363697052;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftElbowEffector_rotateX";
	rename -uid "3B363830-4F8B-24AB-1526-B9AEB74865E6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -44.838316859793984 100 -46.875715521381451
		 179 -44.838316859793984;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftElbowEffector_rotateY";
	rename -uid "47F95576-4170-197C-1F81-20B96C53B33B";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -48.086579145519231 100 -45.830479305852712
		 179 -48.086579145519231;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftElbowEffector_rotateZ";
	rename -uid "ED9C8622-4FA6-146B-70A7-F6B049885C45";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 38.804889127377692 100 43.91202084453947
		 179 38.804889127377692;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightWristEffector_translateX";
	rename -uid "02D2324E-4BE6-8C7F-C75C-F5BF73E549F9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.2253745794296265 100 -1.2240685224533081
		 179 -1.2253745794296265;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightWristEffector_translateY";
	rename -uid "AB05CB42-4F15-C433-70FF-289BEA75F7CA";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.5110709667205811 100 2.4925277233123779
		 179 2.5110709667205811;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightWristEffector_translateZ";
	rename -uid "6ED2813E-452A-B5C5-9ECB-AC92AE71CAF5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.0035555660724639893 100 -0.019780129194259644
		 179 -0.0035555660724639893;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightWristEffector_rotateX";
	rename -uid "657DC4D0-480B-D411-987B-8E9ECCD749A5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 31.777006953731966 100 34.20368427920404
		 179 31.777006953731966;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightWristEffector_rotateY";
	rename -uid "6603CD66-4551-E175-E2AA-C897A8D25F4F";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 53.242634551628953 100 48.912770311099621
		 179 53.242634551628953;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightWristEffector_rotateZ";
	rename -uid "FD51E9FA-4D6D-D60D-E0CC-F6B67E9D0AA7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 34.944704627224034 100 42.910287387527482
		 179 34.944704627224034;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftWristEffector_translateX";
	rename -uid "B42278B5-487B-E37C-AD35-CDA8989F7646";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.2720404863357544 100 1.2325470447540283
		 179 1.2720404863357544;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftWristEffector_translateY";
	rename -uid "BB6C848E-4321-F53A-087F-3A9D71A54625";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.573850154876709 100 2.5164048671722412
		 179 2.573850154876709;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftWristEffector_translateZ";
	rename -uid "86B74775-4C56-47D3-84DF-C498C942954A";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.090910643339157104 100 0.079512089490890503
		 179 0.090910643339157104;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftWristEffector_rotateX";
	rename -uid "0A6B41F1-4BBD-800E-612F-1992D4795DC6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 21.072999402956412 100 16.103985179673195
		 179 21.072999402956412;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftWristEffector_rotateY";
	rename -uid "505FD405-40C7-DE81-8795-D6A1A4DEA9CB";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -65.63575872632974 100 -67.141839065120621
		 179 -65.63575872632974;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftWristEffector_rotateZ";
	rename -uid "BA356AEF-4DF2-A37E-C568-AB8C51284A65";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -39.378764782209203 100 -31.009651733016611
		 179 -39.378764782209203;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightAnkleEffector_translateX";
	rename -uid "2D4B80E1-4BD4-4152-F072-23BD826D5B16";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.5663224458694458 100 -0.5663224458694458
		 179 -0.5663224458694458;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightAnkleEffector_translateY";
	rename -uid "3C36D523-4607-3D70-A2AE-E08D3077DC83";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.41971242427825928 100 0.41971015930175781
		 179 0.41971242427825928;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightAnkleEffector_translateZ";
	rename -uid "A98820D1-46AB-F2BF-07C3-68B0C933E858";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.10972079634666443 100 -0.10972145199775696
		 179 -0.10972079634666443;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightAnkleEffector_rotateX";
	rename -uid "F407EC42-494B-720A-57F1-3CABF3A31438";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightAnkleEffector_rotateY";
	rename -uid "24EC320A-4EE8-B5AD-FD67-8EB2E1DAF494";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.5995810483971007e-06 100 -1.5043638414538896e-06
		 179 -1.5995810483971007e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightAnkleEffector_rotateZ";
	rename -uid "8141D68D-4C7D-AA89-206E-CAB8FF5984B0";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_HipsEffector_translateX";
	rename -uid "663A6779-475D-9CF0-6EAA-5E86C523FD91";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_HipsEffector_translateY";
	rename -uid "8C65F3AF-4C8C-AF5E-C357-818C7E457EB2";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 2.204547643661499 100 2.1076645851135254
		 179 2.204547643661499;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_HipsEffector_translateZ";
	rename -uid "A59EA98B-456D-15A2-0F3D-B5A28B762FF6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_HipsEffector_rotateX";
	rename -uid "CA2C7389-457E-CF5A-3EE8-2092B27E04AE";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_HipsEffector_rotateY";
	rename -uid "D55F2310-44DE-B1B4-3B76-E09F62C7F848";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_HipsEffector_rotateZ";
	rename -uid "62C853F4-48DE-FEBE-AC4B-13BB3D09E522";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftHandThumbEffector_translateX";
	rename -uid "FC193804-420A-23AF-2F91-39A290B50E1A";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.0816686153411865 100 1.014394998550415
		 179 1.0816686153411865;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandThumbEffector_translateY";
	rename -uid "4EEC82D6-4C33-D36C-A346-989BC5E88AA1";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.7622036933898926 100 1.722455620765686
		 179 1.7622036933898926;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftHandThumbEffector_translateZ";
	rename -uid "EC9F9548-405A-C04D-2A09-04A4C5AFF1EC";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.47983148694038391 100 0.48978579044342041
		 179 0.47983148694038391;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftHandThumbEffector_rotateX";
	rename -uid "B4B8AACB-4E5A-C01D-6D2A-668421216482";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -22.572625618196511 100 -23.878563339994084
		 179 -22.572625618196511;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandThumbEffector_rotateY";
	rename -uid "2660A954-4426-797F-7FBA-40AD1B9AAF0E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -64.480496578290129 100 -64.826846188122119
		 179 -64.480496578290129;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftHandThumbEffector_rotateZ";
	rename -uid "D0A31FC2-4CAA-8DAF-49EE-04890D7C59B2";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 42.270712011220489 100 38.940420471437342
		 179 42.270712011220489;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightShoulderEffector_translateX";
	rename -uid "1C7EDF71-432A-A048-B0CA-26A3481C5FF3";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.81514298915863037 100 -0.81514298915863037
		 179 -0.81514298915863037;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightShoulderEffector_translateY";
	rename -uid "64B67B66-41AD-6DF4-AAB5-3AAB07FFB2E3";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.9668822288513184 100 3.8699991703033447
		 179 3.9668822288513184;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightShoulderEffector_translateZ";
	rename -uid "48FAC9E8-43EB-F453-9B9A-E181EAD22A07";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.01630820706486702 100 -0.01630820706486702
		 179 -0.01630820706486702;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightShoulderEffector_rotateX";
	rename -uid "B78CBD02-4F50-A695-883D-E1B71DC123D5";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 31.314506641616997 100 32.9156649171466
		 179 31.314506641616997;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightShoulderEffector_rotateY";
	rename -uid "6D68170E-4D91-3FA6-BF7D-6895A4438B28";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 45.658035744014597 100 40.746157371205157
		 179 45.658035744014597;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightShoulderEffector_rotateZ";
	rename -uid "F275C698-48D4-7D34-1548-17A0574D9215";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 21.404694730786275 100 25.542845165659358
		 179 21.404694730786275;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_ChestEndEffector_translateX";
	rename -uid "DB5BE6B3-4C8D-CF8D-2EC6-B282EFFCD908";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_ChestEndEffector_translateY";
	rename -uid "66C018FC-4EA4-AF4C-77F4-E48421372342";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 4.0072941780090332 100 3.9104111194610596
		 179 4.0072941780090332;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_ChestEndEffector_translateZ";
	rename -uid "EB5EE033-4D45-B87C-9FEA-47879712DE69";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_ChestEndEffector_rotateX";
	rename -uid "7E02C01D-40C2-E7C0-72AE-AF92153E5200";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_ChestEndEffector_rotateY";
	rename -uid "C990B720-4057-31EC-2873-0498E1233F94";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_ChestEndEffector_rotateZ";
	rename -uid "7C91209C-4986-3E46-26C5-98B9BD9C653B";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftKneeEffector_translateX";
	rename -uid "58537A1D-44D6-007D-D2C6-AF83D440D58D";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.72784411907196045 100 0.74411535263061523
		 179 0.72784411907196045;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftKneeEffector_translateY";
	rename -uid "19E3194D-40E3-3573-3FEA-05A9B87B3C45";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.3511145114898682 100 1.2998073101043701
		 179 1.3511145114898682;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftKneeEffector_translateZ";
	rename -uid "7619AAE1-4B4F-9A4A-00EF-F4B1E1262356";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.29547387361526489 100 0.39187738299369812
		 179 0.29547387361526489;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftKneeEffector_rotateX";
	rename -uid "91419A55-45A5-C9D6-27BE-079BC35EC99A";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 6.2042658051397422e-06 100 -0.47036128761465951
		 179 6.2042658051073271e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftKneeEffector_rotateY";
	rename -uid "A217DACF-45A3-3BB8-ACE5-2AB85FDF93DF";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.0252243294844346e-06 100 0.97037969954194514
		 179 3.0252243294853235e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftKneeEffector_rotateZ";
	rename -uid "92C1AA09-4B26-AF50-D2C0-36B9E2BACDDF";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 3.2540915033586381e-05 100 6.0771415729394738
		 179 3.2540915033600997e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftAnkleEffector_translateX";
	rename -uid "404CF126-41CD-9D98-6E23-D98696E5C731";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.56632256507873535 100 0.56632256507873535
		 179 0.56632256507873535;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftAnkleEffector_translateY";
	rename -uid "D401DF8D-43B4-D63F-F1CC-F69875C66378";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.41971242427825928 100 0.41971004009246826
		 179 0.41971242427825928;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftAnkleEffector_translateZ";
	rename -uid "4B7D3FD3-468D-E55C-453C-D89186941F2C";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.10972067713737488 100 -0.10972103476524353
		 179 -0.10972067713737488;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftAnkleEffector_rotateX";
	rename -uid "3C65303A-4900-9E89-71BF-2498CD1371D9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.7185894179481096e-06 100 1.6638661089093137e-06
		 179 1.7185894179481096e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftAnkleEffector_rotateY";
	rename -uid "98CB632C-4FA6-A54E-2FD4-B19324392D26";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.2823008779321114e-06 100 0 179 1.2823008779321114e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftAnkleEffector_rotateZ";
	rename -uid "169D26C2-4F46-C576-29A0-BF917F8850D1";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.9231335520415745e-14 100 0 179 1.9231335520415745e-14;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightFootEffector_translateX";
	rename -uid "AAF370F0-4BB2-6478-5F44-1A8C1A2A50E0";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.69378507137298584 100 -0.69378501176834106
		 179 -0.69378507137298584;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightFootEffector_translateY";
	rename -uid "191B5665-4AA7-CB61-4FBF-8D8900A279BE";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.24177259206771851 100 0.24177032709121704
		 179 0.24177259206771851;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightFootEffector_translateZ";
	rename -uid "1193A599-4D1D-F656-1857-5C90C60ADA3C";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.44145944714546204 100 0.44145879149436951
		 179 0.44145944714546204;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightFootEffector_rotateX";
	rename -uid "A03F5331-4CF8-284D-2C6F-AABC8E4D9E47";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightFootEffector_rotateY";
	rename -uid "5A8D3042-424D-A83A-0CBD-97999CC0A4A3";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.6808711407233157e-06 100 -1.5808150633321651e-06
		 179 -1.6808711407233157e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightFootEffector_rotateZ";
	rename -uid "65245CB4-4E6B-DD1E-C2DD-CC8BEEDD9883";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0 100 0 179 0;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_LeftFootEffector_translateX";
	rename -uid "6E47726B-4555-F6FA-C523-FEAF3C022D81";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.69378519058227539 100 0.69378513097763062
		 179 0.69378519058227539;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftFootEffector_translateY";
	rename -uid "297E2BC8-4011-0A77-6939-76A8947361E7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.24177259206771851 100 0.24177020788192749
		 179 0.24177259206771851;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_LeftFootEffector_translateZ";
	rename -uid "97E554A3-49C1-760F-D0A8-4E9D53910DD9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.44145956635475159 100 0.44145920872688293
		 179 0.44145956635475159;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_LeftFootEffector_rotateX";
	rename -uid "54325C87-4D20-70B4-69C1-67AAAD804230";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.2806604184885151e-06 100 1.4941036264958627e-06
		 179 1.2806604184885151e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftFootEffector_rotateY";
	rename -uid "4BD1FF4E-4E43-2BD2-FFF2-9BA6055F10D4";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.7609079163939712e-06 100 -1.2806602530994652e-06
		 179 -1.7609079163939712e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_LeftFootEffector_rotateZ";
	rename -uid "A64C4549-44BF-C052-3CE3-A7BF697E4509";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.9679678750281517e-14 100 -1.6697906414102677e-14
		 179 -1.9679678750281517e-14;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightKneeEffector_translateX";
	rename -uid "890CF0BE-4F5B-F028-73B3-A091FE0687BF";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -0.72784405946731567 100 -0.74411511421203613
		 179 -0.72784405946731567;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightKneeEffector_translateY";
	rename -uid "AF3DA074-4245-B083-4A40-9394B88D0988";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.3511145114898682 100 1.299808144569397
		 179 1.3511145114898682;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightKneeEffector_translateZ";
	rename -uid "3F4D4AA9-4506-6F3C-7C8C-508E93956EE6";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.29547375440597534 100 0.39187672734260559
		 179 0.29547375440597534;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightKneeEffector_rotateX";
	rename -uid "1B960883-4520-B66E-4629-45A532BD0011";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -5.3454503772473885e-06 100 0.4703621620022353
		 179 -5.3454503772686778e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightKneeEffector_rotateY";
	rename -uid "CAEE2459-46BF-D9C4-9DD2-CEA1D69E1A79";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -6.0420932833196758e-06 100 -0.97037541357534074
		 179 -6.0420932833797499e-06;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightKneeEffector_rotateZ";
	rename -uid "59757445-420E-CDDC-A3E7-BE8A6A59859E";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 4.1481598157286023e-05 100 6.0771315101584751
		 179 4.1481598156933169e-05;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightHandIndexEffector_translateX";
	rename -uid "EDAF1BA2-4BA2-EA93-CA4C-DDAC042CC347";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.0852030515670776 100 -1.0577188730239868
		 179 -1.0852030515670776;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandIndexEffector_translateY";
	rename -uid "B910E280-4F68-AF42-171F-888BF63AFE68";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.6079994440078735 100 1.6238671541213989
		 179 1.6079994440078735;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandIndexEffector_translateZ";
	rename -uid "160414D1-44F3-96F0-6BE4-A58B1C990C33";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.57259321212768555 100 0.60061496496200562
		 179 0.57259321212768555;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightHandIndexEffector_rotateX";
	rename -uid "7B8F0441-4348-A7E2-8FD3-C8BC663464D9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 17.725902218364428 100 24.833056738911051
		 179 17.725902218364428;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandIndexEffector_rotateY";
	rename -uid "6F040DE2-4535-61B4-ECB9-8F8DC983C124";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 7.6503062813411828 100 5.9247474530681083
		 179 7.6503062813411828;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandIndexEffector_rotateZ";
	rename -uid "F10DC326-46C9-27C2-6C06-60B7F5212852";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 60.45369036498186 100 59.015798822075261
		 179 60.45369036498186;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTL -n "Character1_Ctrl_RightHandRingEffector_translateX";
	rename -uid "58F477D5-4B0D-C265-9091-93AC7BD1B2A9";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 -1.1171848773956299 100 -1.1277425289154053
		 179 -1.1171848773956299;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandRingEffector_translateY";
	rename -uid "1F68A2B8-4DD5-EA14-3F70-8399EB5ED46D";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 1.5091052055358887 100 1.505584716796875
		 179 1.5091052055358887;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTL -n "Character1_Ctrl_RightHandRingEffector_translateZ";
	rename -uid "C90FF05A-4FDB-B91F-5449-87A2EE6162D0";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 0.25089722871780396 100 0.29175063967704773
		 179 0.25089722871780396;
	setAttr -s 3 ".kit[2]"  1;
	setAttr -s 3 ".kot[2]"  1;
	setAttr -s 3 ".kix[2]"  1;
	setAttr -s 3 ".kiy[2]"  0;
	setAttr -s 3 ".kox[2]"  1;
	setAttr -s 3 ".koy[2]"  0;
createNode animCurveTA -n "Character1_Ctrl_RightHandRingEffector_rotateX";
	rename -uid "955D09B7-4FCD-0AE6-CA51-14AAD77EE1F7";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 13.44229690286323 100 20.142272568931357
		 179 13.44229690286323;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandRingEffector_rotateY";
	rename -uid "A7560EAA-44DF-9792-06D7-4B8EF578C0FD";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 6.9031929029028278 100 3.6053052083205483
		 179 6.9031929029028278;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
createNode animCurveTA -n "Character1_Ctrl_RightHandRingEffector_rotateZ";
	rename -uid "12B32365-423A-669B-B058-1F9DD608BF40";
	setAttr ".tan" 18;
	setAttr ".wgt" no;
	setAttr -s 3 ".ktv[0:2]"  1 61.169677575710082 100 60.173832838497667
		 179 61.169677575710082;
	setAttr -s 3 ".kit[2]"  2;
	setAttr -s 3 ".kot[2]"  2;
	setAttr ".roti" 5;
select -ne :time1;
	setAttr ".o" 142;
	setAttr ".unw" 142;
select -ne :hardwareRenderingGlobals;
	setAttr ".otfna" -type "stringArray" 22 "NURBS Curves" "NURBS Surfaces" "Polygons" "Subdiv Surface" "Particles" "Particle Instance" "Fluids" "Strokes" "Image Planes" "UI" "Lights" "Cameras" "Locators" "Joints" "IK Handles" "Deformers" "Motion Trails" "Components" "Hair Systems" "Follicles" "Misc. UI" "Ornaments"  ;
	setAttr ".otfva" -type "Int32Array" 22 0 1 1 1 1 1
		 1 1 1 0 0 0 0 0 0 0 0 0
		 0 0 0 0 ;
	setAttr ".fprt" yes;
select -ne :renderPartition;
	setAttr -s 6 ".st";
select -ne :renderGlobalsList1;
select -ne :defaultShaderList1;
	setAttr -s 8 ".s";
select -ne :postProcessList1;
	setAttr -s 2 ".p";
select -ne :defaultRenderUtilityList1;
	setAttr -s 4 ".u";
select -ne :defaultRenderingList1;
select -ne :defaultTextureList1;
	setAttr -s 4 ".tx";
select -ne :initialShadingGroup;
	setAttr -s 3 ".dsm";
	setAttr ".ro" yes;
select -ne :initialParticleSE;
	setAttr ".ro" yes;
select -ne :defaultResolution;
	setAttr ".pa" 1;
select -ne :hardwareRenderGlobals;
	setAttr ".ctrs" 256;
	setAttr ".btrs" 512;
select -ne :characterPartition;
select -ne :ikSystem;
	setAttr -s 4 ".sol";
connectAttr "clip1Source.cl" "clipLibrary1.sc[0]";
connectAttr "Character1_Ctrl_RightHandThumbEffector_translateX.a" "clipLibrary1.cel[0].cev[0].cevr"
		;
connectAttr "Character1_Ctrl_RightHandThumbEffector_translateY.a" "clipLibrary1.cel[0].cev[1].cevr"
		;
connectAttr "Character1_Ctrl_RightHandThumbEffector_translateZ.a" "clipLibrary1.cel[0].cev[2].cevr"
		;
connectAttr "Character1_Ctrl_RightHandThumbEffector_rotateX.a" "clipLibrary1.cel[0].cev[3].cevr"
		;
connectAttr "Character1_Ctrl_RightHandThumbEffector_rotateY.a" "clipLibrary1.cel[0].cev[4].cevr"
		;
connectAttr "Character1_Ctrl_RightHandThumbEffector_rotateZ.a" "clipLibrary1.cel[0].cev[5].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_translateX.a" "clipLibrary1.cel[0].cev[6].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_translateY.a" "clipLibrary1.cel[0].cev[7].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_translateZ.a" "clipLibrary1.cel[0].cev[8].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_rotateX.a" "clipLibrary1.cel[0].cev[9].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_rotateY.a" "clipLibrary1.cel[0].cev[10].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandRingEffector_rotateZ.a" "clipLibrary1.cel[0].cev[11].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_translateX.a" "clipLibrary1.cel[0].cev[12].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_translateY.a" "clipLibrary1.cel[0].cev[13].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_translateZ.a" "clipLibrary1.cel[0].cev[14].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_rotateX.a" "clipLibrary1.cel[0].cev[15].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_rotateY.a" "clipLibrary1.cel[0].cev[16].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandIndexEffector_rotateZ.a" "clipLibrary1.cel[0].cev[17].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_translateX.a" "clipLibrary1.cel[0].cev[18].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_translateY.a" "clipLibrary1.cel[0].cev[19].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_translateZ.a" "clipLibrary1.cel[0].cev[20].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_rotateX.a" "clipLibrary1.cel[0].cev[21].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_rotateY.a" "clipLibrary1.cel[0].cev[22].cevr"
		;
connectAttr "Character1_Ctrl_RightHipEffector_rotateZ.a" "clipLibrary1.cel[0].cev[23].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_translateX.a" "clipLibrary1.cel[0].cev[24].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_translateY.a" "clipLibrary1.cel[0].cev[25].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_translateZ.a" "clipLibrary1.cel[0].cev[26].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_rotateX.a" "clipLibrary1.cel[0].cev[27].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_rotateY.a" "clipLibrary1.cel[0].cev[28].cevr"
		;
connectAttr "Character1_Ctrl_LeftHipEffector_rotateZ.a" "clipLibrary1.cel[0].cev[29].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_translateX.a" "clipLibrary1.cel[0].cev[30].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_translateY.a" "clipLibrary1.cel[0].cev[31].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_translateZ.a" "clipLibrary1.cel[0].cev[32].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_rotateX.a" "clipLibrary1.cel[0].cev[33].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_rotateY.a" "clipLibrary1.cel[0].cev[34].cevr"
		;
connectAttr "Character1_Ctrl_HeadEffector_rotateZ.a" "clipLibrary1.cel[0].cev[35].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_translateX.a" "clipLibrary1.cel[0].cev[36].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_translateY.a" "clipLibrary1.cel[0].cev[37].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_translateZ.a" "clipLibrary1.cel[0].cev[38].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_rotateX.a" "clipLibrary1.cel[0].cev[39].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_rotateY.a" "clipLibrary1.cel[0].cev[40].cevr"
		;
connectAttr "Character1_Ctrl_LeftShoulderEffector_rotateZ.a" "clipLibrary1.cel[0].cev[41].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_translateX.a" "clipLibrary1.cel[0].cev[42].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_translateY.a" "clipLibrary1.cel[0].cev[43].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_translateZ.a" "clipLibrary1.cel[0].cev[44].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_rotateX.a" "clipLibrary1.cel[0].cev[45].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_rotateY.a" "clipLibrary1.cel[0].cev[46].cevr"
		;
connectAttr "Character1_Ctrl_ChestOriginEffector_rotateZ.a" "clipLibrary1.cel[0].cev[47].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_translateX.a" "clipLibrary1.cel[0].cev[48].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_translateY.a" "clipLibrary1.cel[0].cev[49].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_translateZ.a" "clipLibrary1.cel[0].cev[50].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_rotateX.a" "clipLibrary1.cel[0].cev[51].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_rotateY.a" "clipLibrary1.cel[0].cev[52].cevr"
		;
connectAttr "Character1_Ctrl_RightElbowEffector_rotateZ.a" "clipLibrary1.cel[0].cev[53].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_translateX.a" "clipLibrary1.cel[0].cev[54].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_translateY.a" "clipLibrary1.cel[0].cev[55].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_translateZ.a" "clipLibrary1.cel[0].cev[56].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_rotateX.a" "clipLibrary1.cel[0].cev[57].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_rotateY.a" "clipLibrary1.cel[0].cev[58].cevr"
		;
connectAttr "Character1_Ctrl_LeftElbowEffector_rotateZ.a" "clipLibrary1.cel[0].cev[59].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_translateX.a" "clipLibrary1.cel[0].cev[60].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_translateY.a" "clipLibrary1.cel[0].cev[61].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_translateZ.a" "clipLibrary1.cel[0].cev[62].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_rotateX.a" "clipLibrary1.cel[0].cev[63].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_rotateY.a" "clipLibrary1.cel[0].cev[64].cevr"
		;
connectAttr "Character1_Ctrl_RightWristEffector_rotateZ.a" "clipLibrary1.cel[0].cev[65].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_translateX.a" "clipLibrary1.cel[0].cev[66].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_translateY.a" "clipLibrary1.cel[0].cev[67].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_translateZ.a" "clipLibrary1.cel[0].cev[68].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_rotateX.a" "clipLibrary1.cel[0].cev[69].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_rotateY.a" "clipLibrary1.cel[0].cev[70].cevr"
		;
connectAttr "Character1_Ctrl_LeftWristEffector_rotateZ.a" "clipLibrary1.cel[0].cev[71].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_translateX.a" "clipLibrary1.cel[0].cev[72].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_translateY.a" "clipLibrary1.cel[0].cev[73].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_translateZ.a" "clipLibrary1.cel[0].cev[74].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_rotateX.a" "clipLibrary1.cel[0].cev[75].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_rotateY.a" "clipLibrary1.cel[0].cev[76].cevr"
		;
connectAttr "Character1_Ctrl_RightAnkleEffector_rotateZ.a" "clipLibrary1.cel[0].cev[77].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_translateX.a" "clipLibrary1.cel[0].cev[78].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_translateY.a" "clipLibrary1.cel[0].cev[79].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_translateZ.a" "clipLibrary1.cel[0].cev[80].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_rotateX.a" "clipLibrary1.cel[0].cev[81].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_rotateY.a" "clipLibrary1.cel[0].cev[82].cevr"
		;
connectAttr "Character1_Ctrl_HipsEffector_rotateZ.a" "clipLibrary1.cel[0].cev[83].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_translateX.a" "clipLibrary1.cel[0].cev[84].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_translateY.a" "clipLibrary1.cel[0].cev[85].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_translateZ.a" "clipLibrary1.cel[0].cev[86].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_rotateX.a" "clipLibrary1.cel[0].cev[87].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_rotateY.a" "clipLibrary1.cel[0].cev[88].cevr"
		;
connectAttr "Character1_Ctrl_LeftHandThumbEffector_rotateZ.a" "clipLibrary1.cel[0].cev[89].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_translateX.a" "clipLibrary1.cel[0].cev[90].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_translateY.a" "clipLibrary1.cel[0].cev[91].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_translateZ.a" "clipLibrary1.cel[0].cev[92].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_rotateX.a" "clipLibrary1.cel[0].cev[93].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_rotateY.a" "clipLibrary1.cel[0].cev[94].cevr"
		;
connectAttr "Character1_Ctrl_RightShoulderEffector_rotateZ.a" "clipLibrary1.cel[0].cev[95].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_translateX.a" "clipLibrary1.cel[0].cev[96].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_translateY.a" "clipLibrary1.cel[0].cev[97].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_translateZ.a" "clipLibrary1.cel[0].cev[98].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_rotateX.a" "clipLibrary1.cel[0].cev[99].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_rotateY.a" "clipLibrary1.cel[0].cev[100].cevr"
		;
connectAttr "Character1_Ctrl_ChestEndEffector_rotateZ.a" "clipLibrary1.cel[0].cev[101].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_translateX.a" "clipLibrary1.cel[0].cev[102].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_translateY.a" "clipLibrary1.cel[0].cev[103].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_translateZ.a" "clipLibrary1.cel[0].cev[104].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_rotateX.a" "clipLibrary1.cel[0].cev[105].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_rotateY.a" "clipLibrary1.cel[0].cev[106].cevr"
		;
connectAttr "Character1_Ctrl_LeftKneeEffector_rotateZ.a" "clipLibrary1.cel[0].cev[107].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_translateX.a" "clipLibrary1.cel[0].cev[108].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_translateY.a" "clipLibrary1.cel[0].cev[109].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_translateZ.a" "clipLibrary1.cel[0].cev[110].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_rotateX.a" "clipLibrary1.cel[0].cev[111].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_rotateY.a" "clipLibrary1.cel[0].cev[112].cevr"
		;
connectAttr "Character1_Ctrl_LeftAnkleEffector_rotateZ.a" "clipLibrary1.cel[0].cev[113].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_translateX.a" "clipLibrary1.cel[0].cev[114].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_translateY.a" "clipLibrary1.cel[0].cev[115].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_translateZ.a" "clipLibrary1.cel[0].cev[116].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_rotateX.a" "clipLibrary1.cel[0].cev[117].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_rotateY.a" "clipLibrary1.cel[0].cev[118].cevr"
		;
connectAttr "Character1_Ctrl_RightFootEffector_rotateZ.a" "clipLibrary1.cel[0].cev[119].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_translateX.a" "clipLibrary1.cel[0].cev[120].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_translateY.a" "clipLibrary1.cel[0].cev[121].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_translateZ.a" "clipLibrary1.cel[0].cev[122].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_rotateX.a" "clipLibrary1.cel[0].cev[123].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_rotateY.a" "clipLibrary1.cel[0].cev[124].cevr"
		;
connectAttr "Character1_Ctrl_LeftFootEffector_rotateZ.a" "clipLibrary1.cel[0].cev[125].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_translateX.a" "clipLibrary1.cel[0].cev[126].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_translateY.a" "clipLibrary1.cel[0].cev[127].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_translateZ.a" "clipLibrary1.cel[0].cev[128].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_rotateX.a" "clipLibrary1.cel[0].cev[129].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_rotateY.a" "clipLibrary1.cel[0].cev[130].cevr"
		;
connectAttr "Character1_Ctrl_RightKneeEffector_rotateZ.a" "clipLibrary1.cel[0].cev[131].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_translateX.a" "clipLibrary1.cel[0].cev[132].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_translateY.a" "clipLibrary1.cel[0].cev[133].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_translateZ.a" "clipLibrary1.cel[0].cev[134].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_rotateX.a" "clipLibrary1.cel[0].cev[135].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_rotateY.a" "clipLibrary1.cel[0].cev[136].cevr"
		;
connectAttr "Character1_Ctrl_RightHandIndexEffector_rotateZ.a" "clipLibrary1.cel[0].cev[137].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_translateX.a" "clipLibrary1.cel[0].cev[138].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_translateY.a" "clipLibrary1.cel[0].cev[139].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_translateZ.a" "clipLibrary1.cel[0].cev[140].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_rotateX.a" "clipLibrary1.cel[0].cev[141].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_rotateY.a" "clipLibrary1.cel[0].cev[142].cevr"
		;
connectAttr "Character1_Ctrl_RightHandRingEffector_rotateZ.a" "clipLibrary1.cel[0].cev[143].cevr"
		;
// End of test.ma
