<?php
    $GLOBALS['MAX_VALUE'] = 100;
    /**
     * 顯示安裝進度
     **/
    function showProgress($progress){
        $part_undo = floor( ($GLOBALS['MAX_VALUE']-$progress)/4 );
        $part_do   = ceil( $GLOBALS['MAX_VALUE']/4-$part_undo );
        $progress_first = str_repeat('#', $part_do);
        $progress_mid = str_repeat (' ', $part_undo);
        $progress_last = sprintf("%' 3s", $progress);
        echo "\r";
        echo "|{$progress_first}{$progress_mid}| {$progress_last}%";  
    }
    $idx = 0;
    while($idx <= 100){
        showProgress($idx);
        ++$idx;
        usleep(50000);
    }
?>
