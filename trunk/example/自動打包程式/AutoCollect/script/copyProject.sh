#!/bin/sh
/usr/local/Zend/bin/zd $1/source/ $1/encode/
cd $1
sleep 2s
if [ -e ./source/academic/dbcs ]; then
  cp -rf ./source/academic/dbcs ./encode/academic
fi

if [ -e ./source/lib/adodb ]; then
  cp -rf ./source/lib/adodb ./encode/lib
fi

if [ -e ./source/config/sys_config.php ]; then
  cp -f ./source/config/sys_config.php ./encode/config
fi

sleep 1s
cd $1/encode
tar zcvf $2 *
mv $2 $1
find . -name "*" -type f -print | sort > ../encode.log
cd ../source
find . -name "*" -type f -print | sort > ../source.log
cd ..
diff encode.log source.log -y | grep -e ">" > fail.log
rm -rf ./encode/*
rm -rf ./source/*
