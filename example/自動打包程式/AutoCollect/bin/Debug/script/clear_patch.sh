#!/bin/sh

echo "�A�T�w�n�M���ثe�ؿ��U������(*.tgz, *.md5)?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	rm *.tgz
	rm *.md5
fi

