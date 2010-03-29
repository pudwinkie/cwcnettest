#!/bin/sh
#
# script 的功能; 自動化打包工具
# script 的版本資訊； 0.2
# 相關的檔案：encode_arick.sh, genmd5.php
#
# TODO:
#	使用 find+grep 檢驗編碼的方法仍有問題.
#
# cwc 2005/08/12 
###############################################################################
if [ $# -lt 1 ]; then
  echo "使用方法:"
  echo "  arick_net2.sh 檔案的前置名稱 [遠端電腦IP] [專案代號] [放置檔案的資料夾名稱]"
  echo ""
  echo "範例："
  echo "  arick_net2.sh NUU_20050909"
  echo ""
  echo "範例："
  echo "  arick_net2.sh NUU_20050909 192.168.10.120 WM25_ARICK bugfix"
  echo ""
  exit
fi

echo "你要清除 [source] 和 [encode] 資料夾的全部資料嗎 ?(y/n)" 
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
	# 互動抓取遠端資料
	echo "你要抓取遠端的檔案到本機嗎(1/2/3)"
	echo "1) WM24 (192.168.10.110)"
	echo "2) WM25 (192.168.10.120)"
	echo "3) exit"
	read choice
	case $choice in
	1)
		echo "請輸入專案名稱(ex: WM24L_NUU)"
		read pn
		echo "請輸入檔案存放的資料夾(ex: bugfix)"
		read fn
		scp -P 22 -r "nobody@192.168.10.110:/home1/"$pn"/"$fn ./tmp
		mv ./tmp/* ./source
		;;
	2)
		echo "請輸入專案名稱(ex: WM25_NCSI)"
		read pn
		echo "請輸入檔案存放的資料夾(ex: bugfix)"
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
# 進行打包程序
#
echo "你要開始進行程式打包嗎?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	#
	# php 編碼
	#
	echo 'step 1: encode source'
	./encode_arick.sh $1_Encode.tgz

	if [ -e ./source/academic/dbcs ]; then
		mv ./source/academic/dbcs ./encode/academic/
	fi
	#
	# 原始碼打包
	# 
	echo 'step 2: tar source '
	cd source
	tar -cvf ../$1_Source.tgz *
	cd ..

	#
	# 產生 md5
	#
	php ./genmd5.php $1_Encode.tgz

	#
	# 檢查 PHP 編碼是否失敗
	#
	cd ./source
	find . -name "*" -print > ../source.log
	cd ../encode
	find . -name "*" -print > ../encode.log
	cd ..
	diff encode.log source.log -y | grep -e ">" > fail.log

	if [ -s fail.log ]
	then
	  echo "PHP 編碼失敗，檔案清單如下："
	  more fail.log
	  echo "你仍然要繼續嗎?(y/n)"    
	  echo -n ">>"
	  read yn
	  if [ "$yn" = "n" ]; then   
		exit
	  fi
	fi

fi

echo "你要將打包好的檔案放回遠端的電腦嗎?(y/n)" 
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
		
		echo "檔案放置在 /home1/"$3"/"$4"/remote 目錄下"
	else
		case $choice in
		1)
			scp -r ./remote/ "nobody@192.168.10.110:/home1/"$pn"/"$fn
			;;
		2)
			scp -r -P 1500 ./remote/ "wm2@192.168.10.120:/home1/"$pn"/"$fn
			;;
		esac
		echo "檔案放置在 /home1/"$pn"/"$fn"/remote 目錄下"


	fi
	
fi
echo "完成~~~~"
