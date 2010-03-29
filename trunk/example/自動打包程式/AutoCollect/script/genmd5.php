#!/usr/local/bin/php -q
<?php
	$filename = $argv[1];
	if (!file_exists($filename)) {
		die($filename . " 檔案不存在\n");
	}	
	$file_part = explode('.', $filename);
	$file_part[count($file_part)-1] = 'md5';
	$md5 = implode('.', $file_part);

	$fp = fopen($md5, 'w');
	fwrite($fp, md5_file($filename));
	fclose($fp);
?>