<?php
	require_once('CFileSystem.php');
	$path = exec("pwd");
	$BASE_SRC_PATH	= "{$path}/source";
    $BASE_DST_PATH	= "{$path}/encode";
	$SOURCE_LIST	= '/sourceList.txt';
	// �ˬd�ƥ~�W��O�_�s�b
        $fs = new CFileSystem();
	if ( file_exists($BASE_SRC_PATH . $SOURCE_LIST )===true ){
                $lines = file($BASE_SRC_PATH . $SOURCE_LIST);
		foreach ($lines as $line_num => $line) {
			$line = trim($line);
			//echo $line, "\n";
			$data_arr = explode('*', $line) ;			
			//echo $BASE_SRC_PATH.$data_arr[0], "\n";
			// �ˬd�ӷ��ɮ׬O�_�s�b
			$src_file = $BASE_SRC_PATH . $data_arr[0];
			$dst_file = $BASE_DST_PATH.$data_arr[0];
			if ( file_exists($src_file) === true){
				// �ؿ�			
				if (count($data_arr) > 1){
					$fs->createFolder($dst_file, 0777);
				}else{
					// �ɮ�
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
