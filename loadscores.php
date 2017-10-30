<?php

	define("DATABASE", "id1720479_sapereaude");
    // your database's password
    define("PASSWORD", ")z&f2b!e@ZU&qpP0");
    // your database's server
    define("SERVER", "localhost");
    // your database's username
    define("USERNAME", "id1720479_sapereaude");
	
	$db = mysql_connect(SERVER, USERNAME, PASSWORD);
	mysql_select_db(DATABASE, $db);
	mysql_query ("SET NAMES utf8");
	if(!$db){
		die("Connection Failed. ". mysqli_connect_error());
	}
	/*
	* - СОСТОЯНИЯ -
	*	100 - юзер отправляет данные
	*	101 - юзер загружает данные
	*	102 - юзер получает своё место в таблице лидеров
	*	103 - узнаем существует ли юзер (на случай удаления игры)
	*/
	
	$state = $_GET["state"] * 1;	// приведение к инт
	
	if($state === 100)
	{
		$usrId = $_GET["userid"];
		$mmr = $_GET["mmr"];
		
		if(isset($usrId, $mmr))
		{
			$ifExist = mysql_query("SELECT * FROM `d2quiz` WHERE userId='".$usrId."'", $db);
			
			$row = mysql_fetch_row($ifExist);
			
			if (mysql_num_rows($ifExist) > 0 && ($row[2] + 35) >= $mmr)
			{
				$query = mysql_query("UPDATE `d2quiz` SET mmr='".$mmr."' WHERE userId='".$usrId."'", $db);
			}
			else if (mysql_num_rows($ifExist) === 0)
			{
				$query = mysql_query("INSERT INTO `d2quiz` (userId, mmr) VALUES ('".$usrId."', '".$mmr."')", $db);
			}
		}			
	}
	else if ($state  === 101)
	{
		$query = mysql_query("SELECT * FROM `d2quiz` ORDER BY mmr DESC LIMIT 10", $db);
		$rows = array();
		while ($row = mysql_fetch_assoc($query))
		{
			$rows[] = $row; 
		}
		echo json_encode($rows);
	}
	else if ($state === 102)
	{
		$comp = $_GET["comp"];
		$count = mysql_query("SELECT COUNT(*) FROM `d2quiz`", $db);
		$query = mysql_query("SELECT COUNT(*) FROM `d2quiz` WHERE mmr >= '".$comp."'", $db);
		
		echo mysql_result($query, 0)." ".mysql_result($count, 0);
	}
	else if ($state === 103)
	{
		$userId = $_GET["userid"];
		if(isset($userId))
			$query = mysql_query("SELECT * FROM `d2quiz` WHERE userId = '".$userId."'", $db);
		else
			echo "null";
		if (mysql_num_rows($query) > 0)
		{
			$row = mysql_fetch_array($query);
			echo $row['mmr'];
		}
		else
			echo "null";
	}
	else
	{
		
	}
		

?>