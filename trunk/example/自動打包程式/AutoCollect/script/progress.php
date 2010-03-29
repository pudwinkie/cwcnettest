<?php
    $data = array("-", "\\", "|", "/");
    $idx = 0;
	while(true){
        echo "\r"; 
        echo $data[$idx];                   
        ++$idx;
        if (count($data) <= $idx){
            $idx = 0;
        }                
    }
?>