#!/bin/sh
#
# script ���\��; �۰ʤƥ��]�u��
# script ��������T�F 0.2
# �������ɮסGencode_arick.sh, genmd5.php
#
# TODO:
#	�ϥ� find+grep ����s�X����k�������D.
#
# cwc 2005/08/12 
###############################################################################
if [ $# -lt 1 ]; then
  echo "�ϥΤ�k:"
  echo "  arick_net2.sh �ɮת��e�m�W�� [���ݹq��IP] [�M�ץN��] [��m�ɮת���Ƨ��W��]"
  echo ""
  echo "�d�ҡG"
  echo "  arick_net2.sh NUU_20050909"
  echo ""
  echo "�d�ҡG"
  echo "  arick_net2.sh NUU_20050909 192.168.10.120 WM25_ARICK bugfix"
  echo ""
  exit
fi

echo "�A�n�M�� [source] �M [encode] ��Ƨ���������ƶ� ?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	rm -rf ./source/*
	rm -rf ./encode/*
	rm -rf ./tmp/*

	if [ -e encode.log ]; then
		rm encode.log 
	fi
	if [ -e source.log ]; then
		rm source.log 
	fi
	if [ -e fail.log ]; then
		rm fail.log 
	fi	
fi

if [ $# -ge 4 ]; then
	rm -rf ./tmp
	if [ "$2" = "192.168.10.110" ]; then
		scp -P 22 -r nobody@$2:/home1/$3/$4 ./tmp
	else  
		scp -P 1500 -r wm2@$2:/home1/$3/$4 ./tmp
	fi
	mv ./tmp/* ./source
else
	# ���ʧ�����ݸ��
	echo "�A�n������ݪ��ɮר쥻����(1/2/3)"
	echo "1) WM24 (192.168.10.110)"
	echo "2) WM25 (192.168.10.120)"
	echo "3) exit"
	read choice
	case $choice in
	1)
		echo "�п�J�M�צW��(ex: WM24L_NUU)"
		read pn
		echo "�п�J�ɮצs�񪺸�Ƨ�(ex: bugfix)"
		read fn
		scp -P 22 -r "nobody@192.168.10.110:/home1/"$pn"/"$fn ./tmp
		mv ./tmp/* ./source
		;;
	2)
		echo "�п�J�M�צW��(ex: WM25_NCSI)"
		read pn
		echo "�п�J�ɮצs�񪺸�Ƨ�(ex: bugfix)"
		read fn
		rm -rf ./tmp
		scp -P 1500 -r "wm2@192.168.10.120:/home1/"$pn"/"$fn ./tmp
		mv ./tmp/* ./source
		;;
	*)
		#echo "other"
		;;
	esac
fi

#
# �i�楴�]�{��
#
echo "�A�n�}�l�i��{�����]��?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	#
	# php �s�X
	#
	echo 'step 1: encode source'
	./encode_arick.sh $1_Encode.tgz

	if [ -e ./source/academic/dbcs ]; then
		mv ./source/academic/dbcs ./encode/academic/
	fi
	#
	# ��l�X���]
	# 
	echo 'step 2: tar source '
	cd source
	tar -cvf ../$1_Source.tgz *
	cd ..

	#
	# ���� md5
	#
	php ./genmd5.php $1_Encode.tgz

	#
	# �ˬd PHP �s�X�O�_����
	#
	cd ./source
	find . -name "*" -print > ../source.log
	cd ../encode
	find . -name "*" -print > ../encode.log
	cd ..
	diff encode.log source.log -y | grep -e ">" > fail.log

	if [ -s fail.log ]
	then
	  echo "PHP �s�X���ѡA�ɮײM��p�U�G"
	  more fail.log
	  echo "�A���M�n�~���?(y/n)"    
	  echo -n ">>"
	  read yn
	  if [ "$yn" = "n" ]; then   
		exit
	  fi
	fi

fi

echo "�A�n�N���]�n���ɮש�^���ݪ��q����?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	rm -rf ./remote/*
	mv ./$1*.* ./remote	
	if [ $# -ge 4 ]; then
		if [ "$2" = "192.168.10.110" ]; then
			scp -P 22 -r ./remote/ nobody@$2:/home1/$3/$4/
		else			
			scp -r -P 1500 ./remote/ wm2@$2:/home1/$3/$4/
		fi
		
		echo "�ɮש�m�b /home1/"$3"/"$4"/remote �ؿ��U"
	else
		case $choice in
		1)
			scp -r ./remote/ "nobody@192.168.10.110:/home1/"$pn"/"$fn
			;;
		2)
			scp -r -P 1500 ./remote/ "wm2@192.168.10.120:/home1/"$pn"/"$fn
			;;
		esac
		echo "�ɮש�m�b /home1/"$pn"/"$fn"/remote �ؿ��U"


	fi
	
fi
echo "����~~~~"
