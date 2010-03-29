<?php
//require_once('php5.php');

function displayUsage(){
    print <<< BOF
        install.php work_tag

        ex:
        �a�ؿ�: /home2/wmenc/arcik
        intall.php arick
BOF;
}

/**
 * �w�����O
 */
class CInstall{
    var $_work_dir;
    var $_home;     // �a�ؿ�
    var $_file_list;

    /**
     * �غc�l
     **/
    function CInstall($workDir){
        $this->_work_dir = $workDir;
        $this->_home = "/home2/wmenc/{$this->_work_dir}";	
        $this->_file_list = array(
            'arick_net3.sh',	    // ���ݰ���D�{��
            'CFileSystem.php',      // �ɮ׬��� Library
            'php5.php',             // 
            'check.php',            // �T�{�s�X�����G�O�_���T
            'clear_patch.sh',   
            'encode_arick.sh',		// PHP �{���s�X
            'genmd5.php',			// �����ɮ׹��� md5 ��
            'move_spec_files.php'	// �ƥ~�W��
        );	

    }
// public:
    /**
     * �w�ˤ������n�� Method�A�ΨӦw�ƳB�z�{��
     **/
    function process(){
        $this->copyNeededFile();
        $this->createZendEncode();
    }

    /**
     * �إ� Zend Encode Script
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
     * �ƻs���᪺�ɮ�
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