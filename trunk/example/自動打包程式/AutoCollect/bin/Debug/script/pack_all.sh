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

echo "(1/5)�M��������Ƨ�"
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

echo "(2/5)������s�X�ɮ�"
scp -r wm2@$2:$3 ./tmp
mv ./tmp/* ./source

php move_spec_files.php

echo "(3/5)�}�l�s�X"
echo 'step 1: encode source'
./encode_arick.sh $1_Encode.tgz

echo 'step 2: tar source '
cd source
tar -cvf ../$1_Source.tgz *
cd ..

echo "(4/5)���� md5 �s�X"
php ./genmd5.php $1_Encode.tgz

php check.php > error.log

echo "(5/5)�^�ǽs�X�᪺�ɮ�"
rm -rf ./remote/*
mv ./$1*.* ./remote	
mv fail.log ./remote
mv error.log  ./remote
chmod 777 ./remote 
scp -r ./remote/ wm2@$2:$3		
echo "�ɮש�m�b "$3"/remote �ؿ��U"	

echo "����~~~~"
