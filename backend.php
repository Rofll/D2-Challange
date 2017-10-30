<?php
 
error_reporting(0);
 
  $filename  = dirname(__FILE__).'/data.txt';
 
  // store new message in the file
  $msg = isset($_GET['msg']) ? $_GET['msg'] : '';
  $user = $_GET['userName'];
  $stroke = $user.": ".$msg."\n";
  if ($msg != '')
  {
	
	$arr = file ('data.txt');	
	$handle = fopen ($filename, "w");
	
	if (count($arr) > 50)
	{
		for ($i = count($arr) - 50; $i < count($arr); $i++)
		{
			fwrite($handle,$arr[$i]);
		}
	}
	else
	{
		for ($i = 0; $i < count($arr); $i++)
		{
			fwrite($handle,$arr[$i]);
		}
	}
	fwrite($handle, $stroke);
	fclose($handle);
    die();
  }
 
  // infinite loop until the data file is not modified
  $lastmodif    = isset($_GET['timestamp']) ? $_GET['timestamp'] : 0;
  $currentmodif = filemtime($filename);
  while ($currentmodif <= $lastmodif) // check if the data file has been modified
  {
    sleep(10); // sleep 1s to unload the CPU
    clearstatcache();
    $currentmodif = filemtime($filename);
  }
 
  // return a json array
  $response = array();
  $response['stroke']       = file_get_contents($filename);
  $response['timestamp'] = ''.$currentmodif.'';
  echo json_encode($response);
  flush();
 
  ?>