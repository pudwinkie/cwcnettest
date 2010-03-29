#!/usr/local/bin/php
<?php
	function file_process_func($filename){
		echo $filename . '<br>';
	}

	function dump_folder($path, $process_func = ''){
		$dir = opendir($path);
		while(($file = readdir($dir))){
			$filename = $path . '/' . $file;
			if ( is_file( $filename ) ){
				if (function_exists($process_func)) {
					$process_func($filename);
				}
			}else if ( is_dir( $filename) && $file != '.' && $file != '..'){
				dump_folder($filename, $process_func);
			}
		}
	}
	
	function checkAll($filename) {
		$filename = str_replace('/source/', '/encode/', $filename);
		if (!file_exists($filename)){
			echo $filename . "\n";
		}
	}

	$srcPath = './source';
	$dstPath = './encode';
	dump_folder($srcPath, 'checkAll');
?>