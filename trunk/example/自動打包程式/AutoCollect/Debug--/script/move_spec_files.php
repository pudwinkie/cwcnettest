<?php
	require_once('CFileSystem.php');
	$path = exec("pwd");
	$BASE_SRC_PATH	= "{$path}/source";
    $BASE_DST_PATH	= "{$path}/encode";
	$SOURCE_LIST	= '/sourceList.txt';
	// 檢查排外名單是否存在
        $fs = new CFileSystem();
	if ( file_exists($BASE_SRC_PATH . $SOURCE_LIST )===true ){
                $lines = file($BASE_SRC_PATH . $SOURCE_LIST);
		foreach ($lines as $line_num => $line) {
			$line = trim($line);
			//echo $line, "\n";
			$data_arr = explode('*', $line) ;			
			//echo $BASE_SRC_PATH.$data_arr[0], "\n";
			// 檢查來源檔案是否存在
			$src_file = $BASE_SRC_PATH . $data_arr[0];
			$dst_file = $BASE_DST_PATH.$data_arr[0];
			if ( file_exists($src_file) === true){
				// 目錄			
				if (count($data_arr) > 1){
					$fs->createFolder($dst_file, 0777);
				}else{
					// 檔案
					$tmp = explode('/', $dst_file);	
					if (count($tmp) > 0){
						unset( $tmp[ count($tmp)-1 ] );
						$fs->createFolder(implode('/', $tmp) , 0777);						
					}					
				}			

				//echo "fs->copyFile( {$BASE_SRC_PATH}{$data_arr[0]}, {$BASE_DST_PATH}{$data_arr[0]} )";
				$fs->moveFile($src_file, $dst_file);							
			}
		}//end foreach
	}//end if
?>
