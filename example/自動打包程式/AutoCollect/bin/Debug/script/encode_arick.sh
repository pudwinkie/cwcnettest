#!/bin/sh
echo "Encoding ..."
/usr/local/Zend/bin/zd /home2/wmenc/arick/source/ /home2/wmenc/arick/encode/

echo "Making tar ball:" $1
cd arick/encode
tar zcvf $1 *
mv $1 arick