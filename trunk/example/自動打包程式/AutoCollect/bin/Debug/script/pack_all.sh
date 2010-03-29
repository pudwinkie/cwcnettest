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

echo "(1/5)清除相關資料夾"
rm -rf ./source/*
rm -rf ./encode/*
rm -rf ./tmp

if [ -e encode.log ]; then
	rm encode.log 
fi
if [ -e source.log ]; then
	rm source.log 
fi
if [ -e fail.log ]; then
	rm fail.log 
fi	

echo "(2/5)抓取欲編碼檔案"
scp -r wm2@$2:$3 ./tmp
mv ./tmp/* ./source

php move_spec_files.php

echo "(3/5)開始編碼"
echo 'step 1: encode source'
./encode_arick.sh $1_Encode.tgz

echo 'step 2: tar source '
cd source
tar -cvf ../$1_Source.tgz *
cd ..

echo "(4/5)產生 md5 編碼"
php ./genmd5.php $1_Encode.tgz

php check.php > error.log

echo "(5/5)回傳編碼後的檔案"
rm -rf ./remote/*
mv ./$1*.* ./remote	
mv fail.log ./remote
mv error.log  ./remote
chmod 777 ./remote 
scp -r ./remote/ wm2@$2:$3		
echo "檔案放置在 "$3"/remote 目錄下"	

echo "完成~~~~"
