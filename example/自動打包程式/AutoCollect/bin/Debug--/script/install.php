<?php
//require_once('php5.php');

function displayUsage(){
    print <<< BOF
        install.php work_tag

        ex:
        家目錄: /home2/wmenc/arcik
        intall.php arick
BOF;
}

/**
 * 安裝類別
 */
class CInstall{
    var $_work_dir;
    var $_home;     // 家目錄
    var $_file_list;

    /**
     * 建構子
     **/
    function CInstall($workDir){
        $this->_work_dir = $workDir;
        $this->_home = "/home2/wmenc/{$this->_work_dir}";	
        $this->_file_list = array(
            'arick_net3.sh',	    // 遠端執行主程式
            'CFileSystem.php',      // 檔案相關 Library
            'php5.php',             // 
            'check.php',            // 確認編碼的結果是否正確
            'clear_patch.sh',   
            'encode_arick.sh',		// PHP 程式編碼
            'genmd5.php',			// 產生檔案對應 md5 檔
            'move_spec_files.php'	// 排外名單
        );	

    }
// public:
    /**
     * 安裝介面必要的 Method，用來安排處理程序
     **/
    function process(){
        $this->copyNeededFile();
        $this->createZendEncode();
    }

    /**
     * 建立 Zend Encode Script
     *
     **/
    function createZendEncode(){
        $FILE_ENCODE = 'encode_arick.sh';	// 
        $content = <<< BOF
#!/bin/sh
echo "Encoding ..."
/usr/local/Zend/bin/zd {$this->_home}/source/ {$this->_home}/encode/

echo "Making tar ball:" $1
cd {$this->_work_dir}/encode
tar zcvf $1 *
mv $1 {$this->_work_dir}
BOF;
        file_put_contents($FILE_ENCODE, $content);
        echo $content;
    }

    /**
     * 複製必摽的檔案
     **/
    function copyNeededFile(){

    }


}
// ----------------------------------------------------------------------------------------
if (count($argv) != 2){
    displayUsage();
    exit();
}

$wizard = new CInstall( $argv[1] );
$wizard->process();
?>