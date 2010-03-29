#!/bin/sh

echo "你確定要清除目前目錄下全部的(*.tgz, *.md5)?(y/n)" 
read yn
if [ "$yn" = "y" ]; then 
	rm *.tgz
	rm *.md5
fi

